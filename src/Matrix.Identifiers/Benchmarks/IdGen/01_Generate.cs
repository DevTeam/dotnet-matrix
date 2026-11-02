// ReSharper disable CheckNamespace
namespace Matrix.Identifiers.Benchmarks;

public partial class Generate
{
    private readonly global::IdGen.IdGenerator _idGenGenerator = IdentifierData.CreateGenerator();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.IdGen)]
    public long IdGen()
    {
#if MATRIX_VALIDATION
        // IdGen's default clock is stopwatch-based and can differ from UtcNow.
        // Compare against the same source and epoch used to encode the identifier.
        var timeSource = _idGenGenerator.Options.TimeSource;
        var before = timeSource.Epoch
            .AddTicks(timeSource.GetTicks() * timeSource.TickDuration.Ticks)
            .ToUnixTimeMilliseconds();
#endif
        var id = _idGenGenerator.CreateId();
#if MATRIX_VALIDATION
        IdentifierChecks.TimestampAfter(
            LibraryCatalog.IdGen,
            _idGenGenerator.FromId(id).DateTimeOffset.ToUnixTimeMilliseconds(),
            before);
#endif
        return id;
    }
}
