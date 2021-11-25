#nullable enable

using System;
using System.Buffers.Binary;
using System.Numerics;

namespace NetworkPrimitives
{
    internal static class NumericExtensions
    {
        [ExcludeFromCodeCoverage("Internal")]
        public static uint PopCount(this uint v)
        {
            // Intentionally not using BitOperations.PopCount as its not CLS compliant.
            // https://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel
            v = v - ((v >> 1) & 0x55555555);                    
            v = (v & 0x33333333) + ((v >> 2) & 0x33333333);     
            return (((v + (v >> 4)) & 0xF0F0F0F) * 0x1010101) >> 24; 
        }
        
        [ExcludeFromCodeCoverage("Internal")]
        public static byte[] ToBytesBigEndian(this uint value)
        {
            var bytes = new byte[4];
            value.TryWriteBigEndian(bytes, out _);
            return bytes;
        }
        
        [ExcludeFromCodeCoverage("Internal")]
        public static bool TryWriteBigEndian(this uint value, Span<byte> span, out int bytesWritten)
        {
            bytesWritten = default;
            return value.TryWriteBigEndian(ref span, ref bytesWritten);
        }
        
        [ExcludeFromCodeCoverage("Internal")]
        public static bool TryWriteBigEndian(this uint value, ref Span<byte> span, ref int bytesWritten)
        {
            if (!BinaryPrimitives.TryWriteUInt32BigEndian(span, value))
                return false;
            span = span[4..];
            bytesWritten += 4;
            return true;
        }
        
        [ExcludeFromCodeCoverage("Internal")]
        public static uint SwapEndianIfLittleEndian(this uint value) 
            => BitConverter.IsLittleEndian ? value.SwapEndian() : value;

        public static uint SwapEndian(this uint value)
        {
            Span<byte> span = stackalloc byte[4];
            BinaryPrimitives.WriteUInt32BigEndian(span, value);
            return BinaryPrimitives.ReadUInt32LittleEndian(span);
        }
    }
}