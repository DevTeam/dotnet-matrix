using System.Diagnostics.CodeAnalysis;

using Google.Protobuf;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class DeserializeSimpleObject

{



    public void SetupGoogleProtobuf() =>

        _payload = SerializationData.SimpleMessage().ToByteArray();



    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.GoogleProtobuf)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public SimpleMessage GoogleProtobuf()

    {

        var message = SimpleMessage.Parser.ParseFrom(_payload);

        SerializationChecks.SimpleMessage(LibraryCatalog.GoogleProtobuf, message);

        return message;

    }

}

