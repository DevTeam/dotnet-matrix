using ICSharpCode.SharpZipLib.Zip;

namespace Matrix.ZipArchives.Benchmarks;

public partial class AppendEntry
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpZipLib)]
    public byte[] SharpZipLib()
    {
        using var destination = new MemoryStream(_archive.Length + ZipData.AppendedContent.Length + 512);
        destination.Write(_archive);
        destination.Position = 0;
        using (var archive = new ZipFile(destination) { IsStreamOwner = false })
        {
            archive.BeginUpdate(new MemoryArchiveStorage(FileUpdateMode.Direct));
            archive.Add(
                new ByteArrayDataSource(ZipData.AppendedContent),
                ZipData.AppendedEntryName,
                CompressionMethod.Stored);
            archive.CommitUpdate();
        }

        var result = destination.ToArray();
        ZipChecks.Appended(LibraryCatalog.SharpZipLib, result);
        return result;
    }
}
