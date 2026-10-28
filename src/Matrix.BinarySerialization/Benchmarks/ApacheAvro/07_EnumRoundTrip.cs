using System.Diagnostics.CodeAnalysis;



using Avro.IO;



using Avro.Reflect;



// ReSharper disable CheckNamespace



// ReSharper disable InconsistentNaming



namespace Matrix.BinarySerialization.Benchmarks;







public partial class EnumRoundTrip



{



    private ReflectWriter<EnumModel> _writer = null!;



    private ReflectReader<EnumModel> _reader = null!;







    public void SetupApacheAvro()



    {



        var writer = new ClassCache();



        var reader = new ClassCache();



        _writer = new ReflectWriter<EnumModel>(SerializationData.EnumSchema, writer);



        _reader = new ReflectReader<EnumModel>(



            SerializationData.EnumSchema,



            SerializationData.EnumSchema,



            reader);



    }







    [Benchmark]



    [LibraryBenchmark(LibraryCatalog.ApacheAvro)]



    [SuppressMessage("Performance", "CA1822:Mark members as static")]



    public EnumModel ApacheAvro()



    {



        using var stream = new MemoryStream(16);



        _writer.Write(_input, new BinaryEncoder(stream));



        stream.Position = 0;



        var result = _reader.Read(null!, new BinaryDecoder(stream));



        SerializationChecks.Enum(LibraryCatalog.ApacheAvro, result);



        return result;



    }



}



