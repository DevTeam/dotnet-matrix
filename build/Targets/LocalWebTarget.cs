using HostApi;
using HostCommandLine = HostApi.CommandLine;

namespace Build.Targets;

internal sealed class LocalWebTarget(
    IBuildPaths buildPaths,
    IQuietProcessRunner processRunner) : ILocalWebTarget
{
    internal const string Url = "http://localhost:5290";
    private readonly ICommandLineRunner commandLineRunner =
        Host.GetService<ICommandLineRunner>();

    public async Task<int> RunAsync(
        bool launchBrowser,
        CancellationToken cancellationToken)
    {
        var projectPath = Path.Combine(
            buildPaths.SolutionDirectory,
            "src",
            "Matrix.Web",
            "Matrix.Web.csproj");
        Host.Info("Building the local Web application with all current reports.");
        var result = await processRunner.RunAsync(
            DotNet(
                "local Web build",
                "build",
                projectPath,
                "--configuration",
                "Release",
                "--nologo",
                "--verbosity",
                "quiet"),
            "local Web build",
            cancellationToken);
        if (result != 0)
        {
            return result;
        }

        var ready = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var server = processRunner.RunAsync(
            DotNet(
                "local Web application",
                "run",
                "--project",
                projectPath,
                "--configuration",
                "Release",
                "--no-build",
                "--no-launch-profile",
                "--",
                "--urls",
                Url),
            "local Web application",
            cancellationToken,
            cancellationIsSuccess: true,
            outputHandler: output =>
            {
                if (!output.IsError
                    && output.Line.Contains(
                        $"Now listening on: {Url}",
                        StringComparison.OrdinalIgnoreCase))
                {
                    ready.TrySetResult();
                }
            },
            suppressConsoleOutput: false);
        if (await Task.WhenAny(ready.Task, server) == ready.Task)
        {
            Host.Info($"Local .NET Matrix: {Url}");
            if (launchBrowser)
            {
                await OpenBrowserAsync(cancellationToken);
            }

            Host.Info("Press Ctrl+C to stop the Web application.");
        }

        return await server;
    }

    private HostCommandLine DotNet(string operation, params string[] arguments) =>
        new(
            "dotnet",
            buildPaths.SolutionDirectory,
            arguments,
            [],
            operation);

    private async Task OpenBrowserAsync(CancellationToken cancellationToken)
    {
        var commandLine = BrowserCommand();
        var errors = new List<string>();
        var consoleOutput = Console.Out;
        ICommandLineResult result;
        try
        {
            Console.SetOut(TextWriter.Null);
            result = await commandLineRunner.RunAsync(
                commandLine,
                output =>
                {
                    output.Handled = true;
                    if (output.IsError)
                    {
                        lock (errors)
                        {
                            errors.Add(output.Line);
                        }
                    }
                },
                cancellationToken);
        }
        finally
        {
            Console.SetOut(consoleOutput);
        }

        if (result.State == ProcessState.Finished && result.ExitCode == 0)
        {
            Host.Info("Opened the local application in the default browser.");
            return;
        }

        string details;
        lock (errors)
        {
            details = errors.Count == 0 ? string.Empty : $" {string.Join(' ', errors)}";
        }

        Host.Warning($"Could not open the default browser.{details} Open {Url} manually.");
    }

    private HostCommandLine BrowserCommand()
    {
        if (OperatingSystem.IsWindows())
        {
            return new HostCommandLine(
                "cmd.exe",
                buildPaths.SolutionDirectory,
                ["/c", "start", string.Empty, Url],
                [],
                "default browser");
        }

        return new HostCommandLine(
            OperatingSystem.IsMacOS() ? "open" : "xdg-open",
            buildPaths.SolutionDirectory,
            [Url],
            [],
            "default browser");
    }
}
