using Ionic.Zip;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class ReadZip64Archive
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DotNetZip)]
    public ArchiveListing DotNetZip()
    {
        using var source = new MemoryStream(_archive, false);
        using var archive = ZipFile.Read(source);
        long totalLength = 0;
        var totalNameLength = 0;
        var count = 0;
        foreach (ZipEntry entry in archive)
        {
            totalLength += entry.UncompressedSize;
            totalNameLength += entry.FileName.Length;
            count++;
        }

        var result = new ArchiveListing(count, totalLength, totalNameLength);
        ZipChecks.Zip64(LibraryCatalog.DotNetZip, result);
        return result;
    }
}
