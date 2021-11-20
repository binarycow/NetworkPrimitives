using System;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Yang
{
    public readonly struct PrefixedName : IEquatable<PrefixedName>
    {
        public YangPrefix? Prefix { get; }
        public YangIdentifier Name { get; }
        public PrefixedName(YangPrefix? prefix, YangIdentifier name)
        {
            this.Prefix = prefix;
            this.Name = name;
        }        
        public PrefixedName(YangIdentifier name) : this(null, name)
        {
        }
        public override string ToString() => Prefix is null ? Name.ToString() : $"{Prefix.ToString()}:{Name.ToString()}";

        public bool Equals(PrefixedName other) => Nullable.Equals(this.Prefix, other.Prefix) && this.Name.Equals(other.Name);
        public override bool Equals(object? obj) => obj is PrefixedName other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(this.Prefix, this.Name);
        public static bool operator ==(PrefixedName left, PrefixedName right) => left.Equals(right);
        public static bool operator !=(PrefixedName left, PrefixedName right) => !left.Equals(right);
        
        public void Deconstruct(out YangPrefix? prefix, out YangIdentifier name)
        {
            prefix = this.Prefix;
            name = this.Name;
        }
        
        
        
        public static bool TryParse(string? text, out PrefixedName result, YangRfc rfc = YangRfc.Rfc6020)
        {
            var span = new SpanWrapper(text);
            var charsRead = 0;
            return TryParse(ref span, ref charsRead, out result, rfc) && charsRead == text?.Length;
        }
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        
        public static PrefixedName Parse(ReadOnlySpan<char> text, YangRfc rfc = YangRfc.Rfc6020)
            => TryParse(text, out var result, rfc) ? result : throw new FormatException();

        public static bool TryParse(ReadOnlySpan<char> text, out PrefixedName result, YangRfc rfc = YangRfc.Rfc6020)
            => TryParse(text, out var charsRead, out result, rfc) && charsRead == text.Length;
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out PrefixedName result, YangRfc rfc = YangRfc.Rfc6020)
        {
            var span = new SpanWrapper(text);
            charsRead = default;
            return TryParse(ref span, ref charsRead, out result, rfc);
        }
#endif
        
        internal static bool TryParse(ref SpanWrapper text, ref int charsRead, out PrefixedName result, YangRfc rfc)
        {
            result = default;
            var textCopy = text;
            var charCopy = charsRead;
            if (!YangIdentifier.TryParse(ref textCopy, ref charCopy, out var prefix, rfc))
                return false;
            charsRead = charCopy;
            text = textCopy;
            if (!textCopy.TryReadCharacter(ref charCopy, ':'))
            {
                result = new (prefix);
                return true;
            }
            if (!YangIdentifier.TryParse(ref textCopy, ref charCopy, out var identifier, rfc))
            {
                result = new (prefix);
                return true;
            }
            charsRead = charCopy;
            text = textCopy;
            result = new PrefixedName(YangPrefix.Parse(prefix), identifier);
            return true;
        }
    }
}