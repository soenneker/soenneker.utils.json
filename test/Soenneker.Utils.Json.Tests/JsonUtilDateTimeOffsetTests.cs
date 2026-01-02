using AwesomeAssertions;
using Soenneker.Enums.JsonLibrary;
using System;
using Xunit;

namespace Soenneker.Utils.Json.Tests;

public class JsonUtilDateTimeOffsetTests
{
    [Fact]
    public void Serialize_DateTimeOffset_UTC_with_SystemTextJson()
    {
        var dateTimeOffset = new DateTimeOffset(2024, 1, 15, 10, 30, 45, TimeSpan.Zero);
        string? result = JsonUtil.Serialize(dateTimeOffset);
        
        // System.Text.Json serializes DateTimeOffset as ISO 8601 string
        result.Should().NotBeNull();
        result!.Should().Contain("2024-01-15T10:30:45");
        result.Should().Contain("+00:00");
    }

    [Fact]
    public void Serialize_DateTimeOffset_with_timezone_offset_with_SystemTextJson()
    {
        var dateTimeOffset = new DateTimeOffset(2024, 1, 15, 10, 30, 45, TimeSpan.FromHours(-5));
        string? result = JsonUtil.Serialize(dateTimeOffset);
        
        // System.Text.Json serializes DateTimeOffset with timezone offset
        result.Should().NotBeNull();
        result!.Should().Contain("2024-01-15T10:30:45");
        result.Should().Contain("-05:00");
    }

    [Fact]
    public void Serialize_DateTimeOffset_UTC_with_Newtonsoft()
    {
        var dateTimeOffset = new DateTimeOffset(2024, 1, 15, 10, 30, 45, TimeSpan.Zero);
        string? result = JsonUtil.Serialize(dateTimeOffset, libraryType: JsonLibraryType.Newtonsoft);
        
        // Newtonsoft.Json serializes DateTimeOffset as ISO 8601 string
        result.Should().NotBeNull();
        result!.Should().Contain("2024-01-15T10:30:45");
        result.Should().Contain("+00:00");
    }

    [Fact]
    public void Serialize_DateTimeOffset_with_timezone_offset_with_Newtonsoft()
    {
        var dateTimeOffset = new DateTimeOffset(2024, 1, 15, 10, 30, 45, TimeSpan.FromHours(3));
        string? result = JsonUtil.Serialize(dateTimeOffset, libraryType: JsonLibraryType.Newtonsoft);
        
        // Newtonsoft.Json serializes DateTimeOffset with timezone offset
        result.Should().NotBeNull();
        result!.Should().Contain("2024-01-15T10:30:45");
        result.Should().Contain("+03:00");
    }

    [Fact]
    public void Serialize_DateTimeOffset_in_object_with_SystemTextJson()
    {
        var testObject = new { Date = new DateTimeOffset(2024, 1, 15, 10, 30, 45, TimeSpan.Zero) };
        string? result = JsonUtil.Serialize(testObject);
        
        result.Should().NotBeNull();
        // System.Text.Json with WebOptions uses camelCase
        result!.Should().Contain("\"date\"");
        result.Should().Contain("2024-01-15T10:30:45");
        result.Should().Contain("+00:00");
    }

    [Fact]
    public void Serialize_DateTimeOffset_in_object_with_Newtonsoft()
    {
        var testObject = new { Date = new DateTimeOffset(2024, 1, 15, 10, 30, 45, TimeSpan.FromHours(-8)) };
        string? result = JsonUtil.Serialize(testObject, libraryType: JsonLibraryType.Newtonsoft);
        
        result.Should().NotBeNull();
        // Newtonsoft.Json uses PascalCase by default
        result!.Should().Contain("\"Date\"");
        result.Should().Contain("2024-01-15T10:30:45");
        result.Should().Contain("-08:00");
    }

    [Fact]
    public void Round_trip_DateTimeOffset_serialization_with_SystemTextJson()
    {
        var original = new DateTimeOffset(2024, 1, 15, 10, 30, 45, 123, TimeSpan.FromHours(5));
        string? json = JsonUtil.Serialize(original);
        json.Should().NotBeNull();
        var deserialized = JsonUtil.Deserialize<DateTimeOffset>(json!);
        
        deserialized.Should().Be(original);
    }

    [Fact]
    public void Round_trip_DateTimeOffset_serialization_with_Newtonsoft()
    {
        var original = new DateTimeOffset(2024, 1, 15, 10, 30, 45, 123, TimeSpan.FromHours(-7));
        string? json = JsonUtil.Serialize(original, libraryType: JsonLibraryType.Newtonsoft);
        json.Should().NotBeNull();
        var deserialized = JsonUtil.Deserialize<DateTimeOffset>(json!, libraryType: JsonLibraryType.Newtonsoft);
        
        deserialized.Should().Be(original);
    }

    [Fact]
    public void Serialize_DateTimeOffset_MinValue()
    {
        var dateTimeOffset = DateTimeOffset.MinValue;
        string? result = JsonUtil.Serialize(dateTimeOffset);
        
        result.Should().NotBeNullOrEmpty();
        var deserialized = JsonUtil.Deserialize<DateTimeOffset>(result!);
        deserialized.Should().Be(dateTimeOffset);
    }

    [Fact]
    public void Serialize_DateTimeOffset_MaxValue()
    {
        var dateTimeOffset = DateTimeOffset.MaxValue;
        string? result = JsonUtil.Serialize(dateTimeOffset);
        
        result.Should().NotBeNullOrEmpty();
        var deserialized = JsonUtil.Deserialize<DateTimeOffset>(result!);
        deserialized.Should().Be(dateTimeOffset);
    }

    [Fact]
    public void Serialize_DateTimeOffset_with_milliseconds()
    {
        var dateTimeOffset = new DateTimeOffset(2024, 1, 15, 10, 30, 45, 789, TimeSpan.Zero);
        string? result = JsonUtil.Serialize(dateTimeOffset);
        
        result.Should().NotBeNull();
        result!.Should().Contain("2024-01-15T10:30:45.789");
        var deserialized = JsonUtil.Deserialize<DateTimeOffset>(result);
        deserialized.Should().Be(dateTimeOffset);
    }

    [Fact]
    public void Serialize_DateTimeOffset_UTC_uses_plus_00_00_not_Z()
    {
        var dateTimeOffset = new DateTimeOffset(2024, 1, 15, 10, 30, 45, TimeSpan.Zero);
        string? result = JsonUtil.Serialize(dateTimeOffset);
        
        // Default behavior: System.Text.Json uses "+00:00" for UTC, not "Z"
        result.Should().NotBeNull();
        result!.Should().Contain("+00:00");
        result.Should().NotContain("Z");
        result.Should().NotContain("\"Z\"");
    }

    [Fact]
    public void Deserialize_DateTimeOffset_with_Z_format()
    {
        // Even though serialization produces "+00:00", we can deserialize "Z" format
        const string jsonWithZ = "\"2024-01-15T10:30:45Z\"";
        var deserialized = JsonUtil.Deserialize<DateTimeOffset>(jsonWithZ);
        
        deserialized.Should().Be(new DateTimeOffset(2024, 1, 15, 10, 30, 45, TimeSpan.Zero));
    }

    [Fact]
    public void Deserialize_DateTimeOffset_with_Z_format_and_milliseconds()
    {
        const string jsonWithZ = "\"2024-01-15T10:30:45.789Z\"";
        var deserialized = JsonUtil.Deserialize<DateTimeOffset>(jsonWithZ);
        
        deserialized.Should().Be(new DateTimeOffset(2024, 1, 15, 10, 30, 45, 789, TimeSpan.Zero));
    }

    [Fact]
    public void Round_trip_DateTimeOffset_Z_format_compatibility()
    {
        // Test that we can deserialize "Z" format even though we serialize as "+00:00"
        var original = new DateTimeOffset(2024, 1, 15, 10, 30, 45, TimeSpan.Zero);
        
        // Serialize (produces "+00:00")
        string? serialized = JsonUtil.Serialize(original);
        serialized.Should().NotBeNull();
        serialized!.Should().Contain("+00:00");
        
        // But we can also deserialize "Z" format
        const string jsonWithZ = "\"2024-01-15T10:30:45Z\"";
        var fromZ = JsonUtil.Deserialize<DateTimeOffset>(jsonWithZ);
        fromZ.Should().Be(original);
        
        // And deserialize "+00:00" format
        var fromPlus00 = JsonUtil.Deserialize<DateTimeOffset>(serialized);
        fromPlus00.Should().Be(original);
    }

    [Fact]
    public void Serialize_DateTimeOffset_UTC_with_Newtonsoft_uses_plus_00_00_not_Z()
    {
        var dateTimeOffset = new DateTimeOffset(2024, 1, 15, 10, 30, 45, TimeSpan.Zero);
        string? result = JsonUtil.Serialize(dateTimeOffset, libraryType: JsonLibraryType.Newtonsoft);
        
        // Newtonsoft.Json also uses "+00:00" for UTC by default, not "Z"
        result.Should().NotBeNull();
        result!.Should().Contain("+00:00");
        result.Should().NotContain("Z");
        result.Should().NotContain("\"Z\"");
    }
}

