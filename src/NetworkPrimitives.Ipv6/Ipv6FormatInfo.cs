using System;

namespace NetworkPrimitives.Ipv6
{
    internal readonly struct Ipv6FormatInfo : IEquatable<Ipv6FormatInfo>
    {
        public bool Compress { get; }
        public bool CompressGroups { get; }
        public bool Hex { get; }
        public bool Upper { get; }

        public Ipv6FormatInfo(bool compress, bool compressGroups, bool hex, bool upper)
        {
            this.Compress = compress;
            this.CompressGroups = compressGroups;
            this.Hex = hex;
            this.Upper = upper;
        }

        public void Deconstruct(out bool compress, out bool compressGroups, out bool hex, out bool upper)
        {
            compress = this.Compress;
            compressGroups = this.CompressGroups;
            hex = this.Hex;
            upper = this.Upper;
        }

        public bool Equals(Ipv6FormatInfo other) => this.Compress == other.Compress && this.CompressGroups == other.CompressGroups && this.Hex == other.Hex && this.Upper == other.Upper;
        public override bool Equals(object? obj) => obj is Ipv6FormatInfo other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(this.Compress, this.CompressGroups, this.Hex, this.Upper);
        public static bool operator ==(Ipv6FormatInfo left, Ipv6FormatInfo right) => left.Equals(right);
        public static bool operator !=(Ipv6FormatInfo left, Ipv6FormatInfo right) => !left.Equals(right);
    }
}