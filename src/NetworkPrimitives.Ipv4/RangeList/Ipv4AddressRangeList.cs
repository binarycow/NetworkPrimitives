#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NetworkPrimitives.Ipv4
{
    /// <summary>
    /// Represents an immutable list of <see cref="Ipv4AddressRange"/>
    /// </summary>
    public sealed partial class Ipv4AddressRangeList : IReadOnlyList<Ipv4AddressRange>
    {
        private IReadOnlyList<Ipv4AddressRange> Items { get; }
        /// <summary>
        /// An empty instance of <see cref="Ipv4AddressRangeList"/>
        /// </summary>
        public static Ipv4AddressRangeList Empty { get; } = new ();
        private Ipv4AddressRangeList() : this(Array.Empty<Ipv4AddressRange>()) { }
        private Ipv4AddressRangeList(IReadOnlyList<Ipv4AddressRange> ranges) => Items = ranges;

        /// <summary>
        /// Create a mutable list builder
        /// </summary>
        /// <returns>
        /// An instance of <see cref="Builder"/>
        /// </returns>
        public Builder ToBuilder() => new Builder(this.Items.ToList());

        
        
        #region IReadOnlyList

        /// <summary>
        /// The number of <see cref="Ipv4AddressRange"/> in this instance.
        /// </summary>
        public int Count => Items.Count;

        IEnumerator<Ipv4AddressRange> IEnumerable<Ipv4AddressRange>.GetEnumerator() => Items.GetEnumerator();

        /// <summary>
        /// Get a ref struct enumerator to iterate over the ranges covered by this range list.
        /// </summary>
        /// <returns>
        /// A ref struct enumerator
        /// </returns>
        public Ipv4AddressRangeListEnumerator GetEnumerator() => new (new (Items));
        
        /// <summary>
        /// Get a listing of all individual addresses in this set of ranges.
        /// </summary>
        /// <returns>
        /// An <see cref="Ipv4AddressListSpan"/> that represents all individual IP addresses in this instance.
        /// </returns>
        public Ipv4AddressListSpan GetAllAddresses() => Ipv4AddressListSpan.CreateNew(Items);

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        /// <summary>
        /// Gets the range at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the range to get.
        /// </param>
        public Ipv4AddressRange this[int index] => this.Items[index];

        #endregion IReadOnlyList
        
        
        #region Parse

        /// <summary>
        /// Converts an IP address range string to an <see cref="Ipv4AddressRangeList"/> instance.
        /// </summary>
        /// <param name="text">
        /// A string that contains an IP address range string
        /// </param>
        /// <returns>
        /// The <see cref="Ipv4AddressRangeList"/> representation of <paramref name="text"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid IP address range string.
        /// </exception>
        public static Ipv4AddressRangeList Parse(string? text)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));
            return Ipv4AddressRangeList.TryParse(text, out var result)
                ? result
                : throw new FormatException();
        }


        /// <summary>
        /// Converts an IP address range character span to an <see cref="Ipv4AddressRangeList"/> instance.
        /// </summary>
        /// <param name="text">
        /// A string that contains an IP address range character span
        /// </param>
        /// <returns>
        /// The <see cref="Ipv4AddressRangeList"/> representation of <paramref name="text"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid IP address range string.
        /// </exception>
        public static Ipv4AddressRangeList Parse(ReadOnlySpan<char> text)
            => TryParse(text, out var result)
                ? result
                : throw new FormatException();
        

        #endregion Parse

        #region TryParse

        
        /// <summary>
        /// Determines whether the specified string represents a valid IPv4 address range list.
        /// </summary>
        /// <param name="text">
        /// The address range list to validate
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4AddressRangeList"/> version of the string.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an address range list; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string? text, [NotNullWhen(true)] out Ipv4AddressRangeList? result)
        {
            result = default;
            return text is not null && Ipv4AddressRangeList.TryParse(text, out var charsRead, out result) && charsRead == text.Length;
        }

        
        /// <summary>
        /// Determines whether the specified string represents a valid IPv4 address range list.
        /// </summary>
        /// <param name="text">
        /// The address range list to validate
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid address range list
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4AddressRangeList"/> version of the string.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an address range list; otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="text"/> begins with a valid <see cref="Ipv4AddressRangeList"/>, and is followed by other characters, this method will
        /// return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4AddressRangeList"/>.
        /// </returns>
        public static bool TryParse(string? text, out int charsRead, [NotNullWhen(true)] out Ipv4AddressRangeList? result)
        {
            var span = text.GetSpan();
            charsRead = default;
            return TryParse(ref span, ref charsRead, out result);
        }

        
        /// <summary>
        /// Determines whether the specified character span represents a valid IPv4 address range list.
        /// </summary>
        /// <param name="text">
        /// The address range list to validate
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4AddressRangeList"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an address range list; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, [NotNullWhen(true)] out Ipv4AddressRangeList? result)
        {
            return TryParse(text, out var charsRead, out result) && charsRead == text.Length;
        }

        
        /// <summary>
        /// Determines whether the specified character span represents a valid IPv4 address range list.
        /// </summary>
        /// <param name="text">
        /// The address range list to validate
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid address range list
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4AddressRangeList"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an address range list; otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="text"/> begins with a valid <see cref="Ipv4AddressRangeList"/>, and is followed by other characters, this method will
        /// return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4AddressRangeList"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, [NotNullWhen(true)] out Ipv4AddressRangeList? result)
        {
            charsRead = 0;
            return TryParse(ref text, ref charsRead, out result);
        }

        internal static bool TryParse(ref ReadOnlySpan<char> text, ref int charsRead, [NotNullWhen(true)] out Ipv4AddressRangeList? result)
        {
            result = default;
            var textCopy = text;
            var charsReadCopy = charsRead;
            _ = textCopy.TryConsumeWhiteSpace(ref charsReadCopy);
            if (!Ipv4AddressRange.TryParse(ref textCopy, ref charsReadCopy, out var range))
                return false;
            text = textCopy;
            charsRead = charsReadCopy;

            // var builder = ImmutableList.CreateBuilder<Ipv4AddressRange>();
            var builder = new List<Ipv4AddressRange> { range };

            while (textCopy.TryConsumeWhiteSpace(ref charsReadCopy))
            {
                if (!Ipv4AddressRange.TryParse(ref textCopy, ref charsReadCopy, out range))
                    break;
                builder.Add(range);
                text = textCopy;
                charsRead = charsReadCopy;
            }
            _ = textCopy.TryConsumeWhiteSpace(ref charsReadCopy);
            text = textCopy;
            charsRead = charsReadCopy;
            result = new (builder.AsReadOnly());
            return true;
        }

        #endregion TryParse



    }
}