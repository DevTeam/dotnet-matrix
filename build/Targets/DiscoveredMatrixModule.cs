using Matrix;

namespace Build.Targets;

// ReSharper disable once NotAccessedPositionalProperty.Global
internal sealed record DiscoveredMatrixModule(
    MatrixModule Metadata,
    string ProjectPath,
    string AssemblyPath);
