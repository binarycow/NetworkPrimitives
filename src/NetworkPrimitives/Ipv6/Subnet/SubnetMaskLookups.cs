﻿#nullable enable

using System;

namespace NetworkPrimitives.Ipv6
{
    [ExcludeFromCodeCoverage("Internal")]
    internal static class SubnetMaskLookups
    {
        public static byte GetCidr(ulong high, ulong low) => (high, low) switch
        {
            (0x0000000000000000, 0x0000000000000000) => 0,
            (0x8000000000000000, 0x0000000000000000) => 1,
            (0xC000000000000000, 0x0000000000000000) => 2,
            (0xE000000000000000, 0x0000000000000000) => 3,
            (0xF000000000000000, 0x0000000000000000) => 4,
            (0xF800000000000000, 0x0000000000000000) => 5,
            (0xFC00000000000000, 0x0000000000000000) => 6,
            (0xFE00000000000000, 0x0000000000000000) => 7,
            (0xFF00000000000000, 0x0000000000000000) => 8,
            (0xFF80000000000000, 0x0000000000000000) => 9,
            (0xFFC0000000000000, 0x0000000000000000) => 10,
            (0xFFE0000000000000, 0x0000000000000000) => 11,
            (0xFFF0000000000000, 0x0000000000000000) => 12,
            (0xFFF8000000000000, 0x0000000000000000) => 13,
            (0xFFFC000000000000, 0x0000000000000000) => 14,
            (0xFFFE000000000000, 0x0000000000000000) => 15,
            (0xFFFF000000000000, 0x0000000000000000) => 16,
            (0xFFFF800000000000, 0x0000000000000000) => 17,
            (0xFFFFC00000000000, 0x0000000000000000) => 18,
            (0xFFFFE00000000000, 0x0000000000000000) => 19,
            (0xFFFFF00000000000, 0x0000000000000000) => 20,
            (0xFFFFF80000000000, 0x0000000000000000) => 21,
            (0xFFFFFC0000000000, 0x0000000000000000) => 22,
            (0xFFFFFE0000000000, 0x0000000000000000) => 23,
            (0xFFFFFF0000000000, 0x0000000000000000) => 24,
            (0xFFFFFF8000000000, 0x0000000000000000) => 25,
            (0xFFFFFFC000000000, 0x0000000000000000) => 26,
            (0xFFFFFFE000000000, 0x0000000000000000) => 27,
            (0xFFFFFFF000000000, 0x0000000000000000) => 28,
            (0xFFFFFFF800000000, 0x0000000000000000) => 29,
            (0xFFFFFFFC00000000, 0x0000000000000000) => 30,
            (0xFFFFFFFE00000000, 0x0000000000000000) => 31,
            (0xFFFFFFFF00000000, 0x0000000000000000) => 32,
            (0xFFFFFFFF80000000, 0x0000000000000000) => 33,
            (0xFFFFFFFFC0000000, 0x0000000000000000) => 34,
            (0xFFFFFFFFE0000000, 0x0000000000000000) => 35,
            (0xFFFFFFFFF0000000, 0x0000000000000000) => 36,
            (0xFFFFFFFFF8000000, 0x0000000000000000) => 37,
            (0xFFFFFFFFFC000000, 0x0000000000000000) => 38,
            (0xFFFFFFFFFE000000, 0x0000000000000000) => 39,
            (0xFFFFFFFFFF000000, 0x0000000000000000) => 40,
            (0xFFFFFFFFFF800000, 0x0000000000000000) => 41,
            (0xFFFFFFFFFFC00000, 0x0000000000000000) => 42,
            (0xFFFFFFFFFFE00000, 0x0000000000000000) => 43,
            (0xFFFFFFFFFFF00000, 0x0000000000000000) => 44,
            (0xFFFFFFFFFFF80000, 0x0000000000000000) => 45,
            (0xFFFFFFFFFFFC0000, 0x0000000000000000) => 46,
            (0xFFFFFFFFFFFE0000, 0x0000000000000000) => 47,
            (0xFFFFFFFFFFFF0000, 0x0000000000000000) => 48,
            (0xFFFFFFFFFFFF8000, 0x0000000000000000) => 49,
            (0xFFFFFFFFFFFFC000, 0x0000000000000000) => 50,
            (0xFFFFFFFFFFFFE000, 0x0000000000000000) => 51,
            (0xFFFFFFFFFFFFF000, 0x0000000000000000) => 52,
            (0xFFFFFFFFFFFFF800, 0x0000000000000000) => 53,
            (0xFFFFFFFFFFFFFC00, 0x0000000000000000) => 54,
            (0xFFFFFFFFFFFFFE00, 0x0000000000000000) => 55,
            (0xFFFFFFFFFFFFFF00, 0x0000000000000000) => 56,
            (0xFFFFFFFFFFFFFF80, 0x0000000000000000) => 57,
            (0xFFFFFFFFFFFFFFC0, 0x0000000000000000) => 58,
            (0xFFFFFFFFFFFFFFE0, 0x0000000000000000) => 59,
            (0xFFFFFFFFFFFFFFF0, 0x0000000000000000) => 60,
            (0xFFFFFFFFFFFFFFF8, 0x0000000000000000) => 61,
            (0xFFFFFFFFFFFFFFFC, 0x0000000000000000) => 62,
            (0xFFFFFFFFFFFFFFFE, 0x0000000000000000) => 63,
            (0xFFFFFFFFFFFFFFFF, 0x0000000000000000) => 64,
            (0xFFFFFFFFFFFFFFFF, 0x8000000000000000) => 65,
            (0xFFFFFFFFFFFFFFFF, 0xC000000000000000) => 66,
            (0xFFFFFFFFFFFFFFFF, 0xE000000000000000) => 67,
            (0xFFFFFFFFFFFFFFFF, 0xF000000000000000) => 68,
            (0xFFFFFFFFFFFFFFFF, 0xF800000000000000) => 69,
            (0xFFFFFFFFFFFFFFFF, 0xFC00000000000000) => 70,
            (0xFFFFFFFFFFFFFFFF, 0xFE00000000000000) => 71,
            (0xFFFFFFFFFFFFFFFF, 0xFF00000000000000) => 72,
            (0xFFFFFFFFFFFFFFFF, 0xFF80000000000000) => 73,
            (0xFFFFFFFFFFFFFFFF, 0xFFC0000000000000) => 74,
            (0xFFFFFFFFFFFFFFFF, 0xFFE0000000000000) => 75,
            (0xFFFFFFFFFFFFFFFF, 0xFFF0000000000000) => 76,
            (0xFFFFFFFFFFFFFFFF, 0xFFF8000000000000) => 77,
            (0xFFFFFFFFFFFFFFFF, 0xFFFC000000000000) => 78,
            (0xFFFFFFFFFFFFFFFF, 0xFFFE000000000000) => 79,
            (0xFFFFFFFFFFFFFFFF, 0xFFFF000000000000) => 80,
            (0xFFFFFFFFFFFFFFFF, 0xFFFF800000000000) => 81,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFC00000000000) => 82,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFE00000000000) => 83,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFF00000000000) => 84,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFF80000000000) => 85,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFC0000000000) => 86,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFE0000000000) => 87,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFF0000000000) => 88,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFF8000000000) => 89,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFC000000000) => 90,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFE000000000) => 91,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFF000000000) => 92,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFF800000000) => 93,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFC00000000) => 94,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFE00000000) => 95,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFF00000000) => 96,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFF80000000) => 97,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFC0000000) => 98,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFE0000000) => 99,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFF0000000) => 100,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFF8000000) => 101,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFC000000) => 102,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFE000000) => 103,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFF000000) => 104,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFF800000) => 105,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFC00000) => 106,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFE00000) => 107,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFF00000) => 108,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFF80000) => 109,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFC0000) => 110,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFE0000) => 111,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFF0000) => 112,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFF8000) => 113,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFC000) => 114,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFE000) => 115,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFF000) => 116,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFF800) => 117,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFC00) => 118,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFE00) => 119,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFF00) => 120,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFF80) => 121,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFC0) => 122,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFE0) => 123,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFF0) => 124,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFF8) => 125,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFC) => 126,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFE) => 127,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF) => 128,


            _ => 0,
        };

        public static bool IsValidSubnetMask(ulong high, ulong low) => (high, low) switch
        {
            (0x0000000000000000, 0x0000000000000000) => true,
            (0x8000000000000000, 0x0000000000000000) => true,
            (0xC000000000000000, 0x0000000000000000) => true,
            (0xE000000000000000, 0x0000000000000000) => true,
            (0xF000000000000000, 0x0000000000000000) => true,
            (0xF800000000000000, 0x0000000000000000) => true,
            (0xFC00000000000000, 0x0000000000000000) => true,
            (0xFE00000000000000, 0x0000000000000000) => true,
            (0xFF00000000000000, 0x0000000000000000) => true,
            (0xFF80000000000000, 0x0000000000000000) => true,
            (0xFFC0000000000000, 0x0000000000000000) => true,
            (0xFFE0000000000000, 0x0000000000000000) => true,
            (0xFFF0000000000000, 0x0000000000000000) => true,
            (0xFFF8000000000000, 0x0000000000000000) => true,
            (0xFFFC000000000000, 0x0000000000000000) => true,
            (0xFFFE000000000000, 0x0000000000000000) => true,
            (0xFFFF000000000000, 0x0000000000000000) => true,
            (0xFFFF800000000000, 0x0000000000000000) => true,
            (0xFFFFC00000000000, 0x0000000000000000) => true,
            (0xFFFFE00000000000, 0x0000000000000000) => true,
            (0xFFFFF00000000000, 0x0000000000000000) => true,
            (0xFFFFF80000000000, 0x0000000000000000) => true,
            (0xFFFFFC0000000000, 0x0000000000000000) => true,
            (0xFFFFFE0000000000, 0x0000000000000000) => true,
            (0xFFFFFF0000000000, 0x0000000000000000) => true,
            (0xFFFFFF8000000000, 0x0000000000000000) => true,
            (0xFFFFFFC000000000, 0x0000000000000000) => true,
            (0xFFFFFFE000000000, 0x0000000000000000) => true,
            (0xFFFFFFF000000000, 0x0000000000000000) => true,
            (0xFFFFFFF800000000, 0x0000000000000000) => true,
            (0xFFFFFFFC00000000, 0x0000000000000000) => true,
            (0xFFFFFFFE00000000, 0x0000000000000000) => true,
            (0xFFFFFFFF00000000, 0x0000000000000000) => true,
            (0xFFFFFFFF80000000, 0x0000000000000000) => true,
            (0xFFFFFFFFC0000000, 0x0000000000000000) => true,
            (0xFFFFFFFFE0000000, 0x0000000000000000) => true,
            (0xFFFFFFFFF0000000, 0x0000000000000000) => true,
            (0xFFFFFFFFF8000000, 0x0000000000000000) => true,
            (0xFFFFFFFFFC000000, 0x0000000000000000) => true,
            (0xFFFFFFFFFE000000, 0x0000000000000000) => true,
            (0xFFFFFFFFFF000000, 0x0000000000000000) => true,
            (0xFFFFFFFFFF800000, 0x0000000000000000) => true,
            (0xFFFFFFFFFFC00000, 0x0000000000000000) => true,
            (0xFFFFFFFFFFE00000, 0x0000000000000000) => true,
            (0xFFFFFFFFFFF00000, 0x0000000000000000) => true,
            (0xFFFFFFFFFFF80000, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFC0000, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFE0000, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFF0000, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFF8000, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFC000, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFE000, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFF000, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFF800, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFFC00, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFFE00, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFFF00, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFFF80, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFFFC0, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFFFE0, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFFFF0, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFFFF8, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFFFFC, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFFFFE, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0x0000000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0x8000000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xC000000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xE000000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xF000000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xF800000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFC00000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFE00000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFF00000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFF80000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFC0000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFE0000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFF0000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFF8000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFC000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFE000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFF000000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFF800000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFC00000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFE00000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFF00000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFF80000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFC0000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFE0000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFF0000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFF8000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFC000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFE000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFF000000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFF800000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFC00000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFE00000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFF00000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFF80000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFC0000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFE0000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFF0000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFF8000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFC000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFE000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFF000000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFF800000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFC00000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFE00000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFF00000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFF80000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFC0000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFE0000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFF0000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFF8000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFC000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFE000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFF000) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFF800) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFC00) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFE00) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFF00) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFF80) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFC0) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFE0) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFF0) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFF8) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFC) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFE) => true,
            (0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF) => true,
            _ => false,
        };

        public static bool TryConvertIpv6Address(byte cidr, out ulong high, out ulong low)
        {
            bool success;
            (success, high, low) = cidr switch
            {
                0 => (true, (ulong)0x0000000000000000, (ulong)0x0000000000000000),
                1 => (true, 0x8000000000000000, (ulong)0x0000000000000000),
                2 => (true, 0xC000000000000000, (ulong)0x0000000000000000),
                3 => (true, 0xE000000000000000, (ulong)0x0000000000000000),
                4 => (true, 0xF000000000000000, (ulong)0x0000000000000000),
                5 => (true, 0xF800000000000000, (ulong)0x0000000000000000),
                6 => (true, 0xFC00000000000000, (ulong)0x0000000000000000),
                7 => (true, 0xFE00000000000000, (ulong)0x0000000000000000),
                8 => (true, 0xFF00000000000000, (ulong)0x0000000000000000),
                9 => (true, 0xFF80000000000000, (ulong)0x0000000000000000),
                10 => (true, 0xFFC0000000000000, (ulong)0x0000000000000000),
                11 => (true, 0xFFE0000000000000, (ulong)0x0000000000000000),
                12 => (true, 0xFFF0000000000000, (ulong)0x0000000000000000),
                13 => (true, 0xFFF8000000000000, (ulong)0x0000000000000000),
                14 => (true, 0xFFFC000000000000, (ulong)0x0000000000000000),
                15 => (true, 0xFFFE000000000000, (ulong)0x0000000000000000),
                16 => (true, 0xFFFF000000000000, (ulong)0x0000000000000000),
                17 => (true, 0xFFFF800000000000, (ulong)0x0000000000000000),
                18 => (true, 0xFFFFC00000000000, (ulong)0x0000000000000000),
                19 => (true, 0xFFFFE00000000000, (ulong)0x0000000000000000),
                20 => (true, 0xFFFFF00000000000, (ulong)0x0000000000000000),
                21 => (true, 0xFFFFF80000000000, (ulong)0x0000000000000000),
                22 => (true, 0xFFFFFC0000000000, (ulong)0x0000000000000000),
                23 => (true, 0xFFFFFE0000000000, (ulong)0x0000000000000000),
                24 => (true, 0xFFFFFF0000000000, (ulong)0x0000000000000000),
                25 => (true, 0xFFFFFF8000000000, (ulong)0x0000000000000000),
                26 => (true, 0xFFFFFFC000000000, (ulong)0x0000000000000000),
                27 => (true, 0xFFFFFFE000000000, (ulong)0x0000000000000000),
                28 => (true, 0xFFFFFFF000000000, (ulong)0x0000000000000000),
                29 => (true, 0xFFFFFFF800000000, (ulong)0x0000000000000000),
                30 => (true, 0xFFFFFFFC00000000, (ulong)0x0000000000000000),
                31 => (true, 0xFFFFFFFE00000000, (ulong)0x0000000000000000),
                32 => (true, 0xFFFFFFFF00000000, (ulong)0x0000000000000000),
                33 => (true, 0xFFFFFFFF80000000, (ulong)0x0000000000000000),
                34 => (true, 0xFFFFFFFFC0000000, (ulong)0x0000000000000000),
                35 => (true, 0xFFFFFFFFE0000000, (ulong)0x0000000000000000),
                36 => (true, 0xFFFFFFFFF0000000, (ulong)0x0000000000000000),
                37 => (true, 0xFFFFFFFFF8000000, (ulong)0x0000000000000000),
                38 => (true, 0xFFFFFFFFFC000000, (ulong)0x0000000000000000),
                39 => (true, 0xFFFFFFFFFE000000, (ulong)0x0000000000000000),
                40 => (true, 0xFFFFFFFFFF000000, (ulong)0x0000000000000000),
                41 => (true, 0xFFFFFFFFFF800000, (ulong)0x0000000000000000),
                42 => (true, 0xFFFFFFFFFFC00000, (ulong)0x0000000000000000),
                43 => (true, 0xFFFFFFFFFFE00000, (ulong)0x0000000000000000),
                44 => (true, 0xFFFFFFFFFFF00000, (ulong)0x0000000000000000),
                45 => (true, 0xFFFFFFFFFFF80000, (ulong)0x0000000000000000),
                46 => (true, 0xFFFFFFFFFFFC0000, (ulong)0x0000000000000000),
                47 => (true, 0xFFFFFFFFFFFE0000, (ulong)0x0000000000000000),
                48 => (true, 0xFFFFFFFFFFFF0000, (ulong)0x0000000000000000),
                49 => (true, 0xFFFFFFFFFFFF8000, (ulong)0x0000000000000000),
                50 => (true, 0xFFFFFFFFFFFFC000, (ulong)0x0000000000000000),
                51 => (true, 0xFFFFFFFFFFFFE000, (ulong)0x0000000000000000),
                52 => (true, 0xFFFFFFFFFFFFF000, (ulong)0x0000000000000000),
                53 => (true, 0xFFFFFFFFFFFFF800, (ulong)0x0000000000000000),
                54 => (true, 0xFFFFFFFFFFFFFC00, (ulong)0x0000000000000000),
                55 => (true, 0xFFFFFFFFFFFFFE00, (ulong)0x0000000000000000),
                56 => (true, 0xFFFFFFFFFFFFFF00, (ulong)0x0000000000000000),
                57 => (true, 0xFFFFFFFFFFFFFF80, (ulong)0x0000000000000000),
                58 => (true, 0xFFFFFFFFFFFFFFC0, (ulong)0x0000000000000000),
                59 => (true, 0xFFFFFFFFFFFFFFE0, (ulong)0x0000000000000000),
                60 => (true, 0xFFFFFFFFFFFFFFF0, (ulong)0x0000000000000000),
                61 => (true, 0xFFFFFFFFFFFFFFF8, (ulong)0x0000000000000000),
                62 => (true, 0xFFFFFFFFFFFFFFFC, (ulong)0x0000000000000000),
                63 => (true, 0xFFFFFFFFFFFFFFFE, (ulong)0x0000000000000000),
                64 => (true, 0xFFFFFFFFFFFFFFFF, (ulong)0x0000000000000000),
                65 => (true, 0xFFFFFFFFFFFFFFFF, 0x8000000000000000),
                66 => (true, 0xFFFFFFFFFFFFFFFF, 0xC000000000000000),
                67 => (true, 0xFFFFFFFFFFFFFFFF, 0xE000000000000000),
                68 => (true, 0xFFFFFFFFFFFFFFFF, 0xF000000000000000),
                69 => (true, 0xFFFFFFFFFFFFFFFF, 0xF800000000000000),
                70 => (true, 0xFFFFFFFFFFFFFFFF, 0xFC00000000000000),
                71 => (true, 0xFFFFFFFFFFFFFFFF, 0xFE00000000000000),
                72 => (true, 0xFFFFFFFFFFFFFFFF, 0xFF00000000000000),
                73 => (true, 0xFFFFFFFFFFFFFFFF, 0xFF80000000000000),
                74 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFC0000000000000),
                75 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFE0000000000000),
                76 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFF0000000000000),
                77 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFF8000000000000),
                78 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFC000000000000),
                79 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFE000000000000),
                80 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFF000000000000),
                81 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFF800000000000),
                82 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFC00000000000),
                83 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFE00000000000),
                84 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFF00000000000),
                85 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFF80000000000),
                86 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFC0000000000),
                87 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFE0000000000),
                88 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFF0000000000),
                89 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFF8000000000),
                90 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFC000000000),
                91 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFE000000000),
                92 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFF000000000),
                93 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFF800000000),
                94 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFC00000000),
                95 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFE00000000),
                96 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFF00000000),
                97 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFF80000000),
                98 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFC0000000),
                99 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFE0000000),
                100 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFF0000000),
                101 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFF8000000),
                102 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFC000000),
                103 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFE000000),
                104 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFF000000),
                105 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFF800000),
                106 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFC00000),
                107 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFE00000),
                108 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFF00000),
                109 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFF80000),
                110 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFC0000),
                111 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFE0000),
                112 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFF0000),
                113 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFF8000),
                114 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFC000),
                115 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFE000),
                116 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFF000),
                117 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFF800),
                118 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFC00),
                119 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFE00),
                120 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFF00),
                121 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFF80),
                122 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFC0),
                123 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFE0),
                124 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFF0),
                125 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFF8),
                126 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFC),
                127 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFE),
                128 => (true, 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF),
                _ => (false, (ulong)0, (ulong)0),
            };
            return success;
        }
    }
}