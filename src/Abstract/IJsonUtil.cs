using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Soenneker.Utils.Json.Enums;

namespace Soenneker.Utils.Json.Abstract;

/// <summary>
/// A utility library handling (de)serialization and other useful JSON functions <para/>
/// Should be used as a Singleton IoC
/// </summary>
public interface IJsonUtil
{
    /// <summary>
    /// Uses WebOptions as default
    /// </summary>
    [Pure]
    T? Deserialize<T>(string str, JsonLibraryType? libraryType = null);

    /// <summary>
    /// Uses WebOptions as default
    /// </summary>
    [Pure]
    object? Deserialize(string str, Type type, JsonLibraryType? libraryType = null);

    /// <summary>
    /// Accepts a nullable object... if null returns null. If type is left null, will use WebOptions
    /// </summary>
    [Pure]
    string? Serialize(object? obj, JsonOptionType? optionType = null, JsonLibraryType? libraryType = null);

    [Pure]
    ValueTask<T?> ReadJsonFromFile<T>(string path, JsonLibraryType? libraryType = null);

    ValueTask SerializeAndWriteToFile(object? obj, string path, JsonOptionType? optionType = null, JsonLibraryType? libraryType = null);

    [Pure]
    bool IsJsonValid(string str);
}