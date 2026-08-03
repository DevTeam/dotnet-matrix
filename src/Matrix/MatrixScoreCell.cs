namespace Matrix;

/// <summary>
/// What one metric of one scenario contributed to a score, together with the two
/// measurements that produced it. The point of carrying <see cref="Best"/> and
/// <see cref="Step"/> rather than only <see cref="Points"/> is that a reader can
/// redo the arithmetic from what is shown.
/// </summary>
public sealed record MatrixScoreCell(double? Value, double? Best, double Step, double Points)
{
    /// <summary>False when the library reported nothing for this metric here.</summary>
    public bool Measured => Value is not null;

    /// <summary>True when nobody reported this metric, so it was not scored at all.</summary>
    public bool Contested => Best is not null;
}
