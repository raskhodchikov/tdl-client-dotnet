namespace TDL.Client.Runner
{
    public interface IRoundChangesListener
    {
        void OnNewRound(string roundId, string shortName);
    }
}
