using Lururen.Networking.Common;
using Lururen.Networking.Common.Commands;
using Lururen.Networking.SimpleSocketBus;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            public ResourceInfo resourceInfo = new();
            public override void Init()
            {
                this.DataBus = new SocketDataBus();
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
        [InlineData(10)]
        [InlineData(5)]
        [InlineData(2)]
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

            Task[] tasks = new Task[clientAmount];

            for (int i = 0; i < clientAmount; i++)
            {
                var taskCompletionSource = new TaskCompletionSource();
                tasks[i] = taskCompletionSource.Task;

                var cmd = new MultiClientTestCommand();

                clients[i].OnData += (object data) =>
                {
                    Assert.Equal("Test " + cmd.requestId, data);
                    taskCompletionSource.SetResult();
                };

                _ = clients[i].SendCommand(cmd);
            }

            bool completed = Task.WaitAll(tasks, clientAmount * 50);
            Assert.True(completed, $"Test took too long to complete. Processed {tasks.Count(x => x.IsCompleted)} requests.");
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

        [Theory]
        [InlineData(650000)]
        [InlineData(65000)]
        [InlineData(5000)]
        [InlineData(1024)]
        public async Task FileTransmissionTest(int fileSizeBytes)
        {
            var rand = new Random();
            string fileName = $"test-file.txt";
            byte[] testData = new byte[fileSizeBytes];
            rand.NextBytes(testData);
            File.WriteAllBytes(fileName, testData);

            var app = new TestApp();

            var netBus = new SocketNetBus();

            app.resourceInfo = new ResourceInfo();
            app.resourceInfo.Add(fileName, netBus.ComputeChecksum(testData));

            app.Start();

            await netBus.Start();
            await netBus.SendCommand(new RequestResourceInfoCommand());

            var taskCompletionSource = new TaskCompletionSource();
            netBus.OnTransmissionEnd += (transmission) =>
            {
                if (transmission is FileTransmission ft)
                {
                    var transferredBytes = File.ReadAllBytes(netBus.BuildFilePath(ft.FileName));
                    try
                    {
                        Assert.Equal(transferredBytes, testData);
                        taskCompletionSource.SetResult();
                    } catch (Exception ex)
                    {
                        taskCompletionSource.SetException(ex);
                    }

                }
            };

            bool completed = taskCompletionSource.Task.Wait(fileSizeBytes);
            Assert.True(completed, "Test took too long to complete");
        }

    }
}