using Lururen.Client.Graphics.Drawables;

namespace Lururen.Client.ECS
{
    public class BaseSystem<T> where T : Component
    {

        public static List<T> Components = new List<T>();

        public static void Register(T component)
        {
            Components.Add(component);
        }

        public void Update(double deltaTime)
        {
            Components.ForEach(drawable => drawable.Update(deltaTime));
        }
    }
}
