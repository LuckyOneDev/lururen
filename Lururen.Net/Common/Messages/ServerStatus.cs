namespace Lururen.Networking.Common.Messages
{
    public enum ServerState
    {
        OK,
        CLOSED
    }

    public class ServerStatus
    {
        public ServerStatus(ServerState state = ServerState.OK, string? additionalInfo = default)
        {
            State = state;
            AdditionalInfo = additionalInfo;
        }
        public ServerState State { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
