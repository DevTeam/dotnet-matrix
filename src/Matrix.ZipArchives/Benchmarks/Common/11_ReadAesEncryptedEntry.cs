// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.SystemIOCompression,
    FeatureStatus.Unsupported,
    "System.IO.Compression does not support WinZip AES encrypted entries.")]
[MatrixFeature("ReadAesEncryptedEntry", 13, "Read AES Encrypted Entry", "Opens and decrypts one AES-256 encrypted ZIP entry.")]
public partial class ReadAesEncryptedEntry
{
    private readonly byte[] _archive = ZipData.AesArchive;
    private readonly DigestWriteStream _sink = new();
}
