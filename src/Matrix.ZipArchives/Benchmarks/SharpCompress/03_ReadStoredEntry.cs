using SharpCompress.Archives.Zip;

namespace Matrix.ZipArchives.Benchmarks;

public partial class ReadStoredEntry
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
        ZipChecks.Stored(LibraryCatalog.SharpCompress, result);
        return result;
    }
}
