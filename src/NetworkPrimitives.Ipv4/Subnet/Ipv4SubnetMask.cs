﻿using System;
using System.Buffers.Binary;
using System.Net;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    public readonly struct Ipv4SubnetMask : IFormattableBinaryNetworkPrimitive<Ipv4SubnetMask>
    {
        internal uint Value { get; }
        private Ipv4SubnetMask(uint value) => this.Value = value;
        public bool Equals(Ipv4SubnetMask other) => this.Value == other.Value;
        public override bool Equals(object? obj) => obj is Ipv4SubnetMask other && Equals(other);
        public override int GetHashCode() => (int)this.Value;
        public static bool operator ==(Ipv4SubnetMask left, Ipv4SubnetMask right) => left.Equals(right);
        public static bool operator !=(Ipv4SubnetMask left, Ipv4SubnetMask right) => !left.Equals(right);

        public static implicit operator Ipv4Cidr(Ipv4SubnetMask mask) => Ipv4Cidr.Parse(SubnetMaskLookups.GetCidr(mask.Value));
        public static implicit operator Ipv4WildcardMask(Ipv4SubnetMask mask) => Ipv4WildcardMask.Parse(mask);
        public static implicit operator Ipv4SubnetMask(Ipv4Cidr cidr) => new (SubnetMaskLookups.GetSubnetMask(cidr.Value));
        public Ipv4Cidr ToCidr() => this;
        public Ipv4WildcardMask ToWildcardMask() => this;
        public ulong TotalHosts => SubnetMaskLookups.GetTotalHosts(this.Value);
        public uint UsableHosts => SubnetMaskLookups.GetUsableHosts(this.Value);

        public static Ipv4SubnetMask Parse(string? text)
            => TryParse(text, out var mask)
                ? mask
                : throw new FormatException();

        public static Ipv4SubnetMask Parse(uint value)
            => TryParse(value, out var mask)
                ? mask
                : throw new FormatException();

        public static bool TryParse(uint value, out Ipv4SubnetMask result)
        {
            if (SubnetMaskLookups.IsValidSubnetMask(value))
            {
                result = new (value);
                return true;
            }
            result = default;
            return false;
        }
        public bool TryParse(Ipv4WildcardMask value, out Ipv4SubnetMask result) => TryParse(~value.Value, out result);

        public bool TryWriteBytes(Span<byte> destination, out int bytesWritten) 
            => this.Value.TryWriteBigEndian(destination, out bytesWritten);
        public byte[] GetBytes() => this.Value.ToBytesBigEndian();
        int ITryFormat.MaximumLengthRequired => Ipv4Address.MAXIMUM_LENGTH;

        internal bool IsSlash32Or31 => Value is 0xFFFFFFFF or 0xFFFFFFFE;
        internal bool IsSlash32 => Value is 0xFFFFFFFF;

        public IPAddress ToIpAddress() => this.Value.ToIpAddress();
        public bool TryFormat(Span<char> destination, out int charsWritten)
        {
            charsWritten = 0;
            var result = this.GetString();
            if (destination.Length < result.Length)
                return false;
            foreach (var ch in result)
                destination.TryWrite(ch, ref charsWritten);
            return charsWritten == result.Length;
        }
        
        public override string ToString() => this.GetString();
        public string ToString(string? format, IFormatProvider? formatProvider)
            => this.GetString(format, formatProvider);
        public bool TryFormat(Span<char> destination, out int charsWritten, string? format, IFormatProvider? formatProvider)
        {
            return format switch
            {
                null or "M" => Ipv4Formatting.TryFormatDottedDecimal(this.Value, destination, out charsWritten),
                "W" => this.ToWildcardMask().TryFormat(destination, out charsWritten),
                "C" => this.ToCidr().TryFormat(destination, out charsWritten),
                _ => throw new FormatException(),
            };
        }

        public static bool TryParse(IPAddress ipAddress, out Ipv4SubnetMask result)
        {
            result = default;
            return Ipv4Address.TryParse(ipAddress, out var address)
                && TryParse(address, out result);
        }

        public static bool TryParse(ReadOnlySpan<byte> octets, out Ipv4SubnetMask result)
        {
            result = default;
            return Ipv4Address.TryParse(octets, out var address)
                && TryParse(address, out result);
        }

        public static bool TryParse(Ipv4Address address, out Ipv4SubnetMask result)
        {
            if (IsValidMask(address.Value))
            {
                result = new (address.Value);
                return true;
            }
            result = default;
            return false;
        }


        public static bool TryParse(string? text, out int charsRead, out Ipv4SubnetMask result)
        {
            result = default;
            return Ipv4Address.TryParse(text, out charsRead, out var address) && Ipv4SubnetMask.TryParse(address, out result);
        }

        private static bool IsValidMask(uint value) => SubnetMaskLookups.TryGetCidr(value, out _);

        private string GetString() => SubnetMaskLookups.GetString(Value);

        public static bool TryParse(string? text, out Ipv4SubnetMask result)
        {
            if (SubnetMaskLookups.TryGetSubnetMask(text, out var mask))
            {
                result = new (mask);
                return true;
            }
            result = default;
            return false;
        }

        internal static bool TryParse(ref SpanWrapper text, ref int charsRead, out Ipv4SubnetMask result)
        {
            var textCopy = text;
            var readCopy = charsRead;
            result = default;
            if (!Ipv4Address.TryParse(ref textCopy, ref readCopy, out var address) || !TryParse(address, out result))
                return false;
            text = textCopy;
            charsRead = readCopy;
            return true;
        }
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER

        public static Ipv4SubnetMask Parse(ReadOnlySpan<char> text)
            => TryParse(text, out var mask)
                ? mask
                : throw new FormatException();

        public static bool TryParse(ReadOnlySpan<char> text, out Ipv4SubnetMask result)
            => TryParse(text, out var charsRead, out result) && charsRead == text.Length;

        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4SubnetMask result)
        {
            result = default;
            return Ipv4Address.TryParse(text, out charsRead, out var address) && TryParse(address, out result);
        }

#endif
    }
}