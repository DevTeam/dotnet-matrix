using System.Globalization;
// ReSharper disable UseCollectionExpression

namespace Matrix;

/// <summary>
/// The one scoring rule of the project, applied to whatever set of scenarios it
/// is given. In every scenario the fastest library earns
/// <see cref="MaximumPoints"/> and the one allocating least earns another
/// <see cref="MaximumPoints"/>; four times slower is half the points, and a
/// scenario the library did not complete is worth nothing.
/// See workflows/rating.md.
/// </summary>
public static class MatrixScores
{
    /// <summary>What the best result of one scenario on one metric is worth.</summary>
    public const int MaximumPoints = 100;

    /// <summary>Execution time and allocated memory, weighted the same.</summary>
    public const int Metrics = 2;

    /// <summary>
    /// A source generator can complete a preparation scenario in a time that
    /// rounds to zero, and an allocation-free scenario measures zero bytes; the
    /// ratio of two zeroes is undefined. Both metrics are therefore compared at
    /// their own smallest meaningful step, which removes the singularity and
    /// makes two libraries that both measure zero equally best.
    /// A nanosecond is where the measurement itself stops.
    /// </summary>
    private const double TimeResolution = 1;

    /// <summary>
    /// Allocation is counted exactly, so a byte is not the smallest step that
    /// means anything — an object is. Twenty-four bytes is the smallest object
    /// the 64-bit runtime can allocate (header, method table, one field), so
    /// "allocates nothing" is scored one object better than "allocates something",
    /// not infinitely better. At a resolution of one byte a scenario where the
    /// leader allocates zero wiped out everyone else's score for that scenario
    /// regardless of how little they allocated.
    /// </summary>
    private const double MemoryResolution = 24;

    /// <summary>
    /// How the ratio between a result and the best result turns into points.
    /// A rating sums one score per scenario per metric, so this exponent is the
    /// exchange rate between winning once and being decent often, and there is no
    /// neutral value for it. At 1 the scale spends nine tenths of its range on the
    /// first two-fold of the field: one scenario won outright was worth some
    /// thirty scenarios completed thirty times behind, which put a library that
    /// entered six scenarios above one that entered thirteen. At one half the same
    /// perfect result is worth about five of them, and four times slower is half
    /// the points.
    /// </summary>
    private const double Curve = 0.5;

    /// <summary>The most a library can earn over <paramref name="scenarios"/>.</summary>
    public static int Maximum(int scenarios) => scenarios * MaximumPoints * Metrics;

    /// <summary>
    /// A score is only ever read against the maximum, so whole points are enough
    /// above ten. Below it the fraction is the whole story: a library three orders
    /// of magnitude behind earns 0.004, and rounding that to zero would make the
    /// entire bottom of a table look identical.
    /// </summary>
    /// Invariant, because a decimal comma would turn 5.983 points into something a
    /// reader takes for five thousand, and because a chart must not depend on the
    /// locale of the machine that rendered it.
    public static string Format(double points) =>
        points >= 10
            ? points.ToString("0", CultureInfo.InvariantCulture)
            : points.ToString("0.###", CultureInfo.InvariantCulture);

    /// <summary>
    /// The same number where it has to add up. A breakdown exists so a reader can
    /// sum it and land on the total; at whole points a scenario lost by half a
    /// percent prints as a perfect 100, four of them print as 400, and the total
    /// beside them says 399. One decimal is enough to make the arithmetic close,
    /// and a result far behind keeps the fraction that distinguishes it from zero.
    /// </summary>
    public static string FormatExact(double points) =>
        points >= 10
            ? points.ToString("0.#", CultureInfo.InvariantCulture)
            : points.ToString("0.###", CultureInfo.InvariantCulture);

    /// <summary>
    /// <see cref="Format"/>, or nothing once <paramref name="points"/> passes
    /// <paramref name="maximum"/>. A rated library can never pass its field's
    /// maximum — see workflows/rating.md — so this only ever blanks a reference
    /// row's own score once it outscores the whole rated field; there is no
    /// number on that scale left to show it against.
    /// </summary>
    public static string FormatWithinMax(double points, double maximum) =>
        points > maximum ? string.Empty : Format(points);

    public static IReadOnlyDictionary<string, MatrixScore> Create(
        IEnumerable<BenchmarkReportEntry> features,
        IEnumerable<string> libraryIds,
        Func<string, bool>? includeLibrary = null)
    {
        var score = libraryIds.ToDictionary(
            id => id,
            _ => (Time: 0d, Memory: 0d, Covered: 0),
            StringComparer.OrdinalIgnoreCase);
        foreach (var feature in features)
        {
            var results = feature.Results
                .Where(result =>
                    result.Successful && (includeLibrary?.Invoke(result.LibraryId) ?? true))
                .ToArray();
            Award(score, results, result => result.MeanNanoseconds, true);
            Award(score, results, result => result.AllocatedBytesPerOperation, false);
            foreach (var result in results)
            {
                if (result.MeanNanoseconds is null
                    && result.AllocatedBytesPerOperation is null)
                {
                    continue;
                }

                if (score.TryGetValue(result.LibraryId, out var current))
                {
                    score[result.LibraryId] = current with { Covered = current.Covered + 1 };
                }
            }
        }

        return score.ToDictionary(
            entry => entry.Key,
            entry => new MatrixScore(
                entry.Value.Time,
                entry.Value.Memory,
                entry.Value.Covered),
            StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// The same scores <see cref="Create"/> sums, itemised for one library so the
    /// number in the interface can be checked against the measurements it came
    /// from. It goes through <see cref="Award(double,double,double)"/> like the
    /// rating itself, so a breakdown cannot disagree with the total it explains.
    /// </summary>
    public static IReadOnlyList<MatrixScoreDetail> Explain(
        IEnumerable<BenchmarkReportEntry> features,
        string libraryId,
        Func<string, bool>? includeLibrary = null) =>
        features
            .Select(feature =>
            {
                var results = feature.Results.Where(result => result.Successful).ToArray();
                return new MatrixScoreDetail(
                    feature.Id,
                    feature.Name,
                    Cell(results, libraryId, result => result.MeanNanoseconds, TimeResolution, includeLibrary),
                    Cell(
                        results,
                        libraryId,
                        result => result.AllocatedBytesPerOperation,
                        MemoryResolution,
                        includeLibrary));
            })
            .ToArray();

    /// <summary>
    /// <paramref name="includeLibrary"/> decides who defines <c>best</c> — the
    /// field this cell's library is measured against — not whether its own
    /// result is looked up. A library outside that field, such as a hand-written
    /// baseline, still gets its own measurement scored against the field's best;
    /// it simply never becomes the best itself. See workflows/rating.md.
    /// </summary>
    private static MatrixScoreCell Cell(
        IReadOnlyList<BenchmarkResult> results,
        string libraryId,
        Func<BenchmarkResult, double?> metric,
        double resolution,
        Func<string, bool>? includeLibrary)
    {
        var measured = results.Where(result => metric(result) is not null).ToArray();
        var contestants = measured
            .Where(result => includeLibrary?.Invoke(result.LibraryId) ?? true)
            .ToArray();
        if (contestants.Length == 0)
        {
            return new MatrixScoreCell(null, null, resolution, 0);
        }

        var best = contestants.Min(result => metric(result)!.Value);
        var own = measured.FirstOrDefault(result =>
            result.LibraryId.Equals(libraryId, StringComparison.OrdinalIgnoreCase));
        return own is null
            ? new MatrixScoreCell(null, best, resolution, 0)
            : new MatrixScoreCell(
                metric(own)!.Value,
                best,
                resolution,
                Award(best, metric(own)!.Value, resolution));
    }

    /// <summary>
    /// One metric of one scenario. A metric nobody reported is skipped rather
    /// than scored as zero: it is not a competition.
    /// </summary>
    private static void Award(
        Dictionary<string, (double Time, double Memory, int Covered)> score,
        IReadOnlyList<BenchmarkResult> results,
        Func<BenchmarkResult, double?> metric,
        bool time)
    {
        var measured = results.Where(result => metric(result) is not null).ToArray();
        if (measured.Length == 0)
        {
            return;
        }

        var resolution = time ? TimeResolution : MemoryResolution;
        var best = measured.Min(result => metric(result)!.Value);
        foreach (var result in measured)
        {
            if (!score.TryGetValue(result.LibraryId, out var current))
            {
                continue;
            }

            var points = Award(best, metric(result)!.Value, resolution);
            score[result.LibraryId] = time
                ? current with { Time = current.Time + points }
                : current with { Memory = current.Memory + points };
        }
    }

    /// <summary>
    /// The scale itself, and the only place it exists. Both sides carry the same
    /// step, so two results that measure zero are equally best.
    /// </summary>
    private static double Award(double best, double value, double resolution) =>
        MaximumPoints * Math.Pow((best + resolution) / (value + resolution), Curve);
}
