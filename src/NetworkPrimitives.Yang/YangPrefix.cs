#nullable enable

using System;

namespace NetworkPrimitives.Yang
{
    public readonly struct YangPrefix : IEquatable<YangPrefix>
    {
        internal readonly string? Value;
        private YangPrefix(string value) => this.Value = value;

        public static implicit operator string(YangPrefix identifier) => identifier.Value ?? string.Empty;
        public override string ToString() => this;
        public bool Equals(YangPrefix other) => this.Value == other.Value;
        public override bool Equals(object? obj) => obj is YangPrefix other && Equals(other);
        public override int GetHashCode() => (this.Value != null ? this.Value.GetHashCode() : 0);
        public static bool operator ==(YangPrefix left, YangPrefix right) => left.Equals(right);
        public static bool operator !=(YangPrefix left, YangPrefix right) => !left.Equals(right);

        public static YangPrefix Parse(string? text, YangRfc rfc = YangRfc.Rfc6020)
            => TryParse(text, out var result, rfc) ? result : throw new FormatException();
        public static YangPrefix Parse(YangIdentifier identifier) => new YangPrefix(identifier);
        private static bool TryParse(YangIdentifier identifier, out YangPrefix result)
        {
            result = YangPrefix.Parse(identifier);
            return true;
        }
        
        public static bool TryParse(string? text, out YangPrefix result, YangRfc rfc = YangRfc.Rfc6020)
        {
            result = default;
            return YangIdentifier.TryParse(text, out var id, rfc) && TryParse(id, out result);
        }
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        
        public static YangPrefix Parse(ReadOnlySpan<char> text, YangRfc rfc = YangRfc.Rfc6020)
            => TryParse(text, out var result, rfc) ? result : throw new FormatException();
        public static bool TryParse(ReadOnlySpan<char> text, out YangPrefix result, YangRfc rfc = YangRfc.Rfc6020)
            => TryParse(text, out var charsRead, out result, rfc) && charsRead == text.Length;
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out YangPrefix result, YangRfc rfc = YangRfc.Rfc6020)
        {
            result = default;
            return YangIdentifier.TryParse(text, out charsRead, out var id, rfc) && TryParse(id, out result);
        }
#endif
    }
}