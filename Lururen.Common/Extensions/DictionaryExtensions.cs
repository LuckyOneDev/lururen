namespace Lururen.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrCreateList<TKey, TValue>(
            this Dictionary<TKey, List<TValue>> dict,
            TKey key, TValue value) where TKey : notnull
        {
            if (!dict.TryGetValue(key, out List<TValue>? list) || list is null)
            {
                dict.Add(key, new List<TValue>() { value });
            }
            else
            {
                list.Add(value);
            }
        }

        public static void RemoveFromList<TKey, TValue>(
            this Dictionary<TKey, List<TValue>> dict,
            TValue value) where TKey : notnull
        {
            dict.Values.AsParallel().ForAll(x => x?.Remove(value));
        }
        public static void MoveValueToOther<TKey, TValue>(
            this Dictionary<TKey, List<TValue>> dict,
            Dictionary<TKey, List<TValue>> other,
            TValue value) where TKey : notnull
        {
            var kvPair = dict.ToList().AsParallel().FirstOrDefault(x => x.Value.Contains(value));
            if (kvPair.Value is null) return; // needs testing (if no value is present)
            dict.RemoveFromList(value);
            other.AddOrCreateList(kvPair.Key, value);
        }
    }
}