using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Utils.File.Registrars;
using Soenneker.Utils.Json.Abstract;
using System.IO;

namespace Soenneker.Utils.Json.Registrars;

public static class JsonUtilRegistrar
{
    /// <summary>
    /// Adds IJsonUtil as a scoped service. Also adds IFileUtil as Scoped (if it's not currently registered).<para/>
    /// Shorthand for <code>services.TryAddScoped</code> <para/>
    /// </summary>
    public static void AddJsonUtilAsScoped(this IServiceCollection services)
    {
        services.AddFileUtilAsScoped();
        services.TryAddScoped<IJsonUtil, JsonUtil>();
    }

    /// <summary>
    /// Adds IJsonUtil as a singleton service. (Recommended). Also adds IFileUtil as a Singleton (if it's not currently registered).<para/>
    /// Shorthand for <code>services.TryAddSingleton</code> <para/>
    /// </summary>
    public static void AddJsonUtilAsSingleton(this IServiceCollection services)
    {
        services.AddFileUtilAsSingleton();
        services.TryAddSingleton<IJsonUtil, JsonUtil>();
    }
}