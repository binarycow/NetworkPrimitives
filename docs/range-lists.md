[Back to readme.md](../readme.md)

## Address Ranges / Lists

**Parse/enumerate a single address range:**

```c#
var range = Ipv4AddressRange.Parse("10.0.0.0-15");
foreach(var address in range)
{
    Console.WriteLine(address);
}
```

**Parse a range list**

```c#
var rangeText = @"
10.0.0.0-10
10.0.1.0/29
10.1.0.40
";
var rangeList = Ipv4AddressRangeList.Parse(rangeText);
```

**Enumerate a range list**

Option 1:  Nested enumeration

```c#
foreach (var range in rangeList)
{
    Console.WriteLine(range);
    foreach (var address in range)
    {
        Console.WriteLine(address);
    }
}
```

Option 2: Enumerate all addresses

```c#
foreach (var address in rangeList.GetAllAddresses())
{
    Console.WriteLine(address);
}
```