using System.Diagnostics.CodeAnalysis;

using FlatSharp;

// ReSharper disable CheckNamespace

// ReSharper disable InconsistentNaming

namespace Matrix.BinarySerialization.Benchmarks;



public partial class DeserializeNestedObject

{



    public void SetupFlatSharp() => _payload = SerializationData.OrderTableBytes();



    [Benchmark]

    [LibraryBenchmark(LibraryCatalog.FlatSharp)]

    [SuppressMessage("Performance", "CA1822:Mark members as static")]

    public OrderTable? FlatSharp()

    {

        var table = OrderTable.Serializer.Parse(_payload);

        SerializationChecks.NestedTable(LibraryCatalog.FlatSharp, table);

        return table;

    }

}

