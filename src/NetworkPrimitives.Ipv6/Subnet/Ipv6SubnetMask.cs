using System;
using System.Net;

namespace NetworkPrimitives.Ipv6
{
    public readonly struct Ipv6SubnetMask : IEquatable<Ipv6SubnetMask>
    {
        internal readonly ulong Low;
        internal readonly ulong High;
        public Ipv6SubnetMask(ulong low, ulong high)
        {
            this.Low = low;
            this.High = high;
        }
        internal void Deconstruct(out ulong low, out ulong high)
        {
            low = this.Low;
            high = this.High;
        }

        public static implicit operator Ipv6SubnetMask(Ipv6Cidr cidr)
        {
            SubnetMaskLookups.TryConvertIpv6Address((byte)cidr, out var high, out var low);
            return new (low, high);
        }

        public static bool TryParse(Ipv6Address address, out Ipv6SubnetMask result)
        {
            var (low, high) = address;
            if (SubnetMaskLookups.IsValidSubnetMask(high, low))
            {
                result = new (low, high);
                return true;
            }
            result = default;
            return false;
        }
        
        public static Ipv6SubnetMask Parse(string? value)
            => TryParse(value, out var result) ? result : throw new FormatException();
        public static bool TryParse(IPAddress? ipAddress, out Ipv6SubnetMask result)
        {
            result = default;
            return Ipv6Address.TryParse(ipAddress, out var address)
                && TryParse(address, out result);
        }

        public static bool TryParse(ReadOnlySpan<byte> octets, out Ipv6SubnetMask result)
        {
            result = default;
            return Ipv6Address.TryParse(octets, out var address)
                && TryParse(address, out result);
        }

        public static bool TryParse(string? text, out Ipv6SubnetMask result)
            => TryParse(text, out _, out result);

        public IPAddress ToIpAddress() => Ipv6Address.Parse(this).ToIpAddress();

        public override string ToString()
            => Ipv6Formatting.FormatIpv6Address(this.Low, this.High);

        public static bool TryParse(string? text, out int charsRead, out Ipv6SubnetMask result)
        {
            result = default;
            return Ipv6Address.TryParse(text, out charsRead, out var address)
                && TryParse(address, out result);
        }
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv6SubnetMask result)
        {
            result = default;
            return Ipv6Address.TryParse(text, out charsRead, out var address)
                && TryParse(address, out result);
        }
#endif




        public bool Equals(Ipv6SubnetMask other) => this.Low == other.Low && this.High == other.High;
        public override bool Equals(object? obj) => obj is Ipv6SubnetMask other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(this.Low, this.High);
        public static bool operator ==(Ipv6SubnetMask left, Ipv6SubnetMask right) => left.Equals(right);
        public static bool operator !=(Ipv6SubnetMask left, Ipv6SubnetMask right) => !left.Equals(right);
    }
}