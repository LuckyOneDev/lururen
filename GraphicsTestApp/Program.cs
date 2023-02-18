using GraphicsTestApp;
using Lururen.Client.Audio;
using Lururen.Client.Audio.Generic;
using Lururen.Client.Window;
using Lururen.Client.ResourceManagement;

//var client = new TestClient();
//client.Start(new WindowSettings
//{
//    vSyncMode = OpenTK.Windowing.Common.VSyncMode.Adaptive
//});

var device = new ALSoundDevice();
var sound = new Sound("GraphicsTestApp.matselect-1.wav", ResourceLocation.Embeded);
var source = new SoundSource();
await source.Play(sound);