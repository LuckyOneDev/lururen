namespace Lururen.Testing
{
    public class EntityToBeLoaded : Entity
    {
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

    public class ResourceLoadingTests
    {
        [Fact]
        public void EntityLoadsWhenTryingToLoadSameDllToDifferentAssembly()
        {
            DllResourceLoader loader = new();
            loader.LoadDll($"{System.Environment.CurrentDirectory}/Lururen.Testing.dll");
            Entity e = loader.LoadChildOf<Entity>(typeof(EntityToBeLoaded).ToString());
            e.Init();
            e.Update();
            e.Dispose();
        }

        [Fact]
        public void DllLoaderBuilderWorks()
        {
            DllResourceLoaderBuilder builder = new();
            builder.AddDllToBeLoaded($"{System.Environment.CurrentDirectory}/Lururen.Testing.dll");

            IResourceLoader loader = builder.Build();
            _ = loader.LoadChildOf<Entity>(typeof(EntityToBeLoaded).ToString());
        }
    }
}