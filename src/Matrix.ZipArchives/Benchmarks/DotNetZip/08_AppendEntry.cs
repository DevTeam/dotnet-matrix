using Ionic.Zip;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.ZipArchives.Benchmarks;

public partial class AppendEntry
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.DotNetZip)]
    public byte[] DotNetZip()
    {
        using var source = new MemoryStream(_archive, false);
        using var archive = ZipFile.Read(source);
        archive
            .AddEntry(ZipData.AppendedEntryName, ZipData.AppendedContent)
            .CompressionMethod = CompressionMethod.None;
        using var destination = new MemoryStream(_archive.Length + ZipData.AppendedContent.Length + 512);
        archive.Save(destination);
        var result = destination.ToArray();
        ZipChecks.Appended(LibraryCatalog.DotNetZip, result);
        return result;
    }
}
