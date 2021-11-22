using System;
using System.Net;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    public readonly struct Ipv4WildcardMask : IBinaryNetworkPrimitive<Ipv4WildcardMask>
    {
        internal uint Value { get; }

        private Ipv4WildcardMask(uint value) => this.Value = value;
        public static Ipv4WildcardMask Parse(Ipv4SubnetMask mask) => new (~mask.Value);
        public bool Equals(Ipv4WildcardMask other) => this.Value == other.Value;
        public override bool Equals(object? obj) => obj is Ipv4WildcardMask other && Equals(other);
        public override int GetHashCode() => (int)this.Value;
        public static bool operator ==(Ipv4WildcardMask left, Ipv4WildcardMask right) => left.Equals(right);
        public static bool operator !=(Ipv4WildcardMask left, Ipv4WildcardMask right) => !left.Equals(right);

        public bool IsSubnetMask(out Ipv4SubnetMask mask) => Ipv4SubnetMask.TryParse(~Value, out mask);

        public bool TryFormat(Span<char> destination, out int charsWritten) 
            => Ipv4Formatting.TryFormatDottedDecimal(this.Value, destination, out charsWritten);

        public IPAddress ToIpAddress() => this.Value.ToIpAddress();

        public bool TryWriteBytes(Span<byte> destination, out int bytesWritten) 
            => this.Value.TryWriteBigEndian(destination, out bytesWritten);
        public byte[] GetBytes() => this.Value.ToBytesBigEndian();
        int ITryFormat.MaximumLengthRequired => Ipv4Address.MAXIMUM_LENGTH;
        
        public override string ToString() => this.GetString();

        public ulong HostCount => this.Value switch
        {
            0x00000000 => 4294967296,
            0xFFFFFFFF => 1,
            _ => (uint)(1 << (int)this.Value.PopCount()),
        };


        public static bool TryParse(IPAddress ipAddress, out Ipv4WildcardMask result)
        {
            result = default;
            if (!Ipv4Address.TryParse(ipAddress, out var address))
                return false;
            result = new Ipv4WildcardMask(address.Value);
            return true;
        }

        public static bool TryParse(ReadOnlySpan<byte> octets, out Ipv4WildcardMask result)
        {
            result = default;
            if (!Ipv4Address.TryParse(octets, out var address))
                return false;
            result = new Ipv4WildcardMask(address.Value);
            return true;
        }

        public static Ipv4WildcardMask Parse(Ipv4Address address) => new (address.Value);


        public static Ipv4WildcardMask Parse(string? value)
            => TryParse(value, out var result) ? result : throw new FormatException();

        public static bool TryParse(string? text, out Ipv4WildcardMask result)
            => TryParse(text, out var charsRead, out result) && charsRead == text?.Length;

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER

        public static bool TryParse(ReadOnlySpan<char> text, out Ipv4WildcardMask result)
            => TryParse(text, out var charsRead, out result) && charsRead == text.Length;

        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4WildcardMask result)
            => TryParse(new SpanWrapper(text), out charsRead, out result);

        public static Ipv4WildcardMask Parse(ReadOnlySpan<char> value)
            => TryParse(value, out var result) ? result : throw new FormatException();

#endif


        public static bool TryParse(string? text, out int charsRead, out Ipv4WildcardMask result) 
            => TryParse(new SpanWrapper(text), out charsRead, out result);


        internal static bool TryParse(SpanWrapper text, out int charsRead, out Ipv4WildcardMask result)
        {
            charsRead = default;
            return TryParse(ref text, ref charsRead, out result);
        }

        internal static bool TryParse(ref SpanWrapper text, ref int charsRead, out Ipv4WildcardMask result)
        {
            result = default;
            if (!Ipv4Address.TryParse(ref text, ref charsRead, out var address))
                return false;
            result = new (address.Value);
            return true;
        }
    }
}