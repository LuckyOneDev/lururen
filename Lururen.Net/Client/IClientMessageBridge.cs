namespace Lururen.Net.Client
{
    public delegate void OnDataEventHandler(object data);

    public interface IClient : IDisposable
    {
        Task Start();
        Task Stop();
    }
}
