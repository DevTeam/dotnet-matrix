using System.Diagnostics.CodeAnalysis;

using MemoryPack;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class DeserializeSimpleObject

{



    public void SetupMemoryPack() => _payload = MemoryPackSerializer.Serialize(SerializationData.Simple());



    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.MemoryPack)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public SimpleModel? MemoryPack()

    {

        var result = MemoryPackSerializer.Deserialize<SimpleModel>(_payload);

        SerializationChecks.Simple(LibraryCatalog.MemoryPack, result);

        return result;

    }

}

