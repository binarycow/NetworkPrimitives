[Back to readme.md](../readme.md)

## NetworkPrimitives.JsonConverters

The nuget package `NetworkPrimitives.JsonConverters` contains some basic `System.Test.Json` converters for the `NetworkPrimitive` types.

**[Download from nuget.org](https://www.nuget.org/packages/NetworkPrimitives.JsonConverters)**

[![NetworkPrimitives.JsonConverters (Nuget)](https://img.shields.io/nuget/v/NetworkPrimitives.JsonConverters?style=for-the-badge)](https://www.nuget.org/packages/NetworkPrimitives.JsonConverters)
[![NetworkPrimitives.JsonConverters (Nuget)](https://img.shields.io/nuget/dt/NetworkPrimitives.JsonConverters?style=for-the-badge)](https://www.nuget.org/packages/NetworkPrimitives.JsonConverters)

### Usage Instructions

```c#
var jsonSerializerOptions = new JsonSerializerOptions();

// Option 1: Add both IPv4 and IPv6 converters
jsonSerializerOptions = jsonSerializerOptions.AddNetworkPrimitivesConverters();

// Option 2: Add only IPv4 converters
jsonSerializerOptions = jsonSerializerOptions.AddIpv4Converters();

// Option 3: Add only IPv6 converters
jsonSerializerOptions = jsonSerializerOptions.AddIpv6Converters();
```