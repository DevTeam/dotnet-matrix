using ICSharpCode.SharpZipLib.Zip;

namespace Matrix.ZipArchives.Infrastructure;

internal sealed class ByteArrayDataSource(byte[] data) : IStaticDataSource
{
    public Stream GetSource() => new MemoryStream(data, false);
}
