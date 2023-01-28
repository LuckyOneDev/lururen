using GraphicsTestApp;
using Lururen.Client;
using Lururen.Client.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

var client = new TestClient();
var ctx = new Context2D();
client.Start(ctx);
