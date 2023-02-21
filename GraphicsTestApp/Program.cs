using GraphicsTestApp;
using Lururen.Client.Audio;
using Lururen.Client.Audio.Generic;
using Lururen.Client.Window;
using Lururen.Client.ResourceManagement;

var client = new TestClient();
client.Start(new WindowSettings
{
    vSyncMode = OpenTK.Windowing.Common.VSyncMode.Adaptive
});