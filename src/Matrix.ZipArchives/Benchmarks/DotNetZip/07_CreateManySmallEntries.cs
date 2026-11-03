using System.Diagnostics.CodeAnalysis;
using Ionic.Zip;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class CreateManySmallEntries
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DotNetZip)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public byte[] DotNetZip()
    {
        using var destination = new MemoryStream();
        using (var archive = new ZipFile())
        {
            foreach (var item in ZipData.ManySmallEntries)
            {
                archive.AddEntry(item.Name, item.Content).CompressionMethod = CompressionMethod.None;
            }

            archive.Save(destination);
        }

        var result = destination.ToArray();
        ZipChecks.CreatedManySmall(LibraryCatalog.DotNetZip, result);
        return result;
    }
}
