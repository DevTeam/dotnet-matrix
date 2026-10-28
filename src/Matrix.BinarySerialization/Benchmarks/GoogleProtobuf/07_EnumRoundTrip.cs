using System.Diagnostics.CodeAnalysis;
using Google.Protobuf;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Matrix.BinarySerialization.Benchmarks;

public partial class EnumRoundTrip
{
    private EnumMessage _message = new();

    public void SetupGoogleProtobuf() => _message = SerializationData.EnumMessage();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.GoogleProtobuf)]
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public EnumMessage GoogleProtobuf()
    {
        var payload = _message.ToByteArray();
        var result = EnumMessage.Parser.ParseFrom(payload);
        SerializationChecks.EnumMessage(LibraryCatalog.GoogleProtobuf, result);
        return result;
    }
}
