using System;
using System.Net;
using System.Net.Sockets;
using NetworkPrimitives.Ipv4;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    /// <summary>
    /// Represents an IPv4 address
    /// </summary>
    public readonly struct Ipv4Address 
        : IComparable<Ipv4Address>, IComparable, IBinaryNetworkPrimitive<Ipv4Address>
    {
        [ExcludeFromCodeCoverage]
        internal Ipv4Address AddInternal(uint delta) => new (Value + delta);
        [ExcludeFromCodeCoverage]
        internal Ipv4Address SubtractInternal(uint delta) => new (Value - delta);
        
        /// <summary>
        /// Converts a <see cref="Ipv4Address"/> instance to a <see cref="IPAddress"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="IPAddress"/> instance that represents this <see cref="Ipv4Address"/>.
        /// </returns>
        public IPAddress ToIpAddress() => this.Value.ToIpAddress();
        
        private Ipv4Address(uint value) => this.Value = value;
        internal uint Value { get; }

        /// <summary>
        /// The value of the address as an unsigned, 32-bit integer, in network order (big endian)
        /// </summary>
        [ExcludeFromCodeCoverage]
        public uint BigEndianValue => Value;
        
        /// <summary>
        /// The value of the address as an unsigned, 32-bit integer, as little-endian
        /// </summary>
        [ExcludeFromCodeCoverage]
        public uint LittleEndianValue => Value.SwapEndianIfLittleEndian();

        #region Fields

        internal const int MINIMUM_LENGTH = 7; // 1.1.1.1
        internal const int MAXIMUM_LENGTH = 15; // 123.123.123.123
        
        /// <summary>
        /// Represents the Ipv4 address of 0.0.0.0
        /// </summary>
        public static readonly Ipv4Address Any = new (uint.MinValue);
        
        /// <summary>
        /// Represents the Ipv4 address of 255.255.255.255
        /// </summary>
        public static readonly Ipv4Address Broadcast = new (uint.MaxValue);
        
        /// <summary>
        /// Represents the Ipv4 address of 127.0.0.1
        /// </summary>
        public static readonly Ipv4Address LocalHost = new (0x7F000000);

        #endregion Fields
        
        #region Equality

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4Address"/>.
        /// </summary>
        /// <param name="other">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Ipv4Address other) => this.Value == other.Value;
        
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4Address"/>.
        /// </summary>
        /// <param name="obj">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is an instance of <see cref="Ipv4Address"/> and equals the
        /// value of this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj is Ipv4Address other && Equals(other);
        
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => (int)this.Value;

        #endregion Equality

        #region Subnetting

        /// <summary>
        /// Perform a bitwise-and operation with an <see cref="Ipv4Address"/> and an IPv4 subnet mask (as <see cref="Ipv4Cidr"/>).
        /// </summary>
        /// <param name="left">
        /// The <see cref="Ipv4Address"/>.
        /// </param>
        /// <param name="right">
        /// The <see cref="Ipv4Cidr"/>.
        /// </param>
        /// <returns>
        /// The result of the bitwise-and operation.
        /// </returns>
        public static Ipv4Address operator &(Ipv4Address left, Ipv4Cidr right) => left & right.ToSubnetMask();
        
        /// <summary>
        /// Perform a bitwise-and operation with an <see cref="Ipv4Address"/> and an IPv4 subnet mask.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Ipv4Address"/>.
        /// </param>
        /// <param name="right">
        /// The <see cref="Ipv4SubnetMask"/>.
        /// </param>
        /// <returns>
        /// The result of the bitwise-and operation.
        /// </returns>
        public static Ipv4Address operator &(Ipv4Address left, Ipv4SubnetMask right) => new (left.Value & right.Value);
        
        /// <summary>
        /// Perform a bitwise-and operation with an <see cref="Ipv4Address"/> and an IPv4 wildcard mask.
        /// </summary>
        /// <param name="left">
        /// The <see cref="Ipv4Address"/>.
        /// </param>
        /// <param name="right">
        /// The <see cref="Ipv4WildcardMask"/>.
        /// </param>
        /// <returns>
        /// The result of the bitwise-and operation.
        /// </returns>
        public static Ipv4Address operator &(Ipv4Address left, Ipv4WildcardMask right) => new (left.Value & right.Value);
        
        
        /// <summary>
        /// Creates an <see cref="Ipv4Subnet"/> with an <see cref="Ipv4Address"/> and <see cref="Ipv4SubnetMask"/>
        /// </summary>
        /// <param name="left">
        /// The <see cref="Ipv4Address"/>.
        /// </param>
        /// <param name="right">
        /// The <see cref="Ipv4SubnetMask"/>.
        /// </param>
        /// <returns>
        /// The <see cref="Ipv4Subnet"/>
        /// </returns>
        public static Ipv4Subnet operator +(Ipv4Address left, Ipv4SubnetMask right) => new (left, right);
        
        /// <summary>
        /// Creates an <see cref="Ipv4NetworkMatch"/> with an <see cref="Ipv4Address"/> and <see cref="Ipv4WildcardMask"/>
        /// </summary>
        /// <param name="left">
        /// The <see cref="Ipv4Address"/>.
        /// </param>
        /// <param name="right">
        /// The <see cref="Ipv4WildcardMask"/>.
        /// </param>
        /// <returns>
        /// The <see cref="Ipv4NetworkMatch"/>
        /// </returns>
        public static Ipv4NetworkMatch operator +(Ipv4Address left, Ipv4WildcardMask right) => new (left, right);
        
        /// <summary>
        /// Creates an <see cref="Ipv4Subnet"/> with an <see cref="Ipv4Address"/> and <see cref="Ipv4Cidr"/>
        /// </summary>
        /// <param name="left">
        /// The <see cref="Ipv4Address"/>.
        /// </param>
        /// <param name="right">
        /// The <see cref="Ipv4Cidr"/>.
        /// </param>
        /// <returns>
        /// The <see cref="Ipv4Subnet"/>
        /// </returns>
        public static Ipv4Subnet operator /(Ipv4Address left, Ipv4Cidr right) => new (left, right);
        
        /// <summary>
        /// Determines if this <see cref="Ipv4Address"/> instance is a part of a given <see cref="Ipv4Subnet"/>
        /// </summary>
        /// <param name="subnet">
        /// The <see cref="Ipv4Subnet"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this <see cref="Ipv4Address"/> instance is a part of <paramref name="subnet"/>;
        /// otherwise, <see langword="false"/>
        /// </returns>
        public bool IsInSubnet(Ipv4Subnet subnet) => subnet.Contains(this);
        
        #endregion Subnetting

        #region Octets

        /// <summary>
        /// Gets a specific octet from an <see cref="Ipv4Address"/>
        /// </summary>
        /// <param name="index">
        /// The index of the octet to return.  Value must be between 0 and 3, inclusive, and represents the byte
        /// for the value in network order (big endian).
        /// </param>
        /// <returns>
        /// The octet of the <see cref="Ipv4Address"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0 or greater than 3.
        /// </exception>
        public byte GetOctet(int index) => index switch
        {
            0 => (byte)((Value >> 24) & 0xFF),
            1 => (byte)((Value >> 16) & 0xFF),
            2 => (byte)((Value >> 8) & 0xFF),
            3 => (byte)(Value & 0xFF),
            _ => throw new ArgumentOutOfRangeException(nameof(index), index, $"{nameof(index)} must be between 0 and 3, inclusive."),
        };

        internal byte this[int index] => GetOctet(index);
        
        
        /// <summary>
        /// Tries to write the current IP address into a span of bytes.
        /// </summary>
        /// <param name="destination">
        /// When this method returns, the IP address as a span of bytes.
        /// </param>
        /// <param name="bytesWritten">
        /// When this method returns, the number of bytes written into the span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the IP address is successfully written to the given span; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryWriteBytes(Span<byte> destination, out int bytesWritten) 
            => this.Value.TryWriteBigEndian(destination, out bytesWritten);
        
        /// <summary>
        /// Provides a copy of the <see cref="Ipv4Address"/> as an array of <see cref="byte"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="byte"/> array.
        /// </returns>
        public byte[] GetBytes() => this.Value.ToBytesBigEndian();

        #endregion Octets

        #region Formatting

        int ITryFormat.MaximumLengthRequired => Ipv4Address.MAXIMUM_LENGTH;

        /// <summary>
        /// Tries to format the current IP address into the provided span.
        /// </summary>
        /// <param name="destination">
        /// When this method returns, the IP address as a span of characters.
        /// </param>
        /// <param name="charsWritten">
        /// When this method returns, the number of characters written into the span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the formatting was successful; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryFormat(Span<char> destination, out int charsWritten)
            => Ipv4Formatting.TryFormat(this, destination, out charsWritten);

        /// <summary>
        /// Converts an <see cref="Ipv4Address"/> to its standard notation.
        /// </summary>
        /// <returns>
        /// A string that contains the IP address in IPv4 dotted decimal notation.
        /// </returns>
        public override string ToString() => this.GetString();

        #endregion Formatting

        #region Parsing


        
        /// <summary>
        /// Converts an unsigned 32 bit integer to an <see cref="Ipv4Address"/>
        /// </summary>
        /// <param name="value">An unsigned 32 bit integer</param>
        /// <returns>
        /// The Ipv4 address representation of <paramref name="value"/>
        /// </returns>
        public static Ipv4Address Parse(uint value)
            => new Ipv4Address(value);
        
        /// <summary>
        /// Converts an IP address string to an <see cref="Ipv4Address"/> instance.
        /// </summary>
        /// <param name="ipString">
        /// A string that contains an IP address in dotted decimal notation
        /// </param>
        /// <returns>
        /// The Ipv4 address representation of <paramref name="ipString"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ipString"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="ipString"/> is not a valid IP address.
        /// </exception>
        public static Ipv4Address Parse(string? ipString)
        {
            ipString = ipString ?? throw new ArgumentNullException(nameof(ipString));
            return Ipv4Address.TryParse(ipString, out var result) ? result : throw new FormatException();
        }

        /// <summary>
        ///     Converts a <see cref="IPAddress"/> to an <see cref="Ipv4Address"/> instance.
        /// </summary>
        /// <param name="ipAddress">
        ///     A <see cref="IPAddress"/> with address family <see cref="AddressFamily.InterNetwork"/>
        /// </param>
        /// <returns>
        ///     An <see cref="Ipv4Address"/> that represents the <paramref name="ipAddress"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="ipAddress"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="ipAddress"/> is not <see cref="AddressFamily.InterNetwork"/>
        /// </exception>
        public static Ipv4Address Parse(IPAddress? ipAddress) => ipAddress?.AddressFamily switch
        {
            null => throw new ArgumentNullException(nameof(ipAddress)),
            AddressFamily.InterNetwork => ParseInternal(ipAddress),
            _ => throw new ArgumentException(nameof(ipAddress)),
        };

        private static Ipv4Address ParseInternal(IPAddress ipAddress)
        {
            Span<byte> octets = stackalloc byte[4];
            _ = ipAddress.TryWriteBytes(octets, out _);
            _ = TryParse(octets, out var result);
            return result;
        }

        /// <summary>
        /// Converts an IP address represented as a character span to an <see cref="Ipv4Address"/> instance.
        /// </summary>
        /// <param name="ipSpan">
        /// A character span that contains an IP address in dotted decimal notation.
        /// </param>
        /// <returns>
        /// The converted IP address.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="ipSpan"/> is not a valid IPv4 address.
        /// </exception>
        public static Ipv4Address Parse(ReadOnlySpan<char> ipSpan)
            => TryParse(ipSpan, out var result) ? result : throw new FormatException();
        
        
        #endregion Parsing

        #region TryParse

        /// <summary>
        ///     Converts a <see cref="IPAddress"/> to an <see cref="Ipv4Address"/> instance.
        /// </summary>
        /// <param name="ipAddress">
        ///     A <see cref="IPAddress"/> with address family <see cref="AddressFamily.InterNetwork"/>
        /// </param>
        /// <param name="address">
        ///     An <see cref="Ipv4Address"/> that represents the <paramref name="ipAddress"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="ipAddress"/> was able to be converted to an
        ///     <see cref="Ipv4Address"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(IPAddress? ipAddress, out Ipv4Address address)
        {
            address = default;
            if (ipAddress is null || ipAddress.AddressFamily != AddressFamily.InterNetwork)
                return false;
            address = ParseInternal(ipAddress);
            return true;
        }

        /// <summary>
        /// Determines whether the specified byte span represents a valid IP address.
        /// </summary>
        /// <param name="octets">
        /// The byte span to validate
        /// </param>
        /// <param name="address">
        /// When this method returns, the <see cref="Ipv4Address"/> version of the byte span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="octets"/> was able to be parsed as an IP address; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<byte> octets, out Ipv4Address address)
        {
            address = default;
            return octets.Length == 4
                && octets.TryReadUInt32BigEndian(out var value)
                && TryParse(value, out address);
        }
        
        /// <summary>
        /// Determines whether the specified byte span represents a valid IP address.
        /// </summary>
        /// <param name="octets">
        /// The byte span to validate
        /// </param>
        /// <param name="bytesRead">
        /// The number of bytes read for this <see cref="Ipv4Address"/>.
        /// If the method returns <see langword="true"/>, the value will be 4.  Otherwise, it will be 0.
        /// </param>
        /// <param name="address">
        /// When this method returns, the <see cref="Ipv4Address"/> version of the byte span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="octets"/> was able to be parsed as an <see cref="Ipv4Address"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<byte> octets, out int bytesRead, out Ipv4Address address)
        {
            bytesRead = default;
            if (!TryParse(octets, out address))
                return false;
            bytesRead = 4;
            return true;
        }
        
        /// <summary>
        /// Determines whether a string is a valid IPv4 address.
        /// </summary>
        /// <param name="ipString">
        /// The string to validate.
        /// </param>
        /// <param name="address">
        /// The <see cref="Ipv4Address"/> version of the string.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="ipString"/> was able to be parsed as an IP address; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string? ipString, out Ipv4Address address) 
            => TryParse(ipString, out var charsRead, out address) && charsRead == ipString?.Length;

        /// <summary>
        /// Determines whether string begins with a valid IPv4 address.
        /// </summary>
        /// <param name="ipString">
        /// The string to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid Ipv4 address.
        /// </param>
        /// <param name="address">
        /// The <see cref="Ipv4Address"/> version of the string.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="ipString"/> was able to be parsed as an IP address; otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="ipString"/> begins with a valid <see cref="Ipv4Address"/>, and is followed by other characters, this method will
        /// return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4Address"/>.
        /// </returns>
        public static bool TryParse(string? ipString, out int charsRead, out Ipv4Address address)
        {
            address = default;
            return Ipv4Parsing.TryParseDottedDecimalUInt32(ipString, out charsRead, out var value)
                && TryParse(value, out address);
        }

        internal static bool TryParse(uint value, out Ipv4Address address)
        {
            address = new Ipv4Address(value);
            return true;
        }
        
        internal static bool TryParse(ref ReadOnlySpan<char> text, ref int charsRead, out Ipv4Address address)
        {
            address = default;
            return Ipv4Parsing.TryParseDottedDecimalUInt32(ref text, ref charsRead, out var value)
                && TryParse(value, out address);
        }
        
        
        /// <summary>
        /// Determines whether the specified character span represents a valid IPv4 address.
        /// </summary>
        /// <param name="ipSpan">
        /// The character span to validate.
        /// </param>
        /// <param name="address">
        /// The <see cref="Ipv4Address"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="ipSpan"/> was able to be parsed as an IP address; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> ipSpan, out Ipv4Address address)
            => TryParse(ipSpan, out var charsRead, out address) && charsRead == ipSpan.Length;


        /// <summary>
        /// Determines whether the specified character span represents a valid IPv4 address.
        /// </summary>
        /// <param name="ipSpan">
        /// The character span to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid Ipv4 address.
        /// </param>
        /// <param name="address">
        /// The <see cref="Ipv4Address"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="ipSpan"/> was able to be parsed as an IP address; otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="ipSpan"/> begins with a valid <see cref="Ipv4Address"/>, and is followed by other characters, this method will
        /// return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4Address"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> ipSpan, out int charsRead, out Ipv4Address address)
        {
            address = default;
            return Ipv4Parsing.TryParseDottedDecimalUInt32(ipSpan, out charsRead, out var value)
                && TryParse(value, out address);
        }
        
        
        #endregion TryParse

        #region Comparisons
        
        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="Ipv4Address"/> are equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4Address"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4Address"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Ipv4Address left, Ipv4Address right) => left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="Ipv4Address"/> are not equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4Address"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4Address"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Ipv4Address left, Ipv4Address right) => !left.Equals(right);
        
        /// <summary>
        ///     Compares the value of this instance to a specified <see cref="Ipv4Address"/> value,
        ///     and returns an integer that indicates whether this instance is less than, equal to, or
        ///     greater than the specified <see cref="Ipv4Address"/> value.
        /// </summary>
        /// <param name="other">The object to compare to the current instance.</param>
        /// <returns>A signed number indicating the relative values of this instance and the value parameter.</returns>
        public int CompareTo(Ipv4Address other) => this.Value.CompareTo(other.Value);

        /// <summary>
        ///     Compares the value of this instance to a specified object that contains a specified
        ///     <see cref="Ipv4Address"/> value, and returns an integer that indicates whether this
        ///     instance is less than, equal to, or greater than the specified <see cref="Ipv4Address"/> value.
        /// </summary>
        /// <param name="obj">
        ///     A boxed object to compare, or null.
        /// </param>
        /// <returns>A signed number indicating the relative values of this instance and value.</returns>
        /// <exception cref="ArgumentException">
        /// value is not a <see cref="Ipv4Address"/>.
        /// </exception>
        public int CompareTo(object? obj) => obj switch
        {
            null => 1,
            Ipv4Address other => CompareTo(other),
            _ => throw new ArgumentException($"Object must be of type {nameof(Ipv4Address)}"),
        };

        /// <summary>
        ///     Determines whether one specified <see cref="Ipv4Address"/> represents an IPv4 address
        ///     that is less than another specified <see cref="Ipv4Address"/>.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="left"/> is less than
        ///     <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <(Ipv4Address left, Ipv4Address right) => left.CompareTo(right) < 0;
        
        /// <summary>
        ///     Determines whether one specified <see cref="Ipv4Address"/> represents an IPv4 address
        ///     that is greater than another specified <see cref="Ipv4Address"/>.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="left"/> is greater than
        ///     <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >(Ipv4Address left, Ipv4Address right) => left.CompareTo(right) > 0;
        
        /// <summary>
        ///     Determines whether one specified <see cref="Ipv4Address"/> represents an IPv4 address
        ///     that is less than or equal to another specified <see cref="Ipv4Address"/>.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="left"/> is less than or equal to
        ///     <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator <=(Ipv4Address left, Ipv4Address right) => left.CompareTo(right) <= 0;
        
        /// <summary>
        ///     Determines whether one specified <see cref="Ipv4Address"/> represents an IPv4 address
        ///     that is greater than or equal to another specified <see cref="Ipv4Address"/>.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="left"/> is greater than or equal to
        ///     <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator >=(Ipv4Address left, Ipv4Address right) => left.CompareTo(right) >= 0;

        #endregion Comparisons
        


    }

}