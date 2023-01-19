using Lururen.Core.App;
using Lururen.Core.CommandSystem;
using Lururen.Networking.Common.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Networking.Common
{
    public abstract class ProtocolNetBus : INetBus
    {
        protected virtual void DataRecived(object data)
        {
            OnData?.Invoke(data);
        }

        protected virtual void InvokeOnTransmissionEnd(ITransmission transmission)
        {
            OnTransmissionEnd?.Invoke(transmission);
        }


        #region INetBus
        public event OnDataEventHandler OnData;
        public event OnTransmissionEndEventHandler OnTransmissionEnd;
        public abstract Task SendCommand(ICommand message);
        public abstract Task Start();
        public abstract Task Stop();
        public abstract void Dispose();
        #endregion INetBus

        public ProtocolNetBus(string cacheFolder)
        {
            this.CacheFolder = cacheFolder;
            Directory.CreateDirectory(CacheFolder);
        }

        public string BuildFilePath(string fileName) => Path.Combine(CacheFolder, fileName);
        public string CacheFolder = "ClientCache";

        

        protected Action<ArraySegment<byte>> ContiniousTransmissionHandler { get; set; } = null;
        public void ProcessSystemRequest(object data)
        {
            if (ContiniousTransmissionHandler != null && data is ArraySegment<byte> bytes)
            {
                ContiniousTransmissionHandler.Invoke(bytes);
                return;
            }

            switch (data)
            {
                case ServerStatus serverStatus: HandleServerStatus(serverStatus); break;
                case FileTransmission fileTransmission: HandleFileTrasmission(fileTransmission); break;
                case ResourceInfo resourceInfo: HandleResouceInfo(resourceInfo); break;
            }
            OnData?.Invoke(data);
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
                    bool identical = ComputeChecksum(localBytes) == fileCheckSum;
                    if (identical) 
                    {
                        return;
                    }
                    File.Delete(BuildFilePath(fileName));
                }

                _ = SendCommand(new RequestResourceCommand(fileName));
            });
        }

        public byte ComputeChecksum(byte[] data)
        {
            byte sum = 0;
            // Let overflow occur without exceptions
            unchecked
            {
                foreach (byte b in data)
                {
                    sum += b;
                }
            }
            return sum;
        }

        protected void HandleFileTrasmission(FileTransmission transmission)
        {
            FileStream stream = new FileStream(BuildFilePath(transmission.FileName), FileMode.OpenOrCreate);
            int bytesRecived = 0;
            ContiniousTransmissionHandler = (bytes) =>
            {
                stream.Write(bytes);
                bytesRecived += bytes.Count;
                if (bytesRecived >= transmission.SizeBytes)
                {
                    ContiniousTransmissionHandler = null;
                    stream.Close();
                    OnTransmissionEnd?.Invoke(transmission);
                }
            };
        }

        protected void HandleServerStatus(ServerStatus serverStatus)
        {
            if (serverStatus.State == ServerState.CLOSED)
            {
                throw new Exception($"Server closed. {serverStatus.AdditionalInfo}");
            }
        }
    }
}
