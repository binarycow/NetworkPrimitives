#nullable enable

using System;
using NUnit.Framework;

namespace NetworkPrimitives.Ipv4.Tests.RangeList
{
    public class AddressListSpanTests : RangeListTests
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void TestIndexer(RangeListTestCase testCase)
        {
            var (input, _, expectedAddresses) = testCase;
            
            Assert.That(Ipv4AddressRangeList.TryParse(input, out var rangeList));
            Assert.That(rangeList, Is.Not.Null);
            rangeList = rangeList ?? throw new InvalidOperationException();

            var addresses = rangeList.GetAllAddresses();
            
            Assert.That(addresses.Length, Is.EqualTo(expectedAddresses.Count));

            for (var i = 0; i < expectedAddresses.Count; ++i)
            {
                Assert.That(Ipv4Address.TryParse(expectedAddresses[i], out var expected));
                Assert.That(addresses[i], Is.EqualTo(expected));
            }
        }
    }
}