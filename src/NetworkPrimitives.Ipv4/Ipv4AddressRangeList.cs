#nullable enable

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    public sealed class Ipv4AddressRangeList : ImmutableListWrapper<Ipv4AddressRangeList, Ipv4AddressRange>
    {
        public static Ipv4AddressRangeList Empty { get; } = new Ipv4AddressRangeList();
        private Ipv4AddressRangeList() : this(ImmutableList<Ipv4AddressRange>.Empty)
        {
        }
        private Ipv4AddressRangeList(ImmutableList<Ipv4AddressRange> ranges) : base(ranges)
        {
        }
        protected override IEqualityComparer<Ipv4AddressRange> EqualityComparer => Ipv4AddressRange.EqualityComparer;
        protected override Ipv4AddressRangeList CreateInstance(ImmutableList<Ipv4AddressRange> items) => new (items);
        public static bool TryParse(string text, [NotNullWhen(true)] out Ipv4AddressRangeList? result)
            => TryParse(text, out var charsRead, out result) && charsRead == text.Length;

        public Ipv4AddressRangeListEnumerator GetEnumerator() => new (new (Items));
        public Ipv4AddressListSpan GetAllAddresses() => Ipv4AddressListSpan.CreateNew(Items);

        public static bool TryParse(string text, out int charsRead, [NotNullWhen(true)] out Ipv4AddressRangeList? result)
        {
            var span = new SpanWrapper(text);
            charsRead = default;
            return TryParse(ref span, ref charsRead, out result);
       }

        internal static bool TryParse(ref SpanWrapper text, ref int charsRead, [NotNullWhen(true)] out Ipv4AddressRangeList? result)
        {
            result = default;
            var textCopy = text;
            var charsReadCopy = charsRead;
            _ = textCopy.TryConsumeWhiteSpace(ref charsReadCopy);
            if (!Ipv4AddressRange.TryParse(ref textCopy, ref charsReadCopy, out var range))
                return false;
            text = textCopy;
            charsRead = charsReadCopy;

            var builder = ImmutableList.CreateBuilder<Ipv4AddressRange>();
            builder.Add(range);
            
            while (textCopy.TryConsumeWhiteSpace(ref charsReadCopy))
            {
                if (!Ipv4AddressRange.TryParse(ref textCopy, ref charsReadCopy, out range))
                    break;
                builder.Add(range);
                text = textCopy;
                charsRead = charsReadCopy;
            }
            _ = textCopy.TryConsumeWhiteSpace(ref charsReadCopy);
            text = textCopy;
            charsRead = charsReadCopy;
            result = new (builder.ToImmutable());
            return true;
        }
    }
    
    
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

        public static Ipv4AddressListSpan CreateNew(ImmutableList<Ipv4AddressRange> ranges)
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
        public ulong Length { get; }
        int ISlice<Ipv4AddressListSpan, Ipv4Address>.Length => (int)Length;

        
        public Ipv4AddressListSpan Slice(int start) 
            => start >= 0 
                ? this.Slice((ulong)start) 
                : throw new ArgumentOutOfRangeException();

        public Ipv4AddressListSpan Slice(int start, int length)
        {
            if (start < 0 || length < 0)
                throw new ArgumentOutOfRangeException();
            return Slice((ulong)start, (ulong)length);
        }

        public Ipv4AddressListSpan Slice(ulong start) => Slice(start, Length - start);
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

        public IEnumerator<Ipv4Address> GetEnumerator() => new Ipv4AddressListSpanEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        
        
        private sealed class Ipv4AddressListSpanEnumerator : IEnumerator<Ipv4Address>
        {
            private Ipv4AddressListSpan original;
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

    
    public ref struct Ipv4AddressRangeListEnumerator
    {
        private readonly ReadOnlyListSpan<Ipv4AddressRange> original;
        private ReadOnlyListSpan<Ipv4AddressRange> available;
        private Ipv4AddressRange current;
        internal Ipv4AddressRangeListEnumerator(ReadOnlyListSpan<Ipv4AddressRange> original)
        {
            this.available = this.original = original;
            this.current = default;
        }
        public bool MoveNext() => this.available.TrySliceFirst(out this.current);
        public Ipv4AddressRange Current => current;
        public void Reset()
        {
            this.available = this.original;
            this.current = default;
        }
    }
}