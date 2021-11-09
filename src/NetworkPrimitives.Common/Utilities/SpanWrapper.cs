using System;
using System.Diagnostics;

namespace NetworkPrimitives.Utilities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal readonly ref struct SpanWrapper
    {
        
        
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        private readonly ReadOnlySpan<char> text;
        public SpanWrapper(ReadOnlySpan<char> text) => this.text = text;
        private string DebuggerDisplay => this.text.ToString();
        public int Length => this.text.Length;
        public char this[int index] => this.text[index];
        private SpanWrapper SliceImplementation(int start, int length) => new(this.text.Slice(start, length));
#else
        private readonly string text;
        private readonly int startIndex;
        public SpanWrapper(string? text) : this(text, 0, text?.Length ?? 0) { }
        private SpanWrapper(string? text, int startIndex, int length)
        {
            this.text = text ?? string.Empty;
            this.startIndex = startIndex;
            this.Length = length;
        }
        private string DebuggerDisplay => this.text.Substring(this.startIndex);
        public int Length { get; }
        public char this[int index] => this.text[this.startIndex + index];
        private SpanWrapper SliceImplementation(int start, int length) => new (this.text, this.startIndex + start, length);
#endif
        
        public SpanWrapper Slice(int start, int length)
        {
            if ((uint)start > (uint)this.Length || (uint)length > (uint)(this.Length - start))
                throw new ArgumentOutOfRangeException();
            return this.SliceImplementation(start, length);
        }

        public bool IsEmpty => this.text.Length == 0;

    }
}