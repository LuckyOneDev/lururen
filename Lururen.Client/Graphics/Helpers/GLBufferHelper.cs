using Lururen.Client.ResourceManagement;
using OpenTK.Graphics.OpenGL4;

namespace Lururen.Client.Graphics.Helpers
{

    public class GLBufferHelper : MemoryManager<float[]>
    {
        public GLBufferHelper(uint[] Indices)
        {
            BaseIndices = Indices;
        }

        public uint[] BaseIndices { get; }


        protected float[] GetJoined()
        {
            return SharedObjects.SelectMany(x => x.Item1).ToArray();
        }

        public void SetBuffers()
        {
            OpenGLHelper.SetBuffer(BaseIndices, BufferTarget.ElementArrayBuffer);
            OpenGLHelper.SetBuffer(GetJoined(), BufferTarget.ArrayBuffer);
        }

        protected override bool CheckEquality(float[] a, float[] b) =>
            a.SequenceEqual(b);
    }
}