using System;
using System.Diagnostics;

namespace Lururen.Client.ResourceManagement
{
    /// <summary>
    /// Collection that uses less space in memory by storing only unique objects giving non-changing indexes to them.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SharedObjectManager<T>
    {
        protected List<(T Value, int ReferenceCount)> SharedObjects { get; set; } = new();
        protected Dictionary<int, int> IndexMapping { get; set; } = new();

        /// <summary>
        /// Maximum index reached in mapping
        /// </summary>
        int maxMappingIndex = -1;

        protected abstract bool CheckEquality(T a, T b);

        public int Add(T value)
        {
            // Try to find index.
            var actualIndex = SharedObjects.FindIndex(x => CheckEquality(x.Value, value));
            if (actualIndex == -1) // Entry not found
            {
                // Create new mapping entry
                maxMappingIndex++;
                IndexMapping[maxMappingIndex] = SharedObjects.Count;

                // Add data to memory with reference count = 1
                SharedObjects.Add((value, 1));

                return maxMappingIndex;
            } 
            else
            {
                // Increase reference count
                var (val, refCount) = SharedObjects[actualIndex];
                SharedObjects[actualIndex] = (val, refCount + 1);

                // Return mapped index
                return IndexMapping.FirstOrDefault(x => x.Value == actualIndex).Key;
            }
        }

        public int Set(int index, T value)
        {
            // Find actual index
            int actualIndex = GetInnerIndex(index);

            // If value is the same setting is not needed.
            if (CheckEquality(SharedObjects[actualIndex].Value, value)) return index;

            // If index is referenced more than once
            if (SharedObjects[actualIndex].ReferenceCount > 1)
            {
                // Decrease reference counter
                SharedObjects[actualIndex] = (SharedObjects[actualIndex].Value, SharedObjects[actualIndex].ReferenceCount - 1);
                // Add new entry (or find another existing, which Add() does)
                return Add(value);
            } else
            {
                // Just rewrite data
                SharedObjects[actualIndex] = (value, 1);
                return index;
            }
        }

        public void Remove(int index)
        {
            // Find actual index
            int actualIndex = GetInnerIndex(index);

            // Decrease reference counter
            SharedObjects[actualIndex] = (SharedObjects[actualIndex].Item1, SharedObjects[actualIndex].Item2 - 1);

            // Noone references entry anymore
            if (SharedObjects[actualIndex].ReferenceCount == 0) 
            {
                // Remove entry
                SharedObjects.RemoveAt(actualIndex);
                // We have a hole. Reindexing needed.
                Reindex(index);
            }
        }

        public void Reindex(int holeIndex)
        {
            // Invalidate hole index
            IndexMapping[holeIndex] = -1;

            // Decrease every following index
            for (int i = holeIndex + 1; i < IndexMapping.Count; i++)
            {
                IndexMapping[i]--;
            }
        }

        public int GetInnerIndex(int index)
        {
            return IndexMapping[index];
        }

        public T GetValue(int index)
        {
            return SharedObjects[GetInnerIndex(index)].Item1;
        }

    }
}