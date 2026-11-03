using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;

namespace Soenneker.Utils.Json.Tests;

public sealed class JsonLinesTests
{
    [Test]
    public async Task Metadata_WritesCompactLines_AndLeavesStreamOpen(CancellationToken cancellationToken)
    {
        using var stream = new System.IO.MemoryStream();
        var context = new AotJsonContext(new JsonSerializerOptions { WriteIndented = true, NewLine = "\r\n" });
        await JsonUtil.SerializeToJsonLines(stream, Values(["first\nline", "second"], cancellationToken), context.String, cancellationToken);
        Encoding.UTF8.GetString(stream.ToArray()).Should().Be("\"first\\nline\"\n\"second\"\n");
        stream.CanWrite.Should().BeTrue();
    }

    [Test]
    public async Task Reflection_WritesEachValue(CancellationToken cancellationToken)
    {
        using var stream = new System.IO.MemoryStream();
        await JsonUtil.SerializeToJsonLines(stream, Values([1, 2, 3], cancellationToken), cancellationToken: cancellationToken);
        Encoding.UTF8.GetString(stream.ToArray()).Should().Be("1\n2\n3\n");
    }

    [Test]
    public async Task EmptySequence_WritesNothing(CancellationToken cancellationToken)
    {
        using var stream = new System.IO.MemoryStream();
        await JsonUtil.SerializeToJsonLines(stream, Values<int>([], cancellationToken), AotJsonContext.Default.Int32, cancellationToken);
        stream.Length.Should().Be(0);
    }

    [Test]
    public async Task Cancellation_ReachesAsyncProducer()
    {
        using var stream = new System.IO.MemoryStream();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            JsonUtil.SerializeToJsonLines(stream, Values([1]), AotJsonContext.Default.Int32, cancellation.Token));
        stream.CanWrite.Should().BeTrue();
    }

    private static async IAsyncEnumerable<T> Values<T>(T[] values, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (T value in values)
        {
            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();
            yield return value;
        }
    }
}
