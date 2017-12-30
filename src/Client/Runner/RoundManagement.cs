using System;
using System.IO;
using System.Linq;
using System.Text;
using TDL.Client.Audit;
using TDL.Client.Utils;

namespace TDL.Client.Runner
{
    internal static class RoundManagement
    {
        private static readonly string ChallengesPath;
        private static readonly string LastFetchedRoundPath;

        static RoundManagement()
        {
            ChallengesPath = Path.Combine(PathHelper.RepositoryPath, "challenges");
            LastFetchedRoundPath = Path.Combine(ChallengesPath, "XR.txt");
        }

        public static void SaveDescription(IRoundChangesListener listener, string rawDescription, IAuditStream auditStream)
        {
            // DEBT - the first line of the response is the ID for the round, the rest of the responseMessage is the description
            var newlineIndex = rawDescription.IndexOf('\n');
            if (newlineIndex <= 0) return;

            var roundId = rawDescription.Substring(0, newlineIndex);
            var lastFetchedRound = GetLastFetchedRound();
            if (!roundId.Equals(lastFetchedRound))
            {
                listener.OnNewRound(roundId, RunnerAction.GetNewRoundDescription.ShortName);
            }
            SaveDescription(roundId, rawDescription, auditStream);
        }

        public static string SaveDescription(string label, string description, IAuditStream auditStream)
        {
            // Save description.
            var descriptionPath = Path.Combine(ChallengesPath, $"{label}.txt");

            File.WriteAllText(descriptionPath, description.Replace("\n", Environment.NewLine));
            var relativePath = descriptionPath.Replace(PathHelper.RepositoryPath + Path.DirectorySeparatorChar, "");
            auditStream.WriteLine($"Challenge description saved to file: {relativePath}.");

            // Save round label.
            File.WriteAllText(LastFetchedRoundPath, label);

            return "OK";
        }

        public static string GetLastFetchedRound() =>
            File.Exists(LastFetchedRoundPath)
                ? File.ReadLines(LastFetchedRoundPath, Encoding.Default).FirstOrDefault()
                : "noRound";
    }
}
