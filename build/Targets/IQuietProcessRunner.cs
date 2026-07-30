using HostApi;

namespace Build.Targets;

internal interface IQuietProcessRunner
{
    Task<int> RunAsync(
        ICommandLine commandLine,
        string operation,
        CancellationToken cancellationToken,
        bool cancellationIsSuccess = false,
        Action<Output>? outputHandler = null,
        bool suppressConsoleOutput = true);
}
