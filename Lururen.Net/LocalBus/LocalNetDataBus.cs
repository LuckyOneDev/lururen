﻿using Lururen.Core.CommandSystem;
using Lururen.Core.EntitySystem;
using Lururen.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Networking.LocalBus
{
    public class LocalNetDataBus : INetBus, IDataBus
    {
        public bool Running { get; protected set; }

        public event OnDataEventHandler OnData;
        public event OnCommandEventHandler OnCommand;
        public event OnTransmissionEndEventHandler OnTransmissionEnd;

        public Task SendCommand(ICommand message)
        {
            OnCommand.Invoke(Guid.Empty, message); 
            return Task.CompletedTask;
        }

        public Task SendData(Guid clientGuid, object data)
        {
            OnData.Invoke(data);
            return Task.CompletedTask;
        }

        public Task Start()
        {
            Running = true;
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            Running = false;
            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }

        public Task SendContiniousData(Guid client, Stream resourceStream)
        {
            return SendData(client, resourceStream);
        }
    }
}
