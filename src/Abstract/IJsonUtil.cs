using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Soenneker.Enums.JsonLibrary;
using Soenneker.Enums.JsonOptions;

namespace Soenneker.Utils.Json.Abstract;

/// <summary>
/// A utility library handling (de)serialization and other useful JSON functions <para/>
/// Should be used as a Singleton IoC if not using static methods
/// </summary>
public interface IJsonUtil
{
    ValueTask SerializeToFile(object? obj, string path, JsonOptionType? optionType = null, JsonLibraryType? libraryType = null, CancellationToken cancellationToken = default);

    [Pure]
    ValueTask<T?> DeserializeFromFile<T>(string path, ILogger? logger = null, CancellationToken cancellationToken = default);

    [Pure]
    bool IsJsonValid(string str);
}