using Lururen.Core.App;
using Lururen.Core.CommandSystem;
using Lururen.Networking.Common.Commands;
using Lururen.Networking.Common.ServerMessages;

namespace Lururen.Networking.Common.Protocol
{
    public enum ProtocolMessagingMode
    {
        Default,
        Stream
    }

    public abstract class ProtocolMessageBridge : IClientMessageBridge
    {
        #region IClientMessageBridge

        public event OnDataEventHandler? OnData;

        public event OnTransmissionEndEventHandler? OnTransmissionEnd;

        public abstract void Dispose();

        public abstract Task SendCommand(ICommand message);

        public abstract Task Start();

        public abstract Task Stop();

        #endregion IClientMessageBridge

        public ProtocolMessagingMode protocolMessagingMode { get; private set; } = ProtocolMessagingMode.Default;
        public ProtocolMessageBridge(string cacheFolder)
        {
            CacheFolder = cacheFolder;
            Directory.CreateDirectory(CacheFolder);
        }

        public string CacheFolder { get; protected set; }
        protected Action<ArraySegment<byte>>? ContiniousTransmissionHandler { get; set; } = null;

        protected void ProcessMessage(object data)
        {
            switch (protocolMessagingMode)
            {
                case ProtocolMessagingMode.Stream:
                    if (data is ArraySegment<byte> bytes)
                    {
                        ContiniousTransmissionHandler?.Invoke(bytes);
                    }
                    break;

                case ProtocolMessagingMode.Default:
                    switch (data)
                    {
                        case ServerStatus serverStatus: HandleServerStatus(serverStatus); break;
                        case FileTransmission fileTransmission: HandleFileTrasmission(fileTransmission); break;
                        case ResourceInfo resourceInfo: HandleResouceInfo(resourceInfo); break;
                    }
                    OnData?.Invoke(data);
                    break;
            }
        }

        protected string BuildFilePath(string fileName) => Path.Combine(CacheFolder, fileName);
        protected void StartStreamMessaging(Action<ArraySegment<byte>> action)
        {
            protocolMessagingMode = ProtocolMessagingMode.Stream;
            ContiniousTransmissionHandler = action;
        }

        protected void StopStreamMessaging()
        {
            protocolMessagingMode = ProtocolMessagingMode.Default;
            ContiniousTransmissionHandler = null;
        }
        #region Request Handlers

        protected void HandleFileTrasmission(FileTransmission transmission)
        {
            FileStream stream = new FileStream(BuildFilePath(transmission.FileName), FileMode.OpenOrCreate);
            int bytesRecived = 0;
            StartStreamMessaging((bytes) =>
            {
                stream.Write(bytes);
                bytesRecived += bytes.Count;
                if (bytesRecived >= transmission.SizeBytes)
                {
                    StopStreamMessaging();
                    stream.Close();
                    OnTransmissionEnd?.Invoke(transmission);
                }
            });
        }

        protected void HandleResouceInfo(ResourceInfo resourceInfo)
        {
            resourceInfo.Resources.ForEach(resource =>
            {
                var fileName = resource.Item1;
                var fileCheckSum = resource.Item2;

                if (File.Exists(BuildFilePath(fileName)))
                {
                    byte[] localBytes = File.ReadAllBytes(BuildFilePath(fileName));
                    bool identical = ProtocolHelper.GetChecksum(localBytes) == fileCheckSum;
                    if (identical)
                    {
                        return;
                    }
                    File.Delete(BuildFilePath(fileName));
                }

                _ = SendCommand(new RequestResourceCommand(fileName));
            });
        }

        protected void HandleServerStatus(ServerStatus serverStatus)
        {
            if (serverStatus.State == ServerState.CLOSED)
            {
                throw new Exception($"Server closed. {serverStatus.AdditionalInfo}");
            }
        }

        #endregion Request Handlers
    }
}