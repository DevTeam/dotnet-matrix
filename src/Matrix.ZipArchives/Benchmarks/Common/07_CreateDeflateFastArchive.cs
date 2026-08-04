// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("CreateDeflateFastArchive", 7, "Create Deflate Fast Archive", "Creates a Deflate level 1 ZIP archive containing the deterministic mixed corpus.")]
public partial class CreateDeflateFastArchive;
