[Back to readme.md](../readme.md)

## Get basic subnet info

```c#
var subnet = Ipv4Subnet.Parse("10.0.0.0/29");
Console.WriteLine($"  Network Address: {subnet.NetworkAddress}");   // 10.0.0.0
Console.WriteLine($"     First Usable: {subnet.FirstUsable}");      // 10.0.0.1
Console.WriteLine($"      Last Usable: {subnet.LastUsable}");       // 10.0.0.6
Console.WriteLine($"Broadcast Address: {subnet.BroadcastAddress}"); // 10.0.0.7
Console.WriteLine($"      Total Hosts: {subnet.TotalHosts}");       // 8
Console.WriteLine($"     Usable Hosts: {subnet.UsableHosts}");      // 6
foreach (var address in subnet.GetUsableAddresses())
{
    Console.WriteLine(address);
}
```

## Subnet Operations


**Attempt to split a subnet in half:**

```c#
var slash24 = Ipv4Subnet.Parse("10.0.0.0/24");
Console.WriteLine($"Splitting subnet {slash24}");   // 10.0.0.0/24
var success = slash24.TrySplit(out var lowHalf, out var highHalf);
Console.WriteLine($"Success: {success}");           // true
Console.WriteLine($"Low subnet: {lowHalf}");        // 10.0.0.0/25
Console.WriteLine($"High subnet: {highHalf}");      // 10.0.0.128/25
```

**Supernet two or more subnets:**

```c#
var subnetA = Ipv4Subnet.Parse("10.0.0.0/24");
var subnetB = Ipv4Subnet.Parse("10.0.1.0/24");
var subnetC = Ipv4Subnet.Parse("10.0.3.0/24");
var subnetD = Ipv4Subnet.Parse("10.0.255.0/24");
var supernet = Ipv4Subnet.GetContainingSupernet(subnetA, subnetB, subnetC, subnetD);
Console.WriteLine($"Supernet: {supernet}"); // 10.0.0.0/16
```

**Contains:**

```c#
var subnet = Ipv4Subnet.Parse("10.0.0.0/24");

Console.WriteLine(subnet.Contains(Ipv4Address.Parse("10.50.0.0"))); // False
Console.WriteLine(subnet.Contains(Ipv4Address.Parse("10.0.0.50"))); // True
Console.WriteLine(subnet.Contains(Ipv4Subnet.Parse("10.0.0.0/24"))); // True
Console.WriteLine(subnet.Contains(Ipv4Subnet.Parse("10.0.0.128/26"))); // True
Console.WriteLine(subnet.Contains(Ipv4Subnet.Parse("8.0.0.0/8"))); // False
```

## Address Listing

**All Addresses**

Includes network/broadcast addresses

```c#
var subnet = Ipv4Subnet.Parse("10.0.0.0/24");
var addresses = subnet.GetAllAddresses();
foreach(var address in addresses)
{
}
```

**Usable Addresses**

Does not include network/broadcast addresses

(_Note:_ Respects [RFC 3021](https://datatracker.ietf.org/doc/html/rfc3021))

```c#
var subnet = Ipv4Subnet.Parse("10.0.0.0/24");
var addresses = subnet.GetUsableAddresses();
foreach(var address in addresses)
{
}
```