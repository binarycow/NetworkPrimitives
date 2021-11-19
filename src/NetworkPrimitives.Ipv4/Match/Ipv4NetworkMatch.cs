using System;

namespace NetworkPrimitives.Ipv4
{
    public readonly struct Ipv4NetworkMatch : IEquatable<Ipv4NetworkMatch>
    {
        public Ipv4Address Address { get; }
        public Ipv4WildcardMask Mask { get; }

        public Ipv4NetworkMatch(Ipv4Address address, Ipv4WildcardMask mask)
        {
            this.Address = address;
            this.Mask = mask;
        }

        public bool IsMatch(Ipv4Address address) => (address & Mask) == (Address & Mask);
        public static bool IsMatch(Ipv4NetworkMatch match, Ipv4Address address) => match.IsMatch(address);

        public bool Equals(Ipv4NetworkMatch other) => this.Address.Equals(other.Address) && this.Mask.Equals(other.Mask);
        public override bool Equals(object? obj) => obj is Ipv4NetworkMatch other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(this.Address, this.Mask);
        public static bool operator ==(Ipv4NetworkMatch left, Ipv4NetworkMatch right) => left.Equals(right);
        public static bool operator !=(Ipv4NetworkMatch left, Ipv4NetworkMatch right) => !left.Equals(right);
    }
}