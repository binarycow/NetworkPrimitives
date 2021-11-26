#nullable enable

using System;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Switching
{
    public readonly struct MacAddress : IFormattableNetworkPrimitive<MacAddress>
    {
        internal const int MAXIMUM_LENGTH_REQUIRED = 23; // F-E-E-D-D-E-A-D-B-E-E-F
        internal ushort High { get; }
        internal ushort Mid { get; }
        internal ushort Low { get; }

        // TODO: Only for testing, for now.  Make this private later.
        internal MacAddress(ushort high, ushort mid, ushort low)
        {
            this.High = high;
            this.Mid = mid;
            this.Low = low;
        }


        #region Equality

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="MacAddress"/>.
        /// </summary>
        /// <param name="other">
        /// A MAC address to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(MacAddress other) => this.High == other.High && this.Mid == other.Mid && this.Low == other.Low;
        
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is an instance of <see cref="MacAddress"/>
        /// and equals the value of this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj is MacAddress other && Equals(other);
        
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => HashCode.Combine(this.High, this.Mid, this.Low);
        
        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="MacAddress"/> are equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="MacAddress"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="MacAddress"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(MacAddress left, MacAddress right) => left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="MacAddress"/> are not equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="MacAddress"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="MacAddress"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(MacAddress left, MacAddress right) => !left.Equals(right);

        #endregion Equality

        #region Formatting

        /// <summary>
        /// Formats the current MAC address as a string
        /// </summary>
        /// <returns>
        /// The string representation of this MAC address
        /// </returns>
        public override string ToString()
            => this.ToString(MacAddressFormats.Default, null);

        /// <summary>
        /// Formats the current MAC address as a string
        /// </summary>
        /// <param name="macFormat">
        /// The standard MAC address format to use
        /// </param>
        /// <returns>
        /// The string representation of this MAC address
        /// </returns>
        public string ToString(MacAddressFormat macFormat)
            => this.ToString(macFormat, null);
        
        /// <summary>
        /// Formats the current MAC address as a string
        /// </summary>
        /// <param name="format">
        /// A custom MAC address format string
        /// </param>
        /// <returns>
        /// The string representation of this MAC address
        /// </returns>
        public string ToString(string? format)
            => this.ToString(MacAddressFormats.Default, format);
        
        /// <summary>
        /// Formats the current MAC address as a string
        /// </summary>
        /// <param name="macFormat">
        /// The standard MAC address format to use
        /// </param>
        /// <param name="format">
        /// A custom MAC address format string that overrides the MAC address format specified by <paramref name="macFormat"/>
        /// </param>
        /// <returns>
        /// The string representation of this MAC address
        /// </returns>
        public string ToString(MacAddressFormat macFormat, string? format)
        {
            Span<char> chars = stackalloc char[23];
            TryFormat(chars, out var charsWritten, macFormat, format);
            chars = chars[..charsWritten];
            return chars.CreateString();
        }

        /// <summary>
        /// Tries to format the current mac address into the provided character span.
        /// </summary>
        /// <param name="destination">
        /// When this method returns, the mac address, as a span of characters.
        /// </param>
        /// <param name="charsWritten">
        /// When this method returns, the number of characters written into the span.
        /// </param>
        /// <param name="macFormat">
        /// The standard MAC address format to use
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the formatting was successful; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryFormat(Span<char> destination, out int charsWritten, MacAddressFormat macFormat)
            => MacFormatting.TryFormat(this, destination, out charsWritten, null, macFormat);
        
        /// <summary>
        /// Tries to format the current mac address into the provided character span.
        /// </summary>
        /// <param name="destination">
        /// When this method returns, the mac address, as a span of characters.
        /// </param>
        /// <param name="charsWritten">
        /// When this method returns, the number of characters written into the span.
        /// </param>
        /// <param name="macFormat">
        /// The standard MAC address format to use
        /// </param>
        /// <param name="format">
        /// A custom MAC address format string that overrides the MAC address format specified by <paramref name="macFormat"/>
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the formatting was successful; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryFormat(Span<char> destination, out int charsWritten, MacAddressFormat macFormat, string? format)
            => MacFormatting.TryFormat(this, destination, out charsWritten, format, macFormat);


        /// <summary>
        /// Tries to format the current mac address into the provided character span.
        /// </summary>
        /// <param name="destination">
        /// When this method returns, the mac address, as a span of characters.
        /// </param>
        /// <param name="charsWritten">
        /// When this method returns, the number of characters written into the span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the formatting was successful; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryFormat(Span<char> destination, out int charsWritten) 
            => MacFormatting.TryFormat(this, destination, out charsWritten, null, MacAddressFormats.Default);


        bool ITryFormattable.TryFormat(Span<char> destination, out int charsWritten, string? format, IFormatProvider? formatProvider)
        {
            if(formatProvider is null)
                return TryFormat(destination, out charsWritten, MacAddressFormats.Default, format);
            if (formatProvider is not MacAddressFormat macAddressFormat)
                throw new ArgumentException("Unexpected format provider.", nameof(formatProvider));
            return TryFormat(destination, out charsWritten, macAddressFormat, format);
        }

        string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
        {
            Span<char> chars = stackalloc char[23];
            return this.GetString(chars, format, formatProvider);
        }

        
        public bool TryWriteBytes(Span<byte> destination, out int bytesWritten)
        {
            bytesWritten = 0;
            if (destination.Length < 6)
                return false;
            return High.TryWriteBigEndian(ref destination, ref bytesWritten)
                && Mid.TryWriteBigEndian(ref destination, ref bytesWritten)
                && Low.TryWriteBigEndian(ref destination, ref bytesWritten);
        }

        public byte[] GetBytes()
        {
            var bytes = new byte[6];
            TryWriteBytes(bytes, out _);
            return bytes;
        }

        int ITryFormat.MaximumLengthRequired => MAXIMUM_LENGTH_REQUIRED;

        #endregion Formatting
    }
}