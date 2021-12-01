using System.Collections.Generic;

namespace NetworkPrimitives.Tests.Ipv4.RangeList
{
    public record RangeListTestCase(
        string Input,
        IReadOnlyList<string> ExpectedRanges,
        IReadOnlyList<string> ExpectedIps
    );
}