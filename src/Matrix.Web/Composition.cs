using Pure.DI.MS;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

namespace Matrix.Web;

internal partial class Composition : ServiceProviderFactory<Composition>
{
    [Conditional("DI")]
    private static void SetupDI() =>
        DI.Setup()
            .Hint(Hint.ThreadSafe, "Off")
            .Root<IMatrixDataSource>()
            .Arg<HttpClient>("httpClient")
            .Bind<IMatrixDataSource>().As(Lifetime.Singleton)
                .To<GitHubMatrixDataSource>();
}
