using System.Diagnostics.CodeAnalysis;



using MongoDB.Bson;
using MongoDB.Bson.Serialization;



// ReSharper disable CheckNamespace



// ReSharper disable InconsistentNaming



namespace Matrix.BinarySerialization.Benchmarks;







public partial class DeserializeNestedObject



{







    public void SetupMongoBson() =>



        _payload = BsonExtensionMethods.ToBson(SerializationData.Nested());







    [Benchmark]



    [LibraryBenchmark(LibraryCatalog.MongoBson)]



    [SuppressMessage("Performance", "CA1822:Mark members as static")]



    public OrderModel? MongoBson()



    {



        var result = BsonSerializer.Deserialize<OrderModel>(_payload);



        SerializationChecks.Nested(LibraryCatalog.MongoBson, result);



        return result;



    }



}



