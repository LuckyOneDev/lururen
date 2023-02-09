namespace Lururen.Client.ResourceManagement
{
    public abstract class MemoryManager<T>
    {
        private int counter = 0;
        protected Dictionary<Ref<int>, long> ReferenceCount = new();
        protected Dictionary<Ref<int>, T> MemoryData = new();

        protected abstract bool CheckEquality(T a, T b);

        public ref int Add(T value)
        {
            var found = MemoryData.FirstOrDefault(x => CheckEquality(value, x.Value));
            if (found.Value != null)
            {
                ReferenceCount[found.Key]++;
                return ref found.Key.Value;
            }
            else
            {
                var indexRef = new Ref<int>(counter);
                MemoryData.Add(indexRef, value);
                ReferenceCount.Add(indexRef, 1);
                counter++;
                return ref indexRef.Value;
            }
        }

        public void Set(ref int index, T value)
        {
            var indexRef = new Ref<int>(ref index);

            if (ReferenceCount[indexRef] == 1)
            {
                MemoryData[indexRef] = value;
            }
            else
            {
                if (CheckEquality(MemoryData[indexRef], value)) return;

                var countRef = new Ref<int>(counter);

                MemoryData.Add(countRef, value);
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

                MemoryData.Remove(indexRef);
                MemoryData.ToList()
                    .ForEach(x => { if (x.Key.Value > indexRef.Value) x.Key.Value = x.Key.Value - 1; });

                // Invalidate index just in case
                index = -1;
                counter--;
            }
        }

    }
}