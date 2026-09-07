using Build.Targets;
using System.Text.Json;
// ReSharper disable UseCollectionExpression

namespace Matrix.Tests;

internal static class RegressionTests
{
    private static readonly BenchmarkEnvironment Environment = new(
        "test-environment", "test-os", "X64", "X64", ".NET 10", "10.0", "test-x64",
        "10.0", "test-cpu", 1, false, 1000, "stub", "1", "Smoke");

    public static async Task<int> RunAsync()
    {
        (string Name, Func<Task> Run)[] tests =
        [
            ("Core has no BenchmarkDotNet dependency", () => Sync(CoreDependencies)),
            ("Report reads preserve JSON options and missing-file behavior", () => Sync(ReportReads)),
            ("Runner merges partial results and includes baselines", () => Sync(PartialRun)),
            ("Runner rejects duplicate results", () => Sync(DuplicateResults)),
            ("Executor failure preserves reports and cleans artifacts", () => Sync(ExecutorFailure)),
            ("Unsuccessful measurements produce a failed report", () => Sync(FailedMeasurement)),
            ("Presentation targets read injected reports", PresentationReadsAsync)
        ];
        var failed = 0;
        foreach (var (name, run) in tests)
        {
            try
            {
                await run();
                Console.WriteLine($"PASS: {name}");
            }
            catch (Exception error)
            {
                failed++;
                await Console.Error.WriteLineAsync($"FAIL: {name}: {error}");
            }
        }

        Console.WriteLine($"{tests.Length - failed}/{tests.Length} passed.");
        return failed == 0 ? 0 : 1;
    }

    private static Task Sync(Action test)
    {
        test();
        return Task.CompletedTask;
    }

    private static void CoreDependencies()
    {
        var core = typeof(BenchmarkReport).Assembly;
        Check(core.GetName().Name == "Matrix.Core", "Report models must live in Core.");
        Check(core.GetReferencedAssemblies().All(assembly =>
            assembly.Name != "Matrix" && !assembly.Name!.StartsWith("BenchmarkDotNet")),
            "Core must not depend on the execution infrastructure.");
    }

    private static void ReportReads()
    {
        var directory = TemporaryDirectory();
        try
        {
            var path = Path.Combine(directory, "report.json");
            var store = new MatrixReportStore(new JsonSerializerWrapper());
            Check(!store.Exists(path) && store.Read<BenchmarkReport>(path) is null, "Missing reports return null.");
            File.WriteAllText(path, """{"schemaVersion":"2","moduleId":"test","generatedAtUtc":"2026-01-01T00:00:00Z","libraries":[],"features":[]}""");
            var report = store.Read<BenchmarkReport>(path, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            Check(report is { SchemaVersion: 2, ModuleId: "test" }, "Web options must accept camelCase and numeric strings.");
            store.Write(path, report);
            Check(store.Read<BenchmarkReport>(path) is { SchemaVersion: 2, ModuleId: "test", Features.Count: 0 },
                "Default read must round-trip written reports.");
            File.WriteAllText(path, "null");
            Check(store.Exists(path) && store.Read<BenchmarkReport>(path) is null, "Existing null reports remain distinguishable from missing files.");
            File.WriteAllText(path, "{");
            Throws<JsonException>(() => store.Read<BenchmarkReport>(path));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    private static void PartialRun()
    {
        var module = Module();
        var store = new MemoryReportStore();
        var path = Path.Combine(Path.GetTempPath(), $"matrix-test-{Guid.NewGuid():N}.json");
        store.Write(path, new BenchmarkReport(2, module.Id, DateTimeOffset.UtcNow, [Environment],
            [new BenchmarkLibrary("other", "Other", null, null, false)],
            [new BenchmarkReportEntry(1, "feature", "Feature", [Result("other", 30)])]));
        var executor = Executor(Result("selected", 10), Result("baseline", 20));
        var runner = Runner(module, store, executor);
        Check(runner.Run([module.Libraries[1]], new RunnerOptions(path, [], true, null)) == 0, "Run must succeed.");
        Check(executor.Smoke && executor.LibraryIds!.SetEquals(["selected", "baseline"]), "Selected libraries must include the baseline.");
        var report = store.Read<BenchmarkReport>(path)!;
        Check(report.Features.Single().Results.Count == 3, "Unselected results must survive a partial run.");
        Check(report.Features.Single().Results.Single(result => result.LibraryId == "selected") is
            { MeanNanoseconds: 10, EnvironmentId: "test-environment", EvidenceId: not null }, "Measured values and evidence must be retained.");
        Check(!Directory.Exists(executor.ArtifactsDirectory), "Temporary artifacts must be removed.");
    }

    private static void DuplicateResults()
    {
        var store = new MemoryReportStore();
        var executor = Executor(Result("baseline", 1), Result("baseline", 2));
        var module = Module();
        Throws<InvalidOperationException>(() => Runner(module, store, executor).Run(
            module.Libraries, new RunnerOptions("unused-report.json", [], true, null)));
        Check(store.Writes == 0, "Invalid results must not overwrite reports.");
        Check(!Directory.Exists(executor.ArtifactsDirectory), "Rejected runs must clean temporary artifacts.");
    }

    private static void ExecutorFailure()
    {
        var store = new MemoryReportStore();
        var executor = Executor();
        executor.Error = new InvalidOperationException("execution failed");
        var module = Module();
        Throws<InvalidOperationException>(() => Runner(module, store, executor).Run(
            module.Libraries, new RunnerOptions("unused-report.json", [], true, null)));
        Check(store.Writes == 0, "Execution failure must not overwrite reports.");
        Check(!Directory.Exists(executor.ArtifactsDirectory), "Failed executions must clean temporary artifacts.");
    }

    private static void FailedMeasurement()
    {
        var store = new MemoryReportStore();
        var executor = Executor(Result("baseline", 1) with { Successful = false });
        var module = Module();
        Check(Runner(module, store, executor).Run(module.Libraries,
            new RunnerOptions("unused-report.json", [], true, null)) == 1, "Failed measurements must fail the run.");
        Check(!store.Read<BenchmarkReport>("unused-report.json")!.Features.Single().Results.Single().Successful,
            "The report must record unsuccessful measurements.");
    }

    private static async Task PresentationReadsAsync()
    {
        var directory = TemporaryDirectory();
        try
        {
            var module = Module();
            var fixture = new PresentationFixture(directory);
            var store = new MemoryReportStore { RequireWebOptions = true };
            store.Write(Path.Combine(directory, "reports", module.ReportDirectory, "benchmarks.json"),
                new BenchmarkReport(2, module.Id, DateTimeOffset.UtcNow, [], [], []));
            store.Write(Path.Combine(directory, "metadata", module.ReportDirectory, "charts.json"), new MatrixChartCatalog(1, []));
            store.Write(Path.Combine(directory, "metadata", module.ReportDirectory, "libraries.json"), module.LibraryMetadata);
            var scores = new MatrixScores(new MatrixReportInvariants());
            var overviews = new MatrixOverviews(scores);
            var ratings = new MatrixRatings(scores, overviews);
            DiscoveredMatrixModule[] modules = [new(module, "unused.csproj", "unused.dll")];
            var charts = new ReportChartsTarget(fixture, store, overviews, scores, ratings);
            Check(charts.Run(modules) == 0 && store.Reads == 2, "Charts must read reports from the injected reader.");
            var readme = new ReadmeTarget(fixture, fixture, fixture, fixture, store, scores, ratings);
            Check(await readme.RunAsync(modules, CancellationToken.None) == 0, "README generation must succeed.");
            Check(fixture.Model?.Categories.Count == 1 && store.Reads == 5, "README must include in-memory reports without optional features.json.");
            Check(!Directory.Exists(Path.Combine(directory, "metadata")), "The input reports must exist only in memory.");
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    private static MatrixModule Module() => new("test", "Test", "Test", "Test",
        [new MatrixLibrary("baseline", "Baseline", null, null, true), new MatrixLibrary("selected", "Selected", null, null, false),
            new MatrixLibrary("other", "Other", null, null, false)], new MatrixLibraryMetadataCatalog(1, []), new MatrixFeatureCatalog(1, []));

    private static BenchmarkResult Result(string id, double mean) => new(id, true, mean, 0, 0, Environment.Id);

    private static StubBenchmarkExecutor Executor(params BenchmarkResult[] results) => new(
        new BenchmarkExecution(Environment, results.Select(result => new CapturedBenchmarkResult(1, "feature", "Feature", result)).ToArray()));

    private static MatrixBenchmarkRunner Runner(MatrixModule module, IMatrixReportStore store, IBenchmarkExecutor executor) =>
        new(module, new MatrixModuleAssembly(typeof(RegressionTests).Assembly), store, executor, new JsonSerializerWrapper(), new MatrixReportInvariants());

    private static string TemporaryDirectory() => Directory.CreateTempSubdirectory("matrix-tests-").FullName;

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private static void Throws<T>(Action action) where T : Exception
    {
        try { action(); }
        catch (T) { return; }
        throw new InvalidOperationException($"Expected {typeof(T).Name}.");
    }
}
