using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Utils.Json.Abstract;

namespace Soenneker.Utils.Json.Registrars;

/// <summary>
///  A utility library handling (de)serialization and other useful JSON functions
/// </summary>
public static class JsonUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IJsonUtil"/> as a scoped service.<para/>
    /// Shorthand for <code>services.TryAddScoped</code> <para/>
    /// </summary>
    public static IServiceCollection AddJsonUtilAsScoped(this IServiceCollection services)
    {
        services.TryAddScoped<IJsonUtil, JsonUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IJsonUtil"/> as a singleton service. (Recommended).<para/>
    /// Shorthand for <code>services.TryAddSingleton</code> <para/>
    /// </summary>
    public static IServiceCollection AddJsonUtilAsSingleton(this IServiceCollection services)
    {
        services.TryAddSingleton<IJsonUtil, JsonUtil>();

        return services;
    }
}