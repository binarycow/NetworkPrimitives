#nullable enable

using System;

namespace NetworkPrimitives
{
    internal interface ITryFormat
    {
        public int MaximumLengthRequired { get; }
        public bool TryFormat(Span<char> destination, out int charsWritten);
    }

    internal interface ITryFormattable : ITryFormat, IFormattable
    {
        public bool TryFormat(Span<char> destination, out int charsWritten, string? format, IFormatProvider? formatProvider);
    }
    

    [ExcludeFromCodeCoverage("Internal")]
    internal static class TryFormatExtensions
    {
        private const int MAXIMUM_STACKALLOC_LENGTH = 256;
        
        internal static string GetString<T>(
            this T tryFormat,
            Span<char> chars
        ) where T : ITryFormat
        {
            _ = tryFormat.TryFormat(chars, out var charsWritten);
            chars = chars[..charsWritten];
            return chars.CreateString();
        }
        
        
        internal static string GetString<T>(
            this T tryFormat
        ) where T : ITryFormat
        {
            var charsRequired = tryFormat.MaximumLengthRequired;
            var chars = charsRequired >= MAXIMUM_STACKALLOC_LENGTH
                ? new char[charsRequired]
                : stackalloc char[charsRequired];
            return tryFormat.GetString(chars);
        }
        
        internal static string GetString<T>(
            this T tryFormat, 
            string? format, 
            IFormatProvider? formatProvider
        ) where T : ITryFormattable
        {
            var charsRequired = tryFormat.MaximumLengthRequired;
            var chars = charsRequired >= MAXIMUM_STACKALLOC_LENGTH
                ? new char[charsRequired]
                : stackalloc char[charsRequired];
            return tryFormat.GetString(chars, format, formatProvider);
        }
        
        internal static string GetString<T>(
            this T tryFormat,
            Span<char> chars, 
            string? format, 
            IFormatProvider? formatProvider
        ) where T : ITryFormattable
        {
            _ = tryFormat.TryFormat(chars, out var charsWritten, format, formatProvider);
            chars = chars[..charsWritten];
            return chars.CreateString();
        }
    }
}