using System;
using System.Collections;
using System.Collections.Generic;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    /// <summary>
    /// Represents a set of <see cref="Ipv4Address"/>, using an <see cref="Ipv4Address"/> and a
    /// an <see cref="Ipv4WildcardMask"/> as criteria
    /// </summary>
    public readonly struct Ipv4NetworkMatch : INetworkPrimitive<Ipv4NetworkMatch>
    {
        /// <summary>
        /// The address to use as matching criteria
        /// </summary>
        public Ipv4Address Address { get; }
        
        /// <summary>
        /// The wildcard mask
        /// </summary>
        public Ipv4WildcardMask Mask { get; }
        
        /// <summary>
        /// Create an instance of <see cref="Ipv4NetworkMatch"/> from an <see cref="Ipv4Address"/> and <see cref="Ipv4WildcardMask"/>
        /// </summary>
        /// <param name="address">An <see cref="Ipv4Address"/></param>
        /// <param name="mask">An <see cref="Ipv4WildcardMask"/></param>
        public Ipv4NetworkMatch(Ipv4Address address, Ipv4WildcardMask mask)
        {
            this.Address = address & mask;
            this.Mask = mask;
        }
        
        /// <summary>
        /// Create an instance of <see cref="Ipv4NetworkMatch"/> from an <see cref="Ipv4Address"/> and <see cref="Ipv4SubnetMask"/>
        /// </summary>
        /// <param name="address">An <see cref="Ipv4Address"/></param>
        /// <param name="mask">An <see cref="Ipv4SubnetMask"/></param>
        public Ipv4NetworkMatch(Ipv4Address address, Ipv4SubnetMask mask) : this(address, mask.ToWildcardMask())
        {
            
        }
        
        /// <summary>
        /// Create an instance of <see cref="Ipv4NetworkMatch"/> from an <see cref="Ipv4Address"/> and <see cref="Ipv4Cidr"/>
        /// </summary>
        /// <param name="address">An <see cref="Ipv4Address"/></param>
        /// <param name="cidr">An <see cref="Ipv4Cidr"/></param>
        public Ipv4NetworkMatch(Ipv4Address address, Ipv4Cidr cidr) : this(address, cidr.ToWildcardMask())
        {
            
        }
        
        /// <summary>
        /// Create an instance of <see cref="Ipv4NetworkMatch"/> from a <see cref="Ipv4Subnet"/>
        /// </summary>
        /// <param name="subnet">An <see cref="Ipv4Subnet"/></param>
        public Ipv4NetworkMatch(Ipv4Subnet subnet) : this(subnet.NetworkAddress, subnet.Mask.ToWildcardMask())
        {
            
        }

        #region Matching

        /// <summary>
        /// Determines if an address is covered by this <see cref="Ipv4NetworkMatch"/>
        /// </summary>
        /// <param name="address">The address to check.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="address"/> belongs to this network match.
        /// </returns>
        public bool IsMatch(Ipv4Address address) => (address & Mask) == (Address & Mask);

        /// <summary>
        /// Determines if an address is covered by <paramref name="match"/>
        /// </summary>
        /// <param name="match">An <see cref="Ipv4NetworkMatch"/> instance</param>
        /// <param name="address">The address to check.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="address"/> belongs to <paramref name="match"/>
        /// </returns>
        public static bool IsMatch(Ipv4NetworkMatch match, Ipv4Address address) => match.IsMatch(address);

        #endregion Matching
        
        #region Formatting

        int ITryFormat.MaximumLengthRequired => Ipv4Address.MAXIMUM_LENGTH + Ipv4Address.MAXIMUM_LENGTH + 1;
        
        /// <summary>
        /// Converts an <see cref="Ipv4NetworkMatch"/> to its <see cref="string"/> representation.
        /// </summary>
        /// <returns>
        /// A string that contains this network match.
        /// </returns>
        public override string ToString() => this.GetString();

        
        /// <summary>
        /// Tries to format the current network match into the provided span.
        /// </summary>
        /// <param name="destination">
        /// When this method returns, the network match, as a span of characters.
        /// </param>
        /// <param name="charsWritten">
        /// When this method returns, the number of characters written into the span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the formatting was successful; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryFormat(Span<char> destination, out int charsWritten) 
            => Ipv4Formatting.TryFormat(this, destination, out charsWritten);
        
        #endregion Formatting

        #region Equality

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4NetworkMatch"/>.
        /// </summary>
        /// <param name="other">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Ipv4NetworkMatch other) => this.Address.Equals(other.Address) && this.Mask.Equals(other.Mask);
        
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4NetworkMatch"/>.
        /// </summary>
        /// <param name="obj">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is an instance of <see cref="Ipv4NetworkMatch"/> and
        /// equals the value of this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj is Ipv4NetworkMatch other && Equals(other);
        
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => HashCode.Combine(this.Address, this.Mask);
        
        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="Ipv4NetworkMatch"/> are equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4NetworkMatch"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4NetworkMatch"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Ipv4NetworkMatch left, Ipv4NetworkMatch right) => left.Equals(right);
        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="Ipv4NetworkMatch"/> are not equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4NetworkMatch"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4NetworkMatch"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Ipv4NetworkMatch left, Ipv4NetworkMatch right) => !left.Equals(right);

        #endregion Equality

        #region Parse

        /// <summary>
        /// Parse a network match into an <see cref="Ipv4NetworkMatch"/> instance.
        /// </summary>
        /// <param name="text">
        /// A string that contains a network match
        /// </param>
        /// <returns>
        /// An <see cref="Ipv4NetworkMatch"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid <see cref="Ipv4NetworkMatch"/>.
        /// </exception>
        public static Ipv4NetworkMatch Parse(string? text)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));
            return Ipv4NetworkMatch.TryParse(text, out var result) ? result : throw new FormatException();
        }

        /// <summary>
        /// Parse a network match into an <see cref="Ipv4NetworkMatch"/> instance.
        /// </summary>
        /// <param name="span">
        /// A span that contains a network match
        /// </param>
        /// <returns>
        /// An <see cref="Ipv4NetworkMatch"/> instance.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="span"/> is not a valid <see cref="Ipv4NetworkMatch"/>.
        /// </exception>
        public static Ipv4NetworkMatch Parse(ReadOnlySpan<char> span)
            => TryParse(span, out var result) ? result : throw new FormatException();


        #endregion Parse

        #region TryParse
        
        
        /// <summary>
        /// Determines whether the specified string represents a valid <see cref="Ipv4NetworkMatch"/>
        /// </summary>
        /// <param name="text">
        /// The string to validate.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4NetworkMatch"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an <see cref="Ipv4NetworkMatch"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string? text, out Ipv4NetworkMatch result)
            => TryParse(text, out var charsRead, out result) && charsRead == text?.Length;

        
        /// <summary>
        /// Determines whether the specified string represents a valid <see cref="Ipv4NetworkMatch"/>
        /// </summary>
        /// <param name="text">
        /// The string to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid Ipv4 network match.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4NetworkMatch"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an <see cref="Ipv4NetworkMatch"/>;
        /// otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="text"/> begins with a valid <see cref="Ipv4NetworkMatch"/>, and is followed by other
        /// characters, this method will return <see langword="true"/>.  <paramref name="charsRead"/> will contain the
        /// length of the valid <see cref="Ipv4NetworkMatch"/>.
        /// </returns>
        public static bool TryParse(string? text, out int charsRead, out Ipv4NetworkMatch result)
            => TryParse(text.GetSpan(), out charsRead, out result);

        
        /// <summary>
        /// Determines whether the specified character span represents a valid <see cref="Ipv4NetworkMatch"/>
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4NetworkMatch"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an <see cref="Ipv4NetworkMatch"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out Ipv4NetworkMatch result)
            => TryParse(text, out var charsRead, out result) && charsRead == text.Length;

        
        /// <summary>
        /// Determines whether the specified character span represents a valid <see cref="Ipv4NetworkMatch"/>
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid Ipv4 network match.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4NetworkMatch"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an <see cref="Ipv4NetworkMatch"/>;
        /// otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="text"/> begins with a valid <see cref="Ipv4NetworkMatch"/>, and is followed by other characters,
        /// this method will return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of
        /// the valid <see cref="Ipv4NetworkMatch"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4NetworkMatch result)
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
        
        #endregion TryParse

    }
}