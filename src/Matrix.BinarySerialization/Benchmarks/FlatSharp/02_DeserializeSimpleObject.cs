using System.Diagnostics.CodeAnalysis;

using FlatSharp;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class DeserializeSimpleObject

{



    public void SetupFlatSharp() => _payload = SerializationData.SimpleTableBytes();



    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.FlatSharp)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public SimpleTable? FlatSharp()

    {

        var table = SimpleTable.Serializer.Parse(_payload);

        SerializationChecks.SimpleTable(LibraryCatalog.FlatSharp, table);

        return table;

    }

}

