using FluentAssertions;
using Xunit;

namespace Soenneker.Utils.Json.Tests;

public class JsonUtilTests
{
    [Fact]
    public void Format_should_indent()
    {
        var str = "{ \"Test\": \"1\"}";
        string result = JsonUtil.Format(str);
        result.Should().Be("{\r\n  \"Test\": \"1\"\r\n}");
    }
}