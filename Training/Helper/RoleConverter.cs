using System.Text.Json;
using System.Text.Json.Serialization;

public class RolesConverter : JsonConverter<List<string>?>
{
    // JSON -> C#
    public override List<string>? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        if (reader.TokenType != JsonTokenType.StartArray)
            return null;

        var roles = new List<string>();
        bool invalidRole = false;

        while (reader.Read())
        {
            // Array finished
            if (reader.TokenType == JsonTokenType.EndArray)
                break;

            // null, number, boolean etc.
            if (reader.TokenType != JsonTokenType.String)
            {
                invalidRole = true;
                continue;
            }

            var role = reader.GetString();

            // Empty / whitespace
            if (string.IsNullOrWhiteSpace(role))
            {
                invalidRole = true;
                continue;
            }

            roles.Add(role);
        }

        // Any invalid value -> whole Roles becomes null
        if (invalidRole)
            return null;

        return roles.Count > 0 ? roles : null;
    }

    // C# -> JSON
    public override void Write(
        Utf8JsonWriter writer,
        List<string>? value,
        JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();

        foreach (var role in value)
        {
            writer.WriteStringValue(role);
        }

        writer.WriteEndArray();
    }
}