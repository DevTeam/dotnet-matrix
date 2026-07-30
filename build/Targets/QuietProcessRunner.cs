using HostApi;
using System.Text;

namespace Build.Targets;

internal sealed class QuietProcessRunner(IBuildPaths buildPaths) : IQuietProcessRunner
{
    private const int TailSize = 30;
    private readonly ICommandLineRunner commandLineRunner =
        Host.GetService<ICommandLineRunner>();

    public async Task<int> RunAsync(
        ICommandLine commandLine,
        string operation,
        CancellationToken cancellationToken,
        bool cancellationIsSuccess = false,
        Action<Output>? outputHandler = null,
        bool suppressConsoleOutput = true)
    {
        var logsDirectory = Path.Combine(buildPaths.SolutionDirectory, "artifacts", "logs");
        Directory.CreateDirectory(logsDirectory);
        var logPath = Path.Combine(logsDirectory, $"{FileName(operation)}.log");
        var relativeLogPath = Path.GetRelativePath(buildPaths.SolutionDirectory, logPath);

        var tail = new Queue<string>();
        var sync = new object();
        await using var log = new StreamWriter(logPath, false, new UTF8Encoding(false))
        {
            AutoFlush = true
        };

        void Capture(Output output)
        {
            output.Handled = true;
            lock (sync)
            {
                log.WriteLine($"[{(output.IsError ? "stderr" : "stdout")}] {output.Line}");
                tail.Enqueue(output.Line);
                while (tail.Count > TailSize)
                {
                    tail.Dequeue();
                }
            }

            outputHandler?.Invoke(output);
        }

        ICommandLineResult result;
        var standardOutput = Console.Out;
        try
        {
            if (suppressConsoleOutput)
            {
                Console.SetOut(TextWriter.Null);
            }

            result = await commandLineRunner.RunAsync(
                commandLine,
                Capture,
                cancellationToken);
        }
        catch (Exception error)
        {
            Host.Error($"{operation} failed to run: {error.Message}. Log: {relativeLogPath}");
            return 1;
        }
        finally
        {
            if (suppressConsoleOutput)
            {
                Console.SetOut(standardOutput);
            }
        }

        if (result.State == ProcessState.Canceled)
        {
            if (cancellationIsSuccess)
            {
                Host.Info($"{operation} stopped.");
                return 0;
            }

            Host.Warning($"{operation} canceled. Log: {relativeLogPath}");
            return 130;
        }

        if (result.State == ProcessState.Finished && result.ExitCode == 0)
        {
            Host.Trace($"{operation} log: {relativeLogPath}");
            return 0;
        }

        var failure = result.Error is null
            ? $"exit code {result.ExitCode?.ToString() ?? "unknown"}"
            : result.Error.Message;
        Host.Error($"{operation} failed with {failure}. Log: {relativeLogPath}");
        if (tail.Count > 0)
        {
            Host.WriteLine("Last output:", Color.Details);
            foreach (var line in tail)
            {
                Host.WriteLine(line, Color.Details);
            }
        }

        return result.ExitCode ?? 1;
    }

    private static string FileName(string operation) =>
        string.Concat(operation.Select(character =>
            char.IsAsciiLetterOrDigit(character) || character is '-' or '_'
                ? char.ToLowerInvariant(character)
                : '-'));
}
