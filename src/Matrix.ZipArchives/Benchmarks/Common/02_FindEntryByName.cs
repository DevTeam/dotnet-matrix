// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("FindEntryByName", 2, "Find Entry By Name", "Opens a 1,000-entry ZIP archive and locates its final entry by exact name.")]
public partial class FindEntryByName
{
    private readonly byte[] _archive = ZipData.ManySmallArchive;
}
