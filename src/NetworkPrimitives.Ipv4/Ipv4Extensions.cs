#nullable enable

using System;

namespace NetworkPrimitives.Ipv4
{
    public static class Ipv4Extensions
    {
        private const uint CLASS_A_MASK = 0b_10000000_00000000_00000000_00000000;
        private const uint CLASS_B_MASK = 0b_11000000_00000000_00000000_00000000;
        private const uint CLASS_C_MASK = 0b_11100000_00000000_00000000_00000000;
        private const uint CLASS_D_MASK = 0b_11110000_00000000_00000000_00000000;
        private const uint CLASS_A_VALUE = 0b_00000000_00000000_00000000_00000000;
        private const uint CLASS_B_VALUE = 0b_10000000_00000000_00000000_00000000;
        private const uint CLASS_C_VALUE = 0b_11000000_00000000_00000000_00000000;
        private const uint CLASS_D_VALUE = 0b_11100000_00000000_00000000_00000000;

        public static Ipv4AddressRangeType GetRangeType(this Ipv4Address value)
        {
            return CheckAll(value, out var type) ? type : default;
            static bool CheckAll(Ipv4Address value, out Ipv4AddressRangeType result) => 
                CheckOne(Ipv4WellKnownRanges.Rfc1918A.Contains(value), Ipv4AddressRangeType.Rfc1918, out result) 
                || CheckOne(Ipv4WellKnownRanges.Rfc1918B.Contains(value), Ipv4AddressRangeType.Rfc1918, out result)
                || CheckOne(Ipv4WellKnownRanges.Rfc1918C.Contains(value), Ipv4AddressRangeType.Rfc1918, out result)
                || CheckOne(Ipv4WellKnownRanges.CgNat.Contains(value), Ipv4AddressRangeType.CgNat, out result)
                || CheckOne(Ipv4WellKnownRanges.Doc1.Contains(value), Ipv4AddressRangeType.Documentation, out result)
                || CheckOne(Ipv4WellKnownRanges.Doc2.Contains(value), Ipv4AddressRangeType.Documentation, out result)
                || CheckOne(Ipv4WellKnownRanges.Doc3.Contains(value), Ipv4AddressRangeType.Documentation, out result)
                || CheckOne(Ipv4WellKnownRanges.Apipa.Contains(value), Ipv4AddressRangeType.Apipa, out result)
                || CheckOne(Ipv4WellKnownRanges.Loopback.Contains(value), Ipv4AddressRangeType.Loopback, out result)
                || CheckOne(Ipv4WellKnownRanges.Current.Contains(value), Ipv4AddressRangeType.Current, out result)
                || CheckOne(Ipv4WellKnownRanges.Broadcast.Contains(value), Ipv4AddressRangeType.Broadcast, out result)
                || CheckOne(Ipv4WellKnownRanges.Multicast.Contains(value), Ipv4AddressRangeType.Multicast, out result)
                || CheckOne(Ipv4WellKnownRanges.Benchmark.Contains(value), Ipv4AddressRangeType.Benchmark, out result)
            ;
        }
        
        public static Ipv4AddressRangeType GetRangeType(this Ipv4Subnet value)
        {
            return CheckAll(value, out var type) ? type : default;
            static bool CheckAll(Ipv4Subnet value, out Ipv4AddressRangeType result) => 
                CheckOne(Ipv4WellKnownRanges.Rfc1918A.Contains(value), Ipv4AddressRangeType.Rfc1918, out result) 
                || CheckOne(Ipv4WellKnownRanges.Rfc1918B.Contains(value), Ipv4AddressRangeType.Rfc1918, out result)
                || CheckOne(Ipv4WellKnownRanges.Rfc1918C.Contains(value), Ipv4AddressRangeType.Rfc1918, out result)
                || CheckOne(Ipv4WellKnownRanges.CgNat.Contains(value), Ipv4AddressRangeType.CgNat, out result)
                || CheckOne(Ipv4WellKnownRanges.Doc1.Contains(value), Ipv4AddressRangeType.Documentation, out result)
                || CheckOne(Ipv4WellKnownRanges.Doc2.Contains(value), Ipv4AddressRangeType.Documentation, out result)
                || CheckOne(Ipv4WellKnownRanges.Doc3.Contains(value), Ipv4AddressRangeType.Documentation, out result)
                || CheckOne(Ipv4WellKnownRanges.Apipa.Contains(value), Ipv4AddressRangeType.Apipa, out result)
                || CheckOne(Ipv4WellKnownRanges.Loopback.Contains(value), Ipv4AddressRangeType.Loopback, out result)
                || CheckOne(Ipv4WellKnownRanges.Current.Contains(value), Ipv4AddressRangeType.Current, out result)
                || CheckOne(Ipv4WellKnownRanges.Broadcast.Contains(value), Ipv4AddressRangeType.Broadcast, out result)
                || CheckOne(Ipv4WellKnownRanges.Multicast.Contains(value), Ipv4AddressRangeType.Multicast, out result)
                || CheckOne(Ipv4WellKnownRanges.Benchmark.Contains(value), Ipv4AddressRangeType.Benchmark, out result)
            ;
        }
        
        private static bool CheckOne(bool ret, Ipv4AddressRangeType type, out Ipv4AddressRangeType result)
        {
            result = ret ? type : default;
            return ret;
        }
        
        public static Ipv4AddressClass GetAddressClass(this Ipv4Address address)
        {
            var value = address.Value;
            if ((value & CLASS_A_MASK) == CLASS_A_VALUE)
                return Ipv4AddressClass.ClassA;
            if ((value & CLASS_B_MASK) == CLASS_B_VALUE)
                return Ipv4AddressClass.ClassB;
            if ((value & CLASS_C_MASK) == CLASS_C_VALUE)
                return Ipv4AddressClass.ClassC;
            if ((value & CLASS_D_MASK) == CLASS_D_VALUE)
                return Ipv4AddressClass.ClassD;
            return Ipv4AddressClass.ClassE;
        }
    }
}