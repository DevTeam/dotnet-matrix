using System.Diagnostics.CodeAnalysis;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class DeserializeSimpleObject

{



    public void SetupHyperSerializer() =>

        _payload = Hyper.HyperSerializer<SimpleModel>.Serialize(SerializationData.Simple()).ToArray();



    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.HyperSerializer)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public SimpleModel HyperSerializer()

    {

        var result = Hyper.HyperSerializer<SimpleModel>.Deserialize(_payload);

        SerializationChecks.Simple(LibraryCatalog.HyperSerializer, result);

        return result;

    }

}

