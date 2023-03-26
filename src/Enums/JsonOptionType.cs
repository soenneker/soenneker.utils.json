using Ardalis.SmartEnum;

namespace Soenneker.Utils.Json.Enums;

public sealed class JsonOptionType : SmartEnum<JsonOptionType>
{
    /// <summary>
    /// Web defaults, non-strict
    /// </summary>
    public static readonly JsonOptionType Web = new(nameof(Web), 0);

    /// <summary>
    /// Non-camel case
    /// </summary>
    public static readonly JsonOptionType General = new(nameof(General), 1);

    /// <summary>
    /// Non-camel case with indentation WITHOUT escaping. WARNING Dangerous! Do not use unless for internal uses!
    /// </summary>
    /// <remarks>https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-character-encoding</remarks>
    public static readonly JsonOptionType Pretty = new(nameof(Pretty), 2);

    /// <summary>
    /// Non-camel case with indentation, WITH escaping. Safe for output.
    /// </summary>
    /// <remarks>https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-character-encoding</remarks>
    public static readonly JsonOptionType PrettySafe = new(nameof(Pretty), 3);

    private JsonOptionType(string name, int value) : base(name, value)
    {
    }
}