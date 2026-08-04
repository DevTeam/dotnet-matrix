using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Text;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class CreateDeflateFastArchive
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOCompression)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] SystemIOCompression()
    {
        using var destination = new MemoryStream();
        using (var archive = new ZipArchive(destination, ZipArchiveMode.Create, true, Encoding.UTF8))
        {
            foreach (var item in ZipData.MixedEntries)
            {
                var entry = archive.CreateEntry(item.Name, CompressionLevel.Fastest);
                using var content = entry.Open();
                content.Write(item.Content);
            }
        }

        var result = destination.ToArray();
        ZipChecks.CreatedMixed(LibraryCatalog.SystemIOCompression, result);
        return result;
    }
}
