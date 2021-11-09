using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    public readonly partial struct Ipv4AddressRange : IEquatable<Ipv4AddressRange>, IEnumerable<Ipv4Address>, ISlice<Ipv4AddressRange, Ipv4Address>
    {
        public static IEqualityComparer<Ipv4AddressRange> EqualityComparer { get; } = new StartAddressInclusiveLengthEqualityComparer();
        
        private readonly Ipv4Address startAddress;
        private readonly ulong inclusiveLength;

        public Ipv4AddressRange(Ipv4Address startAddress, ulong inclusiveLength)
        {
            this.startAddress = startAddress;
            this.inclusiveLength = inclusiveLength;
        }

        public Ipv4Address this[int index] => index >= 0 ? this.startAddress.AddInternal((uint)index) : throw new ArgumentOutOfRangeException(nameof(index));

        public Ipv4Address this[uint index] => this.startAddress.AddInternal(index);

        public ulong Length => this.inclusiveLength + 1;

        int ISlice<Ipv4AddressRange, Ipv4Address>.Length => (int)this.Length;

        public Ipv4AddressRange Slice(int start, int length)
        {
            if(start < 0 || length < 0) throw new ArgumentOutOfRangeException();
            return Slice((uint)start, (uint)length);
        }
        public Ipv4AddressRange Slice(int start)
        {
            if(start < 0) throw new ArgumentOutOfRangeException();
            return Slice((uint)start);
        }

        public Ipv4AddressRange Slice(ulong start) => Slice((uint)start, (uint)(Length - start));
        public Ipv4AddressRange Slice(uint start, uint length)
        {
            if (start > this.Length || length > (this.Length - start))
                throw new ArgumentOutOfRangeException();
            // Make sure the length is *INCLUSIVE* due to uint bounds.
            --length;
            return new (this.startAddress.AddInternal(start), length);
        }
        
        public Ipv4AddressEnumerator GetEnumerator() 
            => new (this.startAddress, this.inclusiveLength);

        IEnumerator IEnumerable.GetEnumerator() => new ClassEnumerator(this.startAddress, this.inclusiveLength);

        IEnumerator<Ipv4Address> IEnumerable<Ipv4Address>.GetEnumerator() => new ClassEnumerator(this.startAddress, this.inclusiveLength);

        public IEnumerable<Ipv4Address> ToEnumerable()
        {
            using var enumerator = ((IEnumerable<Ipv4Address>)this).GetEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }


        public bool Equals(Ipv4AddressRange other) => this.startAddress.Equals(other.startAddress) && this.inclusiveLength == other.inclusiveLength;
        public override bool Equals(object? obj) => obj is Ipv4AddressRange other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(this.startAddress, this.inclusiveLength);
        public static bool operator ==(Ipv4AddressRange left, Ipv4AddressRange right) => left.Equals(right);
        public static bool operator !=(Ipv4AddressRange left, Ipv4AddressRange right) => !left.Equals(right);

        public static Ipv4AddressRange Parse(string text)
            => TryParse(text, out var value)
                ? value
                : throw new FormatException();
        
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? formatProvider = null)
            => Ipv4Formatting.TryFormat(this, destination, out charsWritten, format, formatProvider);

        public bool TryFormat(Span<char> destination, out int charsWritten) 
            => TryFormat(destination, out charsWritten, default);

        public static bool TryParse(string text, out Ipv4AddressRange result)
            => TryParse(text, out var charsRead, out result) && charsRead == text.Length;
        
        public static bool TryParse(string text, out int charsRead, out Ipv4AddressRange result)
        {
            var span = new SpanWrapper(text);
            charsRead = default;
            return TryParse(ref span, ref charsRead, out result);
        }

        internal static bool TryParse(ref SpanWrapper text, ref int charsRead, out Ipv4AddressRange result)
        {
            result = default;
            if (!Ipv4Subnet.TryParse(ref text, ref charsRead, out var subnet, out var implicitSlash32))
                return false;
            result = subnet.GetAllAddresses();
            if (!implicitSlash32 || subnet.Mask.IsSlash32 == false)
                return true;
            var address = subnet.NetworkAddress;
            var textCopy = text;
            var charsReadCopy = charsRead;
            if (!textCopy.TryReadCharacter(ref charsReadCopy, '-'))
                return true;
            var lastOctet = address[3];
            if (!textCopy.TryParseByte(ref charsReadCopy, out var newOctet) || newOctet < lastOctet)
                return true;
            text = textCopy;
            charsRead = charsReadCopy;
            result = new Ipv4AddressRange(address, (uint)(newOctet - lastOctet));
            return true;
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        
        public static bool TryParse(ReadOnlySpan<char> text, out Ipv4AddressRange result)
            => TryParse(text, out var charsRead, out result) && charsRead == text.Length;

        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4AddressRange result)
        {
            var span = new SpanWrapper(text);
            charsRead = default;
            return TryParse(ref span, ref charsRead, out result);
        }
#endif

        private sealed class StartAddressInclusiveLengthEqualityComparer : IEqualityComparer<Ipv4AddressRange>
        {
            public bool Equals(Ipv4AddressRange x, Ipv4AddressRange y)
                => x == y;

            public int GetHashCode(Ipv4AddressRange obj)
                => obj.GetHashCode();
        }
    }
}