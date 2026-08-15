using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UserManagementSystem.Helper
{
    public class DateOnlyConverter : JsonConverter<DateOnly?>
    {

        /// <summary>
        ///     We will use this method to convert JSON - > C#
        /// </summary>
        /// <param name="reader">Actual value</param>
        /// <param name="typeToConvert">It tells us which C# type we need to convert it into. i.e. DateOnly?</param>

        public override DateOnly? Read(ref Utf8JsonReader reader,Type typeToConvert, JsonSerializerOptions options)
        {
            if (string.IsNullOrWhiteSpace(reader.GetString()))
                return null;

            // Only yyyy-MM-dd is accepted
            if (DateOnly.TryParseExact(
                    reader.GetString(),
                    "yyyy-MM-dd",
                    out var date))
            {
                return date;
            }

            return null;
        }

        /// <summary>
        ///     We wil use this method to convert C# -> Json
        /// </summary>
        /// <param name="writer">will help us to write JSON</param>
        /// <param name="typeToConvert">Actual value that would convert into json</param>


        public override void Write(
            Utf8JsonWriter writer,
            DateOnly? value,
            JsonSerializerOptions options
            )
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd"));
            else
                writer.WriteNullValue();
        }
    }
}
