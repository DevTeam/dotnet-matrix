using SharpCompress.Readers;

namespace Matrix.ZipArchives.Benchmarks;

public partial class SequentialNonSeekableRead
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpCompress)]
    public ArchiveDigest SharpCompress()
    {
        _sink.Reset();
        using var source = new NonSeekableReadStream(_archive);
        using var archive = ReaderFactory.OpenReader(source);
        var count = 0;
        while (archive.MoveToNextEntry())
        {
            if (archive.Entry.IsDirectory)
            {
                continue;
            }

            using var content = archive.OpenEntryStream();
            content.CopyTo(_sink);
            count++;
        }

        var result = new ArchiveDigest(count, _sink.Length, _sink.Hash);
        ZipChecks.ManySmall(LibraryCatalog.SharpCompress, result);
        return result;
    }
}
