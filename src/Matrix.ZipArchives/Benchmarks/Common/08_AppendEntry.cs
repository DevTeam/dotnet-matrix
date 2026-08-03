namespace Matrix.ZipArchives.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("AppendEntry", 10, "Append Entry", "Opens an existing ZIP archive for update and appends one stored entry.")]
public partial class AppendEntry
{
    private readonly byte[] _archive = ZipData.MixedArchive;
}
