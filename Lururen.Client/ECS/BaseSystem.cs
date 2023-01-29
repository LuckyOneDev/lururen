using Lururen.Client.Graphics.Drawables;

namespace Lururen.Client.ECS
{
    public class BaseSystem<T> where T : IComponent
    {

        public static List<T> Components = new List<T>();

        public static void Register(T component)
        {
            component.Init();
            Components.Add(component);
        }

        public void Update(double deltaTime)
        {
            Components.ForEach(drawable => drawable.Update(deltaTime));
        }
    }
}
