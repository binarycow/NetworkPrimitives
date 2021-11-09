#nullable enable

namespace NetworkPrimitives.Ipv4
{
    internal static class SubnetMaskLookups
    {
        public static bool IsValidSubnetMask(uint value) 
            => GetTotalHosts(value) > 0;

        public static bool TryGetCidr(uint value, out byte result)
        {
            bool success;
            (success, result) = SubnetMaskLookups.TryGetCidr(value);
            return success;
        }
        public static byte GetCidr(uint value)
        {
            _ = SubnetMaskLookups.TryGetCidr(value, out var result);
            return result;
        }
        public static bool TryGetSubnetMask(string? value, out uint result)
        {
            bool success;
            (success, result) = SubnetMaskLookups.TryGetSubnetMask(value);
            return success;
        }
        public static uint GetSubnetMask(string value)
        {
            _ = SubnetMaskLookups.TryGetSubnetMask(value, out var result);
            return result;
        }
        public static bool TryGetSubnetMask(byte value, out uint result)
        {
            bool success;
            (success, result) = SubnetMaskLookups.TryGetSubnetMask(value);
            return success;
        }
        public static uint GetSubnetMask(byte value)
        {
            _ = SubnetMaskLookups.TryGetSubnetMask(value, out var result);
            return result;
        }
        
        public static bool TryGetString(uint value, out string result)
        {
            bool success;
            (success, result) = SubnetMaskLookups.TryGetString(value);
            return success;
        }
        public static string GetString(uint value)
        {
            _ = SubnetMaskLookups.TryGetString(value, out var result);
            return result;
        }

        public static ulong GetTotalHosts(byte value) => value switch
        {
            0 => 4294967296,
            1 => 2147483648,
            2 => 1073741824,
            3 => 536870912,
            4 => 268435456,
            5 => 134217728,
            6 => 67108864,
            7 => 33554432,
            8 => 16777216,
            9 => 8388608,
            10 => 4194304,
            11 => 2097152,
            12 => 1048576,
            13 => 524288,
            14 => 262144,
            15 => 131072,
            16 => 65536,
            17 => 32768,
            18 => 16384,
            19 => 8192,
            20 => 4096,
            21 => 2048,
            22 => 1024,
            23 => 512,
            24 => 256,
            25 => 128,
            26 => 64,
            27 => 32,
            28 => 16,
            29 => 8,
            30 => 4,
            31 => 2,
            32 => 1,
            _ => 0,
        };

        public static ulong GetTotalHosts(uint value) => value switch
        {
            0x00000000 => 4294967296,
            0x80000000 => 2147483648,
            0xC0000000 => 1073741824,
            0xE0000000 => 536870912,
            0xF0000000 => 268435456,
            0xF8000000 => 134217728,
            0xFC000000 => 67108864,
            0xFE000000 => 33554432,
            0xFF000000 => 16777216,
            0xFF800000 => 8388608,
            0xFFC00000 => 4194304,
            0xFFE00000 => 2097152,
            0xFFF00000 => 1048576,
            0xFFF80000 => 524288,
            0xFFFC0000 => 262144,
            0xFFFE0000 => 131072,
            0xFFFF0000 => 65536,
            0xFFFF8000 => 32768,
            0xFFFFC000 => 16384,
            0xFFFFE000 => 8192,
            0xFFFFF000 => 4096,
            0xFFFFF800 => 2048,
            0xFFFFFC00 => 1024,
            0xFFFFFE00 => 512,
            0xFFFFFF00 => 256,
            0xFFFFFF80 => 128,
            0xFFFFFFC0 => 64,
            0xFFFFFFE0 => 32,
            0xFFFFFFF0 => 16,
            0xFFFFFFF8 => 8,
            0xFFFFFFFC => 4,
            0xFFFFFFFE => 2,
            0xFFFFFFFF => 1,
            _ => 0,
        };

