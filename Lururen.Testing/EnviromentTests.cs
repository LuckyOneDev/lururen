namespace Lururen.Testing
{
    public class EnviromentTests
    {
        public class TestEntity : Entity
        {
            public bool WasUpdated = false;
            public override void Init()
            {
            }

            public override void Update()
            {
                WasUpdated = true;
            }

            public override void Dispose()
            {
            }

            public override void OnEvent(EventArgs args)
            {
            }
        }

        public class TestApp : Application
        {
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

        [Fact]
        public void UpdateTest()
        {
            Application app = new TestApp();
            Environment env = app.CreateEnvironment();
            env.Init();
            var testEnt = new TestEntity();
            env.AddEntityActive(testEnt, SVector3.Zero);
            env.Update();
            Assert.True(testEnt.WasUpdated);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(11, 10, 10, 10)]
        [InlineData(0, 0, 2, 1)]
        [InlineData(0, 2, 1, 1)]
        public void SearchInRadius_None_Test(int x, int y, int z, int r)
        {
            Application app = new TestApp();
            Environment env = app.CreateEnvironment();
            env.Init();
            env.AddEntityActive(new TestEntity(), new SVector3(x, y, z));
            var found = env.SearchInRadius(SVector3.Zero, r);
            Assert.Empty(found);
        }

        [Theory]
        [InlineData(0, 0, 0, 1)]
        [InlineData(10, 10, 10, 11)]
        [InlineData(0, 0, 1, 2)]
        [InlineData(0, 1, 1, 2)]
        public void SearchInRadius_One_Test(int x, int y, int z, int r)
        {
            Application app = new TestApp();
            Environment env = app.CreateEnvironment();
            env.Init();
            var testEnt = new TestEntity();
            env.AddEntityActive(testEnt, new SVector3(x, y, z));
            var found = env.SearchInRadius(SVector3.Zero, r);
            Assert.Contains(testEnt, found);
        }

        [Fact]
        public void ActivationDeactivationTest()
        {
            Application app = new TestApp();
            Environment env = app.CreateEnvironment();
            env.Init();
            var testEnt = new TestEntity();
            env.AddEntityPassive(testEnt, SVector3.Zero);
            Assert.False(env.IsEntityActive(testEnt));
            env.ActivateEntity(testEnt);
            Assert.True(env.IsEntityActive(testEnt));
            env.DeactivateEntity(testEnt);
            Assert.False(env.IsEntityActive(testEnt));
        }
    }
}
