namespace Matrix.DependencyInjection.Infrastructure;

[AttributeUsage(AttributeTargets.Class)]
public sealed class FeatureBenchmarkAttribute(
    FeatureId id,
    int order,
    string name) : Attribute
{
    public FeatureId Id { get; } = id;

    public int Order { get; } = order;

    public string Name { get; } = name;
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class LibraryBenchmarkAttribute(
    string libraryId,
    bool baseline = false) : Attribute
{
    public string LibraryId { get; } = libraryId;

    public bool Baseline { get; } = baseline;
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class ReportedBenchmarkAttribute(
    double meanNanoseconds = 0,
    double allocatedBytesPerOperation = 0) : Attribute
{
    public double MeanNanoseconds { get; } = meanNanoseconds;

    public double AllocatedBytesPerOperation { get; } = allocatedBytesPerOperation;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class FeatureUnavailableAttribute(
    string libraryId,
    FeatureStatus status,
    string reason) : Attribute
{
    public string LibraryId { get; } = libraryId;

    public FeatureStatus Status { get; } = status;

    public string Reason { get; } = reason;
}