        public static uint GetUsableHosts(byte value) => value switch
        {
            0 => 4294967294,
            1 => 2147483646,
            2 => 1073741822,
            3 => 536870910,
            4 => 268435454,
            5 => 134217726,
            6 => 67108862,
            7 => 33554430,
            8 => 16777214,
            9 => 8388606,
            10 => 4194302,
            11 => 2097150,
            12 => 1048574,
            13 => 524286,
            14 => 262142,
            15 => 131070,
            16 => 65534,
            17 => 32766,
            18 => 16382,
            19 => 8190,
            20 => 4094,
            21 => 2046,
            22 => 1022,
            23 => 510,
            24 => 254,
            25 => 126,
            26 => 62,
            27 => 30,
            28 => 14,
            29 => 6,
            30 => 2,
            31 => 2,
            32 => 1,
            _ => 0,
        };

        public static bool TryGetMaskFromTotalHosts(ulong totalHosts, out uint mask)
        {
            bool success;
            (success, mask) = totalHosts switch
            {
                4294967296 => (true, (uint)0x00000000),
                2147483648 => (true, 0x80000000),
                1073741824 => (true, 0xC0000000),
                536870912 => (true, 0xE0000000),
                268435456 => (true, 0xF0000000),
                134217728 => (true, 0xF8000000),
                67108864 => (true, 0xFC000000),
                33554432 => (true, 0xFE000000),
                16777216 => (true, 0xFF000000),
                8388608 => (true, 0xFF800000),
                4194304 => (true, 0xFFC00000),
                2097152 => (true, 0xFFE00000),
                1048576 => (true, 0xFFF00000),
                524288 => (true, 0xFFF80000),
                262144 => (true, 0xFFFC0000),
                131072 => (true, 0xFFFE0000),
                65536 => (true, 0xFFFF0000),
                32768 => (true, 0xFFFF8000),
                16384 => (true, 0xFFFFC000),
                8192 => (true, 0xFFFFE000),
                4096 => (true, 0xFFFFF000),
                2048 => (true, 0xFFFFF800),
                1024 => (true, 0xFFFFFC00),
                512 => (true, 0xFFFFFE00),
                256 => (true, 0xFFFFFF00),
                128 => (true, 0xFFFFFF80),
                64 => (true, 0xFFFFFFC0),
                32 => (true, 0xFFFFFFE0),
                16 => (true, 0xFFFFFFF0),
                8 => (true, 0xFFFFFFF8),
                4 => (true, 0xFFFFFFFC),
                2 => (true, 0xFFFFFFFE),
                1 => (true, 0xFFFFFFFF),
                _ => (false, default),
            };
            return success;
        }

        public static uint GetUsableHosts(uint value) => value switch
        {
            0x00000000 => 4294967294,
            0x80000000 => 2147483646,
            0xC0000000 => 1073741822,
            0xE0000000 => 536870910,
            0xF0000000 => 268435454,
            0xF8000000 => 134217726,
            0xFC000000 => 67108862,
            0xFE000000 => 33554430,
            0xFF000000 => 16777214,
            0xFF800000 => 8388606,
            0xFFC00000 => 4194302,
            0xFFE00000 => 2097150,
            0xFFF00000 => 1048574,
            0xFFF80000 => 524286,
            0xFFFC0000 => 262142,
            0xFFFE0000 => 131070,
            0xFFFF0000 => 65534,
            0xFFFF8000 => 32766,
            0xFFFFC000 => 16382,
            0xFFFFE000 => 8190,
            0xFFFFF000 => 4094,
            0xFFFFF800 => 2046,
            0xFFFFFC00 => 1022,
            0xFFFFFE00 => 510,
            0xFFFFFF00 => 254,
            0xFFFFFF80 => 126,
            0xFFFFFFC0 => 62,
            0xFFFFFFE0 => 30,
            0xFFFFFFF0 => 14,
            0xFFFFFFF8 => 6,
            0xFFFFFFFC => 2,
            0xFFFFFFFE => 2,
            0xFFFFFFFF => 1,
            _ => 0,
        };

