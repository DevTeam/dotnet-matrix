// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("SequentialNonSeekableRead", 11, "Sequential Non-Seekable Read", "Reads all entries through a forward-only ZIP API from a non-seekable stream.")]
public partial class SequentialNonSeekableRead
{
    private readonly byte[] _archive = ZipData.ManySmallArchive;
    private readonly DigestWriteStream _sink = new();
}
