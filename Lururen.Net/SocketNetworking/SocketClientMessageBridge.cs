﻿using Lururen.Core.CommandSystem;
using Lururen.Networking.Common.Commands;
using Lururen.Networking.Common.Protocol;
using System.Net.Sockets;

namespace Lururen.Networking.SocketNetworking
{
    public class SocketClientMessageBridge : ProtocolMessageBridge
    {
        public SocketClientMessageBridge(string host = "127.0.0.1", int port = 7777) : base("ClientData")
        {
            Host = host;
            Port = port;
        }

        #region IClientMessageBridge

        public override void Dispose()
        {
            Socket?.Dispose();
        }

        public override async Task SendCommand(ICommand command)
        {
            if (Socket is not null)
            {
                await SocketHelper.Send(Socket, command);
            }
            else
            {
                throw new Exception("Socket was not initialized");
            }
        }

        public override async Task Start()
        {
            CancellationTokenSource = new();
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(Host, Port);
            Socket = socket;
            _ = StartRecieveData();
        }

        public override async Task Stop()
        {
            CancellationTokenSource.Cancel();
            await SocketHelper.Send(Socket, new DisconnectCommand());
            await Socket.DisconnectAsync(true);
        }

        #endregion INetBus

        public string Host { get; protected set; }
        public int Port { get; protected set; }
        private CancellationTokenSource? CancellationTokenSource { get; set; }
        private Socket? Socket { get; set; }

        private async Task StartRecieveData()
        {
            while (!CancellationTokenSource.Token.IsCancellationRequested)
            {
                switch (protocolMessagingMode)
                {
                    case ProtocolMessagingMode.Default:
                        await SocketHelper.Recieve<object>(Socket, CancellationTokenSource.Token).ContinueWith((task) =>
                        {
                            var data = task.Result;
                            ProcessMessage(data);
                        }, CancellationTokenSource.Token);
                        break;

                    case ProtocolMessagingMode.Stream:
                        await SocketHelper.RecieveBytes(Socket, CancellationTokenSource.Token).ContinueWith((task) =>
                        {
                            var data = task.Result;
                            ProcessMessage(data);
                        }, CancellationTokenSource.Token);
                        break;
                }
            }
        }
    }
}