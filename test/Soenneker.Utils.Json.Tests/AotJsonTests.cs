using System;
using AwesomeAssertions;

namespace Soenneker.Utils.Json.Tests;

public class AotJsonTests
{
    [Test]
    public void Metadata_round_trip_and_try_deserialization()
    {
        byte[] json = JsonUtil.SerializeToUtf8Bytes(42, AotJsonContext.Default.Int32);
        JsonUtil.Deserialize(json.AsSpan(), AotJsonContext.Default.Int32).Should().Be(42);
        JsonUtil.TryDeserialize(json, out int value, AotJsonContext.Default.Int32).Should().BeTrue();
        value.Should().Be(42);
        JsonUtil.TryDeserialize("invalid"u8, out int _, AotJsonContext.Default.Int32).Should().BeFalse();
        JsonUtil.TryDeserialize(ReadOnlySpan<byte>.Empty, out int _, AotJsonContext.Default.Int32).Should().BeFalse();
    }

    [Test]
    public void Formatting_preserves_names_and_safe_escaping()
    {
        JsonUtil.Format("{\"Name\":\"<tag>\",\"Nothing\":null}", true)
            .Should().Be("{\r\n  \"Name\": \"\\u003Ctag\\u003E\",\r\n  \"Nothing\": null\r\n}");
    }

    [Test]
    public void Reflection_try_deserialization_remains_available()
    {
        JsonUtil.TryDeserialize("42"u8, out int value).Should().BeTrue();
        value.Should().Be(42);
        JsonUtil.TryDeserialize("invalid"u8, out int _).Should().BeFalse();
    }
}
