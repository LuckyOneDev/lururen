using OpenTK.Graphics.OpenGL4;
using System;

namespace Lururen.Client.Graphics.Shapes
{
    public class GLBufferHelper<T> where T : struct
    {
        public GLBufferHelper(uint[] Indices) 
        {
            this.BaseIndices = Indices;
        }

        private int ElementArrayBufferSize = 0;
        private int ArrayBufferSize = 0;

        private int counter = 0;

        Dictionary<Ref<int>, long> ReferenceCount = new();
        Dictionary<Ref<int>, T[]> BufferedData = new();

        public uint[] BaseIndices { get; }

        public ref int Add(T[] value)
        {
            var found = BufferedData.FirstOrDefault(x => Enumerable.SequenceEqual(value, x.Value));
            if (found.Value != null)
            {
                ReferenceCount[found.Key]++;
                return ref found.Key.Value;
            } else
            {
                var indexRef = new Ref<int>(counter);
                BufferedData.Add(indexRef, value);
                ReferenceCount.Add(indexRef, 1);
                counter++;
                return ref indexRef.Value;
            }
        }

        public void Set(ref int index, T[] value)
        {
            var indexRef = new Ref<int>(ref index);

            if (ReferenceCount[indexRef] == 1)
            {
                BufferedData[indexRef] = value;
            } 
            else
            {
                if (Enumerable.SequenceEqual(BufferedData[indexRef], value)) return;

                var countRef = new Ref<int>(counter);

                BufferedData.Add(countRef, value);
                ReferenceCount.Add(countRef, 1);
                ReferenceCount[indexRef]--;
                counter++;
            }
        }

        public void Remove(ref int index)
        {
            var indexRef = new Ref<int>(ref index);

            ReferenceCount[indexRef]--;
            if (ReferenceCount[indexRef] == 0)
            {
                ReferenceCount.Remove(indexRef);
                ReferenceCount.ToList()
                    .ForEach(x => { if (x.Key.Value > indexRef.Value) x.Key.Value = x.Key.Value - 1; });

                BufferedData.Remove(indexRef);
                BufferedData.ToList()
                    .ForEach(x => { if (x.Key.Value > indexRef.Value) x.Key.Value = x.Key.Value - 1; });

                // Invalidate index just in case
                index = -1;
                counter--;
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
    }
}