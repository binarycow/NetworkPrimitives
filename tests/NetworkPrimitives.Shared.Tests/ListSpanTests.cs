#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using NetworkPrimitives.Utilities;
using NUnit.Framework;

namespace NetworkPrimitives.Shared.Tests
{
    [TestFixture]
    public class ListSpanTests
    {
        public IReadOnlyList<int> TestData = Enumerable.Range(0, 1000).ToList().AsReadOnly();

        [Test]
        public void TestSliceWithLength(
            [Random(0, 1000, 5)] int start, 
            [Random(0, 1000, 5)] int length
        )
        {
            var span = new ReadOnlyListSpan<int>(this.TestData);
            Assume.That(start + length < span.Length);
            Assert.DoesNotThrow(() => span = span.Slice(start, length));
            var expected = this.TestData.Skip(start).Take(length).ToArray();
            var actual = span.ToEnumerable().ToArray();
            Assert.That(actual, Is.EquivalentTo(expected));
        }
        
        [Test]
        public void TestSlice(
            [Random(0, 1000, 5)] int start
        )
        {
            var span = new ReadOnlyListSpan<int>(this.TestData);
            Assert.DoesNotThrow(() => span = span[start..]);
            var expected = this.TestData.Skip(start).ToArray();
            var actual = span.ToEnumerable().ToArray();
            Assert.That(actual, Is.EquivalentTo(expected));
        }
    }
}