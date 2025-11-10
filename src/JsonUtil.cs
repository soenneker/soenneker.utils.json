using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Soenneker.Enums.JsonLibrary;
using Soenneker.Enums.JsonOptions;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Json.OptionsCollection;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Soenneker.Utils.Json;

/// <summary>
/// A utility library handling (de)serialization and other useful JSON functionalities
/// </summary>
public static class JsonUtil
{
    /// <summary>
    /// Uses WebOptions as default
    /// </summary>
    [Pure]
    public static T? Deserialize<T>(string str, JsonLibraryType? libraryType = null)
    {
        T? obj;

        if (libraryType is null || libraryType == JsonLibraryType.SystemTextJson)
            obj = JsonSerializer.Deserialize<T>(str, JsonOptionsCollection.WebOptions);
        else
            obj = JsonConvert.DeserializeObject<T>(str, JsonOptionsCollection.Newtonsoft);

        return obj;
    }

    /// <summary>
    /// Uses WebOptions as default
    /// </summary>
    [Pure]
    public static T? Deserialize<T>(Stream stream, JsonLibraryType? libraryType = null)
    {
        T? obj;

        if (libraryType is null || libraryType == JsonLibraryType.SystemTextJson)
            obj = JsonSerializer.Deserialize<T>(stream, JsonOptionsCollection.WebOptions);
        else
            obj = DeserializeViaNewtonsoft<T>(stream, JsonOptionsCollection.Newtonsoft);

        return obj;
    }

    /// <summary>
    /// Uses WebOptions as default. Only uses System.Text.Json
    /// </summary>
    [Pure]
    public static T? Deserialize<T>(Span<byte> byteSpan)
    {
        return JsonSerializer.Deserialize<T>(byteSpan, JsonOptionsCollection.WebOptions);
    }

    /// <summary>
    /// Uses WebOptions as default. Only uses System.Text.Json. Avoids string allocation. Wraps in a Try catch to log.
    /// </summary>
    [Pure]
    public static async ValueTask<T?> Deserialize<T>(HttpResponseMessage response, ILogger? logger = null, CancellationToken cancellationToken = default)
    {
        try
        {
            await using Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).NoSync();
            return await JsonSerializer.DeserializeAsync<T>(contentStream, JsonOptionsCollection.WebOptions, cancellationToken).NoSync();
        }
        catch (Exception e)
        {
            logger?.LogError(e, "Failed to deserialize response content");
            return default;
        }
    }

    /// <summary>
    /// Uses WebOptions as default. Only uses System.Text.Json. Avoids string allocation. Wraps in a Try catch to log.
    /// </summary>
    [Pure]
    public static async ValueTask<T?> Deserialize<T>(Stream stream, ILogger? logger = null, CancellationToken cancellationToken = default)
    {
        try
        {
            return await JsonSerializer.DeserializeAsync<T>(stream, JsonOptionsCollection.WebOptions, cancellationToken).NoSync();
        }
        catch (Exception e)
        {
            logger?.LogError(e, "Failed to deserialize response content");
            return default;
        }
    }

    /// <summary>
    /// Uses WebOptions as default
    /// </summary>
    [Pure]
    public static object? Deserialize(string str, Type type, JsonLibraryType? libraryType = null)
    {
        object? obj;

        if (libraryType is null || libraryType == JsonLibraryType.SystemTextJson)
            obj = JsonSerializer.Deserialize(str, type, JsonOptionsCollection.WebOptions);
        else
            obj = JsonConvert.DeserializeObject(str, JsonOptionsCollection.Newtonsoft);

        return obj;
    }

    /// <summary>
    /// Uses WebOptions as default
    /// </summary>
    [Pure]
    public static object? Deserialize(Stream stream, Type type, JsonLibraryType? libraryType = null)
    {
        object? obj;

        if (libraryType is null || libraryType == JsonLibraryType.SystemTextJson)
            obj = JsonSerializer.Deserialize(stream, type, JsonOptionsCollection.WebOptions);
        else
            obj = DeserializeViaNewtonsoft(stream, JsonOptionsCollection.Newtonsoft);

        return obj;
    }

    /// <summary>
    /// Accepts a nullable object... if null returns null. If type is left null, will use WebOptions
    /// </summary>
    [Pure]
    public static string? Serialize(object? obj, JsonOptionType? optionType = null, JsonLibraryType? libraryType = null)
    {
        if (obj is null)
            return null;

        JsonSerializerOptions options = JsonOptionsCollection.GetOptionsFromType(optionType);

        string str;

        if (libraryType is null || libraryType == JsonLibraryType.SystemTextJson)
            str = JsonSerializer.Serialize(obj, options);
        else
            str = JsonConvert.SerializeObject(obj, JsonOptionsCollection.Newtonsoft);

        return str;
    }

    [Pure]
    public static JsonElement? SerializeToElement(object? obj, JsonOptionType? optionType = null)
    {
        if (obj is null)
            return null;

        JsonSerializerOptions options = JsonOptionsCollection.GetOptionsFromType(optionType);

        return JsonSerializer.SerializeToElement(obj, options);
    }

    /// <summary>
    /// Serializes the stream input
    /// </summary>
    [Pure]
    public static string? Serialize(Stream stream, object? obj, JsonOptionType? optionType = null, JsonLibraryType? libraryType = null)
    {
        if (obj is null)
            return null;

        JsonSerializerOptions options = JsonOptionsCollection.GetOptionsFromType(optionType);

        string str;

        if (libraryType is null || libraryType == JsonLibraryType.SystemTextJson)
            str = JsonSerializer.Serialize(stream, options);
        else
            str = JsonConvert.SerializeObject(obj, JsonOptionsCollection.Newtonsoft);

        return str;
    }

    /// <summary>
    /// Serializes the object into the given stream
    /// </summary>
    public static Task SerializeToStream(Stream stream, object? obj, JsonOptionType? optionType = null, CancellationToken cancellationToken = default)
    {
        JsonSerializerOptions options = JsonOptionsCollection.GetOptionsFromType(optionType);

        return JsonSerializer.SerializeAsync(stream, obj, options, cancellationToken);
    }

    /// <summary>
    /// Serializes an object to a UTF-8 encoded byte array using <see cref="System.Text.Json"/>.
    /// </summary>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="optionType"></param>
    /// <returns>A byte array containing the UTF-8 encoded JSON representation of the object.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the object is <c>null</c>.</exception>
    public static byte[] SerializeToUtf8Bytes(object obj, JsonOptionType? optionType = null)
    {
        JsonSerializerOptions options = JsonOptionsCollection.GetOptionsFromType(optionType);

        return JsonSerializer.SerializeToUtf8Bytes(obj, options);
    }

    public static async ValueTask<T?> DeserializeFromFile<T>(string path, ILogger? logger = null, CancellationToken cancellationToken = default)
    {
        await using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 8192, useAsync: true);
        return await Deserialize<T>(fileStream, logger, cancellationToken).NoSync();
    }

    public static async ValueTask SerializeToFile(object? obj, string path, JsonOptionType? optionType = null, JsonLibraryType? libraryType = null, CancellationToken cancellationToken = default)
    {
        if (obj is null)
            return;

        await using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 8192, useAsync: true);
        await SerializeToStream(fileStream, obj, optionType, cancellationToken).NoSync();
    }

    public static bool TryDeserialize<T>(ReadOnlySpan<byte> utf8Json, out T? value, JsonTypeInfo<T>? typeInfo = null)
    {
        if (utf8Json.Length == 0)
        {
            value = default;
            return false;
        }

        value = typeInfo is null ? JsonSerializer.Deserialize<T>(utf8Json) : JsonSerializer.Deserialize(utf8Json, typeInfo);

        return value is not null;
    }

    public static bool IsJsonValid(string str, ILogger? logger = null)
    {
        var result = false;
        JsonDocument? document = null;

        try
        {
            document = JsonDocument.Parse(str);

            result = true;
        }
        catch
        {
            logger?.LogWarning("JSON is invalid");
            // ignored
        }

        document?.Dispose();

        return result;
    }

    private static void SerializeViaNewtonsoft(object value, Stream stream, JsonSerializerSettings? jsonSerializerSettings)
    {
        using var writer = new StreamWriter(stream);
        using var jsonWriter = new JsonTextWriter(writer);
        var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);
        serializer.Serialize(jsonWriter, value);
    }

    private static T? DeserializeViaNewtonsoft<T>(Stream stream, JsonSerializerSettings? jsonSerializerSettings)
    {
        using var reader = new StreamReader(stream);
        using var jsonReader = new JsonTextReader(reader);
        var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);
        return serializer.Deserialize<T>(jsonReader);
    }

    private static object? DeserializeViaNewtonsoft(Stream stream, JsonSerializerSettings? jsonSerializerSettings)
    {
        using var reader = new StreamReader(stream);
        using var jsonReader = new JsonTextReader(reader);
        var serializer = Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings);
        return serializer.Deserialize<object?>(jsonReader);
    }

    public static string Format(string json, bool forceWindowsLineEndings)
    {
        using (JsonDocument jDoc = JsonDocument.Parse(json))
        {
            string result = JsonSerializer.Serialize(jDoc, JsonOptionsCollection.PrettySafeOptions);

            if (OperatingSystem.IsWindows())
                return result;

            if (forceWindowsLineEndings)
                result = result.Replace(Environment.NewLine, "\r\n");

            return result;
        }
    }
}