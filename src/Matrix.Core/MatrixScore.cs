namespace Matrix;

/// <summary>
/// What one library earned over a set of scenarios. The category rating scores
/// every scenario of the report; an overview group scores the scenarios of that
/// group. Both go through <see cref="MatrixScores"/>, so the two ratings cannot
/// drift apart in method.
/// </summary>
public sealed record MatrixScore(double Time, double Memory, int Covered);
