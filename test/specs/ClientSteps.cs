using TechTalk.SpecFlow;

namespace TDL.Test
{
	[Binding]
	public class ClientSteps
	{
		[Given( @"I start with a clean broker" )]
		public void GivenIStartWithACleanBroker()
		{
			ScenarioContext.Current.Pending();
		}

		[Given( @"I receive the following requests:" )]
		public void GivenIReceiveTheFollowingRequests( Table table )
		{
			ScenarioContext.Current.Pending();
		}

		[Given( @"the broker is not available" )]
		public void GivenTheBrokerIsNotAvailable()
		{
			ScenarioContext.Current.Pending();
		}

		[When( @"I go live with the following processing rules:" )]
		public void WhenIGoLiveWithTheFollowingProcessingRules( Table table )
		{
			ScenarioContext.Current.Pending();
		}

		[Then( @"the client should consume all requests" )]
		public void ThenTheClientShouldConsumeAllRequests()
		{
			ScenarioContext.Current.Pending();
		}

		[Then( @"the client should publish the following responses:" )]
		public void ThenTheClientShouldPublishTheFollowingResponses( Table table )
		{
			ScenarioContext.Current.Pending();
		}

		[Then( @"the client should not consume any request" )]
		public void ThenTheClientShouldNotConsumeAnyRequest()
		{
			ScenarioContext.Current.Pending();
		}

		[Then( @"the client should not publish any response" )]
		public void ThenTheClientShouldNotPublishAnyResponse()
		{
			ScenarioContext.Current.Pending();
		}

		[Then( @"the client should consume first request" )]
		public void ThenTheClientShouldConsumeFirstRequest()
		{
			ScenarioContext.Current.Pending();
		}

		[Then( @"the client should display to console:" )]
		public void ThenTheClientShouldDisplayToConsole( Table table )
		{
			ScenarioContext.Current.Pending();
		}

		[Then( @"I should get no exception" )]
		public void ThenIShouldGetNoException()
		{
			ScenarioContext.Current.Pending();
		}
	}
}
