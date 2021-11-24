#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Yang
{
    public abstract record InstanceIdentifierPredicate
    {
        internal static bool TryParse(
            ref ReadOnlySpan<char> text,
            ref int charsRead,
            [NotNullWhen(true)] out InstanceIdentifierPredicate? predicate,
            YangRfc rfc
        )
        {
            if (!TryParse(text, out var length, out predicate, rfc))
                return false;
            text = text[length..];
            charsRead += length;
            return true;
        }
        internal static bool TryParse(
            ReadOnlySpan<char> text,
            out int charsRead,
            [NotNullWhen(true)] out InstanceIdentifierPredicate? predicate,
            YangRfc rfc
        )
        {
            charsRead = 0;
            predicate = default;
            if (!text.TryReadCharacter(ref charsRead, '['))
                return false;
            text.TryConsumeWhiteSpace(ref charsRead);
            var length = 0;
            if (
                !InstanceIdentifierKeyedListPredicateList.TryParseTyped(text, out length, out predicate, rfc)
                || !InstanceIdentifierLeafListPredicate.TryParseTyped(text, out length, out predicate)
                || !InstanceIdentifierIndexPredicate.TryParseTyped(text, out length, out predicate)
            )
            {
                return false;
            }
            charsRead += length;
            text = text[length..];
            text.TryConsumeWhiteSpace(ref charsRead);
            return text.TryReadCharacter(ref charsRead, ']');
        }
    }

    public record InstanceIdentifierKeyedListPredicateList(IReadOnlyList<InstanceIdentifierKeyedListPredicate> Predicates)
        : InstanceIdentifierPredicate
    {
        internal static bool TryParseTyped(
            ReadOnlySpan<char> text, 
            out int charsRead, 
            [NotNullWhen(true)] out InstanceIdentifierPredicate? predicate,
            YangRfc rfc
        )
        {
            charsRead = default;
            List<InstanceIdentifierKeyedListPredicate>? predicates = null;
            while (InstanceIdentifierKeyedListPredicate.TryParseTyped(ref text, ref charsRead, out var pred, rfc))
            {
                predicates ??= new ();
                predicates.Add(pred);
            }
            predicate = predicates is null
                ? null
                : new InstanceIdentifierKeyedListPredicateList(predicates.AsReadOnly());
            return predicate is not null;
        }
    }
    public record InstanceIdentifierKeyedListPredicate(PrefixedName NodeIdentifier, string Value)
        : InstanceIdentifierPredicate
    {
        internal static bool TryParseTyped(
            ref ReadOnlySpan<char> text,
            ref int charsRead,
            [NotNullWhen(true)] out InstanceIdentifierKeyedListPredicate? predicate,
            YangRfc rfc
        )
        {
            if (!TryParseTyped(text, out var length, out predicate, rfc))
                return false;
            charsRead += length;
            text = text[length..];
            return true;
        }
        internal static bool TryParseTyped(
            ReadOnlySpan<char> text, 
            out int charsRead, 
            [NotNullWhen(true)] out InstanceIdentifierKeyedListPredicate? predicate,
            YangRfc rfc
        )
        {
            charsRead = default;
            predicate = default;
            if(!PrefixedName.TryParse(ref text, ref charsRead, out var prefixedName, rfc))
                return false;
            text.TryConsumeWhiteSpace(ref charsRead);
            if (!text.TryReadCharacter(ref charsRead, '='))
                return false;
            text.TryConsumeWhiteSpace(ref charsRead);
            if (!Parsing.TryReadQuotedText(ref text, ref charsRead, out var result))
                return false;
            predicate = new InstanceIdentifierKeyedListPredicate(prefixedName, result);
            return true;
        }
    }
    public record InstanceIdentifierLeafListPredicate(string Value) 
        : InstanceIdentifierPredicate
    {
        internal static bool TryParseTyped(
            ReadOnlySpan<char> text, 
            out int charsRead, 
            [NotNullWhen(true)] out InstanceIdentifierPredicate? predicate
        )
        {
            charsRead = default;
            predicate = default;
            if (!text.TryReadCharacter(ref charsRead, '.'))
                return false;
            text.TryConsumeWhiteSpace(ref charsRead);
            if (!text.TryReadCharacter(ref charsRead, '='))
                return false;
            text.TryConsumeWhiteSpace(ref charsRead);
            if (!Parsing.TryReadQuotedText(ref text, ref charsRead, out var result))
                return false;
            predicate = new InstanceIdentifierLeafListPredicate(result);
            return true;
        }
    }
    public record InstanceIdentifierIndexPredicate(ulong Index) 
        : InstanceIdentifierPredicate
    {
        internal static bool TryParseTyped(
            ReadOnlySpan<char> text, 
            out int charsRead, 
            [NotNullWhen(true)] out InstanceIdentifierPredicate? predicate
        )
        {
            charsRead = default;
            predicate = text.TryParseUInt64(ref charsRead, out var value) 
                ? new InstanceIdentifierIndexPredicate(value) 
                : null;
            return predicate is not null;
        }
    }
}