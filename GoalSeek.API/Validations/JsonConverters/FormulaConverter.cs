using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoalSeek.API.Validations.JsonConverters
{
    public class FormulaConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value= reader.GetString().Replace(" ", "").ToLower().Replace("*input", "");
            return value;
        }


        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }

    }
}
