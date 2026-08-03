using System.IO.Compression;
using System.Text;

namespace Matrix.ZipArchives.Benchmarks;

public partial class CreateManySmallEntries
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOCompression)]
    public byte[] SystemIOCompression()
    {
        using var destination = new MemoryStream();
        using (var archive = new ZipArchive(destination, ZipArchiveMode.Create, true, Encoding.UTF8))
        {
            foreach (var item in ZipData.ManySmallEntries)
            {
                var entry = archive.CreateEntry(item.Name, CompressionLevel.NoCompression);
                using var content = entry.Open();
                content.Write(item.Content);
            }
        }

        var result = destination.ToArray();
        ZipChecks.CreatedManySmall(LibraryCatalog.SystemIOCompression, result);
        return result;
    }
}
