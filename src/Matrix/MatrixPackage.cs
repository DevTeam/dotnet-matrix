namespace Matrix;

/// <summary>
/// A NuGet package and its exact version, named as a companion of a
/// <see cref="MatrixLibrary"/> rather than as that library's own primary package.
/// </summary>
public sealed record MatrixPackage(string Id, string Version);
