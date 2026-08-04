using HostApi;
using System.Xml.Linq;
using Matrix;
using HostCommandLine = HostApi.CommandLine;

namespace Build.Targets;

internal sealed class MatrixModuleDiscovery(ICommandLineRunner commandLineRunner, IBuildPaths buildPaths) : IMatrixModuleDiscovery
{
    private IReadOnlyList<DiscoveredMatrixModule>? _modules;

    public IReadOnlyList<DiscoveredMatrixModule> Discover() =>
        _modules ??= DiscoverCore();

    private DiscoveredMatrixModule[] DiscoverCore()
    {
        var solutionPath = Path.Combine(buildPaths.SolutionDirectory, "dotnet-matrix.slnx");
        var modules = XDocument
            .Load(solutionPath)
            .Descendants("Project")
            .Select(project => (string?)project.Attribute("Path"))
            .Where(path => path is not null)
            .Select(path => Path.GetFullPath(path!, buildPaths.SolutionDirectory))
            .Where(path =>
                Path.GetFileNameWithoutExtension(path)
                    .StartsWith("Matrix.", StringComparison.OrdinalIgnoreCase))
            .Select(TryDiscover)
            .Where(module => module is not null)
            .Select(module => module!)
            .OrderBy(module => module.Metadata.Id, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        var duplicate = modules
            .GroupBy(module => module.Metadata.Id, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(group => group.Count() > 1);

        return duplicate is null ? modules : throw new InvalidOperationException($"Duplicate matrix module id '{duplicate.Key}'.");
    }

    private DiscoveredMatrixModule? TryDiscover(string projectPath)
    {
        Build(projectPath);
        var assemblyPath = GetTargetPath(projectPath);
        var context = new MatrixAssemblyLoadContext(assemblyPath);
        try
        {
            var assembly = context.LoadFromAssemblyPath(assemblyPath);
            if (!MatrixMetadata.TryRead(assembly, out var metadata))
            {
                return null;
            }

            return new DiscoveredMatrixModule(
                metadata,
                projectPath,
                assemblyPath);
        }
        finally
        {
            context.Unload();
        }
    }

    private void Build(string projectPath) =>
        RunDotNet(
            [
                "build",
                projectPath,
                "--configuration",
                "Release",
                "-p:MatrixMode=Validation",
                "--nologo"
            ],
            $"Cannot build matrix module '{projectPath}'.");

    private string GetTargetPath(string projectPath) =>
        RunDotNet(
            [
                "msbuild",
                projectPath,
                "-nologo",
                "-getProperty:TargetPath",
                "-p:Configuration=Release",
                "-p:MatrixMode=Validation"
            ],
            $"Cannot get TargetPath for matrix module '{projectPath}'.")
        .Trim();

    private string RunDotNet(IReadOnlyList<string> arguments, string error)
    {
        var standardOutput = new List<string>();
        var standardError = new List<string>();
        var sync = new object();
        ICommandLineResult result;
        var consoleOutput = Console.Out;
        try
        {
            Console.SetOut(TextWriter.Null);
            result = commandLineRunner.Run(
                new HostCommandLine(
                    "dotnet",
                    buildPaths.SolutionDirectory,
                    arguments,
                    [],
                    $"dotnet {arguments[0]}"),
                output =>
                {
                    output.Handled = true;
                    lock (sync)
                    {
                        (output.IsError ? standardError : standardOutput).Add(output.Line);
                    }
                });
        }
        finally
        {
            Console.SetOut(consoleOutput);
        }

        if (result.State != ProcessState.Finished || result.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"{error}{Environment.NewLine}"
                + string.Join(Environment.NewLine, standardOutput)
                + Environment.NewLine
                + string.Join(Environment.NewLine, standardError));
        }

        return string.Join(Environment.NewLine, standardOutput);
    }
}
