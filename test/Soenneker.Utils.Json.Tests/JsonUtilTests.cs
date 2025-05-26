using AwesomeAssertions;
using Xunit;

namespace Soenneker.Utils.Json.Tests;

public class JsonUtilTests
{
    [Fact]
    public void Format_should_indent()
    {
        const string str = "{ \"Test\": \"1\"}";
        string result = JsonUtil.Format(str, true);
        result.Should().Be("{\r\n  \"Test\": \"1\"\r\n}");
    }
}