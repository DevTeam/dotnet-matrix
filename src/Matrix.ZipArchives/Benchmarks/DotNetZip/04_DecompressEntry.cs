using Ionic.Zip;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class DecompressEntry
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DotNetZip)]
    public ArchiveDigest DotNetZip()
    {
        _sink.Reset();
        using var source = new MemoryStream(_archive, false);
        using var archive = ZipFile.Read(source);
        using var content = archive[ZipData.LargeEntryName].OpenReader();
        content.CopyTo(_sink);
        var result = new ArchiveDigest(1, _sink.Length, _sink.Hash);
        ZipChecks.Deflated(LibraryCatalog.DotNetZip, result);
        return result;
    }
}
