namespace Lururen.Common.Networking.Messages
{
    public enum ServerState
    {
        OK,
        CLOSED
    }

    public class ServerStatusMessage
    {
        public ServerStatusMessage(ServerState state = ServerState.OK, string? additionalInfo = default)
        {
            State = state;
            AdditionalInfo = additionalInfo;
        }
        public ServerState State { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
