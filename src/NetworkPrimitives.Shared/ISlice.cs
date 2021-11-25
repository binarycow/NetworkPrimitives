#nullable enable

using System;

namespace NetworkPrimitives
{
    internal interface ISlice<out TDerived, out TItem>
        where TDerived : ISlice<TDerived, TItem>
    {
        public TItem this[int index] { get; }
        public int Length { get; }
        public TDerived Slice(int start, int length);
    }
}