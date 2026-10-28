using System.Diagnostics.CodeAnalysis;



using MongoDB.Bson;
using MongoDB.Bson.Serialization;



// ReSharper disable CheckNamespace



// ReSharper disable InconsistentNaming



namespace Matrix.BinarySerialization.Benchmarks;







public partial class DeserializeSimpleObject



{







    public void SetupMongoBson() =>



        _payload = BsonExtensionMethods.ToBson(SerializationData.Simple());







    [Benchmark]



    [LibraryBenchmark(LibraryCatalog.MongoBson)]



    [SuppressMessage("Performance", "CA1822:Mark members as static")]



    public SimpleModel? MongoBson()



    {



        var result = BsonSerializer.Deserialize<SimpleModel>(_payload);



        SerializationChecks.Simple(LibraryCatalog.MongoBson, result);



        return result;



    }



}



