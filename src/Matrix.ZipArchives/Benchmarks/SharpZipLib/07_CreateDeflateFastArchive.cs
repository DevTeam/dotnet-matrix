using ICSharpCode.SharpZipLib.Zip;

namespace Matrix.ZipArchives.Benchmarks;

public partial class CreateDeflateFastArchive
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpZipLib)]
    public byte[] SharpZipLib()
    {
        using var destination = new MemoryStream();
        using (var archive = new ZipOutputStream(destination) { IsStreamOwner = false })
        {
            archive.SetLevel(1);
            foreach (var item in ZipData.MixedEntries)
            {
                archive.PutNextEntry(new ZipEntry(item.Name) { Size = item.Content.Length });
                archive.Write(item.Content);
                archive.CloseEntry();
            }

            archive.Finish();
        }

        var result = destination.ToArray();
        ZipChecks.CreatedMixed(LibraryCatalog.SharpZipLib, result);
        return result;
    }
}
