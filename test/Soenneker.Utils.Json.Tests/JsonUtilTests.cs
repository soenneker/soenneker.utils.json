using AwesomeAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Utils.Json.Tests;

public class JsonUtilTests
{
    [Test]
    public async Task SerializeToFile_ShouldReplaceMappedDestination(CancellationToken cancellationToken)
    {
        string directory = Path.Combine(Path.GetTempPath(), $"mapped-json-test-{Guid.NewGuid()}");
        Directory.CreateDirectory(directory);
        string path = Path.Combine(directory, "output.json");
        try
        {
            await System.IO.File.WriteAllTextAsync(path, "{}", cancellationToken);
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            using (var mapping = MemoryMappedFile.CreateFromFile(stream, null, 0, MemoryMappedFileAccess.Read, HandleInheritability.None, leaveOpen: true))
            using (MemoryMappedViewAccessor view = mapping.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read))
            {
                await JsonUtil.SerializeToFile(new Dictionary<string, int> { ["value"] = 42 }, path, cancellationToken: cancellationToken);
                using JsonDocument document = JsonDocument.Parse(await System.IO.File.ReadAllTextAsync(path, cancellationToken));
                document.RootElement.GetProperty("value").GetInt32().Should().Be(42);
                view.ReadByte(1).Should().Be((byte)'}');
                Directory.GetFiles(directory).Should().ContainSingle();
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public void Format_should_indent()
    {
        const string str = "{ \"Test\": \"1\"}";
        string result = JsonUtil.Format(str, true);
        result.Should().Be("{\r\n  \"Test\": \"1\"\r\n}");
    }
}
