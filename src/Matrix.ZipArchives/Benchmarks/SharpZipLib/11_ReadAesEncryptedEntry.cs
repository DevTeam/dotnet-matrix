using ICSharpCode.SharpZipLib.Zip;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class ReadAesEncryptedEntry
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpZipLib)]
    public ArchiveDigest SharpZipLib()
    {
        _sink.Reset();
        using var source = new MemoryStream(_archive, false);
        using var archive = new ZipFile(source);
        archive.Password = ZipData.Password;
        using var content = archive.GetInputStream(archive.GetEntry(ZipData.EncryptedEntryName));
        content.CopyTo(_sink);
        var result = new ArchiveDigest(1, _sink.Length, _sink.Hash);
        ZipChecks.Encrypted(LibraryCatalog.SharpZipLib, result);
        return result;
    }
}
