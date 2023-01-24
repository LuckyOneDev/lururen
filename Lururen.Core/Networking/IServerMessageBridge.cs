using Lururen.Server.CommandSystem;

namespace Lururen.Server.Networking
{
    public delegate void OnCommandEventHandler(Guid clientGuid, IRunnableCommand command);

    public interface IServerMessageBridge : IDisposable
    {
        public event OnCommandEventHandler OnCommand;

        public Task SendContiniousData(Guid client, Stream resourceStream);

        public Task SendData(Guid clientGuid, object data);

        Task Start();

        Task Stop();
    }
}