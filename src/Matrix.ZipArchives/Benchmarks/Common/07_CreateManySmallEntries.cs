// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature("CreateManySmallEntries", 9, "Create Many Small Entries", "Creates a stored ZIP archive containing 1,000 small files.")]
public partial class CreateManySmallEntries;
