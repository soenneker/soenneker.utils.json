using Ardalis.SmartEnum;
using Newtonsoft.Json;

namespace Soenneker.Utils.Json.Enums;

public sealed class JsonLibraryType : SmartEnum<JsonLibraryType>
{
    public static readonly JsonLibraryType SystemTextJson = new(nameof(SystemTextJson), 0);
    public static readonly JsonLibraryType Newtonsoft = new(nameof(Newtonsoft), 1);

    [JsonConstructor]
    private JsonLibraryType(string name, int value) : base(name, value)
    {
    }
}