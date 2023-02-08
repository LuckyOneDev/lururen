using OpenTK.Graphics.OpenGL4;
using System;

namespace Lururen.Client.Graphics.Shapes
{
    // LinkedList ?
    public class GLBufferHelper<T> where T : struct
    {
        public GLBufferHelper(uint[] Indices) 
        {
            this.BaseIndices = Indices;
        }

        private int ElementArrayBufferSize = 0;
        private int ArrayBufferSize = 0;

        private int counter = 0;

        Dictionary<int, long> ReferenceCount = new();
        Dictionary<int, T[]> BufferedData = new();

        public uint[] BaseIndices { get; }

        public int Add(T[] value)
        {
            var found = BufferedData.FirstOrDefault(x => Enumerable.SequenceEqual(value, x.Value));
            if (found.Value != null)
            {
                ReferenceCount[found.Key]++;
                return found.Key;
            } else
            {
                BufferedData.Add(counter, value);
                ReferenceCount.Add(counter, 1);
                counter++;
                return counter - 1;
            }
        }

        public int Set(int index, T[] value)
        {
            if (ReferenceCount[index] > 1)
            {
                if (Enumerable.SequenceEqual(BufferedData[index], value)) return index;

                BufferedData.Add(counter, value);
                ReferenceCount.Add(counter, 1);
                ReferenceCount[index]--;
                counter++;
                return counter - 1;
            } 
            else
            {
                BufferedData[index] = value;
                return index;
            }
        }

        public void Remove(int index)
        {
            ReferenceCount[index]--;
            if (ReferenceCount[index] <= 0)
            {
                ReferenceCount.Remove(index);
                BufferedData.Remove(index);
            }
        }

        public T[] GetJoined()
        {
            return BufferedData.SelectMany(x => x.Value).ToArray();
        }

        public void SetBuffers()
        {
            var generatedIndices = BaseIndices;
            if (ElementArrayBufferSize != generatedIndices.Length)
            {
                OpenGLHelper.SetBuffer(generatedIndices, BufferTarget.ElementArrayBuffer);
                ElementArrayBufferSize = generatedIndices.Length;
            } else
            {
                OpenGLHelper.SetBuffer(generatedIndices, BufferTarget.ElementArrayBuffer, 0);
            }

            var joinedData = GetJoined();
            if (ArrayBufferSize != joinedData.Length)
            {
                OpenGLHelper.SetBuffer(joinedData, BufferTarget.ArrayBuffer);
                ArrayBufferSize = joinedData.Length;
            } else
            {
                OpenGLHelper.SetBuffer(joinedData, BufferTarget.ArrayBuffer, 0);
            }
        }

        private uint[] GenIndices()
        {
            var genInd = new List<uint>();

            for (uint i = 0; i < BufferedData.Count; i++)
            {
                genInd.AddRange(BaseIndices.Select(ind => ind + i * (uint)BaseIndices.Length));
            }

            return genInd.ToArray();
        }
    }
}