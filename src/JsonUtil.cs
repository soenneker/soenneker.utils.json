using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Soenneker.Enums.JsonLibrary;
using Soenneker.Enums.JsonOptions;
using Soenneker.Extensions.Task;
using Soenneker.Json.OptionsCollection;
using Soenneker.Utils.File.Abstract;
using Soenneker.Utils.Json.Abstract;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Soenneker.Utils.Json;

// TODO: Build option switch method

///<inheritdoc cref="IJsonUtil"/>
public class JsonUtil : IJsonUtil
{
    private readonly IFileUtil _fileUtil;
    private readonly ILogger<JsonUtil> _logger;

    public JsonUtil(IFileUtil fileUtil, ILogger<JsonUtil> logger)
    {
        _logger = logger;
        _fileUtil = fileUtil;
    }

    /// <summary>
    /// Uses WebOptions as default
    /// </summary>
    [Pure]
    public static T? Deserialize<T>(string str, JsonLibraryType? libraryType = null)
    {
        T? obj;

        if (libraryType is null or JsonLibraryType.SystemTextJson)
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

        if (libraryType is null or JsonLibraryType.SystemTextJson)
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
        var obj = JsonSerializer.Deserialize<T>(byteSpan, JsonOptionsCollection.WebOptions);

        return obj;
    }

    /// <summary>
    /// Uses WebOptions as default
    /// </summary>
    [Pure]
    public static object? Deserialize(string str, Type type, JsonLibraryType? libraryType = null)
    {
        object? obj;

        if (libraryType is null or JsonLibraryType.SystemTextJson)
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

        if (libraryType is null or JsonLibraryType.SystemTextJson)
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

        if (libraryType is null or JsonLibraryType.SystemTextJson)
            str = JsonSerializer.Serialize(obj, options);
        else
            str = JsonConvert.SerializeObject(obj, JsonOptionsCollection.Newtonsoft);

        return str;
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

        if (libraryType is null or JsonLibraryType.SystemTextJson)
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

    public async ValueTask<T?> ReadJsonFromFile<T>(string path, JsonLibraryType? libraryType = null, CancellationToken cancellationToken = default)
    {
        string content = await _fileUtil.ReadFile(path, cancellationToken).NoSync();
        var result = Deserialize<T>(content, libraryType);
        return result;
    }

    public Task SerializeAndWriteToFile(object? obj, string path, JsonOptionType? optionType = null, JsonLibraryType? libraryType = null, CancellationToken cancellationToken = default)
    {
        if (obj is null)
            return Task.CompletedTask;

        string content = Serialize(obj, optionType, libraryType)!;

        return _fileUtil.Write(path, content, cancellationToken);
    }

    public bool IsJsonValid(string str)
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
            _logger.LogWarning("JSON is invalid");
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
            string result = JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });

            if (OperatingSystem.IsWindows())
                return result;

            if (forceWindowsLineEndings)
                result = result.Replace(Environment.NewLine, "\r\n");

            return result;
        }
    }
}