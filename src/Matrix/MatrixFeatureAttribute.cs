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
    string description,
    bool rated = true,
    string? reason = null) : Attribute
{
    public string Id { get; } = id;

    public int Order { get; } = order;

    public string Name { get; } = name;

    public string Description { get; } = description;

    /// <summary>
    /// Whether this scenario counts toward the category rating. False is a
    /// named, individually justified exception recorded once per scenario in
    /// its feature contract — never a computed threshold, and never
    /// recalculated as the set of competing libraries changes. See
    /// workflows/rating.md, "The Rated flag that does exist".
    /// </summary>
    public bool Rated { get; } = rated;

    /// <summary>
    /// Why <see cref="Rated"/> is false, shown wherever the scenario appears as
    /// not rated. Required when <see cref="Rated"/> is false — a flag with no
    /// stated reason is exactly the "feature-only" leftover this mechanism
    /// replaced.
    /// </summary>
    public string? Reason { get; } = reason;
}