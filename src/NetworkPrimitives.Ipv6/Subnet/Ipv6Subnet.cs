#nullable enable

using System;

namespace NetworkPrimitives.Ipv6
{
    public readonly struct Ipv6Subnet : IEquatable<Ipv6Subnet>
    {
        public Ipv6Address NetworkAddress { get; }
        public Ipv6SubnetMask Mask { get; }        
        public Ipv6Subnet(Ipv6Address address, Ipv6SubnetMask mask)
        {
            this.NetworkAddress = address & mask;
            this.Mask = mask;
        }

        public bool Equals(Ipv6Subnet other) => this.NetworkAddress.Equals(other.NetworkAddress) && this.Mask.Equals(other.Mask);
        public override bool Equals(object? obj) => obj is Ipv6Subnet other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(this.NetworkAddress, this.Mask);
        public static bool operator ==(Ipv6Subnet left, Ipv6Subnet right) => left.Equals(right);
        public static bool operator !=(Ipv6Subnet left, Ipv6Subnet right) => !left.Equals(right);
    }
}