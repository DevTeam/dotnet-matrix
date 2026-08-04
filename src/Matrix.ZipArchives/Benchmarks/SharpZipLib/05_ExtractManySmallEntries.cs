using ICSharpCode.SharpZipLib.Zip;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class ExtractManySmallEntries
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpZipLib)]
    public ArchiveDigest SharpZipLib()
    {
        _sink.Reset();
        using var source = new MemoryStream(_archive, false);
        using var archive = new ZipFile(source);
        var count = 0;
        foreach (ZipEntry entry in archive)
        {
            using var content = archive.GetInputStream(entry);
            content.CopyTo(_sink);
            count++;
        }

        var result = new ArchiveDigest(count, _sink.Length, _sink.Hash);
        ZipChecks.ManySmall(LibraryCatalog.SharpZipLib, result);
        return result;
    }
}
