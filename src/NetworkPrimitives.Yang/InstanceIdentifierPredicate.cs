#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Yang
{
    public abstract record InstanceIdentifierPredicate
    {
        internal static bool TryParse(
            ref SpanWrapper text,
            ref int charsRead,
            [NotNullWhen(true)] out InstanceIdentifierPredicate? predicate
        )
        {
            if (!TryParse(text, out var length, out predicate))
                return false;
            text = text[length..];
            charsRead += length;
            return true;
        }
        internal static bool TryParse(
            SpanWrapper text,
            out int charsRead,
            [NotNullWhen(true)] out InstanceIdentifierPredicate? predicate
        )
        {
            
            throw new NotImplementedException();
        }
    }
}