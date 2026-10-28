using System.Diagnostics.CodeAnalysis;

using Google.Protobuf;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class DeserializeCollection

{



    public void SetupGoogleProtobuf() =>

        _payload = SerializationData.CollectionMessage().ToByteArray();



    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.GoogleProtobuf)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public CollectionMessage GoogleProtobuf()

    {

        var message = CollectionMessage.Parser.ParseFrom(_payload);

        SerializationChecks.MessageCollection(LibraryCatalog.GoogleProtobuf, message.Items);

        return message;

    }

}

