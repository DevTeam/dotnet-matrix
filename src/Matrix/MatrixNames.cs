namespace Matrix;

public static class MatrixNames
{
    public const string RunConfigurationsCommand = "generate-run-configurations";
    public const string MetadataCommand = "generate-metadata";
    public const string RenderReportsCommand = "render-reports";
    public const string ReadmeCommand = "readme";
    public const string PrepareCommitCommand = "prepare-commit";
    public const string FinalizeCommitCommand = "finalize-commit";
    public const string CiMatrixCommand = "ci-matrix";
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
