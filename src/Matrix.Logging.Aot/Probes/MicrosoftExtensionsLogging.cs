using Microsoft.Extensions.Logging;

namespace Matrix.Logging.Aot;

internal static class AotProbe
{
    public const string Library = "Microsoft.Extensions.Logging";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Configures Microsoft.Extensions.Logging in code and delivers one Information event to an
    /// in-memory provider.
    /// </summary>
    public static int Run()
    {
        var provider = new CountingProvider();
        using var factory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(LogLevel.Information);
            builder.AddProvider(provider);
        });

        factory.CreateLogger("Matrix.Logging.Aot").LogInformation("probe");
        return provider.Count;
    }

    private sealed class CountingProvider : ILoggerProvider, ILogger
    {
        public int Count { get; private set; }

        public ILogger CreateLogger(string categoryName) => this;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) => Count++;

        public void Dispose()
        {
        }
    }
}
