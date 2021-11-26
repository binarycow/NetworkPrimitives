#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetworkPrimitives.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv4.Cidr
{
    [TestFixture]
    public abstract class Ipv4CidrTests
    {
        public static IReadOnlyList<byte> InvalidCidrs = Enumerable.Range(0, 256).Where(x => x >= 33).Select(x => (byte)x).ToArray();
        public static IReadOnlyList<Ipv4CidrTestCase> TestCases = new[]
        {
            new Ipv4CidrTestCase(32, 1, 1, 0xFFFFFFFF),
            new Ipv4CidrTestCase(31, 2, 2, 0xFFFFFFFE),
            new Ipv4CidrTestCase(30, 4, 2, 0xFFFFFFFC),
            new Ipv4CidrTestCase(29, 8, 6, 0xFFFFFFF8),
            new Ipv4CidrTestCase(28, 16, 14, 0xFFFFFFF0),
            new Ipv4CidrTestCase(27, 32, 30, 0xFFFFFFE0),
            new Ipv4CidrTestCase(26, 64, 62, 0xFFFFFFC0),
            new Ipv4CidrTestCase(25, 128, 126, 0xFFFFFF80),
            new Ipv4CidrTestCase(24, 256, 254, 0xFFFFFF00),
            new Ipv4CidrTestCase(23, 512, 510, 0xFFFFFE00),
            new Ipv4CidrTestCase(22, 1024, 1022, 0xFFFFFC00),
            new Ipv4CidrTestCase(21, 2048, 2046, 0xFFFFF800),
            new Ipv4CidrTestCase(20, 4096, 4094, 0xFFFFF000),
            new Ipv4CidrTestCase(19, 8192, 8190, 0xFFFFE000),
            new Ipv4CidrTestCase(18, 16384, 16382, 0xFFFFC000),
            new Ipv4CidrTestCase(17, 32768, 32766, 0xFFFF8000),
            new Ipv4CidrTestCase(16, 65536, 65534, 0xFFFF0000),
            new Ipv4CidrTestCase(15, 131072, 131070, 0xFFFE0000),
            new Ipv4CidrTestCase(14, 262144, 262142, 0xFFFC0000),
            new Ipv4CidrTestCase(13, 524288, 524286, 0xFFF80000),
            new Ipv4CidrTestCase(12, 1048576, 1048574, 0xFFF00000),
            new Ipv4CidrTestCase(11, 2097152, 2097150, 0xFFE00000),
            new Ipv4CidrTestCase(10, 4194304, 4194302, 0xFFC00000),
            new Ipv4CidrTestCase(9, 8388608, 8388606, 0xFF800000),
            new Ipv4CidrTestCase(8, 16777216, 16777214, 0xFF000000),
            new Ipv4CidrTestCase(7, 33554432, 33554430, 0xFE000000),
            new Ipv4CidrTestCase(6, 67108864, 67108862, 0xFC000000),
            new Ipv4CidrTestCase(5, 134217728, 134217726, 0xF8000000),
            new Ipv4CidrTestCase(4, 268435456, 268435454, 0xF0000000),
            new Ipv4CidrTestCase(3, 536870912, 536870910, 0xE0000000),
            new Ipv4CidrTestCase(2, 1073741824, 1073741822, 0xC0000000),
            new Ipv4CidrTestCase(1, 2147483648, 2147483646, 0x80000000),
            new Ipv4CidrTestCase(0, 4294967296, 4294967294, 0x00000000),
        };
    }
}