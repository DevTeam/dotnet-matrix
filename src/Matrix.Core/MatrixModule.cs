namespace Matrix;

public sealed record MatrixModule(
    string Id,
    string Name,
    string RunConfigurationPrefix,
    string ReportDirectory,
    IReadOnlyList<MatrixLibrary> Libraries,
    MatrixLibraryMetadataCatalog LibraryMetadata,
    MatrixFeatureCatalog FeatureMetadata);