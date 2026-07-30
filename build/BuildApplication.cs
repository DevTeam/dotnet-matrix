using Build.Targets;
using Matrix;
using System.CommandLine;

namespace Build;

internal sealed class BuildApplication(
    string[] args,
    CancellationToken cancellationToken,
    IMatrixModuleDiscovery moduleDiscovery,
    IMatrixTarget matrixTarget,
    IMetadataTarget metadataTarget,
    ILibraryTarget libraryTarget,
    IReportChartsTarget reportChartsTarget,
    IReadmeTarget readmeTarget,
    IPrepareCommitTarget prepareCommitTarget,
    ICiMatrixTarget ciMatrixTarget,
    ICiReportsTarget ciReportsTarget,
    IRunConfigurationsTarget runConfigurationsTarget,
    IWebTarget webTarget,
    IReproduceTarget reproduceTarget)
{
    public Task<int> RunAsync()
    {
        var modules = moduleDiscovery.Discover();
        var root = new RootCommand("dotnet-matrix build");
        foreach (var module in modules)
        {
            Register(root, module, MatrixMode.Validation);
            Register(root, module, MatrixMode.Benchmark);
            RegisterLibrary(root, modules, module);
        }

        RegisterRunConfigurations(root, modules);
        RegisterMetadata(root, modules);
        RegisterReportCharts(root, modules, reportChartsTarget);
        RegisterReadme(root, modules, readmeTarget);
        RegisterPrepareCommit(root, modules, prepareCommitTarget);
        RegisterCiMatrix(root, modules);
        RegisterCiReports(root, modules);
        RegisterWeb(root, modules);
        RegisterReproduce(root, modules);
        return root.Parse(args).InvokeAsync();
    }

    private void RegisterMetadata(
        RootCommand root,
        IReadOnlyList<DiscoveredMatrixModule> modules)
    {
        var command = new Command(
            MatrixNames.MetadataCommand,
            "Generate library presentation metadata from matrix projects");
        command.SetAction(_ => metadataTarget.Run(modules));
        root.Subcommands.Add(command);
    }

    private static void RegisterReportCharts(
        RootCommand root,
        IReadOnlyList<DiscoveredMatrixModule> modules,
        IReportChartsTarget reportChartsTarget)
    {
        var command = new Command(
            MatrixNames.RenderReportsCommand,
            "Render benchmark reports as PNG charts");
        command.SetAction(_ => reportChartsTarget.Run(modules));
        root.Subcommands.Add(command);
    }

    private void RegisterReadme(
        RootCommand root,
        IReadOnlyList<DiscoveredMatrixModule> modules,
        IReadmeTarget readme)
    {
        var command = new Command(
            MatrixNames.ReadmeCommand,
            "Generate README.md and its benchmark charts");
        command.SetAction(_ => readme.RunAsync(modules, cancellationToken));
        root.Subcommands.Add(command);
    }

    private void RegisterPrepareCommit(
        RootCommand root,
        IReadOnlyList<DiscoveredMatrixModule> modules,
        IPrepareCommitTarget prepareCommit)
    {
        var command = new Command(
            MatrixNames.PrepareCommitCommand,
            "Validate and benchmark every library, then generate all source-controlled artifacts");
        command.SetAction(_ => prepareCommit.RunAsync(modules, true, cancellationToken));
        root.Subcommands.Add(command);

        var finalizeCommand = new Command(
            MatrixNames.FinalizeCommitCommand,
            "Generate all source-controlled artifacts from the reports already on disk, "
            + "without validating or benchmarking");
        finalizeCommand.SetAction(_ => prepareCommit.RunAsync(modules, false, cancellationToken));
        root.Subcommands.Add(finalizeCommand);
    }

    private void RegisterCiReports(
        RootCommand root,
        IReadOnlyList<DiscoveredMatrixModule> modules)
    {
        var category = new Option<string?>("--category")
        {
            Description = "Exact case-insensitive matrix category id. Empty = all categories."
        };
        var libraries = new Option<string?>("--libraries")
        {
            Description = "Case-insensitive library filter. Comma-separated values and '*' are supported."
        };
        var smoke = new Option<bool>("--smoke")
        {
            Description = "Run one warmup and one measurement iteration."
        };
        var skipBenchmarks = new Option<bool>("--skip-benchmarks")
        {
            Description = "Validate features only."
        };
        var output = new Option<string?>("--output")
        {
            Description = "Directory to stage the report artifact in. Default: artifacts/ci-reports."
        };
        var command = new Command(
            MatrixNames.CiReportsCommand,
            "Validate features, benchmark when validation succeeds, then stage the reports for a CI artifact");
        command.Options.Add(category);
        command.Options.Add(libraries);
        command.Options.Add(smoke);
        command.Options.Add(skipBenchmarks);
        command.Options.Add(output);
        command.SetAction(async parseResult => await ciReportsTarget.RunAsync(
            modules,
            new CiReportsOptions(
                parseResult.GetValue(category),
                parseResult.GetValue(libraries),
                parseResult.GetValue(smoke),
                parseResult.GetValue(skipBenchmarks),
                parseResult.GetValue(output)),
            cancellationToken));
        root.Subcommands.Add(command);
    }

    private void RegisterCiMatrix(
        RootCommand root,
        IReadOnlyList<DiscoveredMatrixModule> modules)
    {
        var output = new Option<string?>("--output")
        {
            Description = "JSON output file. Default: artifacts/ci-matrix.json."
        };
        var command = new Command(
            MatrixNames.CiMatrixCommand,
            "Write discovered matrix category ids as JSON for a CI job matrix");
        command.Options.Add(output);
        command.SetAction(parseResult => ciMatrixTarget.Run(
            modules,
            parseResult.GetValue(output)));
        root.Subcommands.Add(command);
    }

    private void Register(
        RootCommand root,
        DiscoveredMatrixModule module,
        MatrixMode mode)
    {
        var libraries = new Option<string?>("--libraries")
        {
            Description = "Case-insensitive library filter. Comma-separated values and '*' are supported."
        };
        var command = new Command(
            MatrixNames.Command(module.Metadata, mode),
            $"{mode} {module.Metadata.Name} library features");
        command.Options.Add(libraries);

        Option<bool>? smoke = null;
        if (mode == MatrixMode.Benchmark)
        {
            smoke = new Option<bool>("--smoke")
            {
                Description = "Run one warmup and one measurement iteration."
            };
            command.Options.Add(smoke);
        }

        command.SetAction(async parseResult =>
        {
            var libraryFilter = parseResult.GetValue(libraries);
            var isSmoke = smoke is not null && parseResult.GetValue(smoke);
            return await matrixTarget.RunAsync(
                module,
                mode,
                libraryFilter,
                isSmoke,
                cancellationToken);
        });
        root.Subcommands.Add(command);
    }

    private void RegisterLibrary(
        RootCommand root,
        IReadOnlyList<DiscoveredMatrixModule> modules,
        DiscoveredMatrixModule module)
    {
        var library = new Option<string>("--library")
        {
            Description = "Exact case-insensitive library id.",
            Required = true
        };
        var command = new Command(
            MatrixNames.UpdateLibraryCommand(module.Metadata),
            $"Validate and benchmark one {module.Metadata.Name} library, then regenerate all charts and README");
        command.Options.Add(library);
        command.SetAction(async parseResult =>
        {
            var libraryId = parseResult.GetValue(library)!;
            var selectedLibrary = module.Metadata.Libraries.SingleOrDefault(candidate =>
                candidate.Id.Equals(libraryId, StringComparison.OrdinalIgnoreCase));
            // ReSharper disable once InvertIf
            if (selectedLibrary is null)
            {
                await Console.Error.WriteLineAsync(
                    $"Unknown {module.Metadata.Name} library '{libraryId}'. "
                    + $"Available libraries: {string.Join(", ", module.Metadata.Libraries.Select(item => item.Id))}.");
                return 1;
            }

            return await libraryTarget.RunAsync(
                modules,
                module,
                selectedLibrary,
                cancellationToken);
        });
        root.Subcommands.Add(command);
    }

    private void RegisterRunConfigurations(
        RootCommand root,
        IReadOnlyList<DiscoveredMatrixModule> modules)
    {
        var command = new Command(
            MatrixNames.RunConfigurationsCommand,
            "Generate Rider run configurations for all matrix modules");
        command.SetAction(_ => runConfigurationsTarget.Run(modules));
        root.Subcommands.Add(command);
    }

    private void RegisterWeb(
        RootCommand root,
        IReadOnlyList<DiscoveredMatrixModule> modules)
    {
        var command = new Command(
            MatrixNames.BuildWebCommand,
            "Build the .NET Matrix Blazor WebAssembly application");
        command.SetAction(_ => webTarget.RunAsync(modules, cancellationToken));
        root.Subcommands.Add(command);
    }

    private void RegisterReproduce(
        RootCommand root,
        IReadOnlyList<DiscoveredMatrixModule> modules)
    {
        var skipBenchmarks = new Option<bool>("--skip-benchmarks")
        {
            Description = "Use reports already on disk instead of validating and benchmarking every library."
        };
        var noBrowser = new Option<bool>("--no-browser")
        {
            Description = "Start the local Web application without opening a browser."
        };
        var command = new Command(
            MatrixNames.ReproduceCommand,
            "Reproduce all results and run the complete Web application locally");
        command.Options.Add(skipBenchmarks);
        command.Options.Add(noBrowser);
        command.SetAction(parseResult => reproduceTarget.RunAsync(
            modules,
            parseResult.GetValue(skipBenchmarks),
            !parseResult.GetValue(noBrowser),
            cancellationToken));
        root.Subcommands.Add(command);
    }
}
