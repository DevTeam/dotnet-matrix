using System.Diagnostics.CodeAnalysis;

using MemoryPack;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class DeserializeNestedObject

{



    public void SetupMemoryPack() => _payload = MemoryPackSerializer.Serialize(SerializationData.Nested());



    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.MemoryPack)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public OrderModel? MemoryPack()

    {

        var result = MemoryPackSerializer.Deserialize<OrderModel>(_payload);

        SerializationChecks.Nested(LibraryCatalog.MemoryPack, result);

        return result;

    }

}

