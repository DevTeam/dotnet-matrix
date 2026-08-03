using ICSharpCode.SharpZipLib.Zip;

namespace Matrix.ZipArchives.Benchmarks;

public partial class ListEntries
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpZipLib)]
    public ArchiveListing SharpZipLib()
    {
        using var source = new MemoryStream(_archive, false);
        using var archive = new ZipFile(source);
        long totalLength = 0;
        var totalNameLength = 0;
        var count = 0;
        foreach (ZipEntry entry in archive)
        {
            totalLength += entry.Size;
            totalNameLength += entry.Name.Length;
            count++;
        }

        var result = new ArchiveListing(count, totalLength, totalNameLength);
        ZipChecks.Listing(LibraryCatalog.SharpZipLib, result);
        return result;
    }
}
