using System.Diagnostics.CodeAnalysis;

using MongoDB.Bson;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class SerializeNestedObject

{

    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.MongoBson)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public byte[] MongoBson()

    {

        var payload = BsonExtensionMethods.ToBson(_input);

        SerializationChecks.SimplePayload(LibraryCatalog.MongoBson, payload);

        return payload;

    }

}

