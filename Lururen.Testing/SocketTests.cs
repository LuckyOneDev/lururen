using Lururen.Networking.Common;
using Lururen.Networking.SimpleSocketBus;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Lururen.Testing
{
    // Tests need to run seqeuntially because test server holds port 7777
    // Also could be rewritten to use different ports for each instance
    public class SocketTests
    {
        private class TestEntity : Entity
        {
            public TestEntity(string Data)
            {
                TestData = Data;
            }

            public string TestData { get; set; }

            public override void Dispose()
            {
            }

            public override void Init()
            {
            }

            public override void OnEvent(EventArgs args)
            {
            }

            public override void Update()
            {
            }
        }
        private class TestCommand : ICommand
        {
            public void Run(Guid client, Application app)
            {
                app.DataBus.SendData(client, "Test Data");
            }
        }

        private class MultiClientTestCommand : ICommand
        {
            public Guid requestId = Guid.NewGuid();
            public void Run(Guid client, Application app)
            {
                app.DataBus.SendData(client, "Test " + requestId);
            }
        }

        private class TestApp : Application
        {
            public override void Init()
            {
                this.DataBus = new SocketDataBus();
            }

            public override void Dispose()
            {
            }
        }

        [Fact]
        public async Task MessageTransmitTest()
        {
            var app = new TestApp();
            var netBus = new SocketNetBus();
            app.Start();

            await netBus.Start();

            await netBus.SendCommand(new TestCommand());

            netBus.OnData += async (object Data) =>
            {
                await netBus.Stop();
                app.Stop();

                Assert.Equal("Test Data", Data);
            };
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task MultiClientTest(int clientAmount)
        {
            var app = new TestApp();
            var clients = new INetBus[clientAmount];
            for (int i = 0; i < clientAmount; i++)
            {
                clients[i] = new SocketNetBus();
            }

            app.Start();

            for (int i = 0; i < clientAmount; i++)
            {
                await clients[i].Start();
            }

            for (int i = 0; i < clientAmount; i++)
            {
                var cmd = new MultiClientTestCommand();

                clients[i].OnData += (object data) =>
                {
                    Assert.Equal("Test " + cmd.requestId, data);
                };

                await clients[i].SendCommand(cmd);
            }
        }

        [Fact]
        public async Task UnexpectedDisconnectTest()
        {
            var app = new TestApp();
            var netBus = new SocketNetBus();

            app.Start();

            await netBus.Start();

            await netBus.SendCommand(new TestCommand());

            netBus.Dispose();
            Task.Delay(10).Wait();

            Assert.Empty(((SocketDataBus)app.DataBus).Clients);
        }
    }
}