using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using TDL.Client.Audit;
using TDL.Client.Queue;
using TDL.Client.Runner;
using TDL.Client.Utils;
using TDL.Test.Specs.Queue.Runners;
using TDL.Test.Specs.Utils.Logging;
using TDL.Test.Specs.Utils.Extensions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace TDL.Test.Specs.Runner
{
    [Binding]
    public class RunnerSteps
    {
        private WiremockProcess challengeServerStub;
        private WiremockProcess recordingServerStub;
        private string challengeHostname;
        private int port;
        private readonly IAuditStream auditStream = new TestAuditStream();
        private IImplementationRunner implementationRunner = new QuietImplementationRunner();
        private string implementationRunnerMessage;
        private string journeyId;
        private Func<string> actionProviderCallback = () => null;

        [Given(@"There is a challenge server running on ""(.*)"" port (.*)")]
        public void GivenThereIsAChallengeServerRunningOnPort(string hostname, int port)
        {
            this.challengeHostname = hostname;
            this.port = port;

            challengeServerStub = new WiremockProcess(hostname, port);
            challengeServerStub.Reset();
        }

        [Given(@"journeyId is ""(.*)""")]
        public void GivenJourneyIdIs(string journeyId)
        {
            this.journeyId = journeyId;
        }

        [Given(@"the challenge server exposes the following endpoints")]
        public void GivenTheChallengeServerExposesTheFollowingEndpoints(Table table)
        {
            table.CreateSet<ServerConfig>()
                .ToList()
                .ForEach(config => challengeServerStub.CreateNewMapping(config));
        }

        [Given(@"There is a recording server running on ""(.*)"" port (.*)")]
        public void GivenThereIsARecordingServerRunningOnPort(string hostname, int port)
        {
            recordingServerStub = new WiremockProcess(hostname, port);
            recordingServerStub.Reset();
        }

        [Given(@"the recording server exposes the following endpoints")]
        public void GivenTheRecordingServerExposesTheFollowingEndpoints(Table table)
        {
            table.CreateSet<ServerConfig>()
                .ToList()
                .ForEach(config => recordingServerStub.CreateNewMapping(config));
        }

        [Given(@"the action input comes from a provider returning ""(.*)""")]
        public void GivenTheActionInputComesFromAProviderReturning(string s)
        {
            actionProviderCallback = () => s;
        }

        [Given(@"the challenges folder is empty")]
        public void GivenTheChallengesFolderIsEmpty()
        {
            var challengesPath = Path.Combine(PathHelper.RepositoryPath, "challenges");
            if (Directory.Exists(challengesPath))
            {
                var challengesDirectory = new DirectoryInfo(challengesPath);
                challengesDirectory.Empty();
            }
        }

        [Given(@"there is an implementation runner that prints ""(.*)""")]
        public void GivenThereIsAnImplementationRunnerThatPrints(string s)
        {
            implementationRunnerMessage = s;
            implementationRunner = new NoisyImplementationRunner(implementationRunnerMessage, auditStream);
        }

        [Given(@"recording server is returning error")]
        public void GivenRecordingServerIsReturningError()
        {
            recordingServerStub.Reset();
        }

        [Given(@"the challenge server returns (.*), response body ""(.*)"" for all requests")]
        public void GivenTheChallengeServerReturnsResponseBodyForAllRequests(int returnCode, string body)
        {
            var config = new ServerConfig
            {
                EndpointMatches = "^(.*)",
                Status = returnCode,
                Verb = "ANY",
                ResponseBody = body
            };
            challengeServerStub.CreateNewMapping(config);
        }

        [Given(@"the challenge server returns (.*) for all requests")]
        public void GivenTheChallengeServerReturnsForAllRequests(int returnCode)
        {
            var config = new ServerConfig
            {
                EndpointMatches = "^(.*)",
                Status = returnCode,
                Verb = "ANY"
            };
            challengeServerStub.CreateNewMapping(config);
        }

        [When(@"user starts client")]
        public void WhenUserStartsClient()
        {
            var config = ChallengeSessionConfig.ForJourneyId(journeyId)
                .WithServerHostname(challengeHostname)
                .WithPort(port)
                .WithColours(true)
                .WithAuditStream(auditStream)
                .WithRecordingSystemShouldBeOn(true);

            ChallengeSession.ForRunner(implementationRunner)
                    .WithConfig(config)
                    .WithActionProvider(actionProviderCallback)
                    .Start();
        }

        [Then(@"the server interaction should look like:")]
        public void ThenTheServerInteractionShouldLookLike(string expectedOutput)
        {
            var total = auditStream.ToDisplayableString();
            Assert.IsTrue(total.Contains(expectedOutput), "Expected string is not contained in output");
        }

        [Then(@"the file ""(.*)"" should contain")]
        public void ThenTheFileShouldContain(string file, string text)
        {
            var fileContent = File.ReadAllText(file);
            Assert.AreEqual(text, fileContent, "Contents of the file is not what is expected");
        }

        [Then(@"the recording system should be notified with ""(.*)""")]
        public void ThenTheRecordingSystemShouldBeNotifiedWith(string expectedOutput)
        {
            recordingServerStub.VerifyEndpointWasHit("/notify", "POST", expectedOutput);
        }

        [Then(@"the implementation runner should be run with the provided implementations")]
        public void ThenTheImplementationRunnerShouldBeRunWithTheProvidedImplementations()
        {
            var total = auditStream.ToDisplayableString();
            Assert.IsTrue(total.Contains(implementationRunnerMessage));
        }

        [Then(@"the server interaction should contain the following lines:")]
        public void ThenTheServerInteractionShouldContainTheFollowingLines(string expectedOutput)
        {
            var total = auditStream.ToDisplayableString();
            var lines = expectedOutput.Split('\n');
            foreach (var line in lines)
            {
                Assert.IsTrue(total.Contains(line), "Expected string is not contained in output");
            }
        }

        [Then(@"the client should not ask the user for input")]
        public void ThenTheClientShouldNotAskTheUserForInput()
        {
            var total = auditStream.ToDisplayableString();
            Assert.IsFalse(total.Contains("Selected action is:"));
        }
    }
}
