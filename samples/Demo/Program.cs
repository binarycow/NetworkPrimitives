


using System;
using System.Collections;
using System.Collections.Generic;
using NetworkPrimitives.Ipv4;


namespace Demo
{
    internal static class Program
    {
        private static void Main()
        {
var subnet = Ipv4Subnet.Parse("10.0.0.0/28");
Console.WriteLine("Option 1: ref struct");
// Option 1: ref struct
foreach (var address in subnet.GetAllAddresses())
{
    Console.WriteLine($"  {address.ToString()}");
}
Console.WriteLine();
Console.WriteLine("Option 2: ToEnumerable() method");
foreach (var address in subnet.GetUsableAddresses().ToEnumerable())
{
    Console.WriteLine($"  {address.ToString()}");
}
Console.WriteLine();
Console.WriteLine("Option 3: Explicit implementation of IEnumerable<Ipv4Address>");
WriteRange(subnet.GetUsableAddresses());
        }

private static void WriteRange(IEnumerable<Ipv4Address> range)
{
    foreach (var address in range)
    {
        Console.WriteLine($"  {address.ToString()}");
    }
}
    }
}