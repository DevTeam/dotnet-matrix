using ICSharpCode.SharpZipLib.Zip;
using System.Text;

namespace Matrix.ZipArchives.Aot;

internal static class AotProbe
{
    public const string Library = "SharpZipLib";

    public const int ExpectedEvents = 1;

    private const string EntryName = "probe.txt";

    private static readonly byte[] Content = Encoding.UTF8.GetBytes("Matrix.ZipArchives.Aot probe");

    /// <summary>
    /// Writes one stored entry to an in-memory archive and reads it back. Mirrors
    /// <c>Matrix.ZipArchives.Benchmarks.CreateStoredArchive.SharpZipLib</c> and
    /// <c>ReadStoredEntry.SharpZipLib</c>, minus anything the matrix owns, so that what is probed
    /// is the library's own behaviour under Native AOT.
    /// </summary>
    public static int Run()
    {
        using var buffer = new MemoryStream();
        using (var archive = new ZipOutputStream(buffer) { IsStreamOwner = false })
        {
            archive.PutNextEntry(new ZipEntry(EntryName)
            {
                CompressionMethod = CompressionMethod.Stored,
                Size = Content.Length
            });
            archive.Write(Content);
            archive.CloseEntry();
            archive.Finish();
        }

        buffer.Position = 0;
        using var reader = new ZipInputStream(buffer);
        var found = reader.GetNextEntry();
        if (found is null || found.Name != EntryName)
        {
            return 0;
        }

        using var sink = new MemoryStream();
        reader.CopyTo(sink);
        return sink.ToArray().AsSpan().SequenceEqual(Content) ? 1 : 0;
    }
}
