using Build.Targets;
using Matrix;
using Pure.DI;
using System.Diagnostics;
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
            .Bind<IBuildPaths>().As(Lifetime.Singleton).To<BuildPaths>()
            .Bind<IMatrixModuleDiscovery>().As(Lifetime.Singleton).To<MatrixModuleDiscovery>()
            .Bind<ITemplateEngine>().As(Lifetime.Singleton).To<RazorTemplateEngine>()
            .Bind<IMatrixReportStore>().As(Lifetime.Singleton).To<MatrixReportStore>()
            .Bind<IQuietProcessRunner>().As(Lifetime.Singleton).To<QuietProcessRunner>()
            .Bind<IMatrixTarget>().As(Lifetime.Singleton).To<MatrixTarget>()
            .Bind<IMetadataTarget>().As(Lifetime.Singleton).To<MetadataTarget>()
            .Bind<ILibraryTarget>().As(Lifetime.Singleton).To<LibraryTarget>()
            .Bind<IReportChartsTarget>().As(Lifetime.Singleton).To<ReportChartsTarget>()
            .Bind<IReadmeTarget>().As(Lifetime.Singleton).To<ReadmeTarget>()
            .Bind<IPrepareCommitTarget>().As(Lifetime.Singleton).To<PrepareCommitTarget>()
            .Bind<ICiMatrixTarget>().As(Lifetime.Singleton).To<CiMatrixTarget>()
            .Bind<ICiReportsTarget>().As(Lifetime.Singleton).To<CiReportsTarget>()
            .Bind<IWebTarget>().As(Lifetime.Singleton).To<WebTarget>()
            .Bind<ILocalWebTarget>().As(Lifetime.Singleton).To<LocalWebTarget>()
            .Bind<IReproduceTarget>().As(Lifetime.Singleton).To<ReproduceTarget>()
            .Bind<IRunConfigurationsTarget>().As(Lifetime.Singleton)
                .To<RunConfigurationsTarget>();
}
