#nullable enable

using System;
using System.Net;

// ReSharper disable once CheckNamespace
namespace NetworkPrimitives
{
    internal static class IpAddressExtensions
    {
#if !(NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER)
        public static bool TryWriteBytes(this IPAddress ipAddress, Span<byte> bytes, out int charsWritten)
        {
            charsWritten = default;
            var byteArray = ipAddress.GetAddressBytes(); // TODO: Is this the right way to do it?
            if (bytes.Length < byteArray.Length)
                return false;
            for (var i = 0; i < byteArray.Length; ++i)
                bytes[i] = byteArray[i];
            charsWritten = byteArray.Length;
            return true;
        }
#endif
    }
}