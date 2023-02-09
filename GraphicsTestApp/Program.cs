using GraphicsTestApp;
using Lururen.Client.Window;

var client = new TestClient();
client.Start(new WindowSettings 
{ 
    vSyncMode = OpenTK.Windowing.Common.VSyncMode.Adaptive
});
