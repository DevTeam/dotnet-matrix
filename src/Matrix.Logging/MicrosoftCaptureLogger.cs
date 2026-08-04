using Microsoft.Extensions.Logging;

namespace Matrix.Logging;

internal sealed class MicrosoftCaptureLogger(MicrosoftCaptureLoggerProvider provider) :
    ILogger
{
    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull =>
        provider.ScopeProvider.Push(state);

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter) =>
        provider.Capture(logLevel, state, exception, formatter);
}

