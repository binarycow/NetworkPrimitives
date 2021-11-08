#nullable enable

using System;

namespace NetworkPrimitives
{
    internal static class TupleExtensions
    {
        public static bool Try<T>(this (bool Success, T Value) value, out T result)
            where T : struct
        {
            bool success;
            (success, result) = value;
            return success;
        }
    }
}