using System;
using System.Collections.Generic;

namespace NetworkPrimitives.Utilities
{
    public readonly struct ReadOnlyListSpan<T> : IEquatable<ReadOnlyListSpan<T>>, ISlice<ReadOnlyListSpan<T>, T>
    {
        private readonly IReadOnlyList<T> list;
        private readonly int startIndex;
        public ReadOnlyListSpan(IReadOnlyList<T> list) : this(list, 0, list.Count)
        {
        }
        private ReadOnlyListSpan(IReadOnlyList<T> list, int startIndex, int length)
        {
            this.list = list;
            this.startIndex = startIndex;
            this.Length = length;
        }
        public int Length { get; }
        public bool IsEmpty => Length == 0;
        public T this[int index] => this.list[this.startIndex + index];

        public ReadOnlyListSpan<T> Slice(int start, int length)
        {
            if ((uint)start > (uint)this.Length || (uint)length > (uint)(this.Length - start))
                throw new ArgumentOutOfRangeException();
            return new (this.list, this.startIndex + start, length);
        }

        public bool Equals(ReadOnlyListSpan<T> other) => this.list.Equals(other.list) && this.startIndex == other.startIndex && this.Length == other.Length;
        public override bool Equals(object? obj) => obj is ReadOnlyListSpan<T> other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(this.list, this.startIndex, this.Length);
        public static bool operator ==(ReadOnlyListSpan<T> left, ReadOnlyListSpan<T> right) => left.Equals(right);
        public static bool operator !=(ReadOnlyListSpan<T> left, ReadOnlyListSpan<T> right) => !left.Equals(right);
    }
}