#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetworkPrimitives.Ipv6;

namespace NetworkPrimitives.JsonConverters.Ipv6
{
    public class Ipv6SubnetMaskJsonConverter : JsonConverter<Ipv6SubnetMask>
    {
        private Ipv6SubnetMaskJsonConverter() { }
        public static readonly Ipv6SubnetMaskJsonConverter Instance = new ();
        public override Ipv6SubnetMask Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) => Ipv6SubnetMask.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            Ipv6SubnetMask value,
            JsonSerializerOptions options
        ) => writer.WriteStringValue(value.ToString());
    }
}