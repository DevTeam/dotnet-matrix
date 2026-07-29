namespace Matrix;

public interface IMatrixRunner
{
    string DefaultOutputFile { get; }

    int Run(IReadOnlyList<MatrixLibrary> libraries, RunnerOptions options);
}