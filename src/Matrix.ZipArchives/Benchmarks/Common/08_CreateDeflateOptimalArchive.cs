namespace Matrix.ZipArchives.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("CreateDeflateOptimalArchive", 8, "Create Deflate Optimal Archive", "Creates a Deflate level 6 ZIP archive containing the deterministic mixed corpus.")]
public partial class CreateDeflateOptimalArchive;
