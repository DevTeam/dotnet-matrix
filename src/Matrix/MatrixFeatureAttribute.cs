namespace Matrix;

/// <summary>
/// Declares a matrix feature on the class that benchmarks and validates it. The
/// description is the short answer to "what does this scenario actually measure",
/// shown next to the feature everywhere it appears.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class MatrixFeatureAttribute(
    string id,
    int order,
    string name,
    string description) : Attribute
{
    public string Id { get; } = id;

    public int Order { get; } = order;

    public string Name { get; } = name;

    public string Description { get; } = description;
}