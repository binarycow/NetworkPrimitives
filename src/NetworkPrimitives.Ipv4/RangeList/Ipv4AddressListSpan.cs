#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    /// <summary>
    /// A lightweight accessor to a list of <see cref="Ipv4AddressRange"/>
    /// </summary>
    public readonly struct Ipv4AddressListSpan : ISlice<Ipv4AddressListSpan, Ipv4Address>, IEnumerable<Ipv4Address>
    {
        private readonly ReadOnlyListSpan<Ipv4AddressRange> ranges;
        private readonly Ipv4AddressRange currentRange;

        private Ipv4AddressListSpan(IReadOnlyList<Ipv4AddressRange> ranges, Ipv4AddressRange currentRange, ulong length)
            : this(new ReadOnlyListSpan<Ipv4AddressRange>(ranges), currentRange, length)
        {
        }
        
        private Ipv4AddressListSpan(ReadOnlyListSpan<Ipv4AddressRange> ranges, Ipv4AddressRange currentRange, ulong length)
        {
            this.ranges = ranges;
            this.currentRange = currentRange;
            this.Length = length;
        }

        /// <summary>
        /// Create a new instance of <see cref="Ipv4AddressListSpan"/>
        /// </summary>
        /// <param name="ranges">
        /// A list of <see cref="Ipv4AddressRange"/>
        /// </param>
        /// <returns>
        /// An instance of <see cref="Ipv4AddressListSpan"/>
        /// </returns>
        public static Ipv4AddressListSpan CreateNew(IReadOnlyList<Ipv4AddressRange>? ranges)
        {
            var span = new ReadOnlyListSpan<Ipv4AddressRange>(ranges);
            _ = span.TrySliceFirst(out Ipv4AddressRange firstSlice);
            var length = firstSlice.Length;
            for (var i = 0; i < span.Length; ++i)
            {
                length += span[i].Length;
            }
            return new (span, firstSlice, length);
        }

        /// <summary>
        /// Gets the address at the given zero-based index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the address to get.
        /// </param>
        public Ipv4Address this[int index] => GetItem(index);

        private Ipv4Address GetItem(int index)
        {
            var uIndex = (ulong)index;
            if (index < 0 || uIndex > Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            
            if (Check(ref uIndex, this.currentRange, out var item))
            {
                return item;
            }
            for(var i = 0; i < this.ranges.Length; ++i)
            {
                if (Check(ref uIndex, this.ranges[i], out item))
                {
                    return item;
                }
            }
            throw new ArgumentOutOfRangeException(nameof(index)); // TODO: This should never happen.


            static bool Check(ref ulong uIndex, Ipv4AddressRange range, out Ipv4Address item)
            {
                var rangeLength = range.Length;
                if (uIndex < rangeLength)
                {
                    item = range[(uint)uIndex];
                    return true;
                }
                item = default;
                uIndex -= rangeLength;
                return false;
            }
        }

        private ulong CurrentRangeLength => this.currentRange.Length;
        
        /// <summary>
        /// The number of addresses covered by this instance
        /// </summary>
        public ulong Length { get; }
        int ISlice<Ipv4AddressListSpan, Ipv4Address>.Length => (int)Length;

        
        /// <summary>
        /// Retrieves a portion of addresses from this instance. The portion starts at a
        /// specified index and has a specified length.
        /// </summary>
        /// <param name="start">
        /// The zero-based starting index of the resulting <see cref="Ipv4AddressListSpan"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Ipv4AddressListSpan"/> that is equivalent to the portion
        /// of addresses beginning at <paramref name="start"/> in this instance
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="start"/> is invalid.
        /// </exception>
        public Ipv4AddressListSpan Slice(int start) 
            => start >= 0 
                ? this.Slice((ulong)start) 
                : throw new ArgumentOutOfRangeException();

        /// <summary>
        /// Retrieves a portion of addresses from this instance. The portion starts at a
        /// specified index and has a specified length.
        /// </summary>
        /// <param name="start">
        /// The zero-based starting index of the resulting <see cref="Ipv4AddressListSpan"/>.
        /// </param>
        /// <param name="length">
        /// The number of addresses in the resulting <see cref="Ipv4AddressListSpan"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Ipv4AddressListSpan"/> that is equivalent to the portion
        /// of addresses of length <paramref name="length"/> that begins
        /// at <paramref name="start"/> in this instance
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="start"/> or <paramref name="length"/> are invalid.
        /// </exception>
        public Ipv4AddressListSpan Slice(int start, int length)
        {
            if (start < 0 || length < 0)
                throw new ArgumentOutOfRangeException();
            return Slice((ulong)start, (ulong)length);
        }

        /// <summary>
        /// Retrieves a portion of addresses from this instance. The portion starts at a
        /// specified index and has a specified length.
        /// </summary>
        /// <param name="start">
        /// The zero-based starting index of the resulting <see cref="Ipv4AddressListSpan"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Ipv4AddressListSpan"/> that is equivalent to the portion
        /// of addresses beginning at <paramref name="start"/> in this instance
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="start"/> is invalid.
        /// </exception>
        public Ipv4AddressListSpan Slice(ulong start) => Slice(start, Length - start);
        
        
        /// <summary>
        /// Retrieves a portion of addresses from this instance. The portion starts at a
        /// specified index and has a specified length.
        /// </summary>
        /// <param name="start">
        /// The zero-based starting index of the resulting <see cref="Ipv4AddressListSpan"/>.
        /// </param>
        /// <param name="length">
        /// The number of addresses in the resulting <see cref="Ipv4AddressListSpan"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Ipv4AddressListSpan"/> that is equivalent to the portion
        /// of addresses of length <paramref name="length"/> that begins
        /// at <paramref name="start"/> in this instance
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="start"/> or <paramref name="length"/> are invalid.
        /// </exception>
        public Ipv4AddressListSpan Slice(ulong start, ulong length)
        {
            if (start > this.Length || length > this.Length - start)
                throw new ArgumentOutOfRangeException();
            return SliceStart(start).SliceLength(length);
        }

        private Ipv4AddressListSpan SliceStart(ulong start)
        {
            var copy = this;
            while (start >= copy.CurrentRangeLength)
            {
                copy = AdvancePastCurrentRange(out var skippedLength);
                start -= skippedLength;
            }
            return new (copy.ranges, copy.currentRange.Slice(start), copy.Length - start);
        }

        private Ipv4AddressListSpan SliceLength(ulong length)
        {
            var remainingLength = length;
            var rangeLength = this.currentRange.Length;
            if (length < rangeLength)
            {
                return new (
                    Array.Empty<Ipv4AddressRange>(),
                    this.currentRange.Slice(0, (uint)length),
                    length
                );
            }
            remainingLength -= rangeLength;
            for (var i = 0; i < ranges.Length; ++i)
            {
                rangeLength = this.ranges[i].Length;
                if (remainingLength < rangeLength)
                {
                    return new (
                        this.ranges[0..i],
                        this.currentRange,
                        length
                    );
                }
                remainingLength -= rangeLength;
            }
            return new (this.ranges, this.currentRange, length);
        }
        private Ipv4AddressListSpan AdvancePastCurrentRange(out ulong skippedLength)
        {
            skippedLength = this.currentRange.Length;
            var copy = this.ranges;
            if (!copy.TrySliceFirst(out Ipv4AddressRange firstRange))
                firstRange = default;
            return new (copy, firstRange, this.Length - skippedLength);
        }

        /// <summary>
        /// Get an enumerator to iterate over the addresses in this <see cref="Ipv4AddressListSpan"/>
        /// </summary>
        /// <returns>
        /// An <see cref="Ipv4Address"/> enumerator.
        /// </returns>
        public IEnumerator<Ipv4Address> GetEnumerator() => new Ipv4AddressListSpanEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        
        
        private sealed class Ipv4AddressListSpanEnumerator : IEnumerator<Ipv4Address>
        {
            private readonly Ipv4AddressListSpan original;
            private Ipv4AddressListSpan span;
            private Ipv4Address current;
            public Ipv4AddressListSpanEnumerator(Ipv4AddressListSpan span) => this.original = this.span = span;

            public bool MoveNext() => this.span.TrySliceFirst(out this.current);

            public void Reset()
            {
                this.span = this.original;
            }

            public Ipv4Address Current => this.current;

            object IEnumerator.Current => this.Current;

            public void Dispose()
            {
            }
        }
    }

}