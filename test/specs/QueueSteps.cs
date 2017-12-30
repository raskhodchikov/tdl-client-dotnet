﻿using System.Linq;
using NUnit.Framework;
using TDL.Client.Audit;
using TDL.Test.Specs.SpecItems;
using TDL.Test.Specs.Utils.Jmx.Broker;
using TDL.Test.Specs.Utils.Logging;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Diagnostics;
using TDL.Client;
using TDL.Client.Queue;

namespace TDL.Test.Specs
{
    [Binding]
    public class QueueSteps
    {
        private const string Hostname = "localhost";
        private const int Port = 21616;

        private readonly LogAuditStream auditStream = new LogAuditStream(new ConsoleAuditStream());
        private readonly RemoteJmxBroker broker = TestBroker.Instance;

        private RemoteJmxQueue requestQueue;
        private RemoteJmxQueue responseQueue;
        private QueueBasedImplementationRunner queueBasedImplementationRunner;
        private QueueBasedImplementationRunner.Builder queueBasedImplementationRunnerBuilder;

        private long requestCount;
        private long processingTimeMillis = 0;

        [Given(@"^I start with a clean broker and a client for user ""([^""]*)""$")]
        public void GivenIStartWithACleanBroker(string username)
        {
            requestQueue = broker.AddQueue($"{username}.req");
            requestQueue.Purge();

            responseQueue = broker.AddQueue($"{username}.resp");
            responseQueue.Purge();

            auditStream.ClearLog();

            var config = new ImplementationRunnerConfig()
                .SetHostname(Hostname)
                .SetPort(Port)
                .SetUniqueId(username)
                .SetAuditStream(auditStream);

            queueBasedImplementationRunnerBuilder = new QueueBasedImplementationRunner.Builder().SetConfig(config);
            queueBasedImplementationRunner = queueBasedImplementationRunnerBuilder.Create();
        }

        [Given(@"^the broker is not available$")]
        public void GivenTheBrokerIsNotAvailable()
        {
            auditStream.ClearLog();

            var config = new ImplementationRunnerConfig()
                .SetHostname("111")
                .SetPort(Port)
                .SetUniqueId("X")
                .SetAuditStream(auditStream);

            queueBasedImplementationRunnerBuilder = new QueueBasedImplementationRunner.Builder().SetConfig(config);
            queueBasedImplementationRunner = queueBasedImplementationRunnerBuilder.Create();
        }

        [Given(@"I receive the following requests:")]
        public void GivenIReceiveTheFollowingRequests(Table table)
        {
            var requests = table.CreateSet<PayloadSpecItem>().Select(i => i.Payload).ToList();

            requests.ForEach(requestQueue.SendTextMessage);
            requestCount = requests.Count;
        }

        [Given(@"^I receive (\d+) identical requests like:$")]
        public void SentIdenticalRequests(int number, Table table)
        {
            var requests = Enumerable
                .Repeat(table.CreateSet<PayloadSpecItem>().Select(p => p.Payload), number)
                .SelectMany(p => p)
                .ToList();

            requests.ForEach(requestQueue.SendTextMessage);
            requestCount = requests.Count;
        }

        [When(@"I go live with the following processing rules:")]
        public void WhenIGoLiveWithTheFollowingProcessingRules(Table table)
        {
            var processingRuleSpecItems = table.CreateSet<ProcessingRuleSpecItem>().ToList();

            processingRuleSpecItems.ForEach(ruleSpec =>
                queueBasedImplementationRunnerBuilder.WithSolutionFor(
                    ruleSpec.Method,
                    CallImplementationFactory.Get(ruleSpec.Call),
                    ClientActionsFactory.Get(ruleSpec.Action)));

            queueBasedImplementationRunner = queueBasedImplementationRunnerBuilder.Create();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            queueBasedImplementationRunner.Run();
            stopwatch.Stop();

            processingTimeMillis = stopwatch.ElapsedMilliseconds;
        }

        [Then(@"^the time to wait for requests is (\d+)ms$")]
        public void ThenTheTimeToWaitForRequestsIs(int expectedTimeout)
        {
            Assert.AreEqual(expectedTimeout, queueBasedImplementationRunner.RequestTimeoutMilliseconds,
                "The client request timeout has a different value.");
        }

        [Then(@"^the request queue is ""([^""]*)""$")]
        public void ThenTheRequestQueueIs(string expectedName)
        {
            Assert.AreEqual(expectedName, requestQueue.Name,
                "Request queue has a different value.");
        }

        [Then(@"^the response queue is ""([^""]*)""$")]
        public void ThenTheResponseQueyeIs(string expectedName)
        {
            Assert.AreEqual(expectedName, responseQueue.Name,
                "Response queue has a different value.");
        }

        [Then(@"the client should consume all requests")]
        public void ThenTheClientShouldConsumeAllRequests()
        {
            Assert.AreEqual(0, requestQueue.GetSize(),
                "Requests have not been consumed.");
        }

        [Then(@"the client should publish the following responses:")]
        public void ThenTheClientShouldPublishTheFollowingResponses(Table table)
        {
            var expectedResponses = table.CreateSet<PayloadSpecItem>().Select(i => i.Payload).ToList();
            var actualResponses = responseQueue.GetMessageContents();
            Assert.IsTrue(expectedResponses.SequenceEqual(actualResponses),
                "The responses are not correct");
        }

        [Then(@"the client should not consume any request")]
        public void ThenTheClientShouldNotConsumeAnyRequest()
        {
            Assert.AreEqual(requestCount, requestQueue.GetSize(),
                "The request queue has different size. The message has been consumed.");
        }

        [Then(@"the client should not publish any response")]
        public void ThenTheClientShouldNotPublishAnyResponse()
        {
            Assert.AreEqual(0, responseQueue.GetSize(),
                "The response queue has different size. Messages have been published.");
        }

        [Then(@"the client should consume first request")]
        public void ThenTheClientShouldConsumeFirstRequest()
        {
            Assert.AreEqual(requestCount - 1, requestQueue.GetSize(),
                "Wrong number of requests have been consumed.");
        }

        [Then(@"the client should display to console:")]
        public void ThenTheClientShouldDisplayToConsole(Table table)
        {
            var expectedOutputs = table.CreateSet<OutputSpecItem>().ToList();
            var actualOutput = auditStream.GetLog();

            expectedOutputs.ForEach(expectedLine =>
            {
                Assert.IsTrue(actualOutput.Contains(expectedLine.Output));
            });
        }

        [Then(@"I should get no exception")]
        public void ThenIShouldGetNoException()
        {
            // No exceptions.
        }

        [Then(@"^the processing time should be lower than (\d+)ms$")]
        public void ProccessingTimeShouldBeLowerThanMs(long threshold)
        {
            Assert.Less(processingTimeMillis, threshold);
        }
    }
}
