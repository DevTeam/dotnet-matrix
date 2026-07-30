using HostApi;
using HostCommandLine = HostApi.CommandLine;

namespace Build.Targets;

internal sealed class LocalWebTarget(
    IBuildPaths buildPaths,
    IQuietProcessRunner processRunner) : ILocalWebTarget
{
    private const string DynamicUrl = "http://127.0.0.1:0";
    private const string ListeningMarker = "Now listening on: ";
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

        var ready = new TaskCompletionSource<string>(
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
                DynamicUrl),
            "local Web application",
            cancellationToken,
            cancellationIsSuccess: true,
            outputHandler: output =>
            {
                if (TryGetLocalUrl(output, out var url))
                {
                    ready.TrySetResult(url);
                }
            },
            suppressConsoleOutput: false);
        if (await Task.WhenAny(ready.Task, server) == ready.Task)
        {
            var url = await ready.Task;
            Host.Info($"Local .NET Matrix: {url}");
            if (launchBrowser)
            {
                await OpenBrowserAsync(url, cancellationToken);
            }

            Host.Info("Press Ctrl+C to stop the Web application.");
        }

        return await server;
    }

    private static bool TryGetLocalUrl(Output output, out string url)
    {
        url = string.Empty;
        if (output.IsError)
        {
            return false;
        }

        var markerIndex = output.Line.IndexOf(
            ListeningMarker,
            StringComparison.OrdinalIgnoreCase);
        if (markerIndex < 0)
        {
            return false;
        }

        var candidate = output.Line[(markerIndex + ListeningMarker.Length)..].Trim();
        if (!Uri.TryCreate(candidate, UriKind.Absolute, out var uri)
            || !uri.IsLoopback
            || uri.Scheme != Uri.UriSchemeHttp
            || uri.Port <= 0)
        {
            return false;
        }

        url = uri.GetLeftPart(UriPartial.Authority);
        return true;
    }

    private HostCommandLine DotNet(string operation, params string[] arguments) =>
        new(
            "dotnet",
            buildPaths.SolutionDirectory,
            arguments,
            [],
            operation);

    private async Task OpenBrowserAsync(
        string url,
        CancellationToken cancellationToken)
    {
        var commandLine = BrowserCommand(url);
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

        Host.Warning($"Could not open the default browser.{details} Open {url} manually.");
    }

    private HostCommandLine BrowserCommand(string url)
    {
        if (OperatingSystem.IsWindows())
        {
            return new HostCommandLine(
                "cmd.exe",
                buildPaths.SolutionDirectory,
                ["/c", "start", string.Empty, url],
                [],
                "default browser");
        }

        return new HostCommandLine(
            OperatingSystem.IsMacOS() ? "open" : "xdg-open",
            buildPaths.SolutionDirectory,
            [url],
            [],
            "default browser");
    }
}
