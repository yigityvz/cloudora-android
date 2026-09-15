using System;
using System.Collections.Generic;
using Cloudora.Puzzle;

namespace Cloudora.Core
{
    [Serializable]
    public sealed class CloudState
    {
        private readonly List<WeatherType> _elements;

        public int Capacity { get; }
        public IReadOnlyList<WeatherType> Elements => _elements;
        public int Count => _elements.Count;
        public bool IsEmpty => Count == 0;
        public bool IsFull => Count == Capacity;
        public WeatherType Top => _elements[^1];

        public CloudState(int capacity, IEnumerable<WeatherType> elements)
        {
            if (capacity < 1 || capacity > 7) throw new ArgumentOutOfRangeException(nameof(capacity));
            Capacity = capacity;
            _elements = new List<WeatherType>(elements);
            if (_elements.Count > capacity) throw new ArgumentException("Cloud exceeds capacity.", nameof(elements));
        }

        public int TopGroupCount()
        {
            if (IsEmpty) return 0;
            int count = 1;
            for (int i = Count - 2; i >= 0 && _elements[i] == Top; i--) count++;
            return count;
        }

        internal void RemoveTop(int count)
        {
            _elements.RemoveRange(_elements.Count - count, count);
        }

        internal void Add(WeatherType weather, int count)
        {
            for (int i = 0; i < count; i++) _elements.Add(weather);
        }

        internal void SetAt(int index, WeatherType weather) => _elements[index] = weather;

        public CloudState Clone() => new(Capacity, _elements);
        public WeatherType[] ToArray() => _elements.ToArray();
    }
}
