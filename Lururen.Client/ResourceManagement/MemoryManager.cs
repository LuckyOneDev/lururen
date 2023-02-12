namespace Lururen.Client.ResourceManagement
{
    /// <summary>
    /// Base class for everything that should manage shared objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MemoryManager<T>
    {
        protected List<(T, int)> MemoryData { get; set; } = new();
        protected Dictionary<int, int> Mapping { get; set; } = new();

        protected abstract bool CheckEquality(T a, T b);

        public int Add(T value)
        {
            // Try to find index.
            var index = MemoryData.FindIndex(x => CheckEquality(x.Item1, value));
            if (index == -1) // Index not found
            {
                // Add new entry
                MemoryData.Add((value, 1));
                return MemoryData.Count - 1;
            } 
            else
            {
                var (val, refCount) = MemoryData[index];
                MemoryData[index] = (val, refCount + 1);
                return index;
            }
        }

        public int Set(int index, T value)
        {
            // Map index
            int actualIndex = GetIndex(index);
            
            // If rewrite is not needed.
            if (CheckEquality(MemoryData[actualIndex].Item1, value)) return actualIndex;

            // If index is referenced more than once
            if (MemoryData[actualIndex].Item2 > 1)
            {
                // Decrease reference count
                MemoryData[actualIndex] = (MemoryData[actualIndex].Item1, MemoryData[actualIndex].Item2 - 1);
                // Add new entry (or find existing, which Add() does)
                return Add(value);
            } else
            {
                // Just rewrite data
                MemoryData[actualIndex] = (value, 1);
                return actualIndex;
            }
        }

        public void Remove(int index)
        {
            int actualIndex = GetIndex(index);
            MemoryData[actualIndex] = (MemoryData[actualIndex].Item1, MemoryData[actualIndex].Item2 - 1);
            if (MemoryData[actualIndex].Item2 == 0) // Noone references entry anymore
            {
                // We have a hole. Reindexing needed.
                Reindex(actualIndex);
            }
        }

        public void Reindex(int holeIndex)
        {
            for (int i = holeIndex + 1; i < MemoryData.Count; i++)
            {
                Mapping[i] = i - 1;
            }
            MemoryData.RemoveAt(holeIndex);
        }

        public int GetIndex(int index)
        {
            if (Mapping.ContainsKey(index))
            {
                return GetIndex(Mapping[index]);
            } else
            {
                return index;
            }
        }

        public T GetValue(int index)
        {
            return MemoryData[GetIndex(index)].Item1;
        }

    }
}