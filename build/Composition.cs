using Build.Targets;
using Matrix;
using Pure.DI;
using System.Diagnostics;
using HostApi;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

namespace Build;

internal partial class Composition
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Hint(Hint.Resolve, "Off")
            .Hint(Hint.ThreadSafe, "Off")
            .Root<BuildApplication>(nameof(Root))
            .Arg<string[]>("args")
            .Arg<CancellationToken>("cancellationToken")
            .Bind().To(_ => GetService<ICommandLineRunner>())
            .Singleton<BuildPaths, MatrixModuleDiscovery, RazorTemplateEngine, MatrixReportStore, QuietProcessRunner, MatrixTarget, MetadataTarget,
                LibraryTarget, ReportChartsTarget, ReadmeTarget, PrepareCommitTarget, CiMatrixTarget, CiReportsTarget, ImportReportsTarget, WebManifestTarget,
                WebTarget, LocalWebTarget, ReproduceTarget, RunConfigurationsTarget>();
}
