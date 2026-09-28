using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using AwesomeAssertions;

namespace Soenneker.Utils.Json.Tests;

public class SerializeToElementMetadataTests
{
    [Test]
    public void Serializes_values_using_generated_metadata()
    {
        JsonUtil.SerializeToElement(42, AotJsonContext.Default.Int32).GetInt32().Should().Be(42);
        JsonUtil.SerializeToElement("a & b", AotJsonContext.Default.String).GetString().Should().Be("a & b");
    }

    [Test]
    public void Null_value_produces_json_null()
    {
        JsonUtil.SerializeToElement((string?)null, AotJsonContext.Default.String).ValueKind.Should().Be(JsonValueKind.Null);
    }

    [Test]
    public void Null_metadata_is_rejected()
    {
        Action action = () => JsonUtil.SerializeToElement(42, (JsonTypeInfo<int>)null!);
        action.Should().Throw<ArgumentNullException>();
    }
}
