#nullable enable

using System;
using System.Globalization;

namespace NetworkPrimitives.Switching
{
    public static class MacAddressFormats
    {
        public static MacAddressFormat Cisco { get; } 
            = new (4, '.', Casing.Lowercase);

        public static MacAddressFormat Default => MacAddressFormats.Cisco;
    }
}