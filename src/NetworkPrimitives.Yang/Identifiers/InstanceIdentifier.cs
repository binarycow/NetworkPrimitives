using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Yang
{
    public record InstanceIdentifier(
        bool IsAbsolute,
        IReadOnlyList<InstanceIdentifierExpression> Expressions
    )
    {
        public static bool TryParse(
            string text, 
            [NotNullWhen(true)] out InstanceIdentifier? expression,
            YangRfc rfc
        )
        {
            var span = new SpanWrapper(text);
            return TryParse(span, out var charsRead, out expression, rfc) && charsRead == text.Length;
        }
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        public static bool TryParse(
            ReadOnlySpan<char> text,
            [NotNullWhen(true)] out InstanceIdentifier? expression,
            YangRfc rfc
        ) => TryParse(text, out _, out expression, rfc);
        public static bool TryParse(
            ReadOnlySpan<char> text, 
            out int charsRead,
            [NotNullWhen(true)] out InstanceIdentifier? expression,
            YangRfc rfc
        )
        {
            var span = new SpanWrapper(text);
            return TryParse(span, out charsRead, out expression, rfc);
        }
#endif
        internal static bool TryParse(
            SpanWrapper text,
            out int charsRead,
            [NotNullWhen(true)] out InstanceIdentifier? expression,
            YangRfc rfc
        )
        {
            charsRead = 0;
            expression = default;
            if (text.Length == 0)
                return false;
            var absolute = false;
            if (text[0] == '/')
            {
                absolute = true;
                text = text[1..];
                ++charsRead;
            }

            var expressions = new List<InstanceIdentifierExpression>();
            while (InstanceIdentifierExpression.TryParse(ref text, ref charsRead, out var expr, rfc))
            {
                expressions.Add(expr);
                if (!text.TryReadCharacter(ref charsRead, '/'))
                    break;
            }
            expression = new (absolute, expressions.ToList());
            return true;
        }
    }

    public record InstanceIdentifierExpression(PrefixedName Name,
        InstanceIdentifierPredicate? Predicate)
    {
        internal static bool TryParse(
            ref SpanWrapper text,
            ref int charsRead,
            [NotNullWhen(true)] out InstanceIdentifierExpression? expression,
            YangRfc rfc
        )
        {
            if (!TryParse(text, out var length, out expression, rfc))
                return false;
            charsRead += length;
            text = text[length..];
            return true;
        }
        
        internal static bool TryParse(
            SpanWrapper text,
            out int charsRead,
            [NotNullWhen(true)] out InstanceIdentifierExpression? expression,
            YangRfc rfc
        )
        {
            expression = default;
            charsRead = 0;
            if (!PrefixedName.TryParse(ref text, ref charsRead, out var name, rfc))
                return false;
            if (!InstanceIdentifierPredicate.TryParse(ref text, ref charsRead, out var predicate, rfc))
            {
                expression = new (name, null);
                return true;
            }
            expression = new (name, predicate);
            return true;
        }
    }
}