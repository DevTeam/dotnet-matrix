using System.IO.Compression;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

namespace Matrix.ZipArchives.Benchmarks;

public partial class ReadZip64Archive
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOCompression)]
    public ArchiveListing SystemIOCompression()
    {
        using var source = new MemoryStream(_archive, false);
        using var archive = new ZipArchive(source, ZipArchiveMode.Read, false);
        long totalLength = 0;
        var totalNameLength = 0;
        foreach (var entry in archive.Entries)
        {
            totalLength += entry.Length;
            totalNameLength += entry.FullName.Length;
        }

        var result = new ArchiveListing(archive.Entries.Count, totalLength, totalNameLength);
        ZipChecks.Zip64(LibraryCatalog.SystemIOCompression, result);
        return result;
    }
}
