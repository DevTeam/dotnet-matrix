using SharpCompress.Archives.Zip;
using SharpCompress.Readers;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class ReadAesEncryptedEntry
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpCompress)]
    public ArchiveDigest SharpCompress()
    {
        _sink.Reset();
        using var source = new MemoryStream(_archive, false);
        using var archive = ZipArchive.OpenArchive(
            source,
            ReaderOptions.ForExternalStream.WithPassword(ZipData.Password));
        var entry = archive.Entries.Single();
        using var content = entry.OpenEntryStream();
        content.CopyTo(_sink);
        var result = new ArchiveDigest(1, _sink.Length, _sink.Hash);
        ZipChecks.Encrypted(LibraryCatalog.SharpCompress, result);
        return result;
    }
}
