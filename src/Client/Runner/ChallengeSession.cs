using System;
using TDL.Client.Queue;

namespace TDL.Client.Runner
{
    public class ChallengeSession
    {
        private ChallengeSessionConfig config;
        private IImplementationRunner implementationRunner;
        private RecordingSystem recordingSystem;
        private ChallengeServerClient challengeServerClient;
        private Func<string> userInputCallback;

        public static ChallengeSession ForRunner(IImplementationRunner implementationRunner)
        {
            return new ChallengeSession(implementationRunner);
        }

        private ChallengeSession(IImplementationRunner runner)
        {
            implementationRunner = runner;
        }

        public ChallengeSession WithConfig(ChallengeSessionConfig config)
        {
            this.config = config;
            return this;
        }

        public ChallengeSession WithActionProvider(Func<string> callback)
        {
            userInputCallback = callback;
            return this;
        }

        /// <summary>
        /// The entry point.
        /// </summary>
        public void Start()
        {
            recordingSystem = new RecordingSystem(config.RecordingSystemShouldBeOn);
            var auditStream = config.AuditStream;

            if (!recordingSystem.IsRecordingSystemOk())
            {
                auditStream.WriteLine("Please run `record_screen_and_upload` before continuing.");
                return;
            }
            auditStream.WriteLine("Connecting to " + config.Hostname);
            RunApp();
        }

        private void RunApp()
        {
            var auditStream = config.AuditStream;
            challengeServerClient = new ChallengeServerClient(config.Hostname, config.Port, config.JourneyId, config.UseColours);

            try
            {
                bool shouldContinue = CheckStatusOfChallenge();
                if (shouldContinue)
                {
                    var userInput = userInputCallback();
                    auditStream.WriteLine("Selected action is: " + userInput);
                    var roundDescription = ExecuteUserAction(userInput);
                    RoundManagement.SaveDescription(recordingSystem, roundDescription, auditStream);
                }
            }
            catch (ServerErrorException)
            {
                auditStream.WriteLine("Server experienced an error. Try again in a few minutes.");
            }
            catch (OtherCommunicationException)
            {
                auditStream.WriteLine("Client threw an unexpected error. Try again.");
            }
            catch (ClientErrorException e)
            {
                // The client sent something the server didn't expect.
                auditStream.WriteLine(e.Message);
            }
        }

        private bool CheckStatusOfChallenge()
        {
            var auditStream = config.AuditStream;

            var journeyProgress = challengeServerClient.GetJourneyProgress();
            auditStream.WriteLine(journeyProgress);

            var availableActions = challengeServerClient.GetAvailableActions();
            auditStream.WriteLine(availableActions);

            return !availableActions.Contains("No actions available.");
        }

        private string ExecuteUserAction(String userInput)
        {
            if (userInput.Equals("deploy"))
            {
                implementationRunner.Run();
                var lastFetchedRound = RoundManagement.GetLastFetchedRound();
                recordingSystem.DeployNotifyEvent(lastFetchedRound);
            }
            return ExecuteAction(userInput);
        }

        private string ExecuteAction(String userInput)
        {
            var actionFeedback = challengeServerClient.SendAction(userInput);
            config.AuditStream.WriteLine(actionFeedback);
            return challengeServerClient.GetRoundDescription();
        }
    }
}
