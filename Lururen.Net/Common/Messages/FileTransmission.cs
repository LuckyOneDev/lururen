using Lururen.Core.Networking;

namespace Lururen.Networking.Common.Messages
{
    public class FileTransmission : ITransmission
    {
        public string FileName { get; }
        public long SizeBytes { get; }
        public FileTransmission(string fileName, long sizeBytes)
        {
            FileName = fileName;
            SizeBytes = sizeBytes;
        }
    }
}