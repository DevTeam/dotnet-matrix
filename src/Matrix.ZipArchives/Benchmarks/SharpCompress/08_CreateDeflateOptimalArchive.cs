using SharpCompress.Common;
using SharpCompress.Writers.Zip;

namespace Matrix.ZipArchives.Benchmarks;

public partial class CreateDeflateOptimalArchive
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpCompress)]
    public byte[] SharpCompress()
    {
        using var destination = new MemoryStream();
        using (var archive = new ZipWriter(
                   destination,
                   new ZipWriterOptions(CompressionType.Deflate, 6) { LeaveStreamOpen = true }))
        {
            foreach (var item in ZipData.MixedEntries)
            {
                using var content = new MemoryStream(item.Content, false);
                archive.Write(item.Name, content, DateTime.UnixEpoch);
            }
        }

        var result = destination.ToArray();
        ZipChecks.CreatedMixed(LibraryCatalog.SharpCompress, result);
        return result;
    }
}
