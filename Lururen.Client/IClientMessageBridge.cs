using Lururen.Common.CommandSystem;
using Lururen.Common.Networking;

namespace Lururen.Client.Core
{
    public delegate void OnDataEventHandler(object data);
    public delegate void OnTransmissionEndEventHandler(ITransmission transmission);
    public interface IClientMessageBridge : IDisposable
    {
        public event OnDataEventHandler OnData;
        public event OnTransmissionEndEventHandler OnTransmissionEnd;
        public Task SendCommand(ICommand command);
        Task Start();
        Task Stop();
    }
}
