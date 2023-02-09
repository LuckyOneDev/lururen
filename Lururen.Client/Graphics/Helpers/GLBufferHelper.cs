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

        private int ElementArrayBufferSize = 0;
        private int ArrayBufferSize = 0;

        protected float[] GetJoined()
        {
            return MemoryData.Values.SelectMany(x => x).ToArray();
        }

        public void SetBuffers()
        {
            var generatedIndices = BaseIndices;
            if (ElementArrayBufferSize != generatedIndices.Length)
            {
                OpenGLHelper.SetBuffer(generatedIndices, BufferTarget.ElementArrayBuffer);
                ElementArrayBufferSize = generatedIndices.Length;
            }
            else
            {
                OpenGLHelper.SetBuffer(generatedIndices, BufferTarget.ElementArrayBuffer, 0);
            }

            var joinedData = GetJoined();
            if (ArrayBufferSize != joinedData.Length)
            {
                OpenGLHelper.SetBuffer(joinedData, BufferTarget.ArrayBuffer);
                ArrayBufferSize = joinedData.Length;
            }
            else
            {
                OpenGLHelper.SetBuffer(joinedData, BufferTarget.ArrayBuffer, 0);
            }
        }

        protected override bool CheckEquality(float[] a, float[] b) =>
            a.SequenceEqual(b);
    }
}