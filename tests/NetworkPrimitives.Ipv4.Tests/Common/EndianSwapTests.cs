#nullable enable

using System;
using NUnit.Framework;
using NetworkPrimitives;


namespace NetworkPrimitives.Tests.Common
{
    [TestFixture]
    public class EndianSwapTests
    {
        [Test]
        [TestCase((uint)0x00112233, (uint)0x33221100)]
        [TestCase(0xFF116600, (uint)0x006611FF)]
        public void TestEndianSwap(uint input, uint expected)
        {
            Assert.AreEqual(expected, input.SwapEndian());
        }
    }
}