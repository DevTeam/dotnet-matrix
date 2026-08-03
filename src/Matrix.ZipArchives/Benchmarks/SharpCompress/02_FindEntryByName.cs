using SharpCompress.Archives.Zip;

namespace Matrix.ZipArchives.Benchmarks;

public partial class FindEntryByName
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpCompress)]
    public ArchiveEntryInfo SharpCompress()
    {
        using var source = new MemoryStream(_archive, false);
        using var archive = ZipArchive.OpenArchive(source);
        var entry = archive.Entries.First(item =>
            string.Equals(item.Key, ZipData.TargetEntryName, StringComparison.Ordinal));
        var result = new ArchiveEntryInfo(entry.Key!, entry.Size);
        ZipChecks.TargetEntry(LibraryCatalog.SharpCompress, result);
        return result;
    }
}