        public static (bool Success, string Value) TryGetString(uint value) => value switch
        {
            0x00000000 => (true, "0.0.0.0"),
            0x80000000 => (true, "128.0.0.0"),
            0xC0000000 => (true, "192.0.0.0"),
            0xE0000000 => (true, "224.0.0.0"),
            0xF0000000 => (true, "240.0.0.0"),
            0xF8000000 => (true, "248.0.0.0"),
            0xFC000000 => (true, "252.0.0.0"),
            0xFE000000 => (true, "254.0.0.0"),
            0xFF000000 => (true, "255.0.0.0"),
            0xFF800000 => (true, "255.128.0.0"),
            0xFFC00000 => (true, "255.192.0.0"),
            0xFFE00000 => (true, "255.224.0.0"),
            0xFFF00000 => (true, "255.240.0.0"),
            0xFFF80000 => (true, "255.248.0.0"),
            0xFFFC0000 => (true, "255.252.0.0"),
            0xFFFE0000 => (true, "255.254.0.0"),
            0xFFFF0000 => (true, "255.255.0.0"),
            0xFFFF8000 => (true, "255.255.128.0"),
            0xFFFFC000 => (true, "255.255.192.0"),
            0xFFFFE000 => (true, "255.255.224.0"),
            0xFFFFF000 => (true, "255.255.240.0"),
            0xFFFFF800 => (true, "255.255.248.0"),
            0xFFFFFC00 => (true, "255.255.252.0"),
            0xFFFFFE00 => (true, "255.255.254.0"),
            0xFFFFFF00 => (true, "255.255.255.0"),
            0xFFFFFF80 => (true, "255.255.255.128"),
            0xFFFFFFC0 => (true, "255.255.255.192"),
            0xFFFFFFE0 => (true, "255.255.255.224"),
            0xFFFFFFF0 => (true, "255.255.255.240"),
            0xFFFFFFF8 => (true, "255.255.255.248"),
            0xFFFFFFFC => (true, "255.255.255.252"),
            0xFFFFFFFE => (true, "255.255.255.254"),
            0xFFFFFFFF => (true, "255.255.255.255"),
            _ => (false, string.Empty),
        };
        
        public static (bool Success, uint Value) TryGetSubnetMask(byte value) => value switch
        {
            0 => (true, (uint)0x00000000),
            1 => (true, 0x80000000),
            2 => (true, 0xC0000000),
            3 => (true, 0xE0000000),
            4 => (true, 0xF0000000),
            5 => (true, 0xF8000000),
            6 => (true, 0xFC000000),
            7 => (true, 0xFE000000),
            8 => (true, 0xFF000000),
            9 => (true, 0xFF800000),
            10 => (true, 0xFFC00000),
            11 => (true, 0xFFE00000),
            12 => (true, 0xFFF00000),
            13 => (true, 0xFFF80000),
            14 => (true, 0xFFFC0000),
            15 => (true, 0xFFFE0000),
            16 => (true, 0xFFFF0000),
            17 => (true, 0xFFFF8000),
            18 => (true, 0xFFFFC000),
            19 => (true, 0xFFFFE000),
            20 => (true, 0xFFFFF000),
            21 => (true, 0xFFFFF800),
            22 => (true, 0xFFFFFC00),
            23 => (true, 0xFFFFFE00),
            24 => (true, 0xFFFFFF00),
            25 => (true, 0xFFFFFF80),
            26 => (true, 0xFFFFFFC0),
            27 => (true, 0xFFFFFFE0),
            28 => (true, 0xFFFFFFF0),
            29 => (true, 0xFFFFFFF8),
            30 => (true, 0xFFFFFFFC),
            31 => (true, 0xFFFFFFFE),
            32 => (true, 0xFFFFFFFF),
            _ => (false, default),
        };

