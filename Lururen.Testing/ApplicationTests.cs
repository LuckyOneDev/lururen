using Lururen.Networking.SocketNetworking;

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
            private readonly double load;

            public TestHeavyEntity(double load)
            {
                this.load = load;
            }
            public override void Update()
            {
                base.Update();
                for (double i = 0; i < 100_000_000.0f / load; i++)
                {
                }
            }

            public override void OnEvent(EventArgs args)
            {
            }
        }

        public static IEnumerable<object[]> SomeEntitiesData()
        {
            int[] frameCaps = { 30, 50, 60, 70, 80, 90, 100 };
            foreach (int cap in frameCaps)
            {
                yield return new object[] { new TestHeavyEntity(cap), cap };
            }
            foreach (int cap in frameCaps)
            {
                yield return new object[] { new TestEntity(), cap };
            }
        }


        [Theory]
        [MemberData(nameof(SomeEntitiesData))]
        public void UpdatingEnviromentWithActiveEntityTest(TestEntity testEntity, int frameCap)
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

            app.Start(frameCap);
            Thread.Sleep(1000);
            Assert.InRange(ent.updateCount, frameCap * 0.95, frameCap * 1.05); // 5% jitter
            app.Stop();

            Assert.Equal(1, ent.initCount);
            Assert.Equal(0, ent.disposeCount);
        }
    }
}
