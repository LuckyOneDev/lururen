namespace Lururen.Net.Server
{
    public interface IServer : IDisposable
    {


        public Task SendData(Guid clientGuid, object data);

        Task Start();

        Task Stop();
    }
}