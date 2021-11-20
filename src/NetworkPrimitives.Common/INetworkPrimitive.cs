using NetworkPrimitives.Utilities;
using System;

namespace NetworkPrimitives
{
    internal interface INetworkPrimitive<TDerived> : IEquatable<TDerived>, ITryFormat
        where TDerived : INetworkPrimitive<TDerived>
    {
#if STATIC_ABSTRACT
        public static abstract TDerived Parse(string? text);
        public static abstract TDerived Parse(ReadOnlySpan<char> text);
        public static abstract bool TryParse(string? text, out TDerived result);
        public static abstract bool TryParse(string? text, out int charsRead, out TDerived result);
        public static abstract bool TryParse(ReadOnlySpan<char> text, out TDerived result);
        public static abstract bool TryParse(ReadOnlySpan<char> text, out int charsRead, out TDerived result);
#endif
    }
    internal interface IBinaryNetworkPrimitive<TDerived> : INetworkPrimitive<TDerived>
        where TDerived : IBinaryNetworkPrimitive<TDerived>
    {
        public bool TryWriteBytes(Span<byte> destination, out int bytesWritten);
        public byte[] GetBytes();
    }

    internal interface IFormattableNetworkPrimitive<TDerived> : INetworkPrimitive<TDerived>, ITryFormattable
        where TDerived : IFormattableNetworkPrimitive<TDerived>
    {
#if STATIC_ABSTRACT
        public static abstract TDerived ParseExact(string? text, string format);
        public static abstract TDerived ParseExact(ReadOnlySpan<char> text, string format);
        public static abstract bool TryParseExact(string? text, string format, out TDerived result);
        public static abstract bool TryParseExact(string? text, string format, out int charsRead, out TDerived result);
        public static abstract bool TryParseExact(ReadOnlySpan<char> text, string format, out TDerived result);
        public static abstract bool TryParseExact(ReadOnlySpan<char> text, string format, out int charsRead, out TDerived result);
#endif
    }
    internal interface IFormattableBinaryNetworkPrimitive<TDerived> : IBinaryNetworkPrimitive<TDerived>, ITryFormattable
        where TDerived : IFormattableBinaryNetworkPrimitive<TDerived>
    {

    }
}
