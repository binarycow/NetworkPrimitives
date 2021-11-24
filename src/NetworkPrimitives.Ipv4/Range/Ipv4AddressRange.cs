using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    /// <summary>
    /// Represents a single range of <see cref="Ipv4Address"/>
    /// </summary>
    public readonly partial struct Ipv4AddressRange 
        : INetworkPrimitive<Ipv4AddressRange>, 
            IEnumerable<Ipv4Address>, 
            ISlice<Ipv4AddressRange, Ipv4Address>
    {
        internal Ipv4Address StartAddress { get; }
        internal Ipv4Address LastAddressInclusive => StartAddress.AddInternal((uint)this.inclusiveLength);
        internal bool AllIpsSameFirstThreeOctets => (StartAddress / Ipv4Cidr.Parse(24)).Contains(LastAddressInclusive);
        private readonly ulong inclusiveLength;

        internal Ipv4AddressRange(Ipv4Address startAddress, ulong inclusiveLength)
        {
            this.StartAddress = startAddress;
            this.inclusiveLength = inclusiveLength;
        }

        #region Indexing

        /// <summary>
        /// Gets the address at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the address to get.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0 or <paramref name="index"/> is equal to or greater than <see cref="Length"/>.
        /// </exception>
        public Ipv4Address this[int index] 
            => index >= 0 && (ulong)index <= inclusiveLength
                ? this.StartAddress.AddInternal((uint)index)
                : throw new ArgumentOutOfRangeException(nameof(index));

        /// <summary>
        /// Gets the address at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the address to get.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is equal to or greater than <see cref="Length"/>.
        /// </exception>
        public Ipv4Address this[uint index]
            => index <= inclusiveLength
                ? this.StartAddress.AddInternal(index)
                : throw new ArgumentOutOfRangeException(nameof(index));
        

        /// <summary>
        /// The number of addresses in this range.
        /// </summary>
        public ulong Length => this.inclusiveLength + 1;

        int ISlice<Ipv4AddressRange, Ipv4Address>.Length => (int)this.Length;

        #endregion Indexing

        #region Formatting

        int ITryFormat.MaximumLengthRequired => Ipv4Address.MAXIMUM_LENGTH + Ipv4Address.MAXIMUM_LENGTH + 1;

        /// <summary>
        /// Tries to format the current range into the provided span.
        /// </summary>
        /// <param name="destination">
        /// When this method returns, the range as a span of characters.
        /// </param>
        /// <param name="charsWritten">
        /// When this method returns, the number of characters written into the span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the formatting was successful; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryFormat(Span<char> destination, out int charsWritten) 
            => Ipv4Formatting.TryFormat(this, destination, out charsWritten, null, null);

        #endregion Formatting

        #region Subnetting

        /// <summary>
        /// Check to see if the entire range is inside of a given subnet
        /// </summary>
        /// <param name="subnet">The subnet to check</param>
        /// <returns>
        /// <see langword="true"/> if the entire range represented by this instance is inside of the subnet <paramref name="subnet"/>
        /// </returns>
        public bool IsSubnet(out Ipv4Subnet subnet)
        {
            if (!SubnetMaskLookups.TryGetMaskFromTotalHosts(this.Length, out var mask))
            {
                subnet = default;
                return false;
            }
            var subnetMask = Ipv4SubnetMask.Parse(mask);
            if ((this.StartAddress & subnetMask) != this.StartAddress)
            {
                subnet = default;
                return false;
            }
            subnet = this.StartAddress + subnetMask;
            return true;
        }

        #endregion Subnetting

        #region Slice
        
        /// <summary>
        /// Create a smaller range from this range.
        /// </summary>
        /// <param name="start">The start index</param>
        /// <param name="length">The length of the resulting range</param>
        /// <returns>A new <see cref="Ipv4AddressRange"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException" />
        public Ipv4AddressRange Slice(int start, int length)
        {
            if(start < 0 || length < 0) throw new ArgumentOutOfRangeException();
            return Slice((uint)start, (uint)length);
        }
        
        /// <summary>
        /// Create a smaller range from this range.
        /// </summary>
        /// <param name="start">The start index</param>
        /// <returns>A new <see cref="Ipv4AddressRange"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException" />
        public Ipv4AddressRange Slice(int start)
        {
            if(start < 0) throw new ArgumentOutOfRangeException();
            return Slice((uint)start);
        }

        /// <summary>
        /// Create a smaller range from this range.
        /// </summary>
        /// <param name="start">The start index</param>
        /// <returns>A new <see cref="Ipv4AddressRange"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException" />
        public Ipv4AddressRange Slice(ulong start) => Slice((uint)start, (uint)(Length - start));
        
        
        /// <summary>
        /// Create a smaller range from this range.
        /// </summary>
        /// <param name="start">The start index</param>
        /// <param name="length">The length of the resulting range</param>
        /// <returns>A new <see cref="Ipv4AddressRange"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException" />
        public Ipv4AddressRange Slice(uint start, uint length)
        {
            if (start > this.Length || length > (this.Length - start))
                throw new ArgumentOutOfRangeException();
            // Make sure the length is *INCLUSIVE* due to uint bounds.
            --length;
            return new (this.StartAddress.AddInternal(start), length);
        }

        #endregion Slice

        #region Enumeration

        /// <summary>
        /// Gets a <see langword="ref"/> <see langword="struct"/> enumerator for this range.
        /// </summary>
        /// <returns>An <see cref="Ipv4AddressEnumerator"/></returns>
        public Ipv4AddressEnumerator GetEnumerator() 
            => new (this.StartAddress, this.inclusiveLength);

        IEnumerator IEnumerable.GetEnumerator() => new ClassEnumerator(this.StartAddress, this.inclusiveLength);

        IEnumerator<Ipv4Address> IEnumerable<Ipv4Address>.GetEnumerator() => new ClassEnumerator(this.StartAddress, this.inclusiveLength);


        /// <summary>
        /// Converts this range to an <see cref="IEnumerable{Ipv4Address}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Ipv4Address}"/></returns>
        public IEnumerable<Ipv4Address> ToEnumerable() => new ClassEnumerator(this.StartAddress, this.inclusiveLength);


        #endregion Enumeration

        #region Equality

        
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4AddressRange"/>.
        /// </summary>
        /// <param name="other">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Ipv4AddressRange other) 
            => this.StartAddress.Equals(other.StartAddress) && this.inclusiveLength == other.inclusiveLength;
        
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4AddressRange"/>.
        /// </summary>
        /// <param name="obj">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is an instance of <see cref="Ipv4AddressRange"/> and equals the
        /// value of this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj is Ipv4AddressRange other && Equals(other);
        
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => HashCode.Combine(this.StartAddress, this.inclusiveLength);
        
        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="Ipv4AddressRange"/> are equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4AddressRange"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4AddressRange"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Ipv4AddressRange left, Ipv4AddressRange right) => left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="Ipv4AddressRange"/> are not equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4AddressRange"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4AddressRange"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Ipv4AddressRange left, Ipv4AddressRange right) => !left.Equals(right);


        #endregion Equality

        #region Parsing

        
        /// <summary>
        /// Converts an IP address range string to an <see cref="Ipv4AddressRange"/> instance.
        /// </summary>
        /// <param name="text">
        /// A string that contains an IP address range string
        /// </param>
        /// <returns>
        /// An <see cref="Ipv4AddressRange"/> instance
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid <see cref="Ipv4AddressRange"/>
        /// </exception>
        public static Ipv4AddressRange Parse(string? text)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));
            return Ipv4AddressRange.TryParse(text, out var value)
                ? value
                : throw new FormatException();
        }

        
        /// <summary>
        /// Converts an IP address range string to an <see cref="Ipv4AddressRange"/> instance.
        /// </summary>
        /// <param name="text">
        /// A string that contains an IP address range string
        /// </param>
        /// <returns>
        /// An <see cref="Ipv4AddressRange"/> instance
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid <see cref="Ipv4AddressRange"/>
        /// </exception>
        public static Ipv4AddressRange Parse(ReadOnlySpan<char> text)
            => TryParse(text, out var value)
                ? value
                : throw new FormatException();

        #endregion Parsing

        #region TryParse

        
        /// <summary>
        /// Determines whether the specified string represents a valid <see cref="Ipv4AddressRange"/>
        /// </summary>
        /// <param name="text">
        /// The string to validate.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4AddressRange"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an IP address range; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string? text, out Ipv4AddressRange result)
        {
            result = default;
            return text is not null && Ipv4AddressRange.TryParse(text, out var charsRead, out result) && charsRead == text.Length;
        }

        
        /// <summary>
        /// Determines whether the specified string represents a valid <see cref="Ipv4AddressRange"/>
        /// </summary>
        /// <param name="text">
        /// The string to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid <see cref="Ipv4AddressRange"/>
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4AddressRange"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an IP address range; otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="text"/> begins with a valid <see cref="Ipv4AddressRange"/>, and is followed by other characters, this method will
        /// return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4AddressRange"/>.
        /// </returns>
        public static bool TryParse(string? text, out int charsRead, out Ipv4AddressRange result)
        {
            var span = (text ?? string.Empty).AsSpan();
            charsRead = default;
            return TryParse(ref span, ref charsRead, out result);
        }

        /// <summary>
        /// Determines whether the specified character span represents a valid <see cref="Ipv4AddressRange"/>
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4AddressRange"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an IP address range; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out Ipv4AddressRange result)
            => TryParse(text, out var charsRead, out result) && charsRead == text.Length;

        /// <summary>
        /// Determines whether the specified character span represents a valid <see cref="Ipv4AddressRange"/>
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid <see cref="Ipv4AddressRange"/>
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4AddressRange"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an IP address range; otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="text"/> begins with a valid <see cref="Ipv4AddressRange"/>, and is followed by other characters, this method will
        /// return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4AddressRange"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4AddressRange result)
        {
            charsRead = default;
            return TryParse(ref text, ref charsRead, out result);
        }

        
        internal static bool TryParse(ref ReadOnlySpan<char> text, ref int charsRead, out Ipv4AddressRange result)
        {
            result = default;
            if (!Ipv4Subnet.TryParseInternal(ref text, ref charsRead, out var subnet, out var implicitSlash32))
                return false;
            result = subnet.GetAllAddresses();
            if (!implicitSlash32 || subnet.Mask.IsSlash32 == false)
                return true;
            var address = subnet.NetworkAddress;
            var textCopy = text;
            var charsReadCopy = charsRead;
            if (!textCopy.TryReadCharacter(ref charsReadCopy, '-'))
                return true;
            var lastOctet = address[3];
            if (!textCopy.TryParseByte(ref charsReadCopy, out var newOctet) || newOctet < lastOctet)
                return true;
            text = textCopy;
            charsRead = charsReadCopy;
            result = new Ipv4AddressRange(address, (uint)(newOctet - lastOctet));
            return true;
        }

        

        #endregion TryParse
    }
}