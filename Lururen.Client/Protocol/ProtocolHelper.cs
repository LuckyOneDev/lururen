namespace Lururen.Client.Protocol
{
    public static class ProtocolHelper
    {
        public static byte GetChecksum(byte[] data)
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
    }
}
