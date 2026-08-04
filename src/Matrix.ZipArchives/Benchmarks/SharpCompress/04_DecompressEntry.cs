using SharpCompress.Archives.Zip;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class DecompressEntry
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpCompress)]
    public ArchiveDigest SharpCompress()
    {
        _sink.Reset();
        using var source = new MemoryStream(_archive, false);
        using var archive = ZipArchive.OpenArchive(source);
        var entry = archive.Entries.Single();
        using var content = entry.OpenEntryStream();
        content.CopyTo(_sink);
        var result = new ArchiveDigest(1, _sink.Length, _sink.Hash);
        ZipChecks.Deflated(LibraryCatalog.SharpCompress, result);
        return result;
    }
}
