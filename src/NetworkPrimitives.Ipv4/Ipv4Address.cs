﻿using System;
using System.Net;
using System.Net.Sockets;
using NetworkPrimitives.Ipv4;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    public readonly struct Ipv4Address : IEquatable<Ipv4Address>, ITryFormat
    {
        internal const int MINIMUM_LENGTH = 7; // 1.1.1.1
        internal const int MAXIMUM_LENGTH = 15; // 123.123.123.123
        
        private Ipv4Address(uint value) => this.Value = value;
        internal uint Value { get; }

        public uint BigEndianValue => Value;

        public bool Equals(Ipv4Address other) => this.Value == other.Value;
        public override bool Equals(object? obj) => obj is Ipv4Address other && Equals(other);
        public override int GetHashCode() => (int)this.Value;
        public static bool operator ==(Ipv4Address left, Ipv4Address right) => left.Equals(right);
        public static bool operator !=(Ipv4Address left, Ipv4Address right) => !left.Equals(right);


        public static Ipv4Address operator &(Ipv4Address left, Ipv4Cidr right) => left & right.ToSubnetMask();
        public static Ipv4Address operator &(Ipv4Address left, Ipv4SubnetMask right) => new (left.Value & right.Value);
        public static Ipv4Subnet operator +(Ipv4Address left, Ipv4SubnetMask right) => new (left, right);
        public static Ipv4Subnet operator +(Ipv4Address left, Ipv4Cidr right) => new (left, right);
        public static Ipv4Subnet operator /(Ipv4Address left, Ipv4Cidr right) => new (left, right);

        
        
        internal Ipv4Address AddInternal(uint delta) => new (Value + delta);
        internal Ipv4Address SubtractInternal(uint delta) => new (Value - delta);

        public bool IsInSubnet(Ipv4Subnet subnet) => subnet.Contains(this);

        public byte GetOctet(int index)
        {
            if (index is < 0 or > 3)
                throw new ArgumentOutOfRangeException(nameof(index), index, $"{nameof(index)} must be between 0 and 3, inclusive.");
            Span<byte> bytes = stackalloc byte[4];
            TryWriteBytes(bytes, out _);
            return bytes[index];
        }

        internal byte this[int index] => GetOctet(index);
        
        int ITryFormat.MaximumLengthRequired => MAXIMUM_LENGTH;

        public bool TryWriteBytes(Span<byte> destination, out int bytesWritten) 
            => this.Value.TryWriteBigEndian(destination, out bytesWritten);

        public bool TryFormat(Span<char> destination, out int charsWritten)
            => Ipv4Formatting.TryFormat(this, destination, out charsWritten);

        public override string ToString() => this.GetString();

        public static Ipv4Address Parse(uint value)
            => new Ipv4Address(value);
        public static Ipv4Address Parse(string value)
            => TryParse(value, out var result) ? result : throw new FormatException();
        public static bool TryParse(IPAddress ipAddress, out Ipv4Address result)
        {
            result = default;
            if (ipAddress.AddressFamily != AddressFamily.InterNetwork)
                return false;
            Span<byte> octets = stackalloc byte[4];
            return ipAddress.TryWriteBytes(octets, out var written) 
                && written == 4
                && TryParse(octets, out result);
        }

        public static bool TryParse(ReadOnlySpan<byte> octets, out Ipv4Address result)
        {
            result = default;
            return octets.TryReadUInt32BigEndian(out var value)
                && TryParse(value, out result);
        }

        public static bool TryParse(string text, out Ipv4Address result)
        {
            result = default;
            return Ipv4Parsing.TryParseDottedDecimalUInt32(text, out var value)
                && TryParse(value, out result);
        }

        public IPAddress ToIpAddress()
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            Span<byte> bytes = stackalloc byte[4];
#else
            var bytes = new byte[4];
#endif
            TryWriteBytes(bytes, out _);
            return new (bytes);
        }

        public static bool TryParse(string text, out int charsRead, out Ipv4Address result)
        {
            result = default;
            return Ipv4Parsing.TryParseDottedDecimalUInt32(text, out charsRead, out var value)
                && TryParse(value, out result);
        }

        internal static bool TryParse(uint value, out Ipv4Address result)
        {
            result = new Ipv4Address(value);
            return true;
        }
        internal static bool TryParse(ref SpanWrapper text, ref int charsRead, out Ipv4Address result)
        {
            result = default;
            return Ipv4Parsing.TryParseDottedDecimalUInt32(ref text, ref charsRead, out var value)
                && TryParse(value, out result);
        }
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4Address result)
        {
            result = default;
            return Ipv4Parsing.TryParseDottedDecimalUInt32(text, out charsRead, out var value)
                && TryParse(value, out result);
        }
#endif
    }

}