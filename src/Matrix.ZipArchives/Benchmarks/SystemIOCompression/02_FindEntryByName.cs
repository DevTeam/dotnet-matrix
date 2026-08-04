using System.IO.Compression;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class FindEntryByName
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOCompression)]
    public ArchiveEntryInfo SystemIOCompression()
    {
        using var source = new MemoryStream(_archive, false);
        using var archive = new ZipArchive(source, ZipArchiveMode.Read, false);
        var entry = archive.GetEntry(ZipData.TargetEntryName)!;
        var result = new ArchiveEntryInfo(entry.FullName, entry.Length);
        ZipChecks.TargetEntry(LibraryCatalog.SystemIOCompression, result);
        return result;
    }
}
