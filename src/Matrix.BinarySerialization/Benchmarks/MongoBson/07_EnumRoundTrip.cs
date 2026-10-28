using System.Diagnostics.CodeAnalysis;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class EnumRoundTrip

{

    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.MongoBson)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public EnumModel? MongoBson()

    {

        var payload = BsonExtensionMethods.ToBson(_input);

        var result = BsonSerializer.Deserialize<EnumModel>(payload);

        SerializationChecks.Enum(LibraryCatalog.MongoBson, result);

        return result;

    }

}

