using System.Diagnostics.CodeAnalysis;

using MessagePack;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class DeserializeSimpleObject

{



    public void SetupMessagePack() => _payload = MessagePackSerializer.Serialize(SerializationData.Simple());



    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.MessagePack)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public SimpleModel MessagePack()

    {

        var result = MessagePackSerializer.Deserialize<SimpleModel>(_payload);

        SerializationChecks.Simple(LibraryCatalog.MessagePack, result);

        return result;

    }

}

