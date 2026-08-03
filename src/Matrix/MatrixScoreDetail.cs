namespace Matrix;

/// <summary>
/// One scenario of a score, itemised. <see cref="MatrixScores.Explain"/> produces
/// these from the same arithmetic the rating runs, so what a tooltip shows and
/// what a table totals cannot drift apart.
/// </summary>
public sealed record MatrixScoreDetail(
    string FeatureId,
    string Name,
    MatrixScoreCell Time,
    MatrixScoreCell Memory)
{
    public double Points => Time.Points + Memory.Points;

    /// <summary>The library produced at least one measurement here.</summary>
    public bool Covered => Time.Measured || Memory.Measured;
}
