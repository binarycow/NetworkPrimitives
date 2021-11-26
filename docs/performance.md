# Performance Techniques

`NetworkPrimitives` uses the following techniques to maintain high performance:

1. Reduce allocations whenever possible.
2. When appropriate, use `struct` rather than `class`
3. Provide `ReadOnlySpan<char>` overloads for parsing for all types.
4. For types that can be serialized to binary, provide `Span<byte>` overloads.
5. Unless absolutely necessary, use immutable types only.
6. When possible, provide allocation-free enumerators.

## Enumerators

One often overlooked allocation that occurs are enumerators.  A type that implements
`IEnumerable<T>` has a method `GetEnumerator()` that will instantiate an `IEnumerator<T>`.

Additionally, anytime a `struct` is converted to an `interface`, it is boxed - which
creates an allocation.

C# uses duck-typing for `foreach` loops.  `NetworkPrimitives` leverages this to
allow you to iterate over collections without allocating an enumerator.  

For example, this code does not allocate an enumerator:

```c#
Ipv4AddressRange range = subnet.GetAllAddresses();
foreach (var address in range)
{
    Console.WriteLine(address.ToString());
}
```

This is because the `Ipv4AddressRange` type has a `GetEnumerator` method that 
returns an instance of the `Ipv4AddressEnumerator` `ref struct` - no allocation.
The `Ipv4AddressEnumerator` type has a `MoveNext()` method and a `Current`
property that returns an `Ipv4Address` `struct` - no allocation.


If you prefer to have a `class` enumerator, you can do one of two things.

First option, is to call the `ToEnumerable()` method on an `Ipv4AddressRange`.
Note, that this will cause one allocation.

```c#
Ipv4AddressRange range = subnet.GetAllAddresses();
foreach (var address in range.ToEnumerable())
{
    Console.WriteLine(address.ToString());
}
```

Your other option is to simply pass the `Ipv4AddressRange` to a method that
needs an `IEnumerable<Ipv4Address>`.


```c#

private static void Main()
{
  subnet = Ipv4Subnet.Parse("10.0.0.0/24");
  Ipv4AddressRange range = subnet.GetAllAddresses();
  WriteRange(range);
}

private static void WriteRange(IEnumerable<Ipv4Address> range)
{
    foreach (var address in range)
    {
        Console.WriteLine(address.ToString());
    }
}
```



## Framework Versions

### .NET Core 3.0+ or .NET Standard 2.1

Using either .NET Core 3.0 (or higher) or .NET Standard 2.1 will give the
best performance when using the `NetworkPrimitives` library.

### .NET Standard 2.0

If you're stuck on .NET Framework, a .NET Core version prior to 3.0, 
or some other version of .NET, that doesn't mean that `NetworkPrimitives`
is not a high performance library.  It just will not perform as well as 
on .NET Core 3.0 or higher.

Some methods are not available in .NET Standard, which prevents 
`NetworkPrimitives` from gaining the full performance benefits.

For example:

- Constructor `System.String(ReadOnlySpan<char>)` ([docs](https://docs.microsoft.com/en-us/dotnet/api/system.string.-ctor?#System_String__ctor_System_ReadOnlySpan_System_Char__))
  without this constructor, we must instantiate a `char[]` to create the `string`
- Constructor `System.Net.IPAddress(ReadOnlySpan<byte>)` ([docs](https://docs.microsoft.com/en-us/dotnet/api/system.net.ipaddress.-ctor?#System_Net_IPAddress__ctor_System_ReadOnlySpan_System_Byte__))
  without this constructor, we must instantiate a `byte[]` to create the `IPAddress`
- Method `System.Net.IPAddress.TryWriteBytes` ([docs](https://docs.microsoft.com/en-us/dotnet/api/system.net.ipaddress.trywritebytes))
  without this method, we must call `IPAddress.GetAddressBytes()`, which instantiates a `byte[]`.