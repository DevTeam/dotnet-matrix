using Ionic.Zip;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class ExtractManySmallEntries
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DotNetZip)]
    public ArchiveDigest DotNetZip()
    {
        _sink.Reset();
        using var source = new MemoryStream(_archive, false);
        using var archive = ZipFile.Read(source);
        var count = 0;
        foreach (ZipEntry entry in archive)
        {
            using var content = entry.OpenReader();
            content.CopyTo(_sink);
            count++;
        }

        var result = new ArchiveDigest(count, _sink.Length, _sink.Hash);
        ZipChecks.ManySmall(LibraryCatalog.DotNetZip, result);
        return result;
    }
}
