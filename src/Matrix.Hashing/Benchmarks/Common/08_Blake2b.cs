// ReSharper disable CheckNamespace
namespace Matrix.Hashing.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MatrixFeature(
    "Blake2b",
    8,
    "Blake2b",
    "Hashes a 4 KiB buffer with BLAKE2b at a 32-byte digest and returns the eight leading digest bytes.",
    rated: false,
    reason: "With this few rated entrants, the reference is a library's own result, not a result earned against a competitor, so the full 200 points would not reflect a win.")]
[FeatureUnavailable(
    LibraryCatalog.SystemIOHashing,
    FeatureStatus.Unsupported,
    "System.IO.Hashing ships CRC-32, CRC-64, xxHash32, xxHash64, xxHash3 and xxHash128; it implements no BLAKE2b.")]
[FeatureUnavailable(
    LibraryCatalog.K4osHashXxHash,
    FeatureStatus.Unsupported,
    "global::K4os.Hash.xxHash implements xxHash32 and xxHash64 only; it implements no BLAKE2b.")]
[FeatureUnavailable(
    LibraryCatalog.StandartHashXxHash,
    FeatureStatus.Unsupported,
    "Standart.Hash.xxHash implements xxHash32, xxHash64, xxHash3 and xxHash128 only; it implements no BLAKE2b.")]
[FeatureUnavailable(
    LibraryCatalog.HashDepot,
    FeatureStatus.Unsupported,
    "HashDepot implements xxHash, MurmurHash3, FNV and SipHash only; it implements no BLAKE2b.")]
[FeatureUnavailable(
    LibraryCatalog.Crc32NET,
    FeatureStatus.Unsupported,
    "Crc32.NET implements CRC-32 and CRC-32C only; it implements no BLAKE2b.")]
[FeatureUnavailable(
    LibraryCatalog.Blake3,
    FeatureStatus.Unsupported,
    "Blake3 implements BLAKE3, a different algorithm from BLAKE2b.")]
[FeatureUnavailable(
    LibraryCatalog.FarmHashSharp,
    FeatureStatus.Unsupported,
    "FarmHash.Sharp implements FarmHash only; it implements no BLAKE2b.")]
public partial class Blake2b
{
    private readonly byte[] _small = HashData.Small;
    private readonly global::Isopoh.Cryptography.Blake2b.Blake2BConfig _config = new()
    {
        OutputSizeInBytes = 32
    };

    private readonly global::Isopoh.Cryptography.SecureArray.SecureArrayCall _secureArrayCall =
        global::Isopoh.Cryptography.SecureArray.SecureArray.DefaultCall;
}