        public static (bool Success, byte Value) TryGetCidr(uint value) => value switch
        {
            0x00000000 => (true, (byte)0),
            0x80000000 => (true, (byte)1),
            0xC0000000 => (true, (byte)2),
            0xE0000000 => (true, (byte)3),
            0xF0000000 => (true, (byte)4),
            0xF8000000 => (true, (byte)5),
            0xFC000000 => (true, (byte)6),
            0xFE000000 => (true, (byte)7),
            0xFF000000 => (true, (byte)8),
            0xFF800000 => (true, (byte)9),
            0xFFC00000 => (true, (byte)10),
            0xFFE00000 => (true, (byte)11),
            0xFFF00000 => (true, (byte)12),
            0xFFF80000 => (true, (byte)13),
            0xFFFC0000 => (true, (byte)14),
            0xFFFE0000 => (true, (byte)15),
            0xFFFF0000 => (true, (byte)16),
            0xFFFF8000 => (true, (byte)17),
            0xFFFFC000 => (true, (byte)18),
            0xFFFFE000 => (true, (byte)19),
            0xFFFFF000 => (true, (byte)20),
            0xFFFFF800 => (true, (byte)21),
            0xFFFFFC00 => (true, (byte)22),
            0xFFFFFE00 => (true, (byte)23),
            0xFFFFFF00 => (true, (byte)24),
            0xFFFFFF80 => (true, (byte)25),
            0xFFFFFFC0 => (true, (byte)26),
            0xFFFFFFE0 => (true, (byte)27),
            0xFFFFFFF0 => (true, (byte)28),
            0xFFFFFFF8 => (true, (byte)29),
            0xFFFFFFFC => (true, (byte)30),
            0xFFFFFFFE => (true, (byte)31),
            0xFFFFFFFF => (true, (byte)32),
            _ => (false, default),
        };
        
        public static (bool Success, uint Value) TryGetSubnetMask(string? value) => value switch
        {
            "0.0.0.0" => (true, (uint)0x00000000),
            "128.0.0.0" => (true, 0x80000000),
            "192.0.0.0" => (true, 0xC0000000),
            "224.0.0.0" => (true, 0xE0000000),
            "240.0.0.0" => (true, 0xF0000000),
            "248.0.0.0" => (true, 0xF8000000),
            "252.0.0.0" => (true, 0xFC000000),
            "254.0.0.0" => (true, 0xFE000000),
            "255.0.0.0" => (true, 0xFF000000),
            "255.128.0.0" => (true, 0xFF800000),
            "255.192.0.0" => (true, 0xFFC00000),
            "255.224.0.0" => (true, 0xFFE00000),
            "255.240.0.0" => (true, 0xFFF00000),
            "255.248.0.0" => (true, 0xFFF80000),
            "255.252.0.0" => (true, 0xFFFC0000),
            "255.254.0.0" => (true, 0xFFFE0000),
            "255.255.0.0" => (true, 0xFFFF0000),
            "255.255.128.0" => (true, 0xFFFF8000),
            "255.255.192.0" => (true, 0xFFFFC000),
            "255.255.224.0" => (true, 0xFFFFE000),
            "255.255.240.0" => (true, 0xFFFFF000),
            "255.255.248.0" => (true, 0xFFFFF800),
            "255.255.252.0" => (true, 0xFFFFFC00),
            "255.255.254.0" => (true, 0xFFFFFE00),
            "255.255.255.0" => (true, 0xFFFFFF00),
            "255.255.255.128" => (true, 0xFFFFFF80),
            "255.255.255.192" => (true, 0xFFFFFFC0),
            "255.255.255.224" => (true, 0xFFFFFFE0),
            "255.255.255.240" => (true, 0xFFFFFFF0),
            "255.255.255.248" => (true, 0xFFFFFFF8),
            "255.255.255.252" => (true, 0xFFFFFFFC),
            "255.255.255.254" => (true, 0xFFFFFFFE),
            "255.255.255.255" => (true, 0xFFFFFFFF),
            _ => (false, default),
        };
    }
}