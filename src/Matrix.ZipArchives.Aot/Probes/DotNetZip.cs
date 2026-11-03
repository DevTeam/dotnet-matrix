using System.Text;
using Ionic.Zip;

namespace Matrix.ZipArchives.Aot;

internal static class AotProbe
{
    public const string Library = "DotNetZip";

    public const int ExpectedEvents = 1;

    private const string EntryName = "probe.txt";

    private static readonly byte[] Content = Encoding.UTF8.GetBytes("Matrix.ZipArchives.Aot probe");

    /// <summary>
    /// Writes one stored entry to an in-memory archive and reads it back. Mirrors
    /// <c>Matrix.ZipArchives.Benchmarks.CreateStoredArchive.DotNetZip</c> and
    /// <c>ReadStoredEntry.DotNetZip</c>, minus anything the matrix owns, so that what is probed is
    /// the library's own behaviour under Native AOT.
    /// </summary>
    public static int Run()
    {
        using var buffer = new MemoryStream();
        using (var archive = new ZipFile
               {
                   AlternateEncoding = Encoding.UTF8,
                   AlternateEncodingUsage = ZipOption.Always
               })
        {
            archive.AddEntry(EntryName, Content).CompressionMethod = CompressionMethod.None;
            archive.Save(buffer);
        }

        buffer.Position = 0;
        using var reader = ZipFile.Read(buffer, new ReadOptions { Encoding = Encoding.UTF8 });
        var entry = reader[EntryName];
        if (entry is null || entry.FileName != EntryName)
        {
            return 0;
        }

        using var read = entry.OpenReader();
        using var sink = new MemoryStream();
        read.CopyTo(sink);
        return sink.ToArray().AsSpan().SequenceEqual(Content) ? 1 : 0;
    }
}
