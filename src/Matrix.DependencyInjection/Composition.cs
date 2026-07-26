// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
namespace Matrix.DependencyInjection;

internal partial class Composition
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Root<Application>(nameof(Root))
            .Arg<string[]>("args")
            .Bind<MatrixModule>().As(Lifetime.Singleton)
                .To(_ => MatrixMetadata.Read(typeof(Composition).Assembly))
            .Bind<IRunnerOptionsParser>().As(Lifetime.Singleton).To<RunnerOptionsParser>()
            .Bind<IMatrixLibraryCatalog>().As(Lifetime.Singleton).To<MatrixLibraryCatalog>()
            .Bind<IMatrixReportStore>().As(Lifetime.Singleton).To<MatrixReportStore>()
#if MATRIX_VALIDATION
            .Bind<IMatrixRunner>().As(Lifetime.Singleton)
                .To<Validation.FeatureValidationRunner>();
#elif MATRIX_BENCHMARK
            .Bind<IBenchmarkEnvironmentProvider>().As(Lifetime.Singleton)
                .To<BenchmarkEnvironmentProvider>()
            .Bind<IMatrixRunner>().As(Lifetime.Singleton)
                .To<Benchmarking.BenchmarkRun>();
#else
#error MatrixMode must be Validation or Benchmark.
#endif
}
