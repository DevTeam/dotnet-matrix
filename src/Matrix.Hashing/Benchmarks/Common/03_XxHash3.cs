// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "XxHash3",
    3,
    "XxHash3",
    "Hashes a 4 KiB buffer with one call to the library xxHash3 function, sixty-four bits wide.")]
[FeatureUnavailable(
    LibraryCatalog.K4osHashXxHash,
    FeatureStatus.Unsupported,
    "global::K4os.Hash.xxHash exposes XXH32 and XXH64 only; it has no xxHash3 function.")]
[FeatureUnavailable(
    LibraryCatalog.HashDepot,
    FeatureStatus.Unsupported,
    "HashDepot exposes xxHash32 and xxHash64 only; it has no xxHash3 function.")]
[FeatureUnavailable(
    LibraryCatalog.Crc32NET,
    FeatureStatus.Unsupported,
    "Crc32.NET implements CRC-32 and CRC-32C only; it has no xxHash3 function.")]
[FeatureUnavailable(
    LibraryCatalog.Blake3,
    FeatureStatus.Unsupported,
    "Blake3 implements BLAKE3 only; it has no xxHash3 function.")]
[FeatureUnavailable(
    LibraryCatalog.IsopohBlake2b,
    FeatureStatus.Unsupported,
    "global::Isopoh.Cryptography.Blake2b implements BLAKE2b only; it has no xxHash3 function.")]
[FeatureUnavailable(
    LibraryCatalog.FarmHashSharp,
    FeatureStatus.Unsupported,
    "FarmHash.Sharp implements FarmHash only; it has no xxHash3 function.")]
public partial class XxHash3
{
    private readonly byte[] _small = HashData.Small;
}
