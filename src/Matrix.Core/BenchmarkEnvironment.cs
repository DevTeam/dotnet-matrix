
// ReSharper disable UseCollectionExpression
// ReSharper disable UnusedType.Global
// ReSharper disable NotAccessedPositionalProperty.Global

namespace Matrix;

public sealed record BenchmarkEnvironment(
    string Id,
    string OperatingSystem,
    string OsArchitecture,
    string ProcessArchitecture,
    string Framework,
    string RuntimeVersion,
    string RuntimeIdentifier,
    string DotNetSdkVersion,
    string Processor,
    int LogicalCoreCount,
    bool ServerGarbageCollector,
    long StopwatchFrequency,
    string BenchmarkTool,
    string BenchmarkToolVersion,
    string Job);