using System;
using System.Buffers.Binary;
using System.Net;
using System.Runtime.InteropServices;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv6
{
    public readonly struct Ipv6Address : IEquatable<Ipv6Address>// , IFormattable
    {
        private readonly ulong low;
        private readonly ulong high;

        private Ipv6Address(ulong low, ulong high)
        {
            this.low = low;
            this.high = high;
        }
        

        public static Ipv6Address Parse(string? value)
            => TryParse(value, out var result) ? result : throw new FormatException();
        public static bool TryParse(IPAddress? ipAddress, out Ipv6Address result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<byte> octets, out Ipv6Address result)
        {
            throw new NotImplementedException();
        }

        public static bool TryParse(string? text, out Ipv6Address result)
            => TryParse(text, out _, out result);

        public IPAddress ToIpAddress()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
            => Ipv6Formatting.FormatIpv6Address(this.low, this.high);
        // public string ToString(string? format) 
        //     => Ipv6Formatting.FormatIpv6Address(this.low, this.high, format);
        // public string ToString(string? format, IFormatProvider? formatProvider)
        //     => Ipv6Formatting.FormatIpv6Address(this.low, this.high, format, formatProvider);

        public static bool TryParse(string? text, out int charsRead, out Ipv6Address result)
        {
            var spanWrapper = new SpanWrapper(text);
            return TryParse(spanWrapper, out charsRead, out result);
        }
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv6Address result)
        {
            var spanWrapper = new SpanWrapper(text);
            return TryParse(spanWrapper, out charsRead, out result);
        }
#endif



        internal static bool TryParse(SpanWrapper text, out int charsRead, out Ipv6Address result)
        {
            result = default;
            charsRead = default;
            var doubleColonPos = text.IndexOf("::");
            if (doubleColonPos == -1)
                return TryParseUnabbreviated(text, out charsRead, out result);
            var highSpan = text[..doubleColonPos];
            var lowSpan = text[(doubleColonPos + 2)..];
            var highGroupCount = GetColonCount(highSpan) + 1;
            var lowGroupCount = GetColonCount(lowSpan) + 1;
            Span<ushort> bitGroups = stackalloc ushort[8];
            if (highGroupCount + lowGroupCount > 8)
            {
                return false;
            }
            var highGroups = bitGroups[..highGroupCount];
            var lowGroups = bitGroups[^lowGroupCount..];
            if (!TryFillBitGroups(highSpan, ref charsRead, highGroups))
            {
                return false;
            }
            if (!TryFillBitGroups(lowSpan, ref charsRead, lowGroups))
            {
                return false;
            }
            return Create(bitGroups, out result);

            static bool TryFillBitGroups(SpanWrapper text, ref int charsRead, Span<ushort> bitGroups)
            {
                while (bitGroups.Length > 0 && text.TryParseHexUshort(ref charsRead, out var bitGroup))
                {
                    bitGroups[0] = bitGroup;
                    bitGroups = bitGroups[1..];
                    if (text.Length == 0 || text[0] != ':')
                        break;
                    text = text[1..];
                    ++charsRead;
                }
                return bitGroups.Length == 0;
            }
            static int GetColonCount(SpanWrapper text)
            {
                var ct = 0;
                for (var i = 0; i < text.Length; ++i)
                {
                    if (text[i] == ':')
                        ++ct;
                }
                return ct;
            }
        }

        internal static bool TryParse(ref SpanWrapper text, ref int charsRead, out Ipv6Address result)
        {
            if (!Ipv6Address.TryParse(text, out var length, out result)) 
                return false;
            charsRead += length;
            text = text[length..];
            return true;
        }

        internal static bool TryParseUnabbreviated(SpanWrapper text, out int charsRead, out Ipv6Address result)
        {
            charsRead = default;
            Span<ushort> bitGroups = stackalloc ushort[8];
            // Read the first bit groups
            var startLength = 0;
            while (text.TryParseHexUshort(ref charsRead, out var bitGroup))
            {
                bitGroups[startLength++] = bitGroup;
                if (text.Length == 0 || text[0] != ':')
                    break;
                text = text[1..];
                ++charsRead;
            }
            if (startLength == 8) 
                return Ipv6Address.Create(bitGroups, out result);
            result = default;
            return false;
        }

        private static bool Create(Span<ushort> bitGroups, out Ipv6Address result)
        {
            result = new (Convert(bitGroups[4..]), Convert(bitGroups[..4]));
            return true;
            static ulong Convert(Span<ushort> ushorts) 
                => BinaryPrimitives.ReadUInt64BigEndian(
                    MemoryMarshal.Cast<ushort, Byte>(ushorts)
            );
        }


        public bool Equals(Ipv6Address other) => this.low == other.low && this.high == other.high;
        public override bool Equals(object? obj) => obj is Ipv6Address other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(this.low, this.high);

        public static bool operator ==(Ipv6Address left, Ipv6Address right) => left.Equals(right);
        public static bool operator !=(Ipv6Address left, Ipv6Address right) => !left.Equals(right);
    }   
}
