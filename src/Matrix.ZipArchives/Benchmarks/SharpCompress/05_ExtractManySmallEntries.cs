using SharpCompress.Archives.Zip;

namespace Matrix.ZipArchives.Benchmarks;

public partial class ExtractManySmallEntries
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpCompress)]
    public ArchiveDigest SharpCompress()
    {
        _sink.Reset();
        using var source = new MemoryStream(_archive, false);
        using var archive = ZipArchive.OpenArchive(source);
        var count = 0;
        foreach (var entry in archive.Entries)
        {
            using var content = entry.OpenEntryStream();
            content.CopyTo(_sink);
            count++;
        }

        var result = new ArchiveDigest(count, _sink.Length, _sink.Hash);
        ZipChecks.ManySmall(LibraryCatalog.SharpCompress, result);
        return result;
    }
}
