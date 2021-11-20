using System;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    public readonly struct Ipv4Cidr : INetworkPrimitive<Ipv4Cidr>
    {
        internal byte Value { get; }
        private Ipv4Cidr(byte value) => this.Value = value;
        int ITryFormat.MaximumLengthRequired => 2;

        public bool TryFormat(Span<char> destination, out int charsWritten) 
            => Value.TryFormat(destination, out charsWritten);

        public override string ToString() => this.GetString();

        public static explicit operator byte(Ipv4Cidr cidr) => cidr.Value;

        public bool Equals(Ipv4Cidr other) => this.Value == other.Value;
        public override bool Equals(object? obj) => obj switch
        {
            Ipv4Cidr other => Equals(other),
            byte other => this.Value == other,
            _ => false,
        };
        public override int GetHashCode() => this.Value.GetHashCode();
        public static bool operator ==(Ipv4Cidr left, Ipv4Cidr right) => left.Equals(right);
        public static bool operator !=(Ipv4Cidr left, Ipv4Cidr right) => !left.Equals(right);
        public static bool operator >(Ipv4Cidr left, Ipv4Cidr right) => left.Value > right.Value;
        public static bool operator <(Ipv4Cidr left, Ipv4Cidr right) => left.Value < right.Value;
        public static bool operator >=(Ipv4Cidr left, Ipv4Cidr right) => left.Value >= right.Value;
        public static bool operator <=(Ipv4Cidr left, Ipv4Cidr right) => left.Value <= right.Value;
        
        public static bool operator ==(Ipv4Cidr left, byte right) => left.Value == right;
        public static bool operator !=(Ipv4Cidr left, byte right) => left.Value != right;
        public static bool operator >(Ipv4Cidr left, byte right) => left.Value > right;
        public static bool operator <(Ipv4Cidr left, byte right) => left.Value < right;
        public static bool operator >=(Ipv4Cidr left, byte right) => left.Value >= right;
        public static bool operator <=(Ipv4Cidr left, byte right) => left.Value <= right;
        
        public static bool operator ==(byte left, Ipv4Cidr right) => left == right.Value;
        public static bool operator !=(byte left, Ipv4Cidr right) => left != right.Value;
        public static bool operator >(byte left, Ipv4Cidr right) => left > right.Value;
        public static bool operator <(byte left, Ipv4Cidr right) => left < right.Value;
        public static bool operator >=(byte left, Ipv4Cidr right) => left >= right.Value;
        public static bool operator <=(byte left, Ipv4Cidr right) => left <= right.Value;
        
        public ulong TotalHosts => SubnetMaskLookups.GetTotalHosts(this.Value);
        public uint UsableHosts => SubnetMaskLookups.GetUsableHosts(this.Value);


        public static Ipv4Cidr Parse(string? value)
            => TryParse(value, out var result) ? result : throw new FormatException();

        public static Ipv4Cidr Parse(int value)
            => Parse((byte)value);
        public static Ipv4Cidr Parse(byte value)
            => TryParse(value, out var result) ? result : throw new ArgumentOutOfRangeException();
        public static bool TryParse(byte value, out Ipv4Cidr result)
        {
            if (value <= 32)
            {
                result = new (value);
                return true;
            }
            result = default;
            return false;
        }

        public static bool TryParse(string? text, out Ipv4Cidr result)
        {
            result = default;
            return text is not null && Ipv4Cidr.TryParse(text, out var charsRead, out result) && charsRead == text.Length;
        }

        public static bool TryParse(string? text, out int charsRead, out Ipv4Cidr result)
        {
            var span = new SpanWrapper(text);
            charsRead = default;
            return TryParse(ref span, ref charsRead, out result);
        }
        
        internal static bool TryParse(ref SpanWrapper text, ref int charsRead, out Ipv4Cidr result)
        {
            result = default;
            if (!text.TryParseByte(ref charsRead, out var value) || value > 32)
                return false;
            result = new (value);
            return true;
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        public static Ipv4Cidr Parse(ReadOnlySpan<char> text)
            => TryParse(text, out var result) ? result : throw new FormatException();
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4Cidr result)
        {
            var span = new SpanWrapper(text);
            charsRead = default;
            return TryParse(ref span, ref charsRead, out result);
        }
        public static bool TryParse(ReadOnlySpan<char> text, out Ipv4Cidr result)
            => TryParse(text, out var charsRead, out result) && charsRead == text.Length;
#endif
        public Ipv4SubnetMask ToSubnetMask() => this;
    }
}