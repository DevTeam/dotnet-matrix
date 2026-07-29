namespace Matrix;

public interface IMatrixReportStore
{
    T? Read<T>(string fileName);

    void Write<T>(string fileName, T value);

    void WarnEnvironmentMismatch(
        IReadOnlyCollection<BenchmarkEnvironment> existing,
        BenchmarkEnvironment current);
}