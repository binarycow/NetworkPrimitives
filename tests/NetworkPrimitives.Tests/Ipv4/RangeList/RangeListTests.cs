#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using NetworkPrimitives.Tests;
using NetworkPrimitives.Tests.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv4.RangeList
{
    [TestFixture]
    public abstract class RangeListTests
    {
        public static IReadOnlyList<RangeListTestCase> LoadTestCases(string path)
        {
            var jsonString = EmbeddedResourceUtils.ReadFromResourceFile(path) ?? "[]";
            IReadOnlyList<RangeListTestCase>? ret = JsonSerializer.Deserialize<RangeListTestCase[]>(jsonString)?.ToList();
            return ret ?? Array.Empty<RangeListTestCase>();
        }

        public static IReadOnlyList<RangeListTestCase> TestCases { get; } 
            = LoadTestCases("range-test-cases.json");


    }
}