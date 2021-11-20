using System;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Yang
{
    public readonly struct YangIdentifier : IEquatable<YangIdentifier>
    {
        internal readonly string? Value;
        private YangIdentifier(string value) => this.Value = value;

        public static implicit operator string(YangIdentifier identifier) => identifier.Value ?? string.Empty;
        public override string ToString() => this;
        public bool Equals(YangIdentifier other) => this.Value == other.Value;
        public override bool Equals(object? obj) => obj is YangIdentifier other && Equals(other);
        public override int GetHashCode() => (this.Value != null ? this.Value.GetHashCode() : 0);
        public static bool operator ==(YangIdentifier left, YangIdentifier right) => left.Equals(right);
        public static bool operator !=(YangIdentifier left, YangIdentifier right) => !left.Equals(right);


        public static QualifiedName operator +(YangModuleName module, YangIdentifier name) => new (module, name);
        public static PrefixedName operator +(YangPrefix? prefix, YangIdentifier name) => new (prefix, name);
        public static PrefixedName operator +(YangPrefix prefix, YangIdentifier name) => new (prefix, name);

        public static YangIdentifier Parse(YangModuleName value)
            => new YangIdentifier(value);
        public static YangIdentifier Parse(YangPrefix value)
            => new YangIdentifier(value);

        public static YangIdentifier Parse(string? text, YangRfc rfc = YangRfc.Rfc6020)
            => TryParse(text, out var result, rfc) ? result : throw new FormatException();

        public static bool TryParse(string? text, out YangIdentifier result, YangRfc rfc = YangRfc.Rfc6020)
        {
            var span = new SpanWrapper(text);
            var charsRead = 0;
            return TryParse(ref span, ref charsRead, out result, rfc) && charsRead == text?.Length;
        }
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        
        public static YangIdentifier Parse(ReadOnlySpan<char> text, YangRfc rfc = YangRfc.Rfc6020)
            => TryParse(text, out var result, rfc) ? result : throw new FormatException();

        public static bool TryParse(ReadOnlySpan<char> text, out YangIdentifier result, YangRfc rfc = YangRfc.Rfc6020)
            => TryParse(text, out var charsRead, out result, rfc) && charsRead == text.Length;
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out YangIdentifier result, YangRfc rfc = YangRfc.Rfc6020)
        {
            var span = new SpanWrapper(text);
            charsRead = default;
            return TryParse(ref span, ref charsRead, out result, rfc);
        }
#endif
        
        
        internal static bool TryParse(ref SpanWrapper text, ref int charsRead, out YangIdentifier result, YangRfc rfc)
        {
            if (text.Length == 0 || !CheckXml(text, rfc) || !IsValidFirstChar(text[0]))
            {
                result = default;
                return false;
            }
            var original = text;
            text = text[1..];
            var length = 1;
            while (text.Length > 0 && IsValidChar(text[0]))
            {
                ++length;
                text = text[1..];
            }
            var substring = original[..length];
            text = original[length..];
            charsRead += length;
            result = new (substring.GetString());
            return true;
            static bool CheckXml(SpanWrapper text, YangRfc rfc)
            {
                if (rfc != YangRfc.Rfc6020 || text.Length < 3)
                    return true;
                return !(text[0] is 'X' or 'x' && text[1] is 'M' or 'm' && text[2] is 'L' or 'l');
            }
            static bool IsValidFirstChar(char ch) => ch switch
            {
                >= 'A' and <= 'Z' => true,
                >= 'a' and <= 'z' => true,
                '_' => true,
                _ => false,
            };
            static bool IsValidChar(char ch) => ch switch
            {
                >= 'A' and <= 'Z' => true,
                >= 'a' and <= 'z' => true,
                >= '0' and <= '9' => true,
                '_' => true,
                '-' => true,
                '.' => true,
                _ => false,
            };
        }
    }
}