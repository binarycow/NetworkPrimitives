﻿using System;
using System.Diagnostics;

namespace NetworkPrimitives.Utilities
{
    [ExcludeFromCodeCoverage("Internal")]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal readonly ref struct SpanWrapper
    {
        
        
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        public ReadOnlySpan<char> GetSpan() => this.text;
        public string GetString() => new string(this.text);
        private readonly ReadOnlySpan<char> text;
        public SpanWrapper(ReadOnlySpan<char> text) => this.text = text;
        private string DebuggerDisplay => this.text.ToString();
        public int Length => this.text.Length;
        public char this[int index] => this.text[index];
        private SpanWrapper SliceImplementation(int start, int length) => new(this.text.Slice(start, length));
        public int IndexOf(ReadOnlySpan<char> other, StringComparison comparison = StringComparison.Ordinal) 
            => this.text.IndexOf(other, comparison);

#else
        public string GetString() => text.Substring(this.startIndex, this.Length);
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
        public int IndexOf(string other, StringComparison comparison = StringComparison.Ordinal) 
            => this.text.IndexOf(other, this.startIndex, this.Length,  comparison);
#endif
        
        public SpanWrapper Slice(int start, int length)
        {
            if ((uint)start > (uint)this.Length || (uint)length > (uint)(this.Length - start))
                throw new ArgumentOutOfRangeException();
            return this.SliceImplementation(start, length);
        }

        public bool IsEmpty => this.Length == 0;
    }
}