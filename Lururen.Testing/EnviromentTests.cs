using Lururen.Core.Common;
using Lururen.Core.EntitySystem;
using Environment = Lururen.Core.EnviromentSystem.Environment;

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

        [Fact]
        public void UpdateTest()
        {
            Environment env = new Environment();
            env.Init();
            var testEnt = new TestEntity();
            env.AddEntityActive(SVector3.Zero, testEnt);
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
            Environment env = new Environment();
            env.Init();
            env.AddEntityActive(new SVector3(x, y, z), new TestEntity());
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
            Environment env = new Environment();
            env.Init();
            env.AddEntityActive(new SVector3(x, y, z), new TestEntity());
            var found = env.SearchInRadius(SVector3.Zero, r);
            Assert.NotEmpty(found);
        }

        [Fact]
        public void ActivationDeactivationTest()
        {
            Environment env = new Environment();
            env.Init();
            var testEnt = new TestEntity();
            env.AddEntityPassive(SVector3.Zero, testEnt);
            Assert.False(env.IsEntityActive(testEnt));
            env.ActivateEntity(testEnt);
            Assert.True(env.IsEntityActive(testEnt));
            env.DeactivateEntity(testEnt);
            Assert.False(env.IsEntityActive(testEnt));
        }
    }
}
