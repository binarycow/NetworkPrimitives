using System;
using System.Net;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    /// <summary>
    /// Represents an IPv4 Wildcard Mask
    /// </summary>
    public readonly struct Ipv4WildcardMask : IBinaryNetworkPrimitive<Ipv4WildcardMask>, IEquatable<Ipv4SubnetMask>, IEquatable<Ipv4Cidr>
    {
        internal uint Value { get; }
        private Ipv4WildcardMask(uint value) => this.Value = value;

        #region Equality

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4WildcardMask"/>.
        /// </summary>
        /// <param name="other">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Ipv4WildcardMask other) => this.Value == other.Value;
        
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4SubnetMask"/>.
        /// </summary>
        /// <param name="other">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Ipv4SubnetMask other) => Equals(other.ToWildcardMask());
        
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4Cidr"/>.
        /// </summary>
        /// <param name="other">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Ipv4Cidr other) => Equals(other.ToSubnetMask().ToWildcardMask());
        
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4Cidr"/>,
        /// <see cref="Ipv4SubnetMask"/>, or <see cref="Ipv4WildcardMask"/>
        /// </summary>
        /// <param name="obj">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is an instance of <see cref="Ipv4Cidr"/>,
        /// <see cref="Ipv4WildcardMask"/>, or <see cref="Ipv4SubnetMask"/> and equals the value of this instance;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj switch
        {
            Ipv4Cidr other => Equals(other),
            Ipv4SubnetMask other => Equals(other),
            Ipv4WildcardMask other => Equals(other),
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
        /// Returns a value indicating whether two instances of <see cref="Ipv4WildcardMask"/> are equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4WildcardMask"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4WildcardMask"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Ipv4WildcardMask left, Ipv4WildcardMask right) => left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="Ipv4WildcardMask"/> are not equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4WildcardMask"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4WildcardMask"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Ipv4WildcardMask left, Ipv4WildcardMask right) => !left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4WildcardMask"/>
        /// and <see cref="Ipv4SubnetMask"/> are equal.
        /// </summary>
        /// <param name="left">
        /// An instance of <see cref="Ipv4WildcardMask"/> to compare.
        /// </param>
        /// <param name="right">
        /// An instance of <see cref="Ipv4SubnetMask"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Ipv4WildcardMask left, Ipv4SubnetMask right) => left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4WildcardMask"/>
        /// and <see cref="Ipv4SubnetMask"/> are not equal.
        /// </summary>
        /// <param name="left">
        /// An instance of <see cref="Ipv4WildcardMask"/> to compare.
        /// </param>
        /// <param name="right">
        /// An instance of <see cref="Ipv4SubnetMask"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Ipv4WildcardMask left, Ipv4SubnetMask right) => !left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4SubnetMask"/>
        /// and <see cref="Ipv4WildcardMask"/> are equal.
        /// </summary>
        /// <param name="left">
        /// An instance of <see cref="Ipv4SubnetMask"/> to compare.
        /// </param>
        /// <param name="right">
        /// An instance of <see cref="Ipv4WildcardMask"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Ipv4SubnetMask left, Ipv4WildcardMask right) => right.Equals(left);
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4SubnetMask"/>
        /// and <see cref="Ipv4WildcardMask"/> are not equal.
        /// </summary>
        /// <param name="left">
        /// An instance of <see cref="Ipv4SubnetMask"/> to compare.
        /// </param>
        /// <param name="right">
        /// An instance of <see cref="Ipv4WildcardMask"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Ipv4SubnetMask left, Ipv4WildcardMask right) => !right.Equals(left);
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4WildcardMask"/>
        /// and <see cref="Ipv4Cidr"/> are equal.
        /// </summary>
        /// <param name="left">
        /// An instance of <see cref="Ipv4WildcardMask"/> to compare.
        /// </param>
        /// <param name="right">
        /// An instance of <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Ipv4WildcardMask left, Ipv4Cidr right) => left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4WildcardMask"/>
        /// and <see cref="Ipv4Cidr"/> are not equal.
        /// </summary>
        /// <param name="left">
        /// An instance of <see cref="Ipv4WildcardMask"/> to compare.
        /// </param>
        /// <param name="right">
        /// An instance of <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Ipv4WildcardMask left, Ipv4Cidr right) => !left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4Cidr"/>
        /// and <see cref="Ipv4WildcardMask"/> are equal.
        /// </summary>
        /// <param name="left">
        /// An instance of <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <param name="right">
        /// An instance of <see cref="Ipv4WildcardMask"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Ipv4Cidr left, Ipv4WildcardMask right) => right.Equals(left);
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4Cidr"/>
        /// and <see cref="Ipv4WildcardMask"/> are not equal.
        /// </summary>
        /// <param name="left">
        /// An instance of <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <param name="right">
        /// An instance of <see cref="Ipv4WildcardMask"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Ipv4Cidr left, Ipv4WildcardMask right) => !right.Equals(left);

        #endregion Equality

        #region Conversion
        
        /// <summary>
        /// Converts an <see cref="Ipv4SubnetMask"/> to a <see cref="Ipv4WildcardMask"/>
        /// </summary>
        /// <param name="mask">An instance of <see cref="Ipv4SubnetMask"/></param>
        /// <returns>An instance of <see cref="Ipv4WildcardMask"/></returns>
        public static Ipv4WildcardMask Parse(Ipv4SubnetMask mask) => new (~mask.Value);
        
        /// <summary>
        /// Converts an <see cref="Ipv4Address"/> to a <see cref="Ipv4WildcardMask"/>
        /// </summary>
        /// <param name="address">An instance of <see cref="Ipv4Address"/></param>
        /// <returns>An instance of <see cref="Ipv4WildcardMask"/></returns>
        public static Ipv4WildcardMask Parse(Ipv4Address address) => new (address.Value);
        
        /// <summary>
        /// Determines if this instance of <see cref="Ipv4WildcardMask"/> is a valid <see cref="Ipv4SubnetMask"/>
        /// </summary>
        /// <param name="mask">
        /// If successful, an instance of <see cref="Ipv4SubnetMask"/> that represents this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this <see cref="Ipv4WildcardMask"/> can be represented as an
        /// <see cref="Ipv4SubnetMask"/>; otherwise <see langword="false"/>.
        /// </returns>
        public bool IsSubnetMask(out Ipv4SubnetMask mask) => Ipv4SubnetMask.TryParse(~Value, out mask);
        
        /// <summary>
        /// Converts this instance of <see cref="Ipv4WildcardMask"/> to a <see cref="IPAddress"/>
        /// </summary>
        /// <returns>An instance of <see cref="IPAddress"/></returns>
        public IPAddress ToIpAddress() => this.Value.ToIpAddress();
        
        #endregion Conversion

        #region Formatting

        
        /// <summary>
        /// Tries to format the current wildcard mask into the provided span.
        /// </summary>
        /// <param name="destination">
        /// When this method returns, the wildcard mask as a span of characters.
        /// </param>
        /// <param name="charsWritten">
        /// When this method returns, the number of characters written into the span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the formatting was successful; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryFormat(Span<char> destination, out int charsWritten) 
            => Ipv4Formatting.TryFormatDottedDecimal(this.Value, destination, out charsWritten);
        
        int ITryFormat.MaximumLengthRequired => Ipv4Address.MAXIMUM_LENGTH;
        
        /// <summary>
        /// Converts an <see cref="Ipv4WildcardMask"/> to its standard notation.
        /// </summary>
        /// <returns>
        /// A string that contains the IP address in IPv4 dotted decimal notation.
        /// </returns>
        public override string ToString() => this.GetString();

        #endregion Formatting

        #region Octets

        /// <summary>
        /// Tries to write the current wildcard mask into a span of bytes.
        /// </summary>
        /// <param name="destination">
        /// When this method returns, the wildcard mask as a span of bytes.
        /// </param>
        /// <param name="bytesWritten">
        /// When this method returns, the number of bytes written into the span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the wildcard mask is successfully written to the given span; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryWriteBytes(Span<byte> destination, out int bytesWritten) 
            => this.Value.TryWriteBigEndian(destination, out bytesWritten);
        
        /// <summary>
        /// Provides a copy of the <see cref="Ipv4WildcardMask"/> as an array of <see cref="byte"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="byte"/> array.
        /// </returns>
        public byte[] GetBytes() => this.Value.ToBytesBigEndian();
        

        #endregion Octets

        #region Subnetting

        /// <summary>
        /// The total number of hosts allowed by this <see cref="Ipv4WildcardMask"/>.
        /// Includes the broadcast and network addresses.
        /// </summary>
        public ulong HostCount => this.Value switch
        {
            0x00000000 => 4294967296,
            0xFFFFFFFF => 1,
            _ => (uint)(1 << (int)this.Value.PopCount()),
        };
        

        #endregion Subnetting


        #region Parsing

        
        /// <summary>
        /// Converts a wildcard mask string to an <see cref="Ipv4SubnetMask"/> instance.
        /// </summary>
        /// <param name="text">
        /// A string that contains the wildcard mask in dotted decimal form.
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid IPv4 wildcard mask
        /// </exception>
        public static Ipv4WildcardMask Parse(string? text)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));
            return Ipv4WildcardMask.TryParse(text, out var result) ? result : throw new FormatException();
        }

        /// <summary>
        /// Converts a unsigned 32-bit integer that represents a wildcard mask to an <see cref="Ipv4WildcardMask"/> instance.
        /// </summary>
        /// <param name="value">
        /// An unsigned 32-bit integer that represents a IPv4 wildcard mask.
        /// </param>
        /// <returns>
        /// The <see cref="Ipv4WildcardMask"/> representation of <paramref name="value"/>
        /// </returns>
        public static Ipv4WildcardMask Parse(uint value) => new (value);

        /// <summary>
        /// Converts an IPv4 wildcard mask represented by a character span to an <see cref="Ipv4SubnetMask"/> instance.
        /// </summary>
        /// <param name="text">
        /// A character span that contains an Ipv4 wildcard mask
        /// </param>
        /// <returns>
        /// The converted subnet mask
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid IPv4 subnet mask.
        /// </exception>
        public static Ipv4WildcardMask Parse(ReadOnlySpan<char> text)
            => TryParse(text, out var result) ? result : throw new FormatException();


        #endregion Parsing

        #region TryParse

        
        /// <summary>
        /// Converts an <see cref="IPAddress"/> to an <see cref="Ipv4WildcardMask"/> instance.
        /// </summary>
        /// <param name="ipAddress">
        /// An <see cref="Ipv4WildcardMask"/> instance
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4WildcardMask"/> representation of <paramref name="ipAddress"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="ipAddress"/> was able to be converted to an
        ///     <see cref="Ipv4WildcardMask"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(IPAddress ipAddress, out Ipv4WildcardMask result)
        {
            result = default;
            if (!Ipv4Address.TryParse(ipAddress, out var address))
                return false;
            result = new Ipv4WildcardMask(address.Value);
            return true;
        }

        /// <summary>
        /// Determines whether the specified byte span represents a valid wildcard mask.
        /// </summary>
        /// <param name="octets">
        /// The byte span to validate
        /// </param>
        /// <param name="result">
        /// When this method returns, the <see cref="Ipv4WildcardMask"/> version of the byte span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="octets"/> was able to be parsed as a wildcard mask; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<byte> octets, out Ipv4WildcardMask result)
        {
            result = default;
            if (!Ipv4Address.TryParse(octets, out var address))
                return false;
            result = new Ipv4WildcardMask(address.Value);
            return true;
        }

        /// <summary>
        /// Determines whether string begins with a valid IPv4 wildcard mask.
        /// </summary>
        /// <param name="text">
        /// The string to validate.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4WildcardMask"/> version of the string.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an IP wildcard mask; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string? text, out Ipv4WildcardMask result)
            => TryParse(text, out var charsRead, out result) && charsRead == text?.Length;
        
        
        /// <summary>
        /// Determines whether string begins with a valid IPv4 wildcard mask.
        /// </summary>
        /// <param name="text">
        /// The string to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid Ipv4 wildcard mask.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4WildcardMask"/> version of the string.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an IP wildcard mask; otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="text"/> begins with a valid <see cref="Ipv4WildcardMask"/>, and is followed by other characters, this method will
        /// return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4WildcardMask"/>.
        /// </returns>
        public static bool TryParse(string? text, out int charsRead, out Ipv4WildcardMask result) 
            => TryParse(text.GetSpan(), out charsRead, out result);
        
        /// <summary>
        /// Determines whether the specified character span represents a valid IPv4 wildcard mask.
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4WildcardMask"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an IP wildcard mask; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out Ipv4WildcardMask result)
            => TryParse(text, out var charsRead, out result) && charsRead == text.Length;
        
        /// <summary>
        /// Determines whether the specified character span represents a valid IPv4 wildcard mask.
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid Ipv4 wildcard mask.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4WildcardMask"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an IP wildcard mask; otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="text"/> begins with a valid <see cref="Ipv4WildcardMask"/>, and is followed by other characters, this method will
        /// return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4WildcardMask"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4WildcardMask result)
        {
            charsRead = default;
            return Ipv4WildcardMask.TryParse(ref text, ref charsRead, out result);
        }
        


        internal static bool TryParse(ref ReadOnlySpan<char> text, ref int charsRead, out Ipv4WildcardMask result)
        {
            result = default;
            if (!Ipv4Address.TryParse(ref text, ref charsRead, out var address))
                return false;
            result = new (address.Value);
            return true;
        }
        
        #endregion TryParse


    }
}