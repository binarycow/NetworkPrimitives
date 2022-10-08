#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using NetworkPrimitives.Ipv4;
using NetworkPrimitives.Utilities;

#pragma warning disable CS1591
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

        public static Ipv6Subnet Parse(ReadOnlySpan<char> text)
            => TryParse(text, out var result) ? result : throw new FormatException();
        
        public static Ipv6Subnet Parse(string? text)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));
            return Ipv6Subnet.TryParse(text, out var result) ? result : throw new FormatException();
        }

        public static bool TryParse([NotNullWhen(true)] string? text, out Ipv6Subnet result)
        {
            result = default;
            return text is not null && TryParse(text.GetSpan(), out var length, out result) && length == text.Length;
        }
        
        public static bool TryParse(ReadOnlySpan<char> text, out Ipv6Subnet result)
            => TryParse(text, out var length, out result) && length == text.Length;
        
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv6Subnet result)
        {
            charsRead = default;
            return TryParse(ref text, ref charsRead, out result);
        }
        
        internal static bool TryParse(ref ReadOnlySpan<char> text, ref int charsRead, out Ipv6Subnet result)
        {
            result = default;
            if (!Ipv6Address.TryParse(ref text, ref charsRead, out var address))
                return false;
            return TryParseWithCidr(ref text, ref charsRead, address, out result)
                || TryParseWithMask(ref text, ref charsRead, address, out result);
        }

        public override string ToString() => $"{NetworkAddress.ToString()}/{Mask.ToCidr().ToString()}";


        private static bool TryParseWithCidr(ref ReadOnlySpan<char> text, ref int charsRead, Ipv6Address address, out Ipv6Subnet result)
        {
            result = default;
            var textCopy = text;
            var charsReadCopy = charsRead;
            if (!textCopy.TryReadCharacter(ref charsReadCopy, '/'))
                return false;
            if (!Ipv6Cidr.TryParse(ref textCopy, ref charsReadCopy, out var cidr))
                return false;
            result = new (address, cidr);
            charsRead = charsReadCopy;
            text = textCopy;
            return true;
        }
        
        
        private static bool TryParseWithMask(ref ReadOnlySpan<char> text, ref int charsRead, Ipv6Address address, out Ipv6Subnet result)
        {
            result = default;
            var textCopy = text;
            var charsReadCopy = charsRead;
            if (!textCopy.TryReadCharacter(ref charsReadCopy, ' '))
                return false;
            if (!Ipv6SubnetMask.TryParse(ref textCopy, ref charsReadCopy, out var mask))
                return false;
            result = new (address, mask);
            charsRead = charsReadCopy;
            text = textCopy;
            return true;
        }
    }
}