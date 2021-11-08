using System;
using System.Net;
using NetworkPrimitives.Ipv4;

namespace DemoFramework
{
    internal static class Program
    {
        private static void Main()
        {
            _ = Ipv4Address.Parse("0.201.76.200");
        }
    }
}