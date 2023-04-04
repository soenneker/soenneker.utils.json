using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Utils.File.Registrars;
using Soenneker.Utils.Json.Abstract;
using Soenneker.Utils.File.Abstract;

namespace Soenneker.Utils.Json.Registrars;

/// <summary>
///  A utility library handling (de)serialization and other useful JSON functions
/// </summary>
public static class JsonUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IJsonUtil"/> as a scoped service. Also adds <see cref="IFileUtil"/> as Scoped (if it's not currently registered).<para/>
    /// Shorthand for <code>services.TryAddScoped</code> <para/>
    /// </summary>
    public static void AddJsonUtilAsScoped(this IServiceCollection services)
    {
        services.AddFileUtilAsScoped();
        services.TryAddScoped<IJsonUtil, JsonUtil>();
    }

    /// <summary>
    /// Adds <see cref="IJsonUtil"/> as a singleton service. (Recommended). Also adds <see cref="IFileUtil"/> as a Singleton (if it's not currently registered).<para/>
    /// Shorthand for <code>services.TryAddSingleton</code> <para/>
    /// </summary>
    public static void AddJsonUtilAsSingleton(this IServiceCollection services)
    {
        services.AddFileUtilAsSingleton();
        services.TryAddSingleton<IJsonUtil, JsonUtil>();
    }
}