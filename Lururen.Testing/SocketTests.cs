using Lururen.Networking.Common;
using Lururen.Networking.SimpleSocketBus;

namespace Lururen.Testing
{
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

        class TestNetBus : SocketNetBus
        {

        }

        class TestMessage : IMessage
        {
        }

        [Fact]
        public async Task SocketMessageTransmit()
        {
            var dataBus = new TestDataBus();
            var netBus = new TestNetBus();

            _ = dataBus.Start();

            await netBus.Start();

            var result = (await netBus.SendMessage(new TestMessage())).ToList();

            _ = dataBus.Stop();

            var testEnt = (TestEntity)result[0];

            Assert.Equal("TestData", testEnt.TestData);
        }
    }
}
