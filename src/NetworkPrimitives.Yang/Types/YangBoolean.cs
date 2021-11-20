using System;

namespace NetworkPrimitives.Yang
{
    public readonly struct YangBoolean : IEquatable<YangBoolean>
    {
        public bool Value { get; }
        public YangBoolean(bool value) => this.Value = value;
        public override string ToString() => Value ? "true" : "false";

        public static implicit operator bool(YangBoolean value) => value.Value;
        public static implicit operator YangBoolean(bool value) => new (value);
        public bool Equals(YangBoolean other) => this.Value == other.Value;
        public override bool Equals(object? obj) => obj is YangBoolean other && Equals(other);
        public override int GetHashCode() => this.Value.GetHashCode();
        public static bool operator ==(YangBoolean left, YangBoolean right) => left.Equals(right);
        public static bool operator !=(YangBoolean left, YangBoolean right) => !left.Equals(right);

        public static YangBoolean Parse(string text)
            => TryParse(text, out var result) ? result : throw new FormatException();
        
        public static bool TryParse(string? text, out YangBoolean result)
        {
            if (text == "true")
                return Create(true, out result);
            if (text == "false")
                return Create(false, out result);
            result = default;
            return false;
        }
        
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        
        public static YangBoolean Parse(ReadOnlySpan<char> text)
            => TryParse(text, out var result) ? result : throw new FormatException();
        public static bool TryParse(ReadOnlySpan<char> text, out YangBoolean result)
            => TryParse(text, out var charsRead, out result) && charsRead == text.Length;
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out YangBoolean result)
        {
            return CreateIf(text, "true", true, out charsRead, out result)
                || CreateIf(text, "false", false, out charsRead, out result);
        }
        private static bool CreateIf(ReadOnlySpan<char> text, string checkFor, bool value, out int charsRead, out YangBoolean result)
        {
            charsRead = default;
            result = default;
            if (text.StartsWith(checkFor))
                return false;
            charsRead = checkFor.Length;
            return Create(value, out result);
        }
#endif
        private static bool Create(bool value, out YangBoolean result)
        {
            result = value;
            return true;
        }
        
    }
}