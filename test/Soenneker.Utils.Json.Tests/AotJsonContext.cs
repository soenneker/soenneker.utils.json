using System.Text.Json.Serialization;

namespace Soenneker.Utils.Json.Tests;

[JsonSerializable(typeof(int))]
internal partial class AotJsonContext : JsonSerializerContext
{
}
