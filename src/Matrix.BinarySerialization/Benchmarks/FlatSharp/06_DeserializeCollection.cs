using System.Diagnostics.CodeAnalysis;

using FlatSharp;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class DeserializeCollection

{



    public void SetupFlatSharp() => _payload = SerializationData.SimpleListBytes();



    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.FlatSharp)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public SimpleList? FlatSharp()

    {

        var list = SimpleList.Serializer.Parse(_payload);

        SerializationChecks.TableCollection(LibraryCatalog.FlatSharp, list?.Items);

        return list;

    }

}

