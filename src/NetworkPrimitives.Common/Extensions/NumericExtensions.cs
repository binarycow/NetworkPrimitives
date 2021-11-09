﻿#nullable enable

using System;
using System.Buffers.Binary;

namespace NetworkPrimitives
{
    internal static class NumericExtensions
    {
        public static bool TryWriteBigEndian(this uint value, Span<byte> span, out int bytesWritten)
        {
            bytesWritten = default;
            return value.TryWriteBigEndian(ref span, ref bytesWritten);
        }
        
        public static bool TryWriteBigEndian(this uint value, ref Span<byte> span, ref int bytesWritten)
        {
            if (!BinaryPrimitives.TryWriteUInt32BigEndian(span, value))
                return false;
            span = span[4..];
            bytesWritten += 4;
            return true;
        }
        
        
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