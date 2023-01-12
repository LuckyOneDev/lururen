namespace Lururen.Testing
{
    public class ClientServerTesting
    {
        public class TestClient : IClient
        {
            public Dictionary<Uri, IScript> LoadedScripts { get; set; } = new();
            public void ExecuteScript(Uri scriptUri)
            {
                IScript script;
                LoadedScripts.TryGetValue(scriptUri, out script);
                if (script != null) script.Execute(this);
            }
            // Emulating client side data
            public string TestData = "";
            public int ClientId { get; set; }
            public TestGameHost Server { get; private set; }

            public void Connect(IGameHostInfo server)
            {
                this.Server = ((TestGameHostInfo)server).Host;
                var gameData = this.Server.OnConnect(this);
                LoadedScripts.Add(gameData.InitScript.Uri, gameData.InitScript);
                gameData.Scripts.ForEach(script =>
                {
                    LoadedScripts.Add(script.Uri, script);
                });
                ExecuteScript(gameData.InitScript.Uri);
            }
        }

        public class TestGameHostInfo : IGameHostInfo
        {
            public TestGameHostInfo(TestGameHost Host)
            {
                this.Host = Host;
            }

            public TestGameHost Host { get; set; }
            public string Name { get; set; } = "TEST-SERVER";
            public string Version { get; set; } = "NONE";
            public string Description { get; set; } = "None";
            public string Game { get; set; } = "TEST-GAME";
        }

        public class TestGameData : IGameData
        {
            public IScript InitScript { get; set; }
            public List<IScript> Scripts { get; set; } = new();
            public List<IResource> Resources { get; set; } = new();
        }

        public class TestScript1 : IScript
        {
            public Uri Uri => new Uri("http://www.test.test/testscript.cs");

            public byte[] Data => null;

            public ResourceType Type => ResourceType.CScript;

            public void Execute(IClient client)
            {
                TestClient testclient = (TestClient)client;
                testclient.TestData += "init;";
                // Actual path should be used
                testclient.ExecuteScript(new TestScript2().Uri);
            }
        }

        public class TestScript2 : IScript
        {
            public Uri Uri => new Uri("http://www.test.test/testscript2.cs");

            public byte[] Data => null;

            public ResourceType Type => ResourceType.CScript;

            public void Execute(IClient client)
            {
                TestClient testclient = (TestClient)client;
                testclient.TestData += "testscript;";
            }
        }

        public class TestGameHost : IGameHost
        {
            public List<IClient> GetClients()
            {
                return new();
            }

            public IGameData OnConnect(IClient client)
            {
                var gameData = new TestGameData();
                gameData.InitScript = new TestScript1();
                gameData.Scripts = new List<IScript>() { new TestScript2() };
                return gameData;
            }

            public IGameData OnDisonnect(IClient client)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void ClientServerDataTransmit()
        {
            var host = new TestGameHost();
            var client = new TestClient();
            client.Connect(new TestGameHostInfo(host));
            Assert.Equal("init;testscript;", client.TestData);
        }
    }
}
