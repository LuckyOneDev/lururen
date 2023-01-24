namespace Lururen.Common.Networking.Messages
{
    public class FileTransmissionMessage : ITransmission
    {
        public string FileName { get; }
        public long SizeBytes { get; }
        public FileTransmissionMessage(string fileName, long sizeBytes)
        {
            FileName = fileName;
            SizeBytes = sizeBytes;
        }
    }
}