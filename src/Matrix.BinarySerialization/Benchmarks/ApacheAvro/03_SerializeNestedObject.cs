using System.Diagnostics.CodeAnalysis;

using Avro.IO;

using Avro.Reflect;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class SerializeNestedObject

{

    private ReflectWriter<OrderModel> _writer = null!;



    public void SetupApacheAvro()

    {

        var data = new ClassCache();

        _writer = new ReflectWriter<OrderModel>(SerializationData.NestedSchema, data);

    }



    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.ApacheAvro)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public byte[] ApacheAvro()

    {

        using var stream = new MemoryStream(64);

        _writer.Write(_input, new BinaryEncoder(stream));

        var payload = stream.ToArray();

        SerializationChecks.SimplePayload(LibraryCatalog.ApacheAvro, payload);

        return payload;

    }

}

