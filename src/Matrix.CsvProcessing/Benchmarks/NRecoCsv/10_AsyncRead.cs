// ReSharper disable CheckNamespace
namespace Matrix.CsvProcessing.Benchmarks;

[FeatureUnavailable(
    LibraryCatalog.NRecoCsv,
    FeatureStatus.Unsupported,
    "NReco.Csv.CsvReader exposes only the synchronous Read method; it has no asynchronous reading API.")]
public partial class AsyncRead;
