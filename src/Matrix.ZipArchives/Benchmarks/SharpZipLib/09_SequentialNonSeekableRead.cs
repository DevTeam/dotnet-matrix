using ICSharpCode.SharpZipLib.Zip;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class SequentialNonSeekableRead
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpZipLib)]
    public ArchiveDigest SharpZipLib()
    {
        _sink.Reset();
        using var source = new NonSeekableReadStream(_archive);
        using var archive = new ZipInputStream(source);
        var count = 0;
        while (archive.GetNextEntry() is not null)
        {
            archive.CopyTo(_sink);
            count++;
        }

        var result = new ArchiveDigest(count, _sink.Length, _sink.Hash);
        ZipChecks.ManySmall(LibraryCatalog.SharpZipLib, result);
        return result;
    }
}
