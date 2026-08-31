[![](https://img.shields.io/nuget/v/Soenneker.Utils.Json.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Utils.Json/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.json/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.utils.json/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Utils.Json.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Utils.Json/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.utils.json/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.utils.json/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Utils.Json
Serializes, deserializes, validates, formats, and safely writes JSON with System.Text.Json or Newtonsoft.Json.

## Installation

```bash
dotnet add package Soenneker.Utils.Json
```

## Quick start

Serialization, deserialization, validation, and formatting are static operations on `JsonUtil`:

```csharp
using Soenneker.Utils.Json;

string? json = JsonUtil.Serialize(new { Id = 42, Name = "example" });
Document? document = JsonUtil.Deserialize<Document>(json!);

if (!JsonUtil.TryDeserialize<Document>(utf8Json, out Document? parsed))
{
    // The input was empty, invalid, or represented JSON null.
}
```

The default serializer is `System.Text.Json` with `JsonOptionsCollection.WebOptions`. Pass a
`JsonLibraryType` to overloads that support Newtonsoft.Json, or a `JsonTypeInfo<T>` to use
source-generated System.Text.Json metadata.

Register the service only when using the instance-based `WritePretty` file operation:

```csharp
using Soenneker.Utils.Json.Registrars;

services.AddJsonUtilAsSingleton();
```

Then inject `IJsonUtil` and call `WritePretty`.

## Common operations

- `Deserialize()` - Deserializes strings, UTF-8 data, streams, or HTTP response content. Empty strings and byte spans return the default value.
- `Serialize()` - Returns `null` for a null object; otherwise uses WebOptions unless another options profile is selected.
- `SerializeToElement()` - Serializes the value into a nullable `JsonElement` using the requested options.
- `SerializeToStream()` - Serializes the object into the given stream (System.Text.Json by default; can use Newtonsoft if specified).
- `SerializeToUtf8Bytes()` - Serializes an object to a UTF-8 encoded byte array using System.Text.Json.
- `DeserializeFromFile()` - Reads a file asynchronously and deserializes its JSON to `T`; JSON `null` yields `null`.
- `SerializeToFile()` - Serializes to a temporary sibling file and replaces the destination only after serialization succeeds. A null object leaves the destination unchanged.
- `TryDeserialize()` - Returns `false` for empty input, invalid JSON, or a deserialized null value. Supports optional source-generated metadata.
- `IsJsonValid()` - Returns `true` only when the entire input is syntactically valid JSON; invalid input returns `false` and can be logged.
- `Format()` - Parses and pretty-prints JSON, optionally forcing Windows CRLF line endings.
- `WritePretty()` - Reads a JSON file, formats it, and writes the result to the destination path.

The async deserialization overloads that accept a logger return the default value after other
read or deserialization failures, but requested cancellation is propagated. The stream overloads
leave caller-owned streams open.
