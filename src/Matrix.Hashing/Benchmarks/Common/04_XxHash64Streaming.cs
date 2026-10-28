// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "XxHash64Streaming",
    4,
    "XxHash64 Streaming",
    "Hashes a 4 KiB buffer as sixty-four incremental updates of sixty-four bytes each.")]
[FeatureUnavailable(
    LibraryCatalog.StandartHashXxHash,
    FeatureStatus.Unsupported,
    "Standart.Hash.xxHash accepts data only as a complete buffer or a Stream; it has no API that takes caller-supplied chunks.")]
[FeatureUnavailable(
    LibraryCatalog.HashDepot,
    FeatureStatus.Unsupported,
    "HashDepot accepts incremental data only through a Stream; it has no API that takes caller-supplied chunks.")]
[FeatureUnavailable(
    LibraryCatalog.Crc32NET,
    FeatureStatus.Unsupported,
    "Crc32.NET implements CRC-32 and CRC-32C only; it has no incremental xxHash64 API.")]
[FeatureUnavailable(
    LibraryCatalog.Blake3,
    FeatureStatus.Unsupported,
    "Blake3 implements BLAKE3 only; it has no incremental xxHash64 API.")]
[FeatureUnavailable(
    LibraryCatalog.IsopohBlake2b,
    FeatureStatus.Unsupported,
    "global::Isopoh.Cryptography.Blake2b implements BLAKE2b only; it has no incremental xxHash64 API.")]
[FeatureUnavailable(
    LibraryCatalog.FarmHashSharp,
    FeatureStatus.Unsupported,
    "FarmHash.Sharp implements FarmHash only; it has no incremental xxHash64 API.")]
public partial class XxHash64Streaming
{
    private readonly byte[] _small = HashData.Small;
}
