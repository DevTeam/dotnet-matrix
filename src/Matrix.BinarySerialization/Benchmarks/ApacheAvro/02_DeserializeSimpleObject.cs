using System.Diagnostics.CodeAnalysis;







using Avro.IO;







using Avro.Reflect;







// ReSharper disable CheckNamespace







// ReSharper disable InconsistentNaming







namespace Matrix.BinarySerialization.Benchmarks;















public partial class DeserializeSimpleObject







{







    private ReflectReader<SimpleModel> _reader = null!;















    public void SetupApacheAvro()







    {







        _payload = SerializationData.SimpleAvroBytes();







        var data = new ClassCache();







        _reader = new ReflectReader<SimpleModel>(SerializationData.SimpleSchema, SerializationData.SimpleSchema, data);







    }















    [Benchmark]







    [LibraryBenchmark(LibraryCatalog.ApacheAvro)]







    [SuppressMessage("Performance", "CA1822:Mark members as static")]







    public SimpleModel ApacheAvro()







    {







        using var stream = new MemoryStream(_payload, false);







        var result = _reader.Read(null!, new BinaryDecoder(stream));







        SerializationChecks.Simple(LibraryCatalog.ApacheAvro, result);







        return result;







    }







}







