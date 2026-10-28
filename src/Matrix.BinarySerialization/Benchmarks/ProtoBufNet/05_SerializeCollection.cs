using System.Diagnostics.CodeAnalysis;

using ProtoBuf;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class SerializeCollection

{

    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.ProtoBufNet)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public byte[] ProtoBufNet()

    {

        using var stream = new MemoryStream(64);

        Serializer.Serialize(stream, _input);

        var payload = stream.ToArray();

        SerializationChecks.SimplePayload(LibraryCatalog.ProtoBufNet, payload);

        return payload;

    }

}

