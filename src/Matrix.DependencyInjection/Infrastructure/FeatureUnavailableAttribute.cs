namespace Matrix.DependencyInjection.Infrastructure;

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