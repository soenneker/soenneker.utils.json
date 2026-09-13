using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Utils.Json;
using Soenneker.Enums.JsonLibrary;
using Soenneker.Json.OptionsCollection;

namespace Audit;

public class NewtonsoftProfileTests
{
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
    [Test]
    public void NewtonsoftSettingsAreIsolatedFromPublicMutations()
    {
        string before = JsonUtil.Serialize(new { Value = "test" }, libraryType: JsonLibraryType.Newtonsoft)!;
        var settings = JsonOptionsCollection.Newtonsoft;
        settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
        settings.Converters.Clear();
        string after = JsonUtil.Serialize(new { Value = "test" }, libraryType: JsonLibraryType.Newtonsoft)!;
        Check(before == after, "Caller settings mutated JsonUtil profile");
    }
}
