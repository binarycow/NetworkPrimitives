# NetworkPrimitives

Lightweight library for working with various networking objects

### Basic usage

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

### Download

The core `NetworkPrimitives` package contains the types themselves.

[![NetworkPrimitives (Nuget)](https://img.shields.io/nuget/v/NetworkPrimitives?style=for-the-badge)](https://www.nuget.org/packages/NetworkPrimitives)
[![NetworkPrimitives (Nuget)](https://img.shields.io/nuget/dt/NetworkPrimitives?style=for-the-badge)](https://www.nuget.org/packages/NetworkPrimitives)

The package `NetworkPrimitives.JsonConverters` contains converters for `System.Text.Json`

[![NetworkPrimitives.JsonConverters (Nuget)](https://img.shields.io/nuget/v/NetworkPrimitives.JsonConverters?style=for-the-badge)](https://www.nuget.org/packages/NetworkPrimitives.JsonConverters)
[![NetworkPrimitives.JsonConverters (Nuget)](https://img.shields.io/nuget/dt/NetworkPrimitives.JsonConverters?style=for-the-badge)](https://www.nuget.org/packages/NetworkPrimitives.JsonConverters)


### Performance

Overall goals are:

1. Fast parsing times
2. Low (preferably zero) allocations
3. Overall efficiency

### Detailed usage

- [Subnetting](docs/subnetting.md)
- [Address Ranges / Lists](docs/range-lists.md)
- [Performance](docs/performance.md)
- [Json Converters](docs/json-converters.md)