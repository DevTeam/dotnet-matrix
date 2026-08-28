using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Writers.Zip;
using System.Text;

namespace Matrix.ZipArchives.Aot;

internal static class AotProbe
{
    public const string Library = "SharpCompress";

    public const int ExpectedEvents = 1;

    private const string EntryName = "probe.txt";

    private static readonly byte[] Content = Encoding.UTF8.GetBytes("Matrix.ZipArchives.Aot probe");

    /// <summary>
    /// Writes one stored entry to an in-memory archive and reads it back. Mirrors
    /// <c>Matrix.ZipArchives.Benchmarks.CreateStoredArchive.SharpCompress</c> and
    /// <c>ReadStoredEntry.SharpCompress</c>, minus anything the matrix owns, so that what is
    /// probed is the library's own behaviour under Native AOT.
    /// </summary>
    public static int Run()
    {
        using var buffer = new MemoryStream();
        using (var archive = new ZipWriter(
                   buffer,
                   new ZipWriterOptions(CompressionType.None) { LeaveStreamOpen = true }))
        {
            using var content = new MemoryStream(Content, false);
            archive.Write(EntryName, content, DateTime.UnixEpoch);
        }

        buffer.Position = 0;
        using var reader = ZipArchive.OpenArchive(buffer);
        var entry = reader.Entries.Single();
        if (entry.Key != EntryName)
        {
            return 0;
        }

        using var read = entry.OpenEntryStream();
        using var sink = new MemoryStream();
        read.CopyTo(sink);
        return sink.ToArray().AsSpan().SequenceEqual(Content) ? 1 : 0;
    }
}
