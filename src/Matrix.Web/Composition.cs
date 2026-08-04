using Pure.DI.MS;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

namespace Matrix.Web;

internal partial class Composition : ServiceProviderFactory<Composition>
{
    // Only composition roots can be resolved through IServiceProvider, and every
    // one of these is injected into a component with @inject, so each needs a root.
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Hint(Hint.ThreadSafe, "Off")
            .Arg<HttpClient>("httpClient")

            .Root<IMatrixDataSource>()
            .Root<IMatrixView>()
            .Root<IMatrixScoring>()
            .Root<IMatrixMeasures>()
            .Root<IMatrixPalette>()

            .Singleton<GitHubMatrixDataSource, MatrixView, MatrixScoring, MatrixMeasures, MatrixPalette>();
}
