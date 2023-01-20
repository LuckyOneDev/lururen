using Lururen.Core.CommandSystem;

namespace Lururen.Core.Networking
{
    public delegate void OnCommandEventHandler(Guid clientGuid, ICommand command);

    public interface IServerMessageBridge : IDisposable
    {
        public event OnCommandEventHandler OnCommand;

        public Task SendContiniousData(Guid client, Stream resourceStream);

        public Task SendData(Guid clientGuid, object data);

        Task Start();

        Task Stop();
    }
}