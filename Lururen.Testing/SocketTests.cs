using Lururen.Networking.Common;
using Lururen.Networking.SocketBus;

namespace Lururen.Testing
{
    public class SocketTests
    {
        class TestEntity : Entity
        {
            public readonly string TestData = "Test";
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
                    List<Entity> result = new List<Entity>() { new TestEntity() };
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
            await netBus.Connect();

            var result = (await netBus.SendMessage(new TestMessage())).ToList();

            dataBus.Stop();

            Assert.Equal(new TestEntity().TestData, ((TestEntity)result[0]).TestData);
        }
    }
}
