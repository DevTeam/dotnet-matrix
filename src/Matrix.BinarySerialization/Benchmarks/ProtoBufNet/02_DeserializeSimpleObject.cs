using System.Diagnostics.CodeAnalysis;

using ProtoBuf;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class DeserializeSimpleObject

{



    public void SetupProtoBufNet() => _payload = SerializationData.SimpleBytes();



    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.ProtoBufNet)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public SimpleModel ProtoBufNet()

    {

        using var stream = new MemoryStream(_payload, false);

        var result = Serializer.Deserialize<SimpleModel>(stream);

        SerializationChecks.Simple(LibraryCatalog.ProtoBufNet, result);

        return result;

    }

}

