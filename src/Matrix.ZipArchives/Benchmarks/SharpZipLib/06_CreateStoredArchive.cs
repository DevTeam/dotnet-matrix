using ICSharpCode.SharpZipLib.Zip;

namespace Matrix.ZipArchives.Benchmarks;

public partial class CreateStoredArchive
{
    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SharpZipLib)]
    public byte[] SharpZipLib()
    {
        using var destination = new MemoryStream();
        using (var archive = new ZipOutputStream(destination) { IsStreamOwner = false })
        {
            foreach (var item in ZipData.MixedEntries)
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
        ZipChecks.CreatedMixed(LibraryCatalog.SharpZipLib, result);
        return result;
    }
}
