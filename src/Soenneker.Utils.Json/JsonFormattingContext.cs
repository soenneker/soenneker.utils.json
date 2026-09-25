using System.Text.Json;
using System.Text.Json.Serialization;

namespace Soenneker.Utils.Json;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(JsonElement))]
internal partial class JsonFormattingContext : JsonSerializerContext
{
}
