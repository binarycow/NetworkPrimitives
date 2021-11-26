#nullable enable

using System;
using NUnit.Framework;

namespace NetworkPrimitives.Ipv4.Tests.RangeList
{
    public class Parsing : RangeListTests
    {
        
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void TestParse(RangeListTestCase testCase)
        {
            var (input, _, _) = testCase;
            var span = input.AsSpan();
            var length = span.Length;
            
            Assert.That(Ipv4AddressRangeList.TryParse(input, out var charsRead, out _));
            Assert.That(charsRead, Is.EqualTo(length));
            
            Assert.That(Ipv4AddressRangeList.TryParse(input, out _));
            Assert.That(charsRead, Is.EqualTo(length));
            
            Assert.That(Ipv4AddressRangeList.TryParse(span, out charsRead, out _));
            Assert.That(charsRead, Is.EqualTo(length));
            
            Assert.That(Ipv4AddressRangeList.TryParse(span, out _));
            Assert.That(charsRead, Is.EqualTo(length));

            Assert.DoesNotThrow(() => Ipv4AddressRangeList.Parse(input));
            Assert.DoesNotThrow(() => Ipv4AddressRangeList.Parse(input.AsSpan()));
        }

        
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void TestExpectedRanges(RangeListTestCase testCase)
        {
            var (input, expectedRanges, _) = testCase;
            
            Assert.That(Ipv4AddressRangeList.TryParse(input, out var rangeList));
            Assert.That(rangeList, Is.Not.Null);
            rangeList = rangeList ?? throw new InvalidOperationException();
            Assert.That(rangeList.Count, Is.EqualTo(expectedRanges.Count));

            for (var i = 0; i < expectedRanges.Count; ++i)
            {
                Assert.That(Ipv4AddressRange.TryParse(expectedRanges[i], out var expected));
                Assert.That(expected == rangeList[i]);
            }
        }
    }
}