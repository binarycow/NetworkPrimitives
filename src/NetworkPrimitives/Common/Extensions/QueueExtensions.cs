using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NetworkPrimitives;

internal static class QueueExtensions
{
#if !(NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER)
    public static bool TryDequeue<T>(
        this Queue<T> queue, 
        [NotNullWhen(true)] out T? item
    ) where T : notnull
    {
        item = default;
        if (queue.Count == 0)
            return false;
        try
        {
            item = queue.Dequeue();
        }
        catch
        {
            return false;
        }
        return item is not null;
    }
#endif
}