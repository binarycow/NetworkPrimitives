#nullable enable

using System;
using NetworkPrimitives.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv4
{
#if CICD
    [TestFixture]
#endif
    public class Ipv4CidrTests
    {
#if CICD
        [Test]
        [TestCase((byte)32, (ulong)1, (uint)1, (uint)0xFFFFFFFF)]
        [TestCase((byte)31, (ulong)2, (uint)2, (uint)0xFFFFFFFE)]
        [TestCase((byte)30, (ulong)4, (uint)2, (uint)0xFFFFFFFC)]
        [TestCase((byte)29, (ulong)8, (uint)6, (uint)0xFFFFFFF8)]
        [TestCase((byte)28, (ulong)16, (uint)14, (uint)0xFFFFFFF0)]
        [TestCase((byte)27, (ulong)32, (uint)30, (uint)0xFFFFFFE0)]
        [TestCase((byte)26, (ulong)64, (uint)62, (uint)0xFFFFFFC0)]
        [TestCase((byte)25, (ulong)128, (uint)126, (uint)0xFFFFFF80)]
        [TestCase((byte)24, (ulong)256, (uint)254, (uint)0xFFFFFF00)]
        [TestCase((byte)23, (ulong)512, (uint)510, (uint)0xFFFFFE00)]
        [TestCase((byte)22, (ulong)1024, (uint)1022, (uint)0xFFFFFC00)]
        [TestCase((byte)21, (ulong)2048, (uint)2046, (uint)0xFFFFF800)]
        [TestCase((byte)20, (ulong)4096, (uint)4094, (uint)0xFFFFF000)]
        [TestCase((byte)19, (ulong)8192, (uint)8190, (uint)0xFFFFE000)]
        [TestCase((byte)18, (ulong)16384, (uint)16382, (uint)0xFFFFC000)]
        [TestCase((byte)17, (ulong)32768, (uint)32766, (uint)0xFFFF8000)]
        [TestCase((byte)16, (ulong)65536, (uint)65534, (uint)0xFFFF0000)]
        [TestCase((byte)15, (ulong)131072, (uint)131070, (uint)0xFFFE0000)]
        [TestCase((byte)14, (ulong)262144, (uint)262142, (uint)0xFFFC0000)]
        [TestCase((byte)13, (ulong)524288, (uint)524286, (uint)0xFFF80000)]
        [TestCase((byte)12, (ulong)1048576, (uint)1048574, (uint)0xFFF00000)]
        [TestCase((byte)11, (ulong)2097152, (uint)2097150, (uint)0xFFE00000)]
        [TestCase((byte)10, (ulong)4194304, (uint)4194302, (uint)0xFFC00000)]
        [TestCase((byte)9, (ulong)8388608, (uint)8388606, (uint)0xFF800000)]
        [TestCase((byte)8, (ulong)16777216, (uint)16777214, (uint)0xFF000000)]
        [TestCase((byte)7, (ulong)33554432, (uint)33554430, (uint)0xFE000000)]
        [TestCase((byte)6, (ulong)67108864, (uint)67108862, (uint)0xFC000000)]
        [TestCase((byte)5, (ulong)134217728, (uint)134217726, (uint)0xF8000000)]
        [TestCase((byte)4, (ulong)268435456, (uint)268435454, (uint)0xF0000000)]
        [TestCase((byte)3, (ulong)536870912, (uint)536870910, (uint)0xE0000000)]
        [TestCase((byte)2, (ulong)1073741824, (uint)1073741822, (uint)0xC0000000)]
        [TestCase((byte)1, (ulong)2147483648, (uint)2147483646, (uint)0x80000000)]
        [TestCase((byte)0, (ulong)4294967296, (uint)4294967294, (uint)0x00000000)]

        public void TestCidr(byte value, ulong totalHosts, uint usableHosts, uint mask)
        {
            Assert.AreEqual(true, Ipv4Cidr.TryParse(value.ToString(), out var cidr));
            Assert.AreEqual(value, (byte)cidr);
            Assert.AreEqual(totalHosts, cidr.TotalHosts);
            Assert.AreEqual(usableHosts, cidr.UsableHosts);
            Assert.AreEqual(value.ToString(), cidr.ToString());
            Assert.AreEqual(mask, cidr.ToSubnetMask().Value);
        }
#endif
    }
}