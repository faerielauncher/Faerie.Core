using System.Text.Json;
using System.Text.Json.Serialization;

namespace Faerie.Core.Templates
{

    public class ArgumentRuleConverter : JsonConverter<object>
    {
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString()!,
                JsonTokenType.StartObject => JsonSerializer.Deserialize<RulesTemplate>(ref reader),
                JsonTokenType.StartArray => JsonSerializer.Deserialize<List<string>>(ref reader),
                _ => new JsonException(),
            };
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}

