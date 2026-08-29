[![](https://img.shields.io/nuget/v/Soenneker.Utils.Json.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Utils.Json/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.json/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.utils.json/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Utils.Json.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Utils.Json/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.json/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.utils.json/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Utils.Json
A utility library handling (de)serialization and other useful JSON functions.

## Installation

```bash
dotnet add package Soenneker.Utils.Json
```

## Quick start

```csharp
using Soenneker.Utils.Json.Registrars;

services.AddJsonUtilAsSingleton();
```

Then inject `IJsonUtil` wherever you need it.

## Common operations

- `Deserialize()` - Uses WebOptions as default.
- `Serialize()` - Accepts a nullable object.. if null returns null. If optionType is left null, will use WebOptions.
- `SerializeToElement()` - Serializes the value into a nullable `JsonElement` using the requested options.
- `SerializeToStream()` - Serializes the object into the given stream (System.Text.Json by default; can use Newtonsoft if specified).
- `SerializeToUtf8Bytes()` - Serializes an object to a UTF-8 encoded byte array using System.Text.Json.
- `DeserializeFromFile()` - Reads a file asynchronously and deserializes its JSON to `T`; a missing JSON value yields `null`.
- `SerializeToFile()` - Serializes the value and writes its JSON to the target file asynchronously.
- `TryDeserialize()` - True "Try" parse: returns false on invalid JSON. Supports optional source-gen metadata.
- `IsJsonValid()` - Returns `true` only when the entire input is syntactically valid JSON; invalid input returns `false` and can be logged.
- `Format()` - Parses and pretty-prints JSON, optionally forcing Windows CRLF line endings.
- `WritePretty()` - Reads a JSON file, formats it, and writes the result to the destination path.
