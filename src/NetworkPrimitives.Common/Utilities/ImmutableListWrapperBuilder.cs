#nullable enable

using System;

namespace NetworkPrimitives.Utilities
{
    public abstract class ImmutableListWrapperBuilder<TDerived, TItem, TImmutable>
        where TDerived : ImmutableListWrapperBuilder<TDerived, TItem, TImmutable>
    {
        internal ImmutableListWrapperBuilder()
        {
            
        }
        //public abstract TImmutable ToImmutable();
    }
}