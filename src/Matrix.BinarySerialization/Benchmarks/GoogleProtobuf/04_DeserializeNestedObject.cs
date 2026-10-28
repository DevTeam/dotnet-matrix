using System.Diagnostics.CodeAnalysis;

using Google.Protobuf;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class DeserializeNestedObject

{



    public void SetupGoogleProtobuf() =>

        _payload = SerializationData.OrderMessage().ToByteArray();



    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.GoogleProtobuf)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public OrderMessage GoogleProtobuf()

    {

        var message = OrderMessage.Parser.ParseFrom(_payload);

        SerializationChecks.NestedMessage(LibraryCatalog.GoogleProtobuf, message);

        return message;

    }

}

