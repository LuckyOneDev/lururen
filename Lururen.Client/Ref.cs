using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lururen.Client
{
    public class Ref<T> : IEquatable<Ref<T>>
    {
        private T? _value;

        public Ref() { _value = default; }
        public Ref(T value) { _value = value; }
        public Ref(ref T value) { _value = value; }

        public ref T Value => ref _value;

        public bool Equals(Ref<T>? other)
        {
            if (other == null) { return _value == null; }
            if (_value == null) { return other == null; }
            return _value.Equals(other._value);
        }

        public override bool Equals(object? obj)
        {
            return _value.Equals(obj);
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}