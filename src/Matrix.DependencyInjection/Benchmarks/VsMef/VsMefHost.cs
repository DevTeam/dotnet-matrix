using Microsoft.VisualStudio.Composition;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable UseCollectionExpression

namespace Matrix.DependencyInjection.Benchmarks;

// VS MEF composes only attributed parts, so every scenario shares the same discovery pipeline.
internal static class VsMefHost
{
    public static ExportProvider Create(params Type[] partTypes)
    {
        var discovery = new AttributedPartDiscovery(Resolver.DefaultInstance, true);
        var catalog = ComposableCatalog.Create(Resolver.DefaultInstance)
            .AddParts(partTypes.Select(discovery.CreatePart).ToArray()!);
        return CompositionConfiguration.Create(catalog)
            .CreateExportProviderFactory()
            .CreateExportProvider();
    }
}
