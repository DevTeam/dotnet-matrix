using System.IO.Compression;
using System.Text;

namespace Matrix.ZipArchives.Aot;

internal static class AotProbe
{
    public const string Library = "System.IO.Compression";

    public const int ExpectedEvents = 1;

    private const string EntryName = "probe.txt";

    private static readonly byte[] Content = Encoding.UTF8.GetBytes("Matrix.ZipArchives.Aot probe");

    /// <summary>
    /// Writes one stored entry to an in-memory archive and reads it back. Mirrors
    /// <c>Matrix.ZipArchives.Benchmarks.CreateStoredArchive.SystemIOCompression</c> and
    /// <c>ReadStoredEntry.SystemIOCompression</c>, minus anything the matrix owns, so that what is
    /// probed is the library's own behaviour under Native AOT.
    /// </summary>
    public static int Run()
    {
        using var buffer = new MemoryStream();
        using (var archive = new ZipArchive(buffer, ZipArchiveMode.Create, true, Encoding.UTF8))
        {
            var entry = archive.CreateEntry(EntryName, CompressionLevel.NoCompression);
            using var content = entry.Open();
            content.Write(Content);
        }

        buffer.Position = 0;
        using var reader = new ZipArchive(buffer, ZipArchiveMode.Read, false);
        using var read = reader.GetEntry(EntryName)!.Open();
        using var sink = new MemoryStream();
        read.CopyTo(sink);
        return sink.ToArray().AsSpan().SequenceEqual(Content) ? 1 : 0;
    }
}
