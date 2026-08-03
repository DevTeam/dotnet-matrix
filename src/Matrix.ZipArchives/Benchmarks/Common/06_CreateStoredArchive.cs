namespace Matrix.ZipArchives.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("CreateStoredArchive", 6, "Create Stored Archive", "Creates a stored ZIP archive containing a deterministic mixed corpus.")]
public partial class CreateStoredArchive;
