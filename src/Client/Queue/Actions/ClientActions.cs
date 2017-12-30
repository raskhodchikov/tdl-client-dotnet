namespace TDL.Client.Queue.Actions
{
    public static class ClientActions
    {
        public static IClientAction Publish { get; } = new PublishAction();

        public static IClientAction Stop { get; } = new StopAction();

        public static IClientAction PublishAndStop { get; } = new PublishAndStopAction();
    }
}
