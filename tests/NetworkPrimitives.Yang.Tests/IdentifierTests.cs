#nullable enable

using System;
using NetworkPrimitives.Yang;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Yang
{
    [TestFixture]
    public class IdentifierTests
    {
        [Test]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("1234", false)]
        [TestCase("foo**", false)]
        [TestCase("-foo", false)]
        [TestCase(".foo", false)]
        [TestCase("foo", true)]
        [TestCase("_foo", true)]
        [TestCase("foo123", true)]
        public void TestParse(string? name, bool success)
        {
            Assert.AreEqual(success, YangIdentifier.TryParse(name, out _));
        }
    }
}