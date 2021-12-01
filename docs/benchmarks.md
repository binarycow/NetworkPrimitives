
## Benchmarks

These benchmarks compare the performance of this library against [IPNetwork2](https://github.com/lduchosal/ipnetwork),
using the benchmark library [BenchmarkDotNet](https://benchmarkdotnet.org/articles/overview.html).

*Note:* It is not an error when the NetworkPrimitives benchmarks show `-` in the `Gen0` and `Allocated` columns.  
This means that there were no allocations. (or at least, below whatever reporting thresholds BenchmarkDotNet may have...)

To run your own benchmarks, use [NetworkPrimitives.Benchmarks.csproj](../benchmarks/NetworkPrimitives.Benchmarks.csproj)

### Parsing 100 IPv4 Addresses

```c#
public void NetworkPrimitives()
{
    foreach (var address in TestData.RandomIpAddresses)
    {
        _ = NetworkPrimitives.Ipv4.Ipv4Address.Parse(address);
    }
}

public void DotNet()
{
    foreach (var address in TestData.RandomIpAddresses)
    {
        _ = System.Net.IPAddress.Parse(address);
    }
}
        
public void IpNetwork2()
{
    foreach (var address in TestData.RandomIpAddresses)
    {
        _ = System.Net.IPNetwork.Parse(address);
    }
}
```


|            Method |       Mean |     Error |     StdDev |   Gen 0 | Allocated |
|------------------ |-----------:|----------:|-----------:|--------:|----------:|
| NetworkPrimitives |   8.450 μs | 0.2642 μs |  0.7052 μs |       - |         - |
|            DotNet |   6.634 μs | 0.1577 μs |  0.4290 μs |  0.9537 |   4,000 B |
|        IpNetwork2 | 207.892 μs | 4.8725 μs | 13.5017 μs | 34.6680 | 145,053 B |


### Iterating 1 subnet containing 256 IPv4 addresses

```c#
public void NetworkPrimitives()
{
    var subnet = Ipv4Subnet.Parse("10.0.0.0/24");
    foreach (var address in subnet.GetAllAddresses())
    {

    }
}

public void IpNetwork2()
{
    var subnet = System.Net.IPNetwork.Parse("10.0.0.0/24");
    foreach (var address in subnet.ListIPAddress())
    {
        
    }
}
```

|            Method |         Mean |       Error |       StdDev |       Median |    Gen 0 | Allocated |
|------------------ |-------------:|------------:|-------------:|-------------:|---------:|----------:|
| NetworkPrimitives |     970.7 ns |    28.79 ns |     75.84 ns |     932.9 ns |        - |         - |
|        IpNetwork2 | 450,398.3 ns | 5,960.03 ns | 15,908.54 ns | 447,500.9 ns | 134.7656 | 564,865 B |

