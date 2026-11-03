using System.Diagnostics.CodeAnalysis;
using System.Text;
using Ionic.Zip;
using Ionic.Zlib;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class CreateDeflateFastArchive
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DotNetZip)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] DotNetZip()
    {
        using var destination = new MemoryStream();
        using (var archive = new ZipFile
               {
                   CompressionLevel = CompressionLevel.Level1,
                   AlternateEncoding = Encoding.UTF8,
                   AlternateEncodingUsage = ZipOption.Always
               })
        {
            foreach (var item in ZipData.MixedEntries)
            {
                archive.AddEntry(item.Name, item.Content);
            }

            archive.Save(destination);
        }

        var result = destination.ToArray();
        ZipChecks.CreatedMixed(LibraryCatalog.DotNetZip, result);
        return result;
    }
}
