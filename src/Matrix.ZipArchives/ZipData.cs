using System.IO.Compression;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace Matrix.ZipArchives;

internal static class ZipData
{
    public const int ManySmallCount = 1_000;
    public const int Zip64Count = 65_536;
    public const string TargetEntryName = "files/0999.bin";
    public const string LargeEntryName = "large.bin";
    public const string AppendedEntryName = "appended/readme.txt";
    public const string EncryptedEntryName = "secret/data.txt";
    public const string Password = "matrix-password";

    private static readonly Lazy<ZipEntryData[]> ManySmallEntriesValue = new(CreateManySmallEntries);
    private static readonly Lazy<ZipEntryData[]> MixedEntriesValue = new(CreateMixedEntries);
    private static readonly Lazy<byte[]> LargeStoredContentValue = new(() =>
        CreateBinary(4 * 1024 * 1024, 17));
    private static readonly Lazy<byte[]> LargeDeflatedContentValue = new(() =>
        CreateText(4 * 1024 * 1024));
    private static readonly Lazy<byte[]> AppendedContentValue = new(() =>
        Encoding.UTF8.GetBytes("Added by .NET Matrix\n"));
    private static readonly Lazy<byte[]> EncryptedContentValue = new(() => CreateText(64 * 1024));
    private static readonly Lazy<byte[]> ManySmallArchiveValue = new(() =>
        CreateArchive(ManySmallEntries, CompressionLevel.Optimal));
    private static readonly Lazy<byte[]> MixedArchiveValue = new(() =>
        CreateArchive(MixedEntries, CompressionLevel.NoCompression));
    private static readonly Lazy<byte[]> LargeStoredArchiveValue = new(() =>
        CreateArchive(
            [new ZipEntryData(LargeEntryName, LargeStoredContent)],
            CompressionLevel.NoCompression));
    private static readonly Lazy<byte[]> LargeDeflatedArchiveValue = new(() =>
        CreateArchive(
            [new ZipEntryData(LargeEntryName, LargeDeflatedContent)],
            CompressionLevel.Optimal));
    private static readonly Lazy<byte[]> Zip64ArchiveValue = new(CreateZip64Archive);
    private static readonly Lazy<byte[]> AesArchiveValue = new(CreateAesArchive);
    private static readonly Lazy<ArchiveListing> ManySmallListingValue = new(() => new ArchiveListing(
        ManySmallEntries.Length,
        ManySmallEntries.Sum(entry => (long)entry.Content.Length),
        ManySmallEntries.Sum(entry => entry.Name.Length)));
    private static readonly Lazy<ArchiveDigest> ManySmallDigestValue = new(() =>
        Digest(ManySmallEntries));
    private static readonly Lazy<ArchiveDigest> LargeStoredDigestValue = new(() =>
        Digest([new ZipEntryData(LargeEntryName, LargeStoredContent)]));
    private static readonly Lazy<ArchiveDigest> LargeDeflatedDigestValue = new(() =>
        Digest([new ZipEntryData(LargeEntryName, LargeDeflatedContent)]));
    private static readonly Lazy<ArchiveDigest> EncryptedDigestValue = new(() =>
        Digest([new ZipEntryData(EncryptedEntryName, EncryptedContent)]));

    public static ZipEntryData[] ManySmallEntries => ManySmallEntriesValue.Value;

    public static ZipEntryData[] MixedEntries => MixedEntriesValue.Value;

    public static byte[] LargeStoredContent => LargeStoredContentValue.Value;

    public static byte[] LargeDeflatedContent => LargeDeflatedContentValue.Value;

    public static byte[] AppendedContent => AppendedContentValue.Value;

    public static byte[] EncryptedContent => EncryptedContentValue.Value;

    public static byte[] ManySmallArchive => ManySmallArchiveValue.Value;

    public static byte[] MixedArchive => MixedArchiveValue.Value;

    public static byte[] LargeStoredArchive => LargeStoredArchiveValue.Value;

