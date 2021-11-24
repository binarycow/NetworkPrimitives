using System;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    /// <summary>
    /// Represents an IPv4 Subnet Mask, in CIDR notation.
    /// </summary>
    public readonly struct Ipv4Cidr 
        : INetworkPrimitive<Ipv4Cidr>, 
            IComparable<Ipv4Cidr>, 
            IComparable<byte>, 
            IComparable,
            IEquatable<Ipv4Cidr>,
            IEquatable<Ipv4SubnetMask>
    {
        internal static readonly Ipv4Cidr Slash32 = new (32);
        internal byte Value { get; }
        
        private Ipv4Cidr(byte value) => this.Value = value;

        /// <summary>
        /// Converts an <see cref="Ipv4Cidr"/> to its <see cref="byte"/> representation
        /// </summary>
        /// <param name="cidr">An instance of <see cref="Ipv4Cidr"/></param>
        /// <returns>The <see cref="byte"/> representation of the <see cref="Ipv4Cidr"/></returns>
        public static explicit operator byte(Ipv4Cidr cidr) => cidr.Value;
        
        /// <summary>
        /// Converts an <see cref="Ipv4Cidr"/> to an <see cref="Ipv4SubnetMask"/>
        /// </summary>
        /// <returns>
        /// The <see cref="Ipv4SubnetMask"/> instance
        /// </returns>
        public Ipv4SubnetMask ToSubnetMask() => this;
        
        
        /// <summary>
        /// Converts an <see cref="Ipv4Cidr"/> to an <see cref="Ipv4WildcardMask"/>
        /// </summary>
        /// <returns>
        /// The <see cref="Ipv4WildcardMask"/> instance
        /// </returns>
        public Ipv4WildcardMask ToWildcardMask() => this.ToSubnetMask().ToWildcardMask();
        
        #region Equality

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4Cidr"/>.
        /// </summary>
        /// <param name="other">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Ipv4Cidr other) => this.Value == other.Value;

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4SubnetMask"/>.
        /// </summary>
        /// <param name="other">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Ipv4SubnetMask other) => this.Equals(other.ToCidr());

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
        public override int GetHashCode() => this.Value.GetHashCode();
        
        #endregion Equality
        
        #region Formatting
        
        int ITryFormat.MaximumLengthRequired => 2;
        
        /// <summary>
        /// Tries to format the current CIDR into the provided span.
        /// </summary>
        /// <param name="destination">
        /// When this method returns, the CIDR, as a span of characters.
        /// </param>
        /// <param name="charsWritten">
        /// When this method returns, the number of characters written into the span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the formatting was successful; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryFormat(Span<char> destination, out int charsWritten) 
            => Value.TryFormat(destination, out charsWritten);
        
        /// <summary>
        /// Converts an <see cref="Ipv4Cidr"/> to its <see cref="string"/> representation
        /// </summary>
        /// <returns>
        /// A string that contains the CIDR in its <see cref="string"/> representation
        /// </returns>
        public override string ToString() => this.GetString();
        
        #endregion Formatting
        
        #region Parsing

        /// <summary>
        /// Converts an IPv4 CIDR represented as a character span to an <see cref="Ipv4Cidr"/> instance.
        /// </summary>
        /// <param name="text">
        /// A character span that contains an Ipv4 CIDR
        /// </param>
        /// <returns>
        /// The converted CIDR
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid IPv4 address.
        /// </exception>
        public static Ipv4Cidr Parse(ReadOnlySpan<char> text)
            => TryParse(text, out var result) ? result : throw new FormatException();

        /// <summary>
        /// Converts a CIDR string to an <see cref="Ipv4Cidr"/> instance.
        /// </summary>
        /// <param name="text">
        /// A string that contains the CIDR in text form.
        /// </param>
        /// <returns>
        /// An <see cref="Ipv4Cidr"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid IPv4 CIDR
        /// </exception>
        public static Ipv4Cidr Parse(string? text)
            => TryParse(text, out var result) ? result : throw new FormatException();

        /// <summary>
        /// Converts an integer that represents a CIDR to an <see cref="Ipv4Cidr"/> instance.
        /// </summary>
        /// <param name="value">
        /// An integer that represents a CIDR.
        /// </param>
        /// <returns>
        /// The <see cref="Ipv4Cidr"/> representation of <paramref name="value"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is not a valid IPv4 CIDR
        /// </exception>
        public static Ipv4Cidr Parse(int value)
            => TryParse(value, out var result) ? result : throw new ArgumentOutOfRangeException(nameof(value));

        #endregion Parsing
        
        #region TryParse
        
        /// <summary>
        /// Converts an integer that represents a CIDR to an <see cref="Ipv4Cidr"/> instance.
        /// </summary>
        /// <param name="value">
        /// An integer that represents a CIDR.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4Cidr"/> representation of <paramref name="value"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="value"/> was able to be converted to an
        ///     <see cref="Ipv4Cidr"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(int value, out Ipv4Cidr result)
        {
            if (value is >= 0 and <= 32)
            {
                result = new ((byte)value);
                return true;
            }
            result = default;
            return false;
        }

        /// <summary>
        /// Determines whether the specified string represents a valid IPv4 CIDR.
        /// </summary>
        /// <param name="text">
        /// A string that represents a CIDR.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4Cidr"/> representation of <paramref name="text"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="text"/> was able to be converted to an
        ///     <see cref="Ipv4Cidr"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string? text, out Ipv4Cidr result)
        {
            result = default;
            return text is not null && Ipv4Cidr.TryParse(text, out var charsRead, out result) && charsRead == text.Length;
        }

        /// <summary>
        /// Determines whether the specified string represents a valid IPv4 CIDR.
        /// </summary>
        /// <param name="text">
        /// A string that represents a CIDR.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid CIDR.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4Cidr"/> representation of <paramref name="text"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="text"/> was able to be converted to an
        ///     <see cref="Ipv4Cidr"/>; otherwise, <see langword="false"/>.
        ///     If <paramref name="text"/> begins with a valid <see cref="Ipv4Cidr"/>, and is followed by other characters, this method will
        ///     return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4Cidr"/>.
        /// </returns>
        public static bool TryParse(string? text, out int charsRead, out Ipv4Cidr result)
        {
            var span = (text ?? string.Empty).AsSpan();
            charsRead = default;
            return TryParse(ref span, ref charsRead, out result);
        }
        
        /// <summary>
        /// Determines whether the specified character span represents a valid IPv4 CIDR.
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid CIDR.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4Cidr"/> representation of <paramref name="text"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="text"/> was able to be converted to an
        ///     <see cref="Ipv4Cidr"/>; otherwise, <see langword="false"/>.
        ///     If <paramref name="text"/> begins with a valid <see cref="Ipv4Cidr"/>, and is followed by other characters, this method will
        ///     return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4Cidr"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4Cidr result)
        {
            result = default;
            charsRead = default;
            return TryParse(ref text, ref charsRead, out result);
        }
        
        /// <summary>
        /// Determines whether the specified character span represents a valid IPv4 CIDR.
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4Cidr"/> representation of <paramref name="text"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="text"/> was able to be converted to an
        ///     <see cref="Ipv4Cidr"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out Ipv4Cidr result)
            => TryParse(text, out var charsRead, out result) && charsRead == text.Length;
        
        internal static bool TryParse(ref ReadOnlySpan<char> text, ref int charsRead, out Ipv4Cidr result)
        {
            result = default;
            if (!text.TryParseByte(ref charsRead, out var value) || value > 32)
                return false;
            result = new (value);
            return true;
        }
        
        #endregion TryParse
        
        #region Comparisons

        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="Ipv4Cidr"/> are equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Ipv4Cidr left, Ipv4Cidr right) => left.Equals(right);

        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="Ipv4Cidr"/> are not equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Ipv4Cidr left, Ipv4Cidr right) => !left.Equals(right);

        /// <summary>
        /// Returns a value indicating whether a specified <see cref="Ipv4Cidr"/> is greater than another specified <see cref="Ipv4Cidr"/>.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator >(Ipv4Cidr left, Ipv4Cidr right) => left.Value > right.Value;

        /// <summary>
        /// Returns a value indicating whether a specified <see cref="Ipv4Cidr"/> is less than another specified <see cref="Ipv4Cidr"/>.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator <(Ipv4Cidr left, Ipv4Cidr right) => left.Value < right.Value;

        /// <summary>
        /// Returns a value indicating whether a specified <see cref="Ipv4Cidr"/> is greater than or equal to another specified <see cref="Ipv4Cidr"/>.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator >=(Ipv4Cidr left, Ipv4Cidr right) => left.Value >= right.Value;

        /// <summary>
        /// Returns a value indicating whether a specified <see cref="Ipv4Cidr"/> is less than or equal to another specified <see cref="Ipv4Cidr"/>.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator <=(Ipv4Cidr left, Ipv4Cidr right) => left.Value <= right.Value;

        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4Cidr"/> is equal to an instance of <see cref="byte"/>
        /// </summary>
        /// <param name="left">
        /// An <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="byte"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Ipv4Cidr left, byte right) => left.Value == right;

        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4Cidr"/> is not equal to an instance of <see cref="byte"/>
        /// </summary>
        /// <param name="left">
        /// An <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="byte"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Ipv4Cidr left, byte right) => left.Value != right;

        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4Cidr"/> is greater than an instance of <see cref="byte"/>
        /// </summary>
        /// <param name="left">
        /// An <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="byte"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator >(Ipv4Cidr left, byte right) => left.Value > right;

        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4Cidr"/> is less than an instance of <see cref="byte"/>
        /// </summary>
        /// <param name="left">
        /// An <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="byte"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator <(Ipv4Cidr left, byte right) => left.Value < right;

        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4Cidr"/> is greater than or equal to an instance of <see cref="byte"/>
        /// </summary>
        /// <param name="left">
        /// An <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="byte"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is greater or equal to than <paramref name="right"/>; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator >=(Ipv4Cidr left, byte right) => left.Value >= right;

        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="Ipv4Cidr"/> is less than or equal to an instance of <see cref="byte"/>
        /// </summary>
        /// <param name="left">
        /// An <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="byte"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator <=(Ipv4Cidr left, byte right) => left.Value <= right;
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="byte"/> is equal to an instance of <see cref="Ipv4Cidr"/>
        /// </summary>
        /// <param name="left">
        /// A <see cref="byte"/> to compare.
        /// </param>
        /// <param name="right">
        /// An <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(byte left, Ipv4Cidr right) => left == right.Value;
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="byte"/> is not equal to an instance of <see cref="Ipv4Cidr"/>
        /// </summary>
        /// <param name="left">
        /// A <see cref="byte"/> to compare.
        /// </param>
        /// <param name="right">
        /// An <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(byte left, Ipv4Cidr right) => left != right.Value;
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="byte"/> is greater than an instance of <see cref="Ipv4Cidr"/>
        /// </summary>
        /// <param name="left">
        /// A <see cref="byte"/> to compare.
        /// </param>
        /// <param name="right">
        /// An <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator >(byte left, Ipv4Cidr right) => left > right.Value;
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="byte"/> is less than an instance of <see cref="Ipv4Cidr"/>
        /// </summary>
        /// <param name="left">
        /// A <see cref="byte"/> to compare.
        /// </param>
        /// <param name="right">
        /// An <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator <(byte left, Ipv4Cidr right) => left < right.Value;
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="byte"/> is greater than or equal to an instance of <see cref="Ipv4Cidr"/>
        /// </summary>
        /// <param name="left">
        /// A <see cref="byte"/> to compare.
        /// </param>
        /// <param name="right">
        /// An <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator >=(byte left, Ipv4Cidr right) => left >= right.Value;
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="byte"/> is less than or equal to than an instance of <see cref="Ipv4Cidr"/>
        /// </summary>
        /// <param name="left">
        /// A <see cref="byte"/> to compare.
        /// </param>
        /// <param name="right">
        /// An <see cref="Ipv4Cidr"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator <=(byte left, Ipv4Cidr right) => left <= right.Value;
        
        /// <summary>
        ///     Compares the value of this instance to a specified object that contains a specified
        ///     <see cref="Ipv4Cidr"/> value, and returns an integer that indicates whether this
        ///     instance is less than, equal to, or greater than the specified <see cref="Ipv4Cidr"/> value.
        /// </summary>
        /// <param name="obj">
        ///     A boxed object to compare, or null.
        /// </param>
        /// <returns>A signed number indicating the relative values of this instance and value.</returns>
        /// <exception cref="ArgumentException">
        /// value is not a <see cref="Ipv4Cidr"/> or <see cref="byte"/>.
        /// </exception>
        public int CompareTo(object? obj) => obj switch
        {
            null => 1,
            Ipv4Cidr other => CompareTo(other),
            byte other => CompareTo(other),
            _ => throw new ArgumentException($"Object must be of type {nameof(Ipv4Address)}"),
        };
        
        /// <summary>
        ///     Compares the value of this instance to a specified <see cref="Ipv4Cidr"/> value,
        ///     and returns an integer that indicates whether this instance is less than, equal to, or
        ///     greater than the specified <see cref="Ipv4Cidr"/> value.
        /// </summary>
        /// <param name="other">The object to compare to the current instance.</param>
        /// <returns>A signed number indicating the relative values of this instance and the value parameter.</returns>
        public int CompareTo(Ipv4Cidr other) => this.Value.CompareTo(other.Value);
        /// <summary>
        ///     Compares the value of this instance to a specified <see cref="byte"/> value,
        ///     and returns an integer that indicates whether this instance is less than, equal to, or
        ///     greater than the specified <see cref="byte"/> value.
        /// </summary>
        /// <param name="other">The object to compare to the current instance.</param>
        /// <returns>A signed number indicating the relative values of this instance and the value parameter.</returns>
        public int CompareTo(byte other) => this.Value.CompareTo(other);

        #endregion Comparisons

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


        #endregion Subnetting

    }
}