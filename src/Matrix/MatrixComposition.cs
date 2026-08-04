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
            .Singleton(context => {
                context.Inject(out Assembly moduleAssembly);
                return MatrixMetadata.Read(moduleAssembly);
            })
            .Singleton((Assembly moduleAssembly) => new MatrixModuleAssembly(moduleAssembly))
            .Singleton<JsonSerializerWrapper, RunnerOptionsParser, MatrixLibraryCatalog, MatrixReportStore, BenchmarkEnvironmentProvider, MatrixRunnerSelector>();
}
