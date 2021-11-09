using System;
using System.Collections;
using System.Collections.Generic;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    public readonly struct Ipv4Subnet : IEquatable<Ipv4Subnet>, IFormattable, ITryFormattable
    {
        int ITryFormat.MaximumLengthRequired => Ipv4Address.MAXIMUM_LENGTH * 2 + 1;


        public ulong TotalHosts => Mask.TotalHosts;
        public uint UsableHosts => Mask.UsableHosts;
        public Ipv4Address NetworkAddress { get; }
        public Ipv4Address BroadcastAddress => NetworkAddress.AddInternal((uint)(Mask.TotalHosts - 1));
        public Ipv4Address FirstUsable => Mask.IsSlash32Or31 ? NetworkAddress : NetworkAddress.AddInternal(1);
        public Ipv4Address LastUsable => Mask.IsSlash32Or31 ? NetworkAddress : NetworkAddress.AddInternal((uint)(Mask.TotalHosts - 2));
        public Ipv4SubnetMask Mask { get; }
        public Ipv4Subnet(Ipv4Address address, Ipv4SubnetMask mask)
        {
            this.NetworkAddress = address;
            this.Mask = mask;
        }

        public bool TrySplit(out Ipv4Subnet low, out Ipv4Subnet high)
        {
            low = default;
            high = default;
            var cidr = Mask.ToCidr();
            if (cidr.Value == 32) return false;
            var highAddress = NetworkAddress.AddInternal((uint)(TotalHosts / 2));
            cidr = Ipv4Cidr.Parse((byte)(cidr.Value + 1));
            low = new (NetworkAddress, cidr);
            high = new (highAddress, cidr);
            return true;
        }

        public bool Contains(Ipv4Address address) => (address & this.Mask) == NetworkAddress;
        
        public bool Contains(Ipv4Subnet other)
        {
            if (other.Mask.ToCidr() < this.Mask.ToCidr())
                return false;
            return (other.NetworkAddress & this.Mask) == (this.NetworkAddress & this.Mask);
        }

        public override string ToString() => this.GetString();

        public string ToString(string? format, IFormatProvider? formatProvider = null)
            => Ipv4Formatting.ToString(this, format, formatProvider);

        public void Deconstruct(out Ipv4Address networkAddress, out Ipv4SubnetMask mask)
        {
            networkAddress = this.NetworkAddress;
            mask = this.Mask;
        }
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? formatProvider = null)
            => Ipv4Formatting.TryFormat(this, destination, out charsWritten, format, formatProvider);

        public bool TryFormat(Span<char> destination, out int charsWritten) 
            => TryFormat(destination, out charsWritten, default);

        public static Ipv4Subnet Parse(string? address, string? mask) 
            => Ipv4Subnet.TryParse(address, mask, out var result)
                ? result
                : throw new FormatException();
        
        public static Ipv4Subnet Parse(string? text) 
            => Ipv4Subnet.TryParse(text, out var result)
                ? result
                : throw new FormatException();

        public static bool TryParse(string? address, string? maskOrCidr, out Ipv4Subnet result)
        {
            result = default;
            if (!Ipv4Address.TryParse(address, out var parsedAddress))
                return false;
            if (Ipv4SubnetMask.TryParse(maskOrCidr, out var mask))
            {
                result = parsedAddress + mask;
                return true;
            }
            if (!Ipv4Cidr.TryParse(maskOrCidr, out var cidr))
                return false;
            result = parsedAddress / cidr;
            return true;
        }
        
        
        public static bool TryParse(string? text, out Ipv4Subnet result)
            => Ipv4Subnet.TryParseInternal(text, out _, out result, out _);
        public static bool TryParse(string? text, out int charsRead, out Ipv4Subnet result)
            => Ipv4Subnet.TryParseInternal(text, out charsRead, out result, out _);
        internal static bool TryParseInternal(string? text, out int charsRead, out Ipv4Subnet result, out bool implicitSlash32)
        {
            var span = new SpanWrapper(text);
            charsRead = default;
            return Ipv4Subnet.TryParseInternal(ref span, ref charsRead, out result, out implicitSlash32);
        }

        internal static bool TryParseInternal(ref SpanWrapper text, ref int charsRead, out Ipv4Subnet result, out bool implicitSlash32)
        {
            implicitSlash32 = default;
            result = default;
            if (!Ipv4Address.TryParse(ref text, ref charsRead, out var address))
                return false;
            return TryParseWithCidr(ref text, ref charsRead, address, out result)
                || TryParseWithMask(ref text, ref charsRead, address, out result)
                || ParseExact(address, out result, out implicitSlash32);
        }

        private static bool ParseExact(Ipv4Address address, out Ipv4Subnet result, out bool implicitSlash32)
        {
            result = new (address, Ipv4Cidr.Parse(32));
            implicitSlash32 = true;
            return true;
        }

        private static bool TryParseWithCidr(ref SpanWrapper text, ref int charsRead, Ipv4Address address, out Ipv4Subnet result)
        {
            result = default;
            var textCopy = text;
            var charsReadCopy = charsRead;
            if (!textCopy.TryReadCharacter(ref charsReadCopy, '/'))
                return false;
            if (!Ipv4Cidr.TryParse(ref textCopy, ref charsReadCopy, out var cidr))
                return false;
            result = new (address, cidr);
            charsRead = charsReadCopy;
            text = textCopy;
            return true;
        }
        private static bool TryParseWithMask(ref SpanWrapper text, ref int charsRead, Ipv4Address address, out Ipv4Subnet result)
        {
            result = default;
            var textCopy = text;
            var charsReadCopy = charsRead;
            if (!textCopy.TryReadCharacter(ref charsReadCopy, ' '))
                return false;
            if (!Ipv4SubnetMask.TryParse(ref textCopy, ref charsReadCopy, out var mask))
                return false;
            result = new (address, mask);
            charsRead = charsReadCopy;
            text = textCopy;
            return true;
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4Subnet result)
            => TryParse(text, out charsRead, out result, out _);
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4Subnet result, out bool implicitSlash32)
        {
            var span = new SpanWrapper(text);
            charsRead = default;
            return Ipv4Subnet.TryParseInternal(ref span, ref charsRead, out result, out implicitSlash32);
        }
        
        
        public static bool TryParse(ReadOnlySpan<char> text, out Ipv4Subnet result)
            => TryParse(text, out result, out _);
        // TODO: Ambiguous invocation w/ var
        public static bool TryParse(ReadOnlySpan<char> text, out Ipv4Subnet result, out bool implicitSlash32)
            => TryParse(text, out var charsRead, out result, out implicitSlash32) && charsRead == text.Length;
#endif

        public bool Equals(Ipv4Subnet other) => this.NetworkAddress.Equals(other.NetworkAddress) && this.Mask.Equals(other.Mask);
        public override bool Equals(object? obj) => obj is Ipv4Subnet other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(this.NetworkAddress, this.Mask);
        public static bool operator ==(Ipv4Subnet left, Ipv4Subnet right) => left.Equals(right);
        public static bool operator !=(Ipv4Subnet left, Ipv4Subnet right) => !left.Equals(right);
        public Ipv4AddressRange GetUsableAddresses() => new(this.FirstUsable, this.UsableHosts - 1);
        public Ipv4AddressRange GetAllAddresses() => new (NetworkAddress, (uint)(TotalHosts - 1));
    }
}