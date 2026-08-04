using System.Diagnostics.CodeAnalysis;
using ICSharpCode.SharpZipLib.Zip;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class CreateManySmallEntries
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpZipLib)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] SharpZipLib()
    {
        using var destination = new MemoryStream();
        using (var archive = new ZipOutputStream(destination) { IsStreamOwner = false })
        {
            foreach (var item in ZipData.ManySmallEntries)
            {
                archive.PutNextEntry(new ZipEntry(item.Name)
                {
                    CompressionMethod = CompressionMethod.Stored,
                    Size = item.Content.Length
                });
                archive.Write(item.Content);
                archive.CloseEntry();
            }

            archive.Finish();
        }

        var result = destination.ToArray();
        ZipChecks.CreatedManySmall(LibraryCatalog.SharpZipLib, result);
        return result;
    }
}
