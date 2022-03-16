using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NetworkPrimitives;

internal static class StackExtensions
{
#if !(NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER)
    public static bool TryPop<T>(
        this Stack<T> stack, 
        [NotNullWhen(true)] out T? item
    ) where T : notnull
    {
        item = default;
        if (stack.Count == 0)
            return false;
        try
        {
            item = stack.Pop();
        }
        catch
        {
            return false;
        }
        return item is not null;
    }
#endif
}