``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1288 (20H2/October2020Update)
Intel Core i7-1065G7 CPU 1.30GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.100-rc.2.21505.57
  [Host]     : .NET 5.0.11 (5.0.1121.47308), X64 RyuJIT
  Job-XIOEXP : .NET 5.0.11 (5.0.1121.47308), X64 RyuJIT

Runtime=.NET 5.0  IterationCount=30  LaunchCount=3  
WarmupCount=10  

```
|            Method |      Mean |    Error |   StdDev |    Median |   Gen 0 | Allocated |
|------------------ |----------:|---------:|---------:|----------:|--------:|----------:|
| NetworkPrimitives |  52.09 μs | 0.369 μs | 0.991 μs |  51.84 μs |       - |         - |
|        IpNetwork2 | 185.02 μs | 3.111 μs | 8.517 μs | 182.52 μs | 34.6680 | 145,053 B |
