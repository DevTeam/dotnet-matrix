// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "FarmHash64",
    9,
    "FarmHash64",
    "Hashes a 4 KiB buffer with one call to the library FarmHash64 function.",
    rated: false,
    reason: "With this few rated entrants, the reference is a library's own result, not a result earned against a competitor, so the full 200 points would not reflect a win.")]
[FeatureUnavailable(
    LibraryCatalog.SystemIOHashing,
    FeatureStatus.Unsupported,
    "System.IO.Hashing ships CRC-32, CRC-64, xxHash32, xxHash64, xxHash3 and xxHash128; it implements no FarmHash.")]
[FeatureUnavailable(
    LibraryCatalog.K4osHashXxHash,
    FeatureStatus.Unsupported,
    "global::K4os.Hash.xxHash implements xxHash32 and xxHash64 only; it implements no FarmHash.")]
[FeatureUnavailable(
    LibraryCatalog.StandartHashXxHash,
    FeatureStatus.Unsupported,
    "Standart.Hash.xxHash implements xxHash32, xxHash64, xxHash3 and xxHash128 only; it implements no FarmHash.")]
[FeatureUnavailable(
    LibraryCatalog.HashDepot,
    FeatureStatus.Unsupported,
    "HashDepot implements xxHash, MurmurHash3, FNV and SipHash only; it implements no FarmHash.")]
[FeatureUnavailable(
    LibraryCatalog.Crc32NET,
    FeatureStatus.Unsupported,
    "Crc32.NET implements CRC-32 and CRC-32C only; it implements no FarmHash.")]
[FeatureUnavailable(
    LibraryCatalog.Blake3,
    FeatureStatus.Unsupported,
    "Blake3 implements BLAKE3 only; it implements no FarmHash.")]
[FeatureUnavailable(
    LibraryCatalog.IsopohBlake2b,
    FeatureStatus.Unsupported,
    "global::Isopoh.Cryptography.Blake2b implements BLAKE2b only; it implements no FarmHash.")]
public partial class FarmHash64
{
    private readonly byte[] _small = HashData.Small;
}
