﻿using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Soenneker.Enums.JsonLibrary;
using Soenneker.Enums.JsonOptions;

namespace Soenneker.Utils.Json.Abstract;

/// <summary>
/// A utility library handling (de)serialization and other useful JSON functions <para/>
/// Should be used as a Singleton IoC if not using static methods
/// </summary>
public interface IJsonUtil
{
    [Pure]
    ValueTask<T?> ReadJsonFromFile<T>(string path, JsonLibraryType? libraryType = null);

    Task SerializeAndWriteToFile(object? obj, string path, JsonOptionType? optionType = null, JsonLibraryType? libraryType = null);

    [Pure]
    bool IsJsonValid(string str);
}