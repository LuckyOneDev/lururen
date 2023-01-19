using Lururen.Core.CommandSystem;
using Lururen.Core.EntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Networking.Common
{
    public delegate void OnDataEventHandler(object data);
    public delegate void OnTransmissionEndEventHandler(ITransmission transmission);
    public interface INetBus : IDisposable
    {
        public event OnDataEventHandler OnData;
        public event OnTransmissionEndEventHandler OnTransmissionEnd;
        public Task SendCommand(ICommand message);
        Task Start();
        Task Stop();
    }
}
