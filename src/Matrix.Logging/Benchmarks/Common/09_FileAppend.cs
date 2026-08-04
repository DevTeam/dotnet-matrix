namespace Matrix.Logging.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[FeatureUnavailable(
    LibraryCatalog.MicrosoftExtensionsLogging,
    FeatureStatus.Unsupported,
    "Microsoft.Extensions.Logging defines no file provider in the core package.")]
[MatrixFeature(
    "FileAppend",
    9,
    "File Append",
    "Appends one event to an already open file with buffering enabled and no flush per event.")]
public partial class FileAppend
{
    private static readonly string _workDirectory =
        Path.Combine(Path.GetTempPath(), "matrix-logging-fileappend");

    private static string CreateFilePath(string library)
    {
        Directory.CreateDirectory(_workDirectory);
        return Path.Combine(_workDirectory, $"{library}.{Guid.NewGuid():N}.log");
    }

    /// <summary>
    /// Reads the file back once every arm has closed its writer, so delivery is validated against
    /// what actually reached the disk rather than against an in-memory sink.
    /// </summary>
    private static void Verify(string library, string path)
    {
        string[] lines = File.Exists(path) ? File.ReadAllLines(path) : [];
        LoggingChecks.FileAppended(
            library,
            lines.Length,
            lines.Length == 0 ? null : lines[^1]);

        try
        {
            File.Delete(path);
        }
        catch (IOException)
        {
            // Each arm closes its writer before this runs, so this is unexpected - but a leftover
            // file in the temp directory is not worth failing a benchmark over.
        }
    }
}
