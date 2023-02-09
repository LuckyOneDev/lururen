using GraphicsTestApp;
using Lururen.Client.ECS.Planar.Systems;

var a = 24 + sizeof(float) * 16;

var client = new TestClient();
client.Start(new Lururen.Client.WindowSettings 
{ 
    vSyncMode = OpenTK.Windowing.Common.VSyncMode.Adaptive
});
