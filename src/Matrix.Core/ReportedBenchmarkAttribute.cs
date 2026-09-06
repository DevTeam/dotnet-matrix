namespace Matrix;

[AttributeUsage(AttributeTargets.Method)]
public sealed class ReportedBenchmarkAttribute(
    double meanNanoseconds = 0,
    double allocatedBytesPerOperation = 0) : Attribute
{
    public double MeanNanoseconds { get; } = meanNanoseconds;

    public double AllocatedBytesPerOperation { get; } = allocatedBytesPerOperation;
}
