﻿using NUnit.Framework;
using TDL.Test.Specs.Utils;
using TDL.Test.Specs.Utils.Jmx.Broker;
using TechTalk.SpecFlow;

namespace TDL.Test.Specs
{
    [Binding]
    public class ClientSteps
    {
        private const string Hostname = "localhost";
        private const int Port = 28161;
        private const string UniqueId = "testuser@example.com";

        private readonly RemoteJmxQueue requestQueue;
        private readonly RemoteJmxQueue responseQueue;

        private long requestCount;

        public ClientSteps()
        {
            requestQueue = TestBroker.Instance.AddQueue($"{UniqueId}.req");
            requestQueue.Purge();

            responseQueue = TestBroker.Instance.AddQueue($"{UniqueId}.resp");
            responseQueue.Purge();
        }

        [Given(@"I start with a clean broker")]
        public void GivenIStartWithACleanBroker()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I receive the following requests:")]
        public void GivenIReceiveTheFollowingRequests(Table messages)
        {
            foreach (var message in messages.ToStringsIncludingHeader())
            {
                requestQueue.SendTextMessage(message);
                requestCount++;
            }
        }

        [Given(@"the broker is not available")]
        public void GivenTheBrokerIsNotAvailable()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I go live with the following processing rules:")]
        public void WhenIGoLiveWithTheFollowingProcessingRules(Table table)
        {
            ScenarioContext.Current.Pending();
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
            ScenarioContext.Current.Pending();
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
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should get no exception")]
        public void ThenIShouldGetNoException()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
