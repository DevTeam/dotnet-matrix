using SharpCompress.Archives.Zip;

namespace Matrix.ZipArchives.Benchmarks;

public partial class ReadZip64Archive
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpCompress)]
    public ArchiveListing SharpCompress()
    {
        using var source = new MemoryStream(_archive, false);
        using var archive = ZipArchive.OpenArchive(source);
        long totalLength = 0;
        var totalNameLength = 0;
        var count = 0;
        foreach (var entry in archive.Entries)
        {
            totalLength += entry.Size;
            totalNameLength += entry.Key!.Length;
            count++;
        }

        var result = new ArchiveListing(count, totalLength, totalNameLength);
        ZipChecks.Zip64(LibraryCatalog.SharpCompress, result);
        return result;
    }
}
