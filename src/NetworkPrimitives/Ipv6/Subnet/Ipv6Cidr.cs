using System;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv6
{
    public readonly struct Ipv6Cidr : ITryFormat, IEquatable<Ipv6Cidr>
    {
        internal byte Value { get; }
        private Ipv6Cidr(byte value) => this.Value = value;
        int ITryFormat.MaximumLengthRequired => 3;

        public static implicit operator Ipv6Cidr(Ipv6SubnetMask mask)
        {
            var (low, high) = mask;
            var cidr = SubnetMaskLookups.GetCidr(high, low);
            return new Ipv6Cidr(cidr);
        }
        public bool TryFormat(Span<char> destination, out int charsWritten) 
            => Value.TryFormat(destination, out charsWritten);

        public override string ToString() => this.GetString();
        public static explicit operator byte(Ipv6Cidr cidr) => cidr.Value;

        public bool Equals(Ipv6Cidr other) => this.Value == other.Value;
        public override bool Equals(object? obj) => obj is Ipv6Cidr other && Equals(other);
        public override int GetHashCode() => this.Value.GetHashCode();
        public static bool operator ==(Ipv6Cidr left, Ipv6Cidr right) => left.Equals(right);
        public static bool operator !=(Ipv6Cidr left, Ipv6Cidr right) => !left.Equals(right);
        public static bool operator >(Ipv6Cidr left, Ipv6Cidr right) => left.Value > right.Value;
        public static bool operator <(Ipv6Cidr left, Ipv6Cidr right) => left.Value < right.Value;
        public static bool operator >=(Ipv6Cidr left, Ipv6Cidr right) => left.Value >= right.Value;
        public static bool operator <=(Ipv6Cidr left, Ipv6Cidr right) => left.Value <= right.Value;
        
        public static bool operator ==(Ipv6Cidr left, byte right) => left.Value == right;
        public static bool operator !=(Ipv6Cidr left, byte right) => left.Value != right;
        public static bool operator >(Ipv6Cidr left, byte right) => left.Value > right;
        public static bool operator <(Ipv6Cidr left, byte right) => left.Value < right;
        public static bool operator >=(Ipv6Cidr left, byte right) => left.Value >= right;
        public static bool operator <=(Ipv6Cidr left, byte right) => left.Value <= right;
        
        public static bool operator ==(byte left, Ipv6Cidr right) => left == right.Value;
        public static bool operator !=(byte left, Ipv6Cidr right) => left != right.Value;
        public static bool operator >(byte left, Ipv6Cidr right) => left > right.Value;
        public static bool operator <(byte left, Ipv6Cidr right) => left < right.Value;
        public static bool operator >=(byte left, Ipv6Cidr right) => left >= right.Value;
        public static bool operator <=(byte left, Ipv6Cidr right) => left <= right.Value;

        public static Ipv6Cidr Parse(string? value)
            => TryParse(value, out var result) ? result : throw new FormatException();

        public static Ipv6Cidr Parse(int value)
            => Parse((byte)value);
        public static Ipv6Cidr Parse(byte value)
            => TryParse(value, out var result) ? result : throw new ArgumentOutOfRangeException();
        public static bool TryParse(byte value, out Ipv6Cidr result)
        {
            if (value <= 128)
            {
                result = new (value);
                return true;
            }
            result = default;
            return false;
        }

        public static bool TryParse(string? text, out Ipv6Cidr result)
        {
            result = default;
            return text is not null && Ipv6Cidr.TryParse(text, out var charsRead, out result) && charsRead == text.Length;
        }

        public static bool TryParse(string? text, out int charsRead, out Ipv6Cidr result)
        {
            charsRead = default;
            var span = text.GetSpan();
            return TryParse(ref span, ref charsRead, out result);
        }
        
        internal static bool TryParse(ref ReadOnlySpan<char> text, ref int charsRead, out Ipv6Cidr result)
        {
            result = default;
            if (!text.TryParseByte(ref charsRead, out var value) || value > 128)
                return false;
            result = new (value);
            return true;
        }

        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv6Cidr result)
        {
            charsRead = default;
            return TryParse(ref text, ref charsRead, out result);
        }
        public static bool TryParse(ReadOnlySpan<char> text, out Ipv6Cidr result)
            => TryParse(text, out var charsRead, out result) && charsRead == text.Length;
        
        public Ipv6SubnetMask ToSubnetMask() => this;
    }
}