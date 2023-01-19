using Lururen.Core.EntitySystem;
using Lururen.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Lururen.Core.CommandSystem;
using Lururen.Networking.Common.Commands;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Lururen.Networking.Common.ServerMessages;

namespace Lururen.Networking.SimpleSocketBus
{
    public class SocketDataBus : IDataBus
    {
        public Dictionary<Guid, Socket> Clients { get; set; } = new();
        public SocketDataBus(int port = 7777)
        {
            this.Port = port;
        }
        public int Port { get; private set; }

        public event OnCommandEventHandler OnCommand;
        CancellationTokenSource CancellationTokenSource { get; set; }
        public async Task Start()
        {
            CancellationTokenSource = new();
            IPEndPoint ipPoint = new(IPAddress.Any, Port);
            using Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(1000);

            while (!CancellationTokenSource.IsCancellationRequested)
            {
                var handler = await socket.AcceptAsync(CancellationTokenSource.Token);
                _ = StartSession(handler);
            }
        }

        async Task StartSession(Socket handler)
        {
            if (handler is not null)
            {
                Guid guid = Guid.NewGuid();
                Clients.Add(guid, handler);
                ICommand? command = null;
                try
                {
                    while (command is not DisconnectCommand)
                    {
                        command = await SocketHelper.Recieve<ICommand>(handler, CancellationTokenSource.Token);
                        OnCommand.Invoke(guid, command);
                    }
                    Clients.Remove(guid);
                    handler.Close();
                }
                catch (Exception ex)
                {
                    // Add logging 
                    Clients.Remove(guid);
                    handler.Close();
                }
            }
        }

        public async Task Stop()
        {
            foreach (var entry in Clients)
            {
                await SocketHelper.Send(entry.Value, new ServerStatus(ServerState.CLOSED));
            }
            Parallel.ForEach(Clients, x => x.Value.Close());
            Clients.Clear();
        }

        public void Dispose()
        {
        }

        public async Task SendData(Guid clientGuid, object data)
        {
            await SocketHelper.Send(Clients[clientGuid] ,data);
        }

        public async Task SendContiniousData(Guid clientGuid, Stream resourceStream)
        {
            await SocketHelper.SendContiniousData(Clients[clientGuid], resourceStream);
        }
    }
}
