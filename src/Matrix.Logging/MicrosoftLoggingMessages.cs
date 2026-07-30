using Microsoft.Extensions.Logging;

namespace Matrix.Logging;

internal static class MicrosoftLoggingMessages
{
    public static readonly Action<ILogger, decimal, string, Exception?> Template =
        LoggerMessage.Define<decimal, string>(
            LogLevel.Information,
            new EventId(6, nameof(Template)),
            "Total {Amount:0.00} for {Customer}");
}

