using System.IO.Compression;

namespace Matrix.ZipArchives.Benchmarks;

public partial class ExtractManySmallEntries
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOCompression)]
    public ArchiveDigest SystemIOCompression()
    {
        _sink.Reset();
        using var source = new MemoryStream(_archive, false);
        using var archive = new ZipArchive(source, ZipArchiveMode.Read, false);
        foreach (var entry in archive.Entries)
        {
            using var content = entry.Open();
            content.CopyTo(_sink);
        }

        var result = new ArchiveDigest(archive.Entries.Count, _sink.Length, _sink.Hash);
        ZipChecks.ManySmall(LibraryCatalog.SystemIOCompression, result);
        return result;
    }
}
