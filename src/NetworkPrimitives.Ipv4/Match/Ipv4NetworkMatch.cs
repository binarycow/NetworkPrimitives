using System;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    public readonly struct Ipv4NetworkMatch : INetworkPrimitive<Ipv4NetworkMatch>
    {
        public Ipv4Address Address { get; }
        public Ipv4WildcardMask Mask { get; }

        int ITryFormat.MaximumLengthRequired => Ipv4Address.MAXIMUM_LENGTH + Ipv4Address.MAXIMUM_LENGTH + 1;

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

        public override string ToString() => this.GetString();
        public static Ipv4NetworkMatch Parse(string? value)
            => TryParse(value, out var result) ? result : throw new FormatException();

        public static bool TryParse(string? text, out Ipv4NetworkMatch result)
            => TryParse(text, out var charsRead, out result) && charsRead == text?.Length;

        public static bool TryParse(string? text, out int charsRead, out Ipv4NetworkMatch result)
            => TryParse(new SpanWrapper(text), out charsRead, out result);

        public bool TryFormat(Span<char> destination, out int charsWritten) 
            => Ipv4Formatting.TryFormat(this, destination, out charsWritten);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER

        public static Ipv4NetworkMatch Parse(ReadOnlySpan<char> value)
            => TryParse(value, out var result) ? result : throw new FormatException();


        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4NetworkMatch result)
            => TryParse(new SpanWrapper(text), out charsRead, out result);

        public static bool TryParse(ReadOnlySpan<char> text, out Ipv4NetworkMatch result)
            => TryParse(new SpanWrapper(text), out var charsRead, out result) && charsRead == text.Length;
#endif

        internal static bool TryParse(SpanWrapper text, out int charsRead, out Ipv4NetworkMatch result)
        {
            result = default;
            charsRead = default;
            if (!Ipv4Address.TryParse(ref text, ref charsRead, out var address))
                return false;
            if (!text.TryReadCharacter(ref charsRead, ' '))
                return false;
            if (!Ipv4WildcardMask.TryParse(ref text, ref charsRead, out var mask))
                return false;
            result = new (address, mask);
            return true;
        }

        public static bool operator ==(Ipv4NetworkMatch left, Ipv4NetworkMatch right) => left.Equals(right);
        public static bool operator !=(Ipv4NetworkMatch left, Ipv4NetworkMatch right) => !left.Equals(right);


    }
}