using HostApi;
using System.Text;

namespace Build.Targets;

internal sealed class QuietProcessRunner(ICommandLineRunner commandLineRunner, IBuildPaths buildPaths) : IQuietProcessRunner
{
    private const int TailSize = 30;

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
        await using var log = new StreamWriter(logPath, false, new UTF8Encoding(false));
        log.AutoFlush = true;

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
            Error($"{operation} failed to run: {error.Message}. Log: {relativeLogPath}");
            return 1;
        }
        finally
        {
            if (suppressConsoleOutput)
            {
                Console.SetOut(standardOutput);
            }
        }

        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (result.State)
        {
            case ProcessState.Canceled when cancellationIsSuccess:
                Info($"{operation} stopped.");
                return 0;
            case ProcessState.Canceled:
                Warning($"{operation} canceled. Log: {relativeLogPath}");
                return 130;
            case ProcessState.Finished when result.ExitCode == 0:
                Trace($"{operation} log: {relativeLogPath}");
                return 0;
        }

        var failure = result.Error is null
            ? $"exit code {result.ExitCode?.ToString() ?? "unknown"}"
            : result.Error.Message;
        Error($"{operation} failed with {failure}. Log: {relativeLogPath}");
        if (tail.Count <= 0)
        {
            return result.ExitCode ?? 1;
        }

        WriteLine("Last output:", Color.Details);
        foreach (var line in tail)
        {
            WriteLine(line, Color.Details);
        }

        return result.ExitCode ?? 1;

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
    }

    private static string FileName(string operation) =>
        string.Concat(operation.Select(character =>
            char.IsAsciiLetterOrDigit(character) || character is '-' or '_'
                ? char.ToLowerInvariant(character)
                : '-'));
}
