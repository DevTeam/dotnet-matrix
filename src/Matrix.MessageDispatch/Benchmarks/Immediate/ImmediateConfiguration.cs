using Immediate.Handlers.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Matrix.MessageDispatch.Benchmarks.ImmediateLibrary;

/// <summary>
/// Immediate.Handlers generates one <see cref="IHandler{TRequest,TResponse}"/> per annotated
/// handler, and the generated service-collection extension registers them. The container and
/// every resolved handler are created once, so each measured method performs exactly one
/// generated handler call.
/// </summary>
internal static class ImmediateConfiguration
{
    public static readonly SendQueryMessage Query = new(MessageDispatchData.Value);

    public static readonly SendCommandMessage Command = new(MessageDispatchData.Value);

    public static readonly AsyncQueryMessage AsyncQuery = new(MessageDispatchData.Value);

    public static readonly BehaviorQueryMessage BehaviorQuery = new(MessageDispatchData.Value);

    public static readonly ThrowingQueryMessage ThrowingQuery = new(MessageDispatchData.Value);

    public static readonly ServiceProvider Provider = BuildProvider(ServiceLifetime.Singleton);

    public static readonly IHandler<SendQueryMessage, int> QueryHandler =
        Provider.GetRequiredService<IHandler<SendQueryMessage, int>>();

    public static readonly IHandler<SendCommandMessage, ImmediateUnit> CommandHandler =
        Provider.GetRequiredService<IHandler<SendCommandMessage, ImmediateUnit>>();

    public static readonly IHandler<AsyncQueryMessage, int> AsyncHandler =
        Provider.GetRequiredService<IHandler<AsyncQueryMessage, int>>();

    public static readonly IHandler<BehaviorQueryMessage, int> BehaviorHandler =
        Provider.GetRequiredService<IHandler<BehaviorQueryMessage, int>>();

    public static readonly IHandler<ThrowingQueryMessage, int> ThrowingHandler =
        Provider.GetRequiredService<IHandler<ThrowingQueryMessage, int>>();

    public static int Counter;

    public static int Observed;

    public static int Handled;

    /// <summary>Builds one container for the registration scenario.</summary>
    public static ServiceProvider BuildScope() => BuildProvider(ServiceLifetime.Singleton);

    private static ServiceProvider BuildProvider(ServiceLifetime lifetime)
    {
        var services = new ServiceCollection();
        services.AddMatrixMessageDispatchHandlers(lifetime);
        return services.BuildServiceProvider();
    }
}

/// <summary>
/// The response type of a command: <see cref="IHandler{TRequest,TResponse}"/> always returns a
/// value, so a command declares that its handler has no meaningful result.
/// </summary>
public readonly record struct ImmediateUnit;

public sealed record SendQueryMessage(int Value);

public sealed record SendCommandMessage(int Value);

public sealed record AsyncQueryMessage(int Value);

public sealed record BehaviorQueryMessage(int Value);

public sealed record ThrowingQueryMessage(int Value);

[Handler]
public static partial class SendQueryMessageHandler
{
    private static ValueTask<int> HandleAsync(SendQueryMessage request, CancellationToken cancellationToken)
    {
        ImmediateConfiguration.Counter++;
        return new(request.Value + 1);
    }
}

[Handler]
public static partial class SendCommandMessageHandler
{
    private static ValueTask<ImmediateUnit> HandleAsync(
        SendCommandMessage request,
        CancellationToken cancellationToken)
    {
        ImmediateConfiguration.Counter++;
        return new(new ImmediateUnit());
    }
}

[Handler]
public static partial class AsyncQueryMessageHandler
{
    private static async ValueTask<int> HandleAsync(
        AsyncQueryMessage request,
        CancellationToken cancellationToken)
    {
        await Task.Yield();
        ImmediateConfiguration.Counter++;
        return request.Value + 1;
    }
}

[Handler]
[Behaviors(typeof(CountingBehavior<,>))]
public static partial class BehaviorQueryMessageHandler
{
    private static ValueTask<int> HandleAsync(
        BehaviorQueryMessage request,
        CancellationToken cancellationToken)
    {
        ImmediateConfiguration.Counter++;
        return new(request.Value + 1);
    }
}

[Handler]
[Behaviors(typeof(RecoveringBehavior<,>))]
public static partial class ThrowingQueryMessageHandler
{
    private static ValueTask<int> HandleAsync(
        ThrowingQueryMessage request,
        CancellationToken cancellationToken) =>
        throw new InvalidOperationException("The handler of this scenario always fails.");
}

/// <summary>Counts its own request type on the way to the handler.</summary>
public sealed class CountingBehavior<TRequest, TResponse> : Behavior<TRequest, TResponse>
{
    public override async ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        if (request is BehaviorQueryMessage)
        {
            ImmediateConfiguration.Observed++;
        }

        return await Next(request, cancellationToken).ConfigureAwait(false);
    }
}

/// <summary>
/// Immediate.Handlers has no exception handler type; a behaviour is its documented
/// extension point, so the failure of this scenario's own request type is recovered here.
/// </summary>
public sealed class RecoveringBehavior<TRequest, TResponse> : Behavior<TRequest, TResponse>
{
    public override async ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await Next(request, cancellationToken).ConfigureAwait(false);
        }
        catch (InvalidOperationException)
        {
            ImmediateConfiguration.Handled++;
            return (TResponse)(object)MessageDispatchData.RecoveredAnswer;
        }
    }
}
