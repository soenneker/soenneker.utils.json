using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Soenneker.Utils.File.Abstract;
using Soenneker.Utils.Json.Abstract;
using Soenneker.Utils.Json.Enums;
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

    public T? Deserialize<T>(string str, JsonLibraryType? libraryType = null)
    {
        T? obj;

        if (libraryType == null || libraryType == JsonLibraryType.SystemTextJson)
            obj = JsonSerializer.Deserialize<T>(str, JsonOptionsLibrary.WebOptions);
        else
            obj = JsonConvert.DeserializeObject<T>(str, JsonOptionsLibrary.Newtonsoft);

        return obj;
    }

    public object? Deserialize(string str, Type type, JsonLibraryType? libraryType = null)
    {
        object? obj;

        if (libraryType == null || libraryType == JsonLibraryType.SystemTextJson)
            obj = JsonSerializer.Deserialize(str, type, JsonOptionsLibrary.WebOptions);
        else
            obj = JsonConvert.DeserializeObject(str, JsonOptionsLibrary.Newtonsoft);

        return obj;
    }

    public string? Serialize(object? obj, JsonOptionType? optionType = null, JsonLibraryType? libraryType = null)
    {
        if (obj is null)
            return null;

        JsonSerializerOptions options = JsonOptionsLibrary.GetOptionsFromType(optionType);

        string str;
        if (libraryType == null || libraryType == JsonLibraryType.SystemTextJson)
            str = JsonSerializer.Serialize(obj, options);
        else
            str = JsonConvert.SerializeObject(obj, JsonOptionsLibrary.Newtonsoft);

        return str;
    }
    
    public async ValueTask<T?> ReadJsonFromFile<T>(string path, JsonLibraryType? libraryType = null)
    {
        string content = await _fileUtil.ReadFile(path);
        var result = Deserialize<T>(content, libraryType);
        return result;
    }

    public async ValueTask SerializeAndWriteToFile(object? obj, string path, JsonOptionType? optionType = null, JsonLibraryType? libraryType = null)
    {
        if (obj is null)
            return;

        string content = Serialize(obj, optionType, libraryType)!;

        await _fileUtil.WriteFile(path, content);
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
}