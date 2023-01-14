﻿using Lururen.Networking.Common;
using Lururen.Networking.SimpleSocketBus;

namespace Lururen.Testing
{
    // Test need to run seqeuntially because test server holds port 7777
    // Also could be rewritten to use different ports for each instance
    public class SocketTests
    {
        class TestEntity : Entity
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
        class TestDataBus : SocketDataBus
        {
            public override async Task<IEnumerable<Entity>> OnMessage(IMessage command)
            {
                if (command is TestMessage)
                {
                    List<Entity> result = new List<Entity>() { new TestEntity("TestData") };
                    return result;
                } else
                {
                    return new List<Entity>();
                }
                
            }
        }

        class TestMultiDataBus : SocketDataBus
        {
            static int counter = 0;
            public override async Task<IEnumerable<Entity>> OnMessage(IMessage command)
            {
                if (command is TestMessage)
                {
                    List<Entity> result = new List<Entity>() { new TestEntity("TestData" + counter) };
                    counter++;
                    return result;
                }
                else
                {
                    return new List<Entity>();
                }

            }
        }

        class TestNetBus : SocketNetBus
        {

        }

        class TestMessage : IMessage
        {
        }

        [Fact]
        public async Task SocketMessageTransmitTest()
        {
            var dataBus = new TestDataBus();
            var netBus = new TestNetBus();

            _ = dataBus.Start();

            await netBus.Start();

            var result = (await netBus.SendMessage(new TestMessage())).ToList();

            await netBus.Stop();
            await dataBus.Stop();
            

            var testEnt = (TestEntity)result[0];

            Assert.Equal("TestData", testEnt.TestData);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public async Task MultiClientTest(int clientAmount)
        {
            var dataBus = new TestMultiDataBus();
            var clients = new INetBus[clientAmount];
            for (int i = 0; i < clientAmount; i++)
            {
                clients[i] = new TestNetBus();
            }

            _ = dataBus.Start();

            for (int i = 0; i < clientAmount; i++)
            {
                await clients[i].Start();
            }

            var results = new Entity[clientAmount];

            for (int i = 0; i < clientAmount; i++)
            {
                results[i] = (await clients[i].SendMessage(new TestMessage())).First();
            }

            for (int i = 0; i < clientAmount; i++)
            {
                await clients[i].Stop();
            }

            _ = dataBus.Stop();

            for (int i = 0; i < clientAmount; i++)
            {
                var testEnt = (TestEntity)results[i];
                Assert.Equal("TestData" + i, testEnt.TestData);
            }

            
        }
    }
}
