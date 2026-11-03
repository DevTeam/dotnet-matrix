// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.DotNetZip,
    FeatureStatus.Unsupported,
    "DotNetZip requires a seekable stream: ZipInputStream reads Stream.Position and throws NotSupportedException before it returns the first entry.")]
[MatrixFeature("SequentialNonSeekableRead", 11, "Sequential Non-Seekable Read", "Reads all entries through a forward-only ZIP API from a non-seekable stream.")]
public partial class SequentialNonSeekableRead
{
    private readonly byte[] _archive = ZipData.ManySmallArchive;
    private readonly DigestWriteStream _sink = new();
}
