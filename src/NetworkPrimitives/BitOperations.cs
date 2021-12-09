using System;
using System.Runtime.CompilerServices;
#if !NETCOREAPP3_0_OR_GREATER

// ReSharper disable once CheckNamespace
namespace System.Numerics
{
    internal static class BitOperations
    {
        public static int PopCount(uint value)
        {
            const uint c1 = 0x_55555555u;
            const uint c2 = 0x_33333333u;
            const uint c3 = 0x_0F0F0F0Fu;
            const uint c4 = 0x_01010101u;

            value -= (value >> 1) & c1;
            value = (value & c2) + ((value >> 2) & c2);
            value = (((value + (value >> 4)) & c3) * c4) >> 24;

            return (int)value;
        }

    }
}
#endif

namespace NetworkPrimitives
{
    internal static class BitOperationsEx
    {
        public static uint RoundUpToPowerOf2(uint value)
        {
#if NET6_0_OR_GREATER
            return System.Numerics.BitOperations.RoundUpToPowerOf2(value);
#else
            --value;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            return value + 1;
#endif
        }
    }
}