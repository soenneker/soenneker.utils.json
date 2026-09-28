using System.Text.Json.Serialization;

namespace Soenneker.Utils.Json.Tests;

[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(string))]
internal partial class AotJsonContext : JsonSerializerContext
{
}
