namespace Matrix;

public sealed class MatrixRunnerSelector(
    MatrixRunMode mode,
    MatrixFeatureValidationRunner validationRunner,
    MatrixBenchmarkRunner benchmarkRunner) : IMatrixRunner
{
    private IMatrixRunner Runner => mode switch
    {
        MatrixRunMode.Validation => validationRunner,
        MatrixRunMode.Benchmark => benchmarkRunner,
        _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
    };

    public string DefaultOutputFile => Runner.DefaultOutputFile;

    public int Run(IReadOnlyList<MatrixLibrary> libraries, RunnerOptions options) =>
        Runner.Run(libraries, options);
}
