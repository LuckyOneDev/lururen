using Lururen.Server.Core.CommandSystem;

namespace Lururen.Server.Core.Networking
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