    public static byte[] LargeDeflatedArchive => LargeDeflatedArchiveValue.Value;

    public static byte[] Zip64Archive => Zip64ArchiveValue.Value;

    public static byte[] AesArchive => AesArchiveValue.Value;

    public static ArchiveListing ManySmallListing => ManySmallListingValue.Value;

    public static ArchiveDigest ManySmallDigest => ManySmallDigestValue.Value;

    public static ArchiveDigest LargeStoredDigest => LargeStoredDigestValue.Value;

    public static ArchiveDigest LargeDeflatedDigest => LargeDeflatedDigestValue.Value;

    public static ArchiveDigest EncryptedDigest => EncryptedDigestValue.Value;

    private static ZipEntryData[] CreateManySmallEntries() => Enumerable
        .Range(0, ManySmallCount)
        .Select(index => new ZipEntryData(
            $"files/{index:0000}.bin",
            CreateBinary(4 * 1024, index + 1)))
        .ToArray();

    private static ZipEntryData[] CreateMixedEntries() =>
    [
        new("empty.txt", []),
        new("docs/readme.txt", Encoding.UTF8.GetBytes("ZIP Archives Matrix\n")),
        new("nested/данные.json", Encoding.UTF8.GetBytes("{\"name\":\"Матрица\",\"ok\":true}\n")),
        new("binary/random.bin", CreateBinary(64 * 1024, 73)),
        new("text/lorem.txt", CreateText(128 * 1024))
    ];

    private static byte[] CreateBinary(int length, int seed)
    {
        var result = new byte[length];
        new Random(seed).NextBytes(result);
        return result;
    }

    private static byte[] CreateText(int length)
    {
        var pattern = Encoding.UTF8.GetBytes(
            "The quick brown fox jumps over the lazy dog. 0123456789\n");
        var result = new byte[length];
        for (var offset = 0; offset < result.Length; offset += pattern.Length)
        {
            pattern.AsSpan(0, Math.Min(pattern.Length, result.Length - offset))
                .CopyTo(result.AsSpan(offset));
        }

        return result;
    }

    private static byte[] CreateArchive(
        IReadOnlyList<ZipEntryData> entries,
        CompressionLevel compressionLevel)
    {
        using var destination = new MemoryStream();
        using (var archive = new System.IO.Compression.ZipArchive(
                   destination,
                   ZipArchiveMode.Create,
                   true,
                   Encoding.UTF8))
        {
            foreach (var item in entries)
            {
                var entry = archive.CreateEntry(item.Name, compressionLevel);
                using var target = entry.Open();
                target.Write(item.Content);
            }
        }

        return destination.ToArray();
    }

    private static byte[] CreateZip64Archive()
    {
        using var destination = new MemoryStream();
        using (var archive = new System.IO.Compression.ZipArchive(
                   destination,
                   ZipArchiveMode.Create,
                   true,
                   Encoding.UTF8))
        {
            for (var index = 0; index < Zip64Count; index++)
            {
                archive.CreateEntry($"empty/{index:00000}.txt", CompressionLevel.NoCompression);
            }
        }

        return destination.ToArray();
    }

    private static byte[] CreateAesArchive()
    {
        using var destination = new MemoryStream();
        using (var zip = new ZipOutputStream(destination) { IsStreamOwner = false, Password = Password })
        {
            zip.SetLevel(6);
            var entry = new ZipEntry(EncryptedEntryName)
            {
                AESKeySize = 256,
                DateTime = DateTime.UnixEpoch
            };
            zip.PutNextEntry(entry);
            zip.Write(EncryptedContent);
            zip.CloseEntry();
            zip.Finish();
        }

        return destination.ToArray();
    }

    private static ArchiveDigest Digest(IEnumerable<ZipEntryData> entries)
    {
        using var sink = new DigestWriteStream();
        var count = 0;
        foreach (var entry in entries)
        {
            sink.Write(entry.Content);
            count++;
        }

        return new ArchiveDigest(count, sink.Length, sink.Hash);
    }
}
