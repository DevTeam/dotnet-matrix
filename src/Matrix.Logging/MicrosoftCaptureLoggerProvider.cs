using Microsoft.Extensions.Logging;

namespace Matrix.Logging;

internal sealed class MicrosoftCaptureLoggerProvider :
    ILoggerProvider,
    ISupportExternalScope
{
    private readonly MicrosoftCaptureLogger _logger;
    private int _count;

    public MicrosoftCaptureLoggerProvider()
    {
        _logger = new MicrosoftCaptureLogger(this);
    }

    public int Count => Volatile.Read(ref _count);

    public CapturedLogEvent? Last { get; private set; }

    public IExternalScopeProvider ScopeProvider { get; private set; } =
        new LoggerExternalScopeProvider();

    public ILogger CreateLogger(string categoryName) => _logger;

    public void SetScopeProvider(IExternalScopeProvider scopeProvider) =>
        ScopeProvider = scopeProvider;

    public void Dispose()
    {
    }

    public void Capture<TState>(
        LogLevel logLevel,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        Interlocked.Increment(ref _count);
#if MATRIX_VALIDATION
        var properties = new Dictionary<string, object?>(StringComparer.Ordinal);
        string? template = null;
        AddProperties(state, properties, ref template);
        ScopeProvider.ForEachScope(
            (scope, values) =>
            {
                string? ignoredTemplate = null;
                AddProperties(scope, values, ref ignoredTemplate);
            },
            properties);
        Last = new CapturedLogEvent(
            logLevel.ToString(),
            formatter(state, exception),
            template,
            exception,
            properties);
#endif
    }

#if MATRIX_VALIDATION
    private static void AddProperties<TState>(
        TState state,
        IDictionary<string, object?> properties,
        ref string? template)
    {
        if (state is not IEnumerable<KeyValuePair<string, object?>> values)
        {
            return;
        }

        foreach (var pair in values)
        {
            if (pair.Key == "{OriginalFormat}")
            {
                template = pair.Value?.ToString();
            }
            else
            {
                properties[pair.Key] = pair.Value;
            }
        }
    }
#endif
}

