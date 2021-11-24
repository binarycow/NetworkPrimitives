// See https://aka.ms/new-console-template for more information

using System;
using System.Net;
using NetworkPrimitives.Ipv4;



/*********************************\
|          Ipv4Address            |
\*********************************/

Span<char> chars = stackalloc char[128];
Span<byte> bytes = stackalloc byte[128];

// Parse
var address = Ipv4Address.Parse("10.0.0.0");
_ = Ipv4Address.Parse(0xA0000000);
_ = Ipv4Address.Parse(IPAddress.Parse("10.0.0.0"));
#if NETCOREAPP2_1_OR_GREATER
_ = Ipv4Address.Parse("10.0.0.0".AsSpan());
#endif

// TryParse
_ = Ipv4Address.TryParse("10.0.0.0", out address);
_ = Ipv4Address.TryParse("10.0.0.0", out var charsRead, out address);
_ = Ipv4Address.TryParse(IPAddress.Parse("10.0.0.0"), out address);
_ = Ipv4Address.TryParse(new byte[] { }.AsSpan(), out address);
#if NETCOREAPP2_1_OR_GREATER
_ = Ipv4Address.TryParse("10.0.0.0".AsSpan(), out address);
_ = Ipv4Address.TryParse("10.0.0.0".AsSpan(), out charsRead, out address);
#endif
//TODO: After next update _ = Ipv4Address.TryParse(new byte[] { }.AsSpan(), out var bytesRead, out address);

// Properties
_ = address.BigEndianValue;
_ = address.LittleEndianValue;

_ = address.Equals(address);
_ = address.Equals((object)address);
_ = address.GetHashCode();
_ = address.CompareTo(address);
_ = address.CompareTo((object)address);
_ = address.GetBytes();
_ = address.GetOctet(0);
_ = address.ToString();
_ = address.TryFormat(chars, out var charsWritten);
_ = address.IsInSubnet(Ipv4Subnet.Parse("10.0.0.0/24"));
_ = address.ToIpAddress();
_ = address.TryWriteBytes(bytes, out var bytesWritten);
_ = address.GetAddressClass();
_ = address.GetRangeType();

/*********************************\
|           Ipv4Subnet            |
\*********************************/

var subnet = Ipv4Subnet.Parse("10.0.0.0/24");
subnet = Ipv4Subnet.Parse("10.0.0.0", "255.255.255.0");
subnet = Ipv4Subnet.Parse("10.0.0.0", 24);
subnet = Ipv4Subnet.Parse("10.0.0.0/24".AsSpan());