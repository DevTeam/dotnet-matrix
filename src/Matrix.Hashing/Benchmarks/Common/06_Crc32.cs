// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "Crc32",
    6,
    "Crc32",
    "Computes the CRC-32 (IEEE) checksum of a 4 KiB buffer with one call.")]
[FeatureUnavailable(
    LibraryCatalog.K4osHashXxHash,
    FeatureStatus.Unsupported,
    "global::K4os.Hash.xxHash implements xxHash only; it computes no CRC-32 checksum.")]
[FeatureUnavailable(
    LibraryCatalog.StandartHashXxHash,
    FeatureStatus.Unsupported,
    "Standart.Hash.xxHash implements xxHash only; it computes no CRC-32 checksum.")]
[FeatureUnavailable(
    LibraryCatalog.HashDepot,
    FeatureStatus.Unsupported,
    "HashDepot implements xxHash, MurmurHash3, FNV and SipHash only; it computes no CRC-32 checksum.")]
[FeatureUnavailable(
    LibraryCatalog.Blake3,
    FeatureStatus.Unsupported,
    "Blake3 implements BLAKE3 only; it computes no CRC-32 checksum.")]
[FeatureUnavailable(
    LibraryCatalog.IsopohBlake2b,
    FeatureStatus.Unsupported,
    "global::Isopoh.Cryptography.Blake2b implements BLAKE2b only; it computes no CRC-32 checksum.")]
[FeatureUnavailable(
    LibraryCatalog.FarmHashSharp,
    FeatureStatus.Unsupported,
    "FarmHash.Sharp implements FarmHash only; it computes no CRC-32 checksum.")]
public partial class Crc32
{
    private readonly byte[] _small = HashData.Small;
}
