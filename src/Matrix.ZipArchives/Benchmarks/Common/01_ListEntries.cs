// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("ListEntries", 1, "List Entries", "Opens a ZIP archive and enumerates 1,000 entry names and sizes.")]
public partial class ListEntries
{
    private readonly byte[] _archive = ZipData.ManySmallArchive;
}
