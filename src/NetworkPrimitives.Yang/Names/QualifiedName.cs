#nullable enable

using System;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Yang
{
    public readonly struct QualifiedName : IEquatable<QualifiedName>
    {
        public YangModuleName ModuleName { get; }
        public YangIdentifier Name { get; }
        public QualifiedName(YangModuleName moduleName, YangIdentifier name)
        {
            this.ModuleName = moduleName;
            this.Name = name;
        }
        public override string ToString() => $"{this.ModuleName.ToString()}:{Name.ToString()}";

        public bool Equals(QualifiedName other) => this.ModuleName.Equals(other.ModuleName) && this.Name.Equals(other.Name);
        public override bool Equals(object? obj) => obj is QualifiedName other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(this.ModuleName, this.Name);
        public static bool operator ==(QualifiedName left, QualifiedName right) => left.Equals(right);
        public static bool operator !=(QualifiedName left, QualifiedName right) => !left.Equals(right);

        public void Deconstruct(out YangModuleName moduleName, out YangIdentifier name)
        {
            moduleName = this.ModuleName;
            name = this.Name;
        }
        
        
        public static bool TryParse(string? text, out QualifiedName result, YangRfc rfc = YangRfc.Rfc6020)
        {
            var span = new SpanWrapper(text);
            var charsRead = 0;
            return TryParse(ref span, ref charsRead, out result, rfc) && charsRead == text?.Length;
        }
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        
        public static QualifiedName Parse(ReadOnlySpan<char> text, YangRfc rfc = YangRfc.Rfc6020)
            => TryParse(text, out var result, rfc) ? result : throw new FormatException();

        public static bool TryParse(ReadOnlySpan<char> text, out QualifiedName result, YangRfc rfc = YangRfc.Rfc6020)
            => TryParse(text, out var charsRead, out result, rfc) && charsRead == text.Length;
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out QualifiedName result, YangRfc rfc = YangRfc.Rfc6020)
        {
            var span = new SpanWrapper(text);
            charsRead = default;
            return TryParse(ref span, ref charsRead, out result, rfc);
        }
#endif
        
        internal static bool TryParse(ref SpanWrapper text, ref int charsRead, out QualifiedName result, YangRfc rfc)
        {
            result = default;
            var textCopy = text;
            var charCopy = charsRead;
            if (!YangIdentifier.TryParse(ref textCopy, ref charCopy, out var moduleName, rfc))
                return false;
            if (!textCopy.TryReadCharacter(ref charCopy, ':'))
                return false;
            if (!YangIdentifier.TryParse(ref textCopy, ref charCopy, out var identifier, rfc))
                return false;
            charsRead = charCopy;
            text = textCopy;
            result = new QualifiedName(YangModuleName.Parse(moduleName), identifier);
            return true;
        }
    }
}