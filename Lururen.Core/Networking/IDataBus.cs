using Lururen.Core.CommandSystem;

namespace Lururen.Networking.Common
{
    public delegate void OnCommandEventHandler(Guid clientGuid, ICommand command);

    public interface IDataBus : IDisposable
    {
        public event OnCommandEventHandler OnCommand;

        bool Running { get; }

        public Task SendData(Guid clientGuid, object data);

        Task Start();

        Task Stop();
    }
}