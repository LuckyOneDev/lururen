using System;
using System.Diagnostics;

namespace Lururen.Client.ResourceManagement
{
    /// <summary>
    /// Base class for everything that should manage shared objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MemoryManager<T>
    {
        protected List<(T Value, int ReferenceCount)> MemoryData { get; set; } = new();
        protected Dictionary<int, int> Mapping { get; set; } = new();

        int maxIndex = -1;

        protected abstract bool CheckEquality(T a, T b);

        public int Add(T value)
        {
            // Try to find index.
            var actualIndex = MemoryData.FindIndex(x => CheckEquality(x.Value, value));
            if (actualIndex == -1) // Entry not found
            {
                // Add new entry

                // Create new mapping entry
                maxIndex++;
                Mapping[maxIndex] = MemoryData.Count;

                // Add data to memory with reference count = 1
                MemoryData.Add((value, 1));

                Debug.WriteLine($"Add {maxIndex}");
                return maxIndex;
            } 
            else
            {
                // Increase reference count
                var (val, refCount) = MemoryData[actualIndex];
                MemoryData[actualIndex] = (val, refCount + 1);

                // Return mapped index
                return Mapping.FirstOrDefault(x => x.Value == actualIndex).Key;
            }
        }

        public int Set(int index, T value)
        {
            // Find actual index
            int actualIndex = GetInnerIndex(index);

            // If value is the same setting is not needed.
            if (CheckEquality(MemoryData[actualIndex].Value, value)) return index;

            // If index is referenced more than once
            if (MemoryData[actualIndex].ReferenceCount > 1)
            {
                // Decrease reference counter
                MemoryData[actualIndex] = (MemoryData[actualIndex].Value, MemoryData[actualIndex].ReferenceCount - 1);
                // Add new entry (or find another existing, which Add() does)
                return Add(value);
            } else
            {
                // Just rewrite data
                MemoryData[actualIndex] = (value, 1);
                return index;
            }
        }

        public void Remove(int index)
        {
            // Find actual index
            int actualIndex = GetInnerIndex(index);

            Debug.WriteLine($"Remove {index}");

            // Decrease reference counter
            MemoryData[actualIndex] = (MemoryData[actualIndex].Item1, MemoryData[actualIndex].Item2 - 1);

            // Noone references entry anymore
            if (MemoryData[actualIndex].ReferenceCount == 0) 
            {
                // Remove entry
                MemoryData.RemoveAt(actualIndex);
                // We have a hole. Reindexing needed.
                Reindex(index);
            }
        }

        public void Reindex(int holeIndex)
        {
            // Invalidate hole index
            Mapping[holeIndex] = -1;

            // Decrease every following index
            for (int i = holeIndex + 1; i < Mapping.Count; i++)
            {
                Mapping[i]--;
            }
            
            Debug.WriteLine($"Reindex {holeIndex}");
        }

        public int GetInnerIndex(int index)
        {
            return Mapping[index];
        }

        public T GetValue(int index)
        {
            return MemoryData[GetInnerIndex(index)].Item1;
        }

    }
}