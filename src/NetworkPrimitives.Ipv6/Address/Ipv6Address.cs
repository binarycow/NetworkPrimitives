using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv6
{
    public readonly struct Ipv6Address : IEquatable<Ipv6Address>// , IFormattable
    {
        internal readonly ulong Low;
        internal readonly ulong High;

        private Ipv6Address(ulong low, ulong high)
        {
            this.Low = low;
            this.High = high;
        }

        internal void Deconstruct(out ulong low, out ulong high)
        {
            low = this.Low;
            high = this.High;
        }

        public static Ipv6Address operator &(Ipv6Address address, Ipv6SubnetMask mask)
            => new (address.Low & mask.Low, address.High & mask.High);
        public static Ipv6Subnet operator +(Ipv6Address address, Ipv6SubnetMask mask)
            => new (address, mask);
        public static Ipv6Subnet operator /(Ipv6Address address, Ipv6Cidr cidr)
            => new (address, cidr);
        
        public static Ipv6Address Parse(Ipv6SubnetMask value) => new (value.Low, value.High);


        public static Ipv6Address Parse(string? value)
            => TryParse(value, out var result) ? result : throw new FormatException();
        public static bool TryParse(IPAddress? ipAddress, out Ipv6Address result)
        {
            result = default;
            if (ipAddress is null || ipAddress.AddressFamily != AddressFamily.InterNetworkV6) return false;
            Span<byte> bytes = stackalloc byte[16];
            return ipAddress.TryWriteBytes(bytes, out _) && TryParse(bytes, out result);
        }

        public static bool TryParse(ReadOnlySpan<byte> octets, out Ipv6Address result)
        {
            result = default;
            if (octets.Length < 16)
                return false;
            var longs = MemoryMarshal.Cast<byte, ulong>(octets);
            result = new Ipv6Address(longs[0], longs[1]);
            return true;
        }

        public static bool TryParse(string? text, out Ipv6Address result)
            => TryParse(text, out _, out result);

        public IPAddress ToIpAddress()
        {
            Span<ulong> longs = stackalloc ulong[2];
            longs[0] = this.High;
            longs[1] = this.Low;
            var octets = MemoryMarshal.Cast<ulong, byte>(longs);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            return new (octets);
#else
            return new (octets.ToArray() ?? Array.Empty<byte>());
#endif
        }

        public override string ToString()
            => Ipv6Formatting.FormatIpv6Address(this.Low, this.High);

        public static bool TryParse(string? text, out int charsRead, out Ipv6Address result)
        {
            var spanWrapper = text.GetSpan();
            return TryParse(spanWrapper, out charsRead, out result);
        }
        internal static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv6Address result)
        {
            if (Ipv6Parsing.TryParseIpv6Address(text, out charsRead, out var high, out var low))
            {
                result = new Ipv6Address(low, high);
                return true;
            }
            result = default;
            return false;
        }

        internal static bool TryParse(ref ReadOnlySpan<char> text, ref int charsRead, out Ipv6Address result)
        {
            if (!Ipv6Address.TryParse(text, out var length, out result)) 
                return false;
            charsRead += length;
            text = text[length..];
            return true;
        }



        public bool Equals(Ipv6Address other) => this.Low == other.Low && this.High == other.High;
        public override bool Equals(object? obj) => obj is Ipv6Address other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(this.Low, this.High);

        public static bool operator ==(Ipv6Address left, Ipv6Address right) => left.Equals(right);
        public static bool operator !=(Ipv6Address left, Ipv6Address right) => !left.Equals(right);
    }   
}
