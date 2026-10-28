using System.Diagnostics.CodeAnalysis;







using Avro.IO;







using Avro.Reflect;







// ReSharper disable CheckNamespace







// ReSharper disable InconsistentNaming







namespace Matrix.BinarySerialization.Benchmarks;















public partial class DeserializeNestedObject







{







    private ReflectReader<OrderModel> _reader = null!;















    public void SetupApacheAvro()







    {







        _payload = SerializationData.NestedAvroBytes();







        var data = new ClassCache();







        _reader = new ReflectReader<OrderModel>(SerializationData.NestedSchema, SerializationData.NestedSchema, data);







    }















    [Benchmark]







    [LibraryBenchmark(LibraryCatalog.ApacheAvro)]







    [SuppressMessage("Performance", "CA1822:Mark members as static")]







    public OrderModel ApacheAvro()







    {







        using var stream = new MemoryStream(_payload, false);







        var result = _reader.Read(null!, new BinaryDecoder(stream));







        SerializationChecks.Nested(LibraryCatalog.ApacheAvro, result);







        return result;







    }







}







