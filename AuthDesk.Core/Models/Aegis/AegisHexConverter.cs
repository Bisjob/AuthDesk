using Newtonsoft.Json;
using AuthDesk.Core.Tools;

public class HexConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(byte[]);
    }

    public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
    {
        return Hex.ToBytes(reader.Value.ToString());
    }

    public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
    {
        var bytes = (byte[]) value;
        writer.WriteValue(Hex.FromBytes(bytes));
    }
}
