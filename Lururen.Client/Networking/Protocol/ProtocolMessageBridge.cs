using Lururen.Common.CommandSystem;
using Lururen.Common.Networking.Messages;
using Lururen.Common.Types;

namespace Lururen.Client.Networking.Protocol
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

        public ProtocolMessagingMode ProtocolMessagingMode { get; private set; } = ProtocolMessagingMode.Default;
        public ProtocolMessageBridge(string cacheFolder)
        {
            CacheFolder = cacheFolder;
            Directory.CreateDirectory(CacheFolder);
        }

        public string CacheFolder { get; protected set; }
        protected Action<ArraySegment<byte>>? ContiniousTransmissionHandler { get; set; } = null;

        protected void ProcessMessage(object data)
        {
            switch (ProtocolMessagingMode)
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
                        case ServerStatusMessage serverStatus: HandleServerStatus(serverStatus); break;
                        case FileTransmissionMessage fileTransmission: HandleFileTrasmission(fileTransmission); break;
                        case ResourceInfo resourceInfo: HandleResouceInfo(resourceInfo); break;
                    }
                    OnData?.Invoke(data);
                    break;
            }
        }

        protected string BuildFilePath(string fileName) => Path.Combine(CacheFolder, fileName);
        protected void StartStreamMessaging(Action<ArraySegment<byte>> action)
        {
            ProtocolMessagingMode = ProtocolMessagingMode.Stream;
            ContiniousTransmissionHandler = action;
        }

        protected void StopStreamMessaging()
        {
            ProtocolMessagingMode = ProtocolMessagingMode.Default;
            ContiniousTransmissionHandler = null;
        }
        #region Request Handlers

        protected void HandleFileTrasmission(FileTransmissionMessage transmission)
        {
            FileStream stream = new(BuildFilePath(transmission.FileName), FileMode.OpenOrCreate);
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

                _ = SendCommand(new RequestResourceCommandDTO(fileName));
            });
        }

        protected void HandleServerStatus(ServerStatusMessage serverStatus)
        {
            if (serverStatus.State == ServerState.CLOSED)
            {
                throw new Exception($"Server closed. {serverStatus.AdditionalInfo}");
            }
        }

        #endregion
    }
}