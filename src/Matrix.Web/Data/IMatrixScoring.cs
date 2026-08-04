// ReSharper disable CheckNamespace
namespace Matrix.Web;

/// <summary>
/// The rating rule as the interface sees it: the standings, the place a library
/// holds, the points behind a number and the way a number is written down.
/// See workflows/rating.md for the rule itself.
/// </summary>
internal interface IMatrixScoring
{
    /// <summary>How many places carry a medal.</summary>
    int Places { get; }

    /// <summary>Execution time and allocated memory, weighted the same.</summary>
    int Metrics { get; }

    /// <summary>What the best result of one scenario on one metric is worth.</summary>
    int MaximumPoints { get; }

    /// <summary>Whole points above ten, because a table reads a score against its maximum.</summary>
    string Format(double points);

    /// <summary>
    /// One decimal above ten, for a breakdown that has to add up to the total
    /// printed beside it.
    /// </summary>
    string FormatExact(double points);

    IReadOnlyList<MatrixMedals> Rating(
        CategoryReport report,
        IReadOnlySet<string> selectedLibraries);

    /// <summary>
    /// Where the library stands in the category rating, counting from one, or null
    /// when it does not take part.
    /// </summary>
    int? Place(IReadOnlyList<MatrixMedals> rating, string libraryId);

    MatrixMedals? Standing(IReadOnlyList<MatrixMedals> rating, string libraryId);

    string Hint(
        CategoryReport report,
        IReadOnlyList<BenchmarkReportEntry> features,
        IReadOnlySet<string> selectedLibraries,
        string libraryId,
        bool? metric);
}
