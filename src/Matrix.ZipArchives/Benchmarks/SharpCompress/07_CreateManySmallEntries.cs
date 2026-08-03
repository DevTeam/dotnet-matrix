using SharpCompress.Common;
using SharpCompress.Writers.Zip;

namespace Matrix.ZipArchives.Benchmarks;

public partial class CreateManySmallEntries
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpCompress)]
    public byte[] SharpCompress()
    {
        using var destination = new MemoryStream();
        using (var archive = new ZipWriter(
                   destination,
                   new ZipWriterOptions(CompressionType.None) { LeaveStreamOpen = true }))
        {
            foreach (var item in ZipData.ManySmallEntries)
            {
                using var content = new MemoryStream(item.Content, false);
                archive.Write(item.Name, content, DateTime.UnixEpoch);
            }
        }

        var result = destination.ToArray();
        ZipChecks.CreatedManySmall(LibraryCatalog.SharpCompress, result);
        return result;
    }
}
