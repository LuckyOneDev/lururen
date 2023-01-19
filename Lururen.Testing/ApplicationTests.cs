using Lururen.Core.EntitySystem;
using Lururen.Networking.Common;
using Lururen.Networking.Local;
using Lururen.Networking.SimpleSocketBus;

namespace Lururen.Testing
{
    public class ApplicationTests
    {
        public class TestApp : Application
        {
            public TestApp() : base(new SocketServerMessageBridge())
            {
            }

            public override void Dispose()
            {
            }

            public override Stream GetResource(string resourceName)
            {
                throw new NotImplementedException();
            }

            public override ResourceInfo GetResourceInfo()
            {
                throw new NotImplementedException();
            }

            public override void Init()
            {
            }
        }

        public class TestEntity : Entity
        {
            public int updateCount = 0;
            public int initCount = 0;
            public int disposeCount = 0;
            public override void Init()
            {
                initCount++;
            }

            public override void Update()
            {
                updateCount++;
            }

            public override void Dispose()
            {
                disposeCount++;
            }

            public override void OnEvent(EventArgs args)
            {
            }
        }

        // Represents heavy update operation
        public class TestHeavyEntity : TestEntity
        {
            public override void Update()
            {
                base.Update();
                Task.Delay(10).Wait();
            }

            public override void OnEvent(EventArgs args)
            {
            }
        }

        public static IEnumerable<object[]> SomeEntitiesData()
        {
            yield return new object[] { new TestEntity() };
            yield return new object[] { new TestHeavyEntity() };
        }


        [Theory]
        [MemberData(nameof(SomeEntitiesData))]
        public void UpdatingEnviromentWithActiveEntityTest(TestEntity testEntity)
        {
            TestApp app = new TestApp();
            Environment env = app.CreateEnvironment();
            TestEntity ent = testEntity;
            env.AddEntity(ent, Active: true);

            Assert.Equal(0, ent.updateCount);
            Assert.Equal(0, ent.initCount);
            Assert.Equal(0, ent.disposeCount);

            app.LoadEnviroment(env);

            Assert.Equal(0, ent.updateCount);
            Assert.Equal(1, ent.initCount);
            Assert.Equal(0, ent.disposeCount);

            app.Start(50);
            Thread.Sleep(1000);
            Assert.InRange(ent.updateCount, 48, 50);
            app.Stop();

            Assert.Equal(1, ent.initCount);
            Assert.Equal(0, ent.disposeCount);
        }
    }
}
