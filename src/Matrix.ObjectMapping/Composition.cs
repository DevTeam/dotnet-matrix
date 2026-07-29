// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
namespace Matrix.ObjectMapping;

internal partial class Composition
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Root<MatrixApplication>(nameof(Root))
            .Arg<string[]>("args")
            .Bind<MatrixModule>().As(Lifetime.Singleton)
                .To(_ => MatrixMetadata.Read(typeof(Composition).Assembly))
            .Bind<MatrixModuleAssembly>().As(Lifetime.Singleton)
                .To(_ => new MatrixModuleAssembly(typeof(Composition).Assembly))
            .Bind<IRunnerOptionsParser>().As(Lifetime.Singleton).To<RunnerOptionsParser>()
            .Bind<IMatrixLibraryCatalog>().As(Lifetime.Singleton).To<MatrixLibraryCatalog>()
            .Bind<IMatrixReportStore>().As(Lifetime.Singleton).To<MatrixReportStore>()
#if MATRIX_VALIDATION
            .Bind<IMatrixRunner>().As(Lifetime.Singleton)
                .To<MatrixFeatureValidationRunner>();
#elif MATRIX_BENCHMARK
            .Bind<IBenchmarkEnvironmentProvider>().As(Lifetime.Singleton)
                .To<BenchmarkEnvironmentProvider>()
            .Bind<IMatrixRunner>().As(Lifetime.Singleton)
                .To<MatrixBenchmarkRunner>();
#else
#error MatrixMode must be Validation or Benchmark.
#endif
}
