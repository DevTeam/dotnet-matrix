// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "XxHash64",
    1,
    "XxHash64",
    "Hashes a 4 KiB buffer with one call to the library xxHash64 function.")]
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
public partial class XxHash64
{
    private readonly byte[] _small = HashData.Small;
}
