using ICSharpCode.SharpZipLib.Zip;

namespace Matrix.ZipArchives.Benchmarks;

public partial class FindEntryByName
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpZipLib)]
    public ArchiveEntryInfo SharpZipLib()
    {
        using var source = new MemoryStream(_archive, false);
        using var archive = new ZipFile(source);
        var entry = archive.GetEntry(ZipData.TargetEntryName)!;
        var result = new ArchiveEntryInfo(entry.Name, entry.Size);
        ZipChecks.TargetEntry(LibraryCatalog.SharpZipLib, result);
        return result;
    }
}
