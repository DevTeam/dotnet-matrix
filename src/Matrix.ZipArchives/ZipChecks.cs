using System.IO.Compression;
using System.Text;

namespace Matrix.ZipArchives;

internal static class ZipChecks
{
    [Conditional("MATRIX_VALIDATION")]
    public static void Listing(string library, ArchiveListing actual) =>
        MatrixValidation.Require(
            library,
            actual == ZipData.ManySmallListing,
            $"Listing differs. Expected {ZipData.ManySmallListing}, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void TargetEntry(string library, ArchiveEntryInfo actual) =>
        MatrixValidation.Require(
            library,
            actual == new ArchiveEntryInfo(ZipData.TargetEntryName, 4 * 1024),
            $"Target entry differs. Found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void Stored(string library, ArchiveDigest actual) =>
        Digest(library, actual, ZipData.LargeStoredDigest, "stored entry");

    [Conditional("MATRIX_VALIDATION")]
    public static void Deflated(string library, ArchiveDigest actual) =>
        Digest(library, actual, ZipData.LargeDeflatedDigest, "deflated entry");

    [Conditional("MATRIX_VALIDATION")]
    public static void ManySmall(string library, ArchiveDigest actual) =>
        Digest(library, actual, ZipData.ManySmallDigest, "many-small archive");

    [Conditional("MATRIX_VALIDATION")]
    public static void Encrypted(string library, ArchiveDigest actual) =>
        Digest(library, actual, ZipData.EncryptedDigest, "AES entry");

    [Conditional("MATRIX_VALIDATION")]
    public static void Zip64(string library, ArchiveListing actual) =>
        MatrixValidation.Require(
            library,
            actual.EntryCount == ZipData.Zip64Count,
            $"Expected {ZipData.Zip64Count} Zip64 entries, found {actual.EntryCount}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void CreatedMixed(string library, byte[] archive) =>
        ValidateArchive(library, archive, ZipData.MixedEntries);

    [Conditional("MATRIX_VALIDATION")]
    public static void CreatedManySmall(string library, byte[] archive) =>
        ValidateArchive(library, archive, ZipData.ManySmallEntries);

    [Conditional("MATRIX_VALIDATION")]
    public static void Appended(string library, byte[] archive) =>
        ValidateArchive(
            library,
            archive,
            [.. ZipData.MixedEntries, new ZipEntryData(ZipData.AppendedEntryName, ZipData.AppendedContent)]);

    private static void Digest(
        string library,
        ArchiveDigest actual,
        ArchiveDigest expected,
        string operation) =>
        MatrixValidation.Require(
            library,
            actual == expected,
            $"Digest for {operation} differs. Expected {expected}, found {actual}.");

    private static void ValidateArchive(
        string library,
        byte[] archiveBytes,
        IReadOnlyList<ZipEntryData> expected)
    {
        using var source = new MemoryStream(archiveBytes, false);
        using var archive = new System.IO.Compression.ZipArchive(
            source,
            ZipArchiveMode.Read,
            false,
            Encoding.UTF8);
        MatrixValidation.Require(
            library,
            archive.Entries.Count == expected.Count,
            $"Expected {expected.Count} entries, found {archive.Entries.Count}.");
        for (var index = 0; index < expected.Count; index++)
        {
            var item = expected[index];
            var entry = archive.GetEntry(item.Name);
            MatrixValidation.Require(library, entry is not null, $"Entry '{item.Name}' was not found.");
            using var content = entry!.Open();
            using var destination = new MemoryStream();
            content.CopyTo(destination);
            MatrixValidation.Require(
                library,
                destination.ToArray().AsSpan().SequenceEqual(item.Content),
                $"Entry '{item.Name}' content differs.");
        }
    }
}
