using Lururen.Core.EntitySystem;

namespace Lururen.Testing
{
    public class ApplicationTests
    {
        public class TestApp : Application
        {
            public override void Dispose()
            {
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

        [Fact]
        public void UpdatingEnviromentWithActiveEntityTest()
        {
            TestApp app = new TestApp();
            Environment env = app.CreateEnvironment();
            TestEntity ent = new TestEntity();
            env.AddEntity(ent, Active: true);

            Assert.Equal(0, ent.updateCount);
            Assert.Equal(0, ent.initCount);
            Assert.Equal(0, ent.disposeCount);

            app.LoadEnviroment(env);

            Assert.Equal(0, ent.updateCount);
            Assert.Equal(1, ent.initCount);
            Assert.Equal(0, ent.disposeCount);

            app.Start(TimeSpan.FromMilliseconds(1));
            Thread.Sleep(5);

            Assert.InRange(ent.updateCount, 1, 6);
            Assert.Equal(1, ent.initCount);
            Assert.Equal(0, ent.disposeCount);
        }
    }
}
