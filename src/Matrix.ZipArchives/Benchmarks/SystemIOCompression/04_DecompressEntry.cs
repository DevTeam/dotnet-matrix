using System.IO.Compression;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class DecompressEntry
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOCompression)]
    public ArchiveDigest SystemIOCompression()
    {
        _sink.Reset();
        using var source = new MemoryStream(_archive, false);
        using var archive = new ZipArchive(source, ZipArchiveMode.Read, false);
        using var content = archive.GetEntry(ZipData.LargeEntryName)!.Open();
        content.CopyTo(_sink);
        var result = new ArchiveDigest(1, _sink.Length, _sink.Hash);
        ZipChecks.Deflated(LibraryCatalog.SystemIOCompression, result);
        return result;
    }
}
