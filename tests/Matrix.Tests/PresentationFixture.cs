using Build.Targets;
using System.Text;

namespace Matrix.Tests;

internal sealed class PresentationFixture(string directory) :
    IBuildPaths, IMetadataTarget, IReportChartsTarget, ITemplateEngine
{
    public string SolutionDirectory => directory;
    public ReadmeModel? Model { get; private set; }

    public int Run(IReadOnlyList<DiscoveredMatrixModule> modules) => 0;

    public async Task RenderAsync<TModel>(
        string templateName,
        TModel model,
        Stream stream,
        CancellationToken cancellationToken)
    {
        Model = (ReadmeModel)(object)model!;
        await stream.WriteAsync("Generated from injected reports"u8.ToArray(), cancellationToken);
    }
}
