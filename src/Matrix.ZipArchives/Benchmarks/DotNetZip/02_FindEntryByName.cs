using Ionic.Zip;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class FindEntryByName
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DotNetZip)]
    public ArchiveEntryInfo DotNetZip()
    {
        using var source = new MemoryStream(_archive, false);
        using var archive = ZipFile.Read(source);
        var entry = archive[ZipData.TargetEntryName];
        var result = new ArchiveEntryInfo(entry.FileName, entry.UncompressedSize);
        ZipChecks.TargetEntry(LibraryCatalog.DotNetZip, result);
        return result;
    }
}
