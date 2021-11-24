using System;

namespace NetworkPrimitives.Ipv4
{
    /// <summary>
    /// In the classful networking architecture, an IP address is assigned to one of five classes.
    /// </summary>
    /// <remarks>
    /// Classful IP addressing is obsolete.  This type is provided for your information, but you should
    /// do your best to remove your need to use this type.
    /// </remarks>
    public enum Ipv4AddressClass
    {
        /// <summary>
        /// IPv4 addresses between 0.0.0.0 and 127.255.255.255, with a default subnet mask of 255.0.0.0
        /// </summary>
        ClassA,
        /// <summary>
        /// IPv4 addresses between 128.0.0.0 and 191.255.255.255, with a default subnet mask of 255.255.0.0
        /// </summary>
        ClassB,
        /// <summary>
        /// IPv4 addresses between 192.0.0.0 and 223.255.255.255, with a default subnet mask of 255.255.255.0
        /// </summary>
        ClassC,
        /// <summary>
        /// IPv4 addresses between 224.0.0.0 and 239.255.255.255; Multicast.
        /// </summary>
        ClassD,
        /// <summary>
        /// IPv4 addresses between 240.0.0.0 and 255.255.255.255; Reserved.
        /// </summary>
        ClassE,
    }
}