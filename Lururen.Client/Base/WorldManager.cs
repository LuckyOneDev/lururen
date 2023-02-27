using Lururen.Client.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Client.Base
{
    public static class WorldManager
    {
        static Application Application { get; set; }
        public static World CurrentWorld { get; private set; }

        static Dictionary<string, World> Worlds { get; set; } = new();

        public static void Init(Application app)
        {
            Application = app;
            CreateWorld("default");
            SetActiveWorld("default");
        }

        public static void SetActiveWorld(string worldId)
        {
            CurrentWorld = Worlds[worldId];
        }

        public static void CreateWorld(string worldId)
        {
            Worlds.Add(worldId, Application.CreateWorld(worldId));
        }
    }
}
