// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "XxHash64Large",
    5,
    "XxHash64 Large",
    "Hashes a 1 MiB buffer with one call to the library xxHash64 function, where throughput rather than call overhead dominates.")]
[FeatureUnavailable(
    LibraryCatalog.Crc32NET,
    FeatureStatus.Unsupported,
    "Crc32.NET implements CRC-32 and CRC-32C only; it has no xxHash function.")]
[FeatureUnavailable(
    LibraryCatalog.Blake3,
    FeatureStatus.Unsupported,
    "Blake3 implements BLAKE3 only; it has no xxHash function.")]
[FeatureUnavailable(
    LibraryCatalog.IsopohBlake2b,
    FeatureStatus.Unsupported,
    "global::Isopoh.Cryptography.Blake2b implements BLAKE2b only; it has no xxHash function.")]
[FeatureUnavailable(
    LibraryCatalog.FarmHashSharp,
    FeatureStatus.Unsupported,
    "FarmHash.Sharp implements FarmHash only; it has no xxHash function.")]
public partial class XxHash64Large
{
    private readonly byte[] _large = HashData.Large;
}
