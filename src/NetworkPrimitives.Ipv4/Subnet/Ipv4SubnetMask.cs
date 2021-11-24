using System;
using System.Buffers.Binary;
using System.Net;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    /// <summary>
    /// Represents an IPv4 Subnet Mask, in subnet mask notation.
    /// </summary>
    public readonly struct Ipv4SubnetMask : IBinaryNetworkPrimitive<Ipv4SubnetMask>, IEquatable<Ipv4SubnetMask>, IEquatable<Ipv4Cidr>
    {
        internal uint Value { get; }
        private Ipv4SubnetMask(uint value) => this.Value = value;
        
        #region Equality

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4SubnetMask"/>.
        /// </summary>
        /// <param name="other">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Ipv4SubnetMask other) => this.Value == other.Value;

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4Cidr"/>.
        /// </summary>
        /// <param name="other">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Ipv4Cidr other) => this.Equals(other.ToSubnetMask());

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4Cidr"/>.
        /// </summary>
        /// <param name="obj">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is an instance of <see cref="Ipv4Cidr"/> or
        /// <see cref="Ipv4SubnetMask"/> and equals the value of this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj switch
        {
            Ipv4Cidr other => Equals(other),
            Ipv4SubnetMask other => Equals(other),
            byte other => this.Value == other,
            _ => false,
        };
        
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => (int)this.Value;
        
        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="Ipv4SubnetMask"/> are equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4SubnetMask"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4SubnetMask"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Ipv4SubnetMask left, Ipv4SubnetMask right) => left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="Ipv4SubnetMask"/> are not equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4SubnetMask"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4SubnetMask"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Ipv4SubnetMask left, Ipv4SubnetMask right) => !left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4SubnetMask"/> is equal to an instance of <see cref="Ipv4Cidr"/>.
        /// </summary>
        /// <param name="left">
        /// An instance of <see cref="Ipv4SubnetMask"/>.
        /// </param>
        /// <param name="right">
        /// An instance of <see cref="Ipv4Cidr"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Ipv4SubnetMask left, Ipv4Cidr right) => left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4SubnetMask"/> is not equal to an instance of <see cref="Ipv4Cidr"/>.
        /// </summary>
        /// <param name="left">
        /// An instance of <see cref="Ipv4SubnetMask"/>.
        /// </param>
        /// <param name="right">
        /// An instance of <see cref="Ipv4Cidr"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Ipv4SubnetMask left, Ipv4Cidr right) => !left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4Cidr"/> is equal to an instance of <see cref="Ipv4SubnetMask"/>.
        /// </summary>
        /// <param name="left">
        /// An instance of <see cref="Ipv4Cidr"/>.
        /// </param>
        /// <param name="right">
        /// An instance of <see cref="Ipv4SubnetMask"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Ipv4Cidr left, Ipv4SubnetMask right) => left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4Cidr"/> is not equal to an instance of <see cref="Ipv4SubnetMask"/>.
        /// </summary>
        /// <param name="left">
        /// An instance of <see cref="Ipv4Cidr"/>.
        /// </param>
        /// <param name="right">
        /// An instance of <see cref="Ipv4SubnetMask"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Ipv4Cidr left, Ipv4SubnetMask right) => !left.Equals(right);

        #endregion Equality

        #region Conversion

        /// <summary>
        /// Converts an <see cref="Ipv4SubnetMask"/> to an <see cref="Ipv4Cidr"/>
        /// </summary>
        /// <param name="mask">An instance of <see cref="Ipv4SubnetMask"/></param>
        /// <returns>The corresponding instance of <see cref="Ipv4Cidr"/></returns>
        public static implicit operator Ipv4Cidr(Ipv4SubnetMask mask) => Ipv4Cidr.Parse(SubnetMaskLookups.GetCidr(mask.Value));
        
        /// <summary>
        /// Converts an <see cref="Ipv4SubnetMask"/> to an <see cref="Ipv4WildcardMask"/>
        /// </summary>
        /// <param name="mask">An instance of <see cref="Ipv4SubnetMask"/></param>
        /// <returns>The corresponding instance of <see cref="Ipv4WildcardMask"/></returns>
        public static implicit operator Ipv4WildcardMask(Ipv4SubnetMask mask) => Ipv4WildcardMask.Parse(mask);
        
        /// <summary>
        /// Converts an <see cref="Ipv4Cidr"/> to an <see cref="Ipv4SubnetMask"/>
        /// </summary>
        /// <param name="cidr">An instance of <see cref="Ipv4Cidr"/></param>
        /// <returns>The corresponding instance of <see cref="Ipv4SubnetMask"/></returns>
        public static implicit operator Ipv4SubnetMask(Ipv4Cidr cidr) => new (SubnetMaskLookups.GetSubnetMask(cidr.Value));
        
        /// <summary>
        /// Converts this instance to an <see cref="Ipv4Cidr"/>
        /// </summary>
        /// <returns>The corresponding instance of <see cref="Ipv4Cidr"/></returns>
        public Ipv4Cidr ToCidr() => this;
        
        /// <summary>
        /// Converts this instance to an <see cref="Ipv4WildcardMask"/>
        /// </summary>
        /// <returns>The corresponding instance of <see cref="Ipv4WildcardMask"/></returns>
        public Ipv4WildcardMask ToWildcardMask() => this;

        /// <summary>
        /// Converts this instance to an <see cref="IPAddress"/>
        /// </summary>
        /// <returns>The corresponding instance of <see cref="IPAddress"/></returns>
        public IPAddress ToIpAddress() => this.Value.ToIpAddress();
        #endregion Conversion

        #region Subnetting

        /// <summary>
        /// The total number of hosts allowed by this <see cref="Ipv4Cidr"/>.
        /// Includes the broadcast and network addresses.
        /// </summary>
        public ulong TotalHosts => SubnetMaskLookups.GetTotalHosts(this.Value);
        
        /// <summary>
        /// The number of usable hosts allowed by this <see cref="Ipv4Cidr"/>.
        /// Does not include the broadcast or network address.
        /// RFC 3021 states that for a /31 network, the number of usable hosts is 2.
        /// The number of usable hosts for a /32 network is 1.
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc3021">RFC3021</seealso>
        public uint UsableHosts => SubnetMaskLookups.GetUsableHosts(this.Value);
        
        internal bool IsSlash32Or31 => Value is 0xFFFFFFFF or 0xFFFFFFFE;
        internal bool IsSlash32 => Value is 0xFFFFFFFF;
        private static bool IsValidMask(uint value) => SubnetMaskLookups.TryGetCidr(value, out _);

        #endregion Subnetting

        #region Parsing

        /// <summary>
        /// Converts a subnet mask string to an <see cref="Ipv4SubnetMask"/> instance.
        /// </summary>
        /// <param name="text">
        /// A string that contains the subnet mask in dotted decimal form.
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid IPv4 subnet mask
        /// </exception>
        public static Ipv4SubnetMask Parse(string? text)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));
            return Ipv4SubnetMask.TryParse(text, out var mask)
                ? mask
                : throw new FormatException();
        }

        /// <summary>
        /// Converts a unsigned 32-bit integer that represents a subnet mask to an <see cref="Ipv4Cidr"/> instance.
        /// </summary>
        /// <param name="value">
        /// An integer that represents a IPv4 subnet mask.
        /// </param>
        /// <returns>
        /// The <see cref="Ipv4SubnetMask"/> representation of <paramref name="value"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is not a valid IPv4 subnet mask
        /// </exception>
        public static Ipv4SubnetMask Parse(uint value)
            => TryParse(value, out var mask)
                ? mask
                : throw new FormatException();
        
        /// <summary>
        /// Converts an IPv4 subnet mask represented by a character span to an <see cref="Ipv4SubnetMask"/> instance.
        /// </summary>
        /// <param name="text">
        /// A character span that contains an Ipv4 subnet mask
        /// </param>
        /// <returns>
        /// The converted subnet mask
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid IPv4 subnet mask.
        /// </exception>
        public static Ipv4SubnetMask Parse(ReadOnlySpan<char> text)
            => TryParse(text, out var mask)
                ? mask
                : throw new FormatException();

        #endregion Parsing

        #region TryParse

        
        /// <summary>
        /// Converts an unsigned 32-bit integer that represents a subnet mask to an <see cref="Ipv4SubnetMask"/> instance.
        /// </summary>
        /// <param name="value">
        /// An unsigned 32-bit integer that represents a CIDR.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4SubnetMask"/> representation of <paramref name="value"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="value"/> was able to be converted to an
        ///     <see cref="Ipv4SubnetMask"/>; otherwise, <see langword="false"/>.
        /// </returns>
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
        
        /// <summary>
        /// Converts an <see cref="Ipv4WildcardMask"/> to an <see cref="Ipv4SubnetMask"/> instance.
        /// </summary>
        /// <param name="value">
        /// An <see cref="Ipv4WildcardMask"/> instance
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4SubnetMask"/> representation of <paramref name="value"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="value"/> was able to be converted to an
        ///     <see cref="Ipv4SubnetMask"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryParse(Ipv4WildcardMask value, out Ipv4SubnetMask result) => TryParse(~value.Value, out result);

        /// <summary>
        /// Converts an <see cref="IPAddress"/> to an <see cref="Ipv4SubnetMask"/> instance.
        /// </summary>
        /// <param name="ipAddress">
        /// An <see cref="Ipv4SubnetMask"/> instance
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4SubnetMask"/> representation of <paramref name="ipAddress"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="ipAddress"/> was able to be converted to an
        ///     <see cref="Ipv4SubnetMask"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(IPAddress ipAddress, out Ipv4SubnetMask result)
        {
            result = default;
            return Ipv4Address.TryParse(ipAddress, out var address)
                && TryParse(address, out result);
        }

        /// <summary>
        /// Converts an <see cref="Ipv4Address"/> to an <see cref="Ipv4SubnetMask"/> instance.
        /// </summary>
        /// <param name="address">
        /// An <see cref="Ipv4Address"/> instance
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4SubnetMask"/> representation of <paramref name="address"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="address"/> was able to be converted to an
        ///     <see cref="Ipv4SubnetMask"/>; otherwise, <see langword="false"/>.
        /// </returns>
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

        /// <summary>
        /// Determines whether the specified byte span represents a valid subnet mask.
        /// </summary>
        /// <param name="octets">
        /// The byte span to validate
        /// </param>
        /// <param name="result">
        /// When this method returns, the <see cref="Ipv4SubnetMask"/> version of the byte span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="octets"/> was able to be parsed as a subnet mask; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<byte> octets, out Ipv4SubnetMask result)
        {
            result = default;
            return Ipv4Address.TryParse(octets, out var address)
                && TryParse(address, out result);
        }


        /// <summary>
        /// Determines whether string begins with a valid IPv4 subnet mask.
        /// </summary>
        /// <param name="text">
        /// The string to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid Ipv4 subnet mask.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4SubnetMask"/> version of the string.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an IP subnet mask; otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="text"/> begins with a valid <see cref="Ipv4SubnetMask"/>, and is followed by other characters, this method will
        /// return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4SubnetMask"/>.
        /// </returns>
        public static bool TryParse(string? text, out int charsRead, out Ipv4SubnetMask result)
        {
            result = default;
            return Ipv4Address.TryParse(text, out charsRead, out var address) && Ipv4SubnetMask.TryParse(address, out result);
        }

        
        /// <summary>
        /// Determines whether string begins with a valid IPv4 subnet mask.
        /// </summary>
        /// <param name="text">
        /// The string to validate.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4SubnetMask"/> version of the string.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an IP subnet mask; otherwise, <see langword="false"/>.
        /// </returns>
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

        internal static bool TryParse(ref ReadOnlySpan<char> text, ref int charsRead, out Ipv4SubnetMask result)
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

        
        /// <summary>
        /// Determines whether the specified character span represents a valid IPv4 subnet mask.
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4SubnetMask"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an IP subnet mask; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out Ipv4SubnetMask result)
            => TryParse(text, out var charsRead, out result) && charsRead == text.Length;

        
        /// <summary>
        /// Determines whether the specified character span represents a valid IPv4 subnet mask.
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid Ipv4 subnet mask.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4SubnetMask"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an IP subnet mask; otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="text"/> begins with a valid <see cref="Ipv4SubnetMask"/>, and is followed by other characters, this method will
        /// return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4SubnetMask"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4SubnetMask result)
        {
            result = default;
            return Ipv4Address.TryParse(text, out charsRead, out var address) && TryParse(address, out result);
        }
        
        #endregion TryParse

        #region Octets

        /// <summary>
        /// Tries to write the current subnet mask into a span of bytes.
        /// </summary>
        /// <param name="destination">
        /// When this method returns, the subnet mask as a span of bytes.
        /// </param>
        /// <param name="bytesWritten">
        /// When this method returns, the number of bytes written into the span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the subnet mask is successfully written to the given span; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryWriteBytes(Span<byte> destination, out int bytesWritten) 
            => this.Value.TryWriteBigEndian(destination, out bytesWritten);
        
        /// <summary>
        /// Provides a copy of the <see cref="Ipv4SubnetMask"/> as an array of <see cref="byte"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="byte"/> array.
        /// </returns>
        public byte[] GetBytes() => this.Value.ToBytesBigEndian();
        

        #endregion Octets

        #region Formatting

        int ITryFormat.MaximumLengthRequired => Ipv4Address.MAXIMUM_LENGTH;
        
        
        /// <summary>
        /// Tries to format the current subnet mask into the provided span.
        /// </summary>
        /// <param name="destination">
        /// When this method returns, the subnet mask as a span of characters.
        /// </param>
        /// <param name="charsWritten">
        /// When this method returns, the number of characters written into the span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the formatting was successful; otherwise, <see langword="false"/>.
        /// </returns>
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
        
        /// <summary>
        /// Converts an <see cref="Ipv4SubnetMask"/> to its standard notation.
        /// </summary>
        /// <returns>
        /// A string that contains the IP address in IPv4 dotted decimal notation.
        /// </returns>
        public override string ToString() => this.GetString();

        private string GetString() => SubnetMaskLookups.GetString(Value);
        
        #endregion Formatting
    }
}