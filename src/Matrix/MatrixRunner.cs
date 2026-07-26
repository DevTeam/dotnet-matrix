namespace Matrix;

public enum MatrixMode
{
    Validation,
    Benchmark
}

public static class MatrixNames
{
    public const string RunConfigurationsCommand = "generate-run-configurations";
    public const string MetadataCommand = "generate-metadata";
    public const string RenderReportsCommand = "render-reports";
    public const string ReadmeCommand = "readme";
    public const string PrepareCommitCommand = "prepare-commit";
    public const string FinalizeCommitCommand = "finalize-commit";
    public const string CiReportsCommand = "ci-reports";
    public const string BuildWebCommand = "build-web";
    public const string BuildWebConfiguration = "Build .NET Matrix";
    public const string MetadataConfiguration = "Generate Metadata";
    public const string RenderReportsConfiguration = "Render Reports";
    public const string ReadmeConfiguration = "Generate README";
    public const string PrepareCommitConfiguration = "Prepare Commit";
    public const string FinalizeCommitConfiguration = "Finalize Commit";

    public static string Command(MatrixModule module, MatrixMode mode) =>
        $"{module.Id}-{(mode == MatrixMode.Validation ? "validate" : "benchmarks")}";

    public static string UpdateLibraryCommand(MatrixModule module) =>
        $"{module.Id}-update-library";

    public static string Configuration(MatrixModule module, MatrixMode mode, string library) =>
        $"{module.RunConfigurationPrefix} - "
        + $"{(mode == MatrixMode.Validation ? "Validate" : "Benchmark")} - {library}";

    public static string UpdateLibraryConfiguration(MatrixModule module, string library) =>
        $"{module.RunConfigurationPrefix} - Update - {library}";
}

public interface IMatrixRunner
{
    string DefaultOutputFile { get; }

    int Run(IReadOnlyList<MatrixLibrary> libraries, RunnerOptions options);
}

public interface IRunnerOptionsParser
{
    RunnerOptions Parse(string[] args, string defaultOutput);
}

public sealed record RunnerOptions(
    string OutputFile,
    IReadOnlyList<string> Libraries,
    bool Smoke);

public sealed class RunnerOptionsParser : IRunnerOptionsParser
{
    public RunnerOptions Parse(string[] args, string defaultOutput)
    {
        var output = defaultOutput;
        var libraries = new List<string>();
        var smoke = false;
        for (var index = 0; index < args.Length; index++)
        {
            switch (args[index])
            {
                case "--output" when index + 1 < args.Length:
                    output = args[++index];
                    break;

                case "--libraries" when index + 1 < args.Length:
                    while (index + 1 < args.Length && !args[index + 1].StartsWith("--", StringComparison.Ordinal))
                    {
                        libraries.Add(args[++index]);
                    }

                    break;

                case "--smoke":
                    smoke = true;
                    break;

                default:
                    throw new ArgumentException($"Unknown or incomplete argument '{args[index]}'.");
            }
        }

        return new RunnerOptions(output, libraries, smoke);
    }
}
