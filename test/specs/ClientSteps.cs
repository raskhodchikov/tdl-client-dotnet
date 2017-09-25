using System.Linq;
using NUnit.Framework;
using TDL.Client;
using TDL.Client.Audit;
using TDL.Test.Specs.SpecItems;
using TDL.Test.Specs.Utils.Jmx.Broker;
using TDL.Test.Specs.Utils.Logging;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace TDL.Test.Specs
{
    [Binding]
    public class ClientSteps
    {
        private const string Hostname = "localhost";
        private const int Port = 21616;
        private const string UniqueId = "testuser@example.com";

        private readonly LogAuditStream auditStream = new LogAuditStream(new ConsoleAuditStream());
        private readonly RemoteJmxBroker broker = TestBroker.Instance;

        private RemoteJmxQueue requestQueue;
        private RemoteJmxQueue responseQueue;
        private TdlClient client;

        private long requestCount;

        [Given(@"I start with a clean broker")]
        public void GivenIStartWithACleanBroker()
        {
            requestQueue = broker.AddQueue($"{UniqueId}.req");
            requestQueue.Purge();

            responseQueue = broker.AddQueue($"{UniqueId}.resp");
            responseQueue.Purge();

            client = TdlClient.Build()
                .SetHostname(Hostname)
                .SetPort(Port)
                .SetUniqueId(UniqueId)
                .SetTimeToWaitForRequests(5)
                .SetAuditStream(auditStream)
                .Create();
        }

        [Given(@"I receive the following requests:")]
        public void GivenIReceiveTheFollowingRequests(Table table)
        {
            var requests = table.CreateSet<PayloadSpecItem>().Select(i => i.Payload).ToList();

            requests.ForEach(requestQueue.SendTextMessage);
            requestCount = requests.Count;
        }

        [Given(@"the broker is not available")]
        public void GivenTheBrokerIsNotAvailable()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I go live with the following processing rules:")]
        public void WhenIGoLiveWithTheFollowingProcessingRules(Table table)
        {
            var processingRuleSpecItems = table.CreateSet<ProcessingRuleSpecItem>().ToList();

            var processingRules = new ProcessingRules();
            processingRuleSpecItems.ForEach(ruleSpec =>
                processingRules
                    .On(ruleSpec.Method)
                    .Call(CallImplementationFactory.Get(ruleSpec.Call))
                    .Then(ClientActionsFactory.Get(ruleSpec.Action)));

            client.GoLiveWith(processingRules);
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
    }
}
