using System.Linq;
using NUnit.Framework;
using TDL.Test.Specs.SpecItems;
using TDL.Test.Specs.Utils.Jmx.Broker;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace TDL.Test.Specs
{
    [Binding]
    public class ClientSteps
    {
        private const string Hostname = "localhost";
        private const int Port = 28161;
        private const string UniqueId = "testuser@example.com";

        private RemoteJmxQueue requestQueue;
        private RemoteJmxQueue responseQueue;

        private long requestCount;

        [Given(@"I start with a clean broker")]
        public void GivenIStartWithACleanBroker()
        {
            requestQueue = TestBroker.Instance.AddQueue($"{UniqueId}.req");
            requestQueue.Purge();

            responseQueue = TestBroker.Instance.AddQueue($"{UniqueId}.resp");
            responseQueue.Purge();
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
        
        // TODO
        [When(@"I go live with the following processing rules:")]
        public void WhenIGoLiveWithTheFollowingProcessingRules(Table table)
        {
            var processingRuleSpecItems = table.CreateSet<ProcessingRuleSpecItem>();

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
            var expectedResponses = table.CreateSet<PayloadSpecItem>().Select(i => i.Payload);
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
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should get no exception")]
        public void ThenIShouldGetNoException()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
