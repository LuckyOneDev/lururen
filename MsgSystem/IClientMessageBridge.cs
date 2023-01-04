namespace lururen
{
    internal interface IClientMessageBridge
    {
        void SendMessage(IGameClient client, string message);
        void AddClient(IGameClient client);
        void RemoveClient(IGameClient client);
        void OnMessage();
    }
}