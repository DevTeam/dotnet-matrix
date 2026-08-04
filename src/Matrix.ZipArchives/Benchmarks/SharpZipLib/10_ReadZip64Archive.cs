using ICSharpCode.SharpZipLib.Zip;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class ReadZip64Archive
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
        ZipChecks.Zip64(LibraryCatalog.SharpZipLib, result);
        return result;
    }
}
