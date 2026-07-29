using System.Diagnostics;
using System.Reflection;
using Pure.DI;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
namespace Matrix;

internal partial class MatrixComposition
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Hint(Hint.ThreadSafe, "Off")
            .Hint(Hint.Resolve, "Off")
            .Hint(Hint.ToString, "Off")
            .Root<MatrixApplication>(nameof(Root))
            .Arg<string[]>("args")
            .Arg<Assembly>("moduleAssembly")
            .Arg<MatrixRunMode>("mode")
            .Bind<MatrixModule>().As(Lifetime.Singleton)
                .To(context =>
                {
                    context.Inject(out Assembly moduleAssembly);
                    return MatrixMetadata.Read(moduleAssembly);
                })
            .Bind<MatrixModuleAssembly>().As(Lifetime.Singleton)
                .To((Assembly moduleAssembly) => new MatrixModuleAssembly(moduleAssembly))
            .Bind<IRunnerOptionsParser>().As(Lifetime.Singleton).To<RunnerOptionsParser>()
            .Bind<IMatrixLibraryCatalog>().As(Lifetime.Singleton).To<MatrixLibraryCatalog>()
            .Bind<IMatrixReportStore>().As(Lifetime.Singleton).To<MatrixReportStore>()
            .Bind<IBenchmarkEnvironmentProvider>().As(Lifetime.Singleton)
                .To<BenchmarkEnvironmentProvider>()
            .Bind<IMatrixRunner>().As(Lifetime.Singleton).To<MatrixRunnerSelector>();
}
