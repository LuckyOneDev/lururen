﻿namespace Lururen.Testing
{
    // Tests need to run seqeuntially because test server holds port 7777
    // Also could be rewritten to use different ports for each instance
    public class SocketTests
    {
        private class TestEntity : ServerEntity
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

            public override void Update(double deltaTime)
            {
            }
        }

        private class TestCommand : IRunnableCommand
        {
            public void Run(Guid client, Application app)
            {
                app.MessageBridge.SendData(client, "Test Data");
            }
        }

        private class MultiClientTestCommand : IRunnableCommand
        {
            public Guid requestId = Guid.NewGuid();

            public void Run(Guid client, Application app)
            {
                app.MessageBridge.SendData(client, "Test " + requestId);
            }
        }

        private class TestApp : Application
        {
            public ResourceInfo resourceInfo = new();

            public TestApp() : base(new SocketServerMessageBridge())
            {
            }

            public override void Init()
            {
            }

            public override void Dispose()
            {
            }

            public override ResourceInfo GetResourceInfo()
            {
                return resourceInfo;
            }

            public override Stream GetResource(string resourceName)
            {
                return new FileStream(resourceName, FileMode.Open);
            }
        }

        [Fact]
        public async Task MessageTransmitTest()
        {
            Application app = new TestApp();
            SocketClientMessageBridge netBus = new();
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

        [Theory(Timeout = 1000)]
        [InlineData(10)]
        [InlineData(5)]
        [InlineData(2)]
        public async Task MultiClientTest(int clientAmount)
        {
            Application app = new TestApp();
            IClientMessageBridge[] clients = new IClientMessageBridge[clientAmount];
            for (int i = 0; i < clientAmount; i++)
            {
                clients[i] = new SocketClientMessageBridge();
            }

            app.Start();

            for (int i = 0; i < clientAmount; i++)
            {
                await clients[i].Start();
            }

            Task[] tasks = new Task[clientAmount];

            for (int i = 0; i < clientAmount; i++)
            {
                TaskCompletionSource taskCompletionSource = new();
                tasks[i] = taskCompletionSource.Task;

                MultiClientTestCommand cmd = new();

                clients[i].OnData += (object data) =>
                {
                    Assert.Equal("Test " + cmd.requestId, data);
                    taskCompletionSource.SetResult();
                };

                _ = clients[i].SendCommand(cmd);
            }
        }

        [Fact]
        public async Task UnexpectedDisconnectTest()
        {
            Application app = new TestApp();
            SocketClientMessageBridge netBus = new();

            app.Start();

            await netBus.Start();

            await netBus.SendCommand(new TestCommand());

            netBus.Dispose();
            Task.Delay(10).Wait();

            Assert.Empty(((SocketServerMessageBridge)app.MessageBridge).GetClients());
        }

        [Theory(Timeout = 1000)]
        [InlineData(10000)]
        public async Task FileTransmissionTest(int fileSizeBytes)
        {
            Random rand = new();
            string fileName = $"test-file-{fileSizeBytes}.txt";
            byte[] testData = new byte[fileSizeBytes];
            rand.NextBytes(testData);
            File.WriteAllBytes(fileName, testData);

            TestApp app = new();

            SocketClientMessageBridge netBus = new();

            app.resourceInfo = new ResourceInfo();
            app.resourceInfo.Add(fileName, ProtocolHelper.GetChecksum(testData));

            app.Start();

            await netBus.Start();
            await netBus.SendCommand(new RequestResourceInfoCommand());

            TaskCompletionSource taskCompletionSource = new();
            netBus.OnTransmissionEnd += (transmission) =>
            {
                if (transmission is FileTransmissionMessage ft)
                {
                    byte[] transferredBytes = File.ReadAllBytes(Path.Combine("ClientData", ft.FileName));
                    Assert.Equal(transferredBytes, testData);
                    taskCompletionSource.SetResult();
                }
            };
        }
    }
}