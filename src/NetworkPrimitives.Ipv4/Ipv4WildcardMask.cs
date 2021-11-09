﻿using System;
using System.Net;

namespace NetworkPrimitives.Ipv4
{
    public readonly struct Ipv4WildcardMask : IEquatable<Ipv4WildcardMask>, ITryFormat
    {
        public int MaximumLengthRequired => Ipv4Address.MAXIMUM_LENGTH;
        internal uint Value { get; }

        private Ipv4WildcardMask(uint value) => this.Value = value;
        public static Ipv4WildcardMask Parse(Ipv4SubnetMask mask) => new (~mask.Value);
        public bool Equals(Ipv4WildcardMask other) => this.Value == other.Value;
        public override bool Equals(object? obj) => obj is Ipv4WildcardMask other && Equals(other);
        public override int GetHashCode() => (int)this.Value;
        public static bool operator ==(Ipv4WildcardMask left, Ipv4WildcardMask right) => left.Equals(right);
        public static bool operator !=(Ipv4WildcardMask left, Ipv4WildcardMask right) => !left.Equals(right);
        
        
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

        public static Ipv4WildcardMask Parse(Ipv4Address address) => new Ipv4WildcardMask(address.Value);


        public static bool TryParse(string text, out int charsRead, out Ipv4WildcardMask result)
        {
            result = default;
            if (!Ipv4Address.TryParse(text, out charsRead, out var address))
                return false;
            result = new Ipv4WildcardMask(address.Value);
            return true;
        }
    }
}