namespace Lururen.Client.Graphics.Shapes
{
    // LinkedList ?
    public class PrecalculationCollection<T>
    {
        public PrecalculationCollection(int entrySize) 
        {
            this.EntrySize = entrySize;
            JoinedData = new T[ReserveSize * entrySize];
        }

        public void Reserve(int amount = ReserveSize)
        {
            var newArr = new T[EntrySize * (counter + amount)];
            JoinedData.CopyTo(newArr, 0);
            JoinedData = newArr;
            GC.Collect();
        }

        private const int ReserveSize = 4096;

        private uint counter = 0;
        public int EntrySize { get; }

        T[] JoinedData;

        public uint Add(T[] inst)
        {
            if (counter * EntrySize >= JoinedData.Length)
            {
                Reserve();
            }

            inst.CopyTo(JoinedData, counter * EntrySize);

            counter++;
            return counter - 1;
        }

        public void Set(uint index, T[] value)
        {
            value.CopyTo(JoinedData, index * EntrySize);
        }

        public void Remove(uint index)
        {
            JoinedData.CopyTo(JoinedData, index * EntrySize);
            counter--;
        }

        public T[] GetJoined()
        {
            return JoinedData;
        }
    }
}