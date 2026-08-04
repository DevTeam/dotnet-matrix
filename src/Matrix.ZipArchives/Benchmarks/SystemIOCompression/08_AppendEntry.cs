using System.IO.Compression;
using System.Text;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class AppendEntry
{
    [Benchmark(Baseline = true)]
    [LibraryBenchmark(LibraryCatalog.SystemIOCompression)]
    public byte[] SystemIOCompression()
    {
        using var destination = new MemoryStream(_archive.Length + ZipData.AppendedContent.Length + 512);
        destination.Write(_archive);
        destination.Position = 0;
        using (var archive = new ZipArchive(destination, ZipArchiveMode.Update, true, Encoding.UTF8))
        {
            var entry = archive.CreateEntry(ZipData.AppendedEntryName, CompressionLevel.NoCompression);
            using var content = entry.Open();
            content.Write(ZipData.AppendedContent);
        }

        var result = destination.ToArray();
        ZipChecks.Appended(LibraryCatalog.SystemIOCompression, result);
        return result;
    }
}
