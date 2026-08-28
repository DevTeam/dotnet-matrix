using Microsoft.Extensions.Logging;
using ZLogger;

namespace Matrix.Logging.Aot;

internal static class AotProbe
{
    public const string Library = "ZLogger";

    public const int ExpectedEvents = 1;

    /// <summary>
    /// Configures ZLogger in code and delivers one Information event to an in-memory processor.
    /// </summary>
    /// <remarks>
    /// ZLogger delivers through a background queue, so the factory is disposed before the count
    /// is read: disposal drains the queue. Microsoft.Extensions.Logging is compiled in as well,
    /// because ZLogger plugs into it, so a trim warning here belongs to the pair.
    /// </remarks>
    public static int Run()
    {
        var processor = new CountingProcessor();
        using (var factory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(LogLevel.Information);
            builder.AddZLoggerLogProcessor(processor);
        }))
        {
            factory.CreateLogger("Matrix.Logging.Aot").ZLogInformation($"probe");
        }

        return processor.Count;
    }

    private sealed class CountingProcessor : IAsyncLogProcessor
    {
        private int _count;

        public int Count => Volatile.Read(ref _count);

        public void Post(IZLoggerEntry log)
        {
            Interlocked.Increment(ref _count);
            log.Return();
        }

        public ValueTask DisposeAsync() => default;
    }
}
