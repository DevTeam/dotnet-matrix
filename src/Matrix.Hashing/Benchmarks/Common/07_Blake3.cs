// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "Blake3",
    7,
    "Blake3",
    "Hashes a 4 KiB buffer with BLAKE3 and returns the eight leading digest bytes.",
    rated: false,
    reason: "With this few rated entrants, the reference is a library's own result, not a result earned against a competitor, so the full 200 points would not reflect a win.")]
[FeatureUnavailable(
    LibraryCatalog.SystemIOHashing,
    FeatureStatus.Unsupported,
    "System.IO.Hashing ships CRC-32, CRC-64, xxHash32, xxHash64, xxHash3 and xxHash128; it implements no BLAKE3.")]
[FeatureUnavailable(
    LibraryCatalog.K4osHashXxHash,
    FeatureStatus.Unsupported,
    "global::K4os.Hash.xxHash implements xxHash32 and xxHash64 only; it implements no BLAKE3.")]
[FeatureUnavailable(
    LibraryCatalog.StandartHashXxHash,
    FeatureStatus.Unsupported,
    "Standart.Hash.xxHash implements xxHash32, xxHash64, xxHash3 and xxHash128 only; it implements no BLAKE3.")]
[FeatureUnavailable(
    LibraryCatalog.HashDepot,
    FeatureStatus.Unsupported,
    "HashDepot implements xxHash, MurmurHash3, FNV and SipHash only; it implements no BLAKE3.")]
[FeatureUnavailable(
    LibraryCatalog.Crc32NET,
    FeatureStatus.Unsupported,
    "Crc32.NET implements CRC-32 and CRC-32C only; it implements no BLAKE3.")]
[FeatureUnavailable(
    LibraryCatalog.IsopohBlake2b,
    FeatureStatus.Unsupported,
    "global::Isopoh.Cryptography.Blake2b implements BLAKE2b, a different algorithm from BLAKE3.")]
[FeatureUnavailable(
    LibraryCatalog.FarmHashSharp,
    FeatureStatus.Unsupported,
    "FarmHash.Sharp implements FarmHash only; it implements no BLAKE3.")]
public partial class Blake3Hash
{
    private readonly byte[] _small = HashData.Small;
}
