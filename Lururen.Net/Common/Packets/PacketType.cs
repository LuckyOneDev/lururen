namespace Lururen.Net.Common.Packets
{
    public enum PacketType : byte
    {
        #region Initial Section
        // Client -> Server
        Handshake,

        // Server -> Client
        ServerInfo,
        # endregion Initial Section

        #region Login Section
        // Client -> Server
        LoginStart,

        // Server -> Client
        LoginSuccess,

        // Server -> Client
        LoginReject,

        // Server -> Client
        ConfigureChannel,

        // Client -> Server
        ConfigureChannelResponse,
        #endregion Login Section

        #region Resource Exchange Section
        // Client -> Server
        RequestAvailableResources,

        // Server -> Client
        ResponseAvailableResources,

        // Client -> Server
        RequestResource,

        // Server -> Client
        Resource,

        // Client -> Server
        ResourceExchangeEnd,
        #endregion Resource Exchange Section


        #region Main Section
        // Client -> Server
        Ready,

        // Server -> Client
        ReadyResponse,
        Disconnect
        #endregion Main Section
    }
}