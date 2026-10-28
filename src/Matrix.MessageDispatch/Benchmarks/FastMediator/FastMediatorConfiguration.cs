using FastMediator.Core;
using FastMediator.DependencyInjection;
using FastMediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Matrix.MessageDispatch.Benchmarks.FastMediatorLibrary;

/// <summary>
/// FastMediator caches delegates per request type and resolves handlers from the container.
/// The container, the dispatcher and the message instances are created once, so each measured
/// method performs exactly one Send or Publish.
/// </summary>
/// <remarks>
/// FastMediator 2.0.1 hands every notification handler the whole handler list instead of only
/// itself, so with three registered handlers each handler is invoked three times. Notification
/// fan-out is therefore declared unsupported rather than measured with a wrong invocation count.
/// </remarks>
internal static class FastMediatorConfiguration
{
    public static readonly SendQueryMessage Query = new(MessageDispatchData.Value);

    public static readonly SendCommandMessage Command = new(MessageDispatchData.Value);

    public static readonly AsyncQueryMessage AsyncQuery = new(MessageDispatchData.Value);

    public static readonly BehaviorQueryMessage BehaviorQuery = new(MessageDispatchData.Value);

    public static readonly ThrowingQueryMessage ThrowingQuery = new(MessageDispatchData.Value);

    public static readonly ServiceProvider Provider = BuildMediator();

    public static readonly Dispatcher Dispatcher = Provider.GetRequiredService<Dispatcher>();

    public static int Counter;

    public static int Observed;

    public static int Handled;

    /// <summary>Builds one container for the registration scenario.</summary>
    public static ServiceProvider BuildScope() => BuildMediator();

    private static ServiceProvider BuildMediator()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        // FastMediator discovers its IPipelineBehavior implementations itself; registering
        // them again would run every behavior twice.
        services.AddFastMediator(
            loggerFactory: services.BuildServiceProvider().GetRequiredService<ILoggerFactory>());
        return services.BuildServiceProvider();
    }
}

public sealed record SendQueryMessage(int Value) : IRequest<int>;

/// <summary>
/// FastMediator 2.0.1 has no request type without a response, so the command declares an
/// integer response it does not use; the observable result is still the handler count.
/// </summary>
public sealed record SendCommandMessage(int Value) : IRequest<int>;

public sealed record AsyncQueryMessage(int Value) : IAsyncRequest<int>;

public sealed record BehaviorQueryMessage(int Value) : IRequest<int>;

public sealed record ThrowingQueryMessage(int Value) : IRequest<int>;

public sealed class SendQueryHandler : IRequestHandler<SendQueryMessage, int>
{
    public int Handle(SendQueryMessage request)
    {
        FastMediatorConfiguration.Counter++;
        return request.Value + 1;
    }
}

public sealed class SendCommandHandler : IRequestHandler<SendCommandMessage, int>
{
    public int Handle(SendCommandMessage request)
    {
        FastMediatorConfiguration.Counter++;
        return request.Value;
    }
}

public sealed class AsyncQueryHandler : IAsyncRequestHandler<AsyncQueryMessage, int>
{
    public Task<int> HandleAsync(AsyncQueryMessage request, CancellationToken cancellationToken)
    {
        FastMediatorConfiguration.Counter++;
        return Task.FromResult(request.Value + 1);
    }
}

public sealed class BehaviorQueryHandler : IRequestHandler<BehaviorQueryMessage, int>
{
    public int Handle(BehaviorQueryMessage request)
    {
        FastMediatorConfiguration.Counter++;
        return request.Value + 1;
    }
}

public sealed class ThrowingQueryHandler : IRequestHandler<ThrowingQueryMessage, int>
{
    public int Handle(ThrowingQueryMessage request) =>
        throw new InvalidOperationException("The handler of this scenario always fails.");
}

/// <summary>Counts its own request type on the way to the handler.</summary>
public sealed class CountingBehavior : IPipelineBehavior<BehaviorQueryMessage, int>
{
    public int Handle(BehaviorQueryMessage request, Func<BehaviorQueryMessage, int> next)
    {
        FastMediatorConfiguration.Observed++;
        return next(request);
    }
}

/// <summary>
/// FastMediator has no exception handler type; a pipeline behaviour is its documented
/// extension point, so the failure of this scenario's own request type is recovered here.
/// </summary>
public sealed class RecoveringBehavior : IPipelineBehavior<ThrowingQueryMessage, int>
{
    public int Handle(ThrowingQueryMessage request, Func<ThrowingQueryMessage, int> next)
    {
        try
        {
            return next(request);
        }
        catch (InvalidOperationException)
        {
            FastMediatorConfiguration.Handled++;
            return MessageDispatchData.RecoveredAnswer;
        }
    }
}
