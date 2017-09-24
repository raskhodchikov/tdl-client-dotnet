namespace TDL.Client.Actions
{
    public static class ClientActions
    {
        public static IClientAction Publish { get; } = new PublishAction();
    }
}