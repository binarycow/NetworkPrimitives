using System;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Yang
{
    public readonly struct YangModuleName : IEquatable<YangModuleName>
    {
        internal readonly string? Value;
        private YangModuleName(string value) => this.Value = value;

        public static implicit operator string(YangModuleName identifier) => identifier.Value ?? string.Empty;
        public override string ToString() => this;
        public bool Equals(YangModuleName other) => this.Value == other.Value;
        public override bool Equals(object? obj) => obj is YangModuleName other && Equals(other);
        public override int GetHashCode() => (this.Value != null ? this.Value.GetHashCode() : 0);
        public static bool operator ==(YangModuleName left, YangModuleName right) => left.Equals(right);
        public static bool operator !=(YangModuleName left, YangModuleName right) => !left.Equals(right);

        public static YangModuleName Parse(string? text, YangRfc rfc = YangRfc.Rfc6020)
            => TryParse(text, out var result, rfc) ? result : throw new FormatException();
        public static YangModuleName Parse(YangIdentifier identifier) => new YangModuleName(identifier);
        private static bool TryParse(YangIdentifier identifier, out YangModuleName result)
        {
            result = YangModuleName.Parse(identifier);
            return true;
        }
        
        public static bool TryParse(string? text, out YangModuleName result, YangRfc rfc = YangRfc.Rfc6020)
        {
            result = default;
            return YangIdentifier.TryParse(text, out var id, rfc) && TryParse(id, out result);
        }
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        
        public static YangModuleName Parse(ReadOnlySpan<char> text, YangRfc rfc = YangRfc.Rfc6020)
            => TryParse(text, out var result, rfc) ? result : throw new FormatException();
        public static bool TryParse(ReadOnlySpan<char> text, out YangModuleName result, YangRfc rfc = YangRfc.Rfc6020)
            => TryParse(text, out var charsRead, out result, rfc) && charsRead == text.Length;
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out YangModuleName result, YangRfc rfc = YangRfc.Rfc6020)
        {
            result = default;
            return YangIdentifier.TryParse(text, out charsRead, out var id, rfc) && TryParse(id, out result);
        }
#endif
    }
}