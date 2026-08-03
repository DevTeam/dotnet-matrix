using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Writers.Zip;

namespace Matrix.ZipArchives.Benchmarks;

public partial class AppendEntry
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpCompress)]
    public byte[] SharpCompress()
    {
        using var source = new MemoryStream(_archive, false);
        using var archive = ZipArchive.OpenArchive(source);
        using var appendedContent = new MemoryStream(ZipData.AppendedContent, false);
        archive.AddEntry(
            ZipData.AppendedEntryName,
            appendedContent,
            false,
            ZipData.AppendedContent.Length,
            DateTime.UnixEpoch);
        using var destination = new MemoryStream();
        archive.SaveTo(
            destination,
            new ZipWriterOptions(CompressionType.None) { LeaveStreamOpen = true });
        var result = destination.ToArray();
        ZipChecks.Appended(LibraryCatalog.SharpCompress, result);
        return result;
    }
}
