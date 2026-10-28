using Cortex.Mediator;
using Cortex.Mediator.Behaviors;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Cortex.Mediator.Notifications;
using Cortex.Mediator.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Matrix.MessageDispatch.Benchmarks.CortexLibrary;

/// <summary>
/// Cortex.Mediator separates commands, queries and notifications and resolves their handlers
/// and behaviours from the container. The container and the message instances are created
/// once, so each measured method performs exactly one Send, Publish or Query.
/// </summary>
internal static class CortexConfiguration
{
    public static readonly SendQueryMessage Query = new(MessageDispatchData.Value);

    public static readonly SendCommandMessage Command = new(MessageDispatchData.Value);

    public static readonly AsyncQueryMessage AsyncQuery = new(MessageDispatchData.Value);

    public static readonly PublishNotificationMessage Notification = new(MessageDispatchData.Value);

    public static readonly BehaviorQueryMessage BehaviorQuery = new(MessageDispatchData.Value);

    public static readonly ThrowingQueryMessage ThrowingQuery = new(MessageDispatchData.Value);

    public static readonly ServiceProvider Provider = BuildMediator();

    public static readonly IMediator Mediator = Provider.GetRequiredService<IMediator>();

    public static int Counter;

    public static int Observed;

    public static int Handled;

    /// <summary>Builds one container for the registration scenario.</summary>
    public static ServiceProvider BuildScope() => BuildMediator();

    private static ServiceProvider BuildMediator()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IExceptionHandler<int>, RecoveringExceptionHandler>();
        services.AddSingleton<IExceptionHandler, RecoveringExceptionHandler>();
        services.AddCortexMediator(
            [typeof(CortexConfiguration)],
            options =>
            {
                options.AddOpenQueryPipelineBehavior(typeof(CountingQueryBehavior<,>));
                options.AddOpenQueryPipelineBehavior(typeof(ExceptionHandlingQueryBehavior<,>));
            });
        return services.BuildServiceProvider();
    }
}

public sealed record SendQueryMessage(int Value) : IQuery<int>;

public sealed record SendCommandMessage(int Value) : ICommand;

public sealed record AsyncQueryMessage(int Value) : IQuery<int>;

public sealed record PublishNotificationMessage(int Value) : INotification;

public sealed record BehaviorQueryMessage(int Value) : IQuery<int>;

public sealed record ThrowingQueryMessage(int Value) : IQuery<int>;

public sealed class SendQueryHandler : IQueryHandler<SendQueryMessage, int>
{
    public Task<int> Handle(SendQueryMessage query, CancellationToken cancellationToken)
    {
        CortexConfiguration.Counter++;
        return Task.FromResult(query.Value + 1);
    }
}

public sealed class SendCommandHandler : ICommandHandler<SendCommandMessage>
{
    public Task Handle(SendCommandMessage command, CancellationToken cancellationToken)
    {
        CortexConfiguration.Counter++;
        return Task.CompletedTask;
    }
}

public sealed class AsyncQueryHandler : IQueryHandler<AsyncQueryMessage, int>
{
    public async Task<int> Handle(AsyncQueryMessage query, CancellationToken cancellationToken)
    {
        await Task.Yield();
        CortexConfiguration.Counter++;
        return query.Value + 1;
    }
}

public sealed class PublishNotificationHandlerA : INotificationHandler<PublishNotificationMessage>
{
    public Task Handle(PublishNotificationMessage notification, CancellationToken cancellationToken)
    {
        CortexConfiguration.Counter++;
        return Task.CompletedTask;
    }
}

public sealed class PublishNotificationHandlerB : INotificationHandler<PublishNotificationMessage>
{
    public Task Handle(PublishNotificationMessage notification, CancellationToken cancellationToken)
    {
        CortexConfiguration.Counter++;
        return Task.CompletedTask;
    }
}

public sealed class PublishNotificationHandlerC : INotificationHandler<PublishNotificationMessage>
{
    public Task Handle(PublishNotificationMessage notification, CancellationToken cancellationToken)
    {
        CortexConfiguration.Counter++;
        return Task.CompletedTask;
    }
}

public sealed class BehaviorQueryHandler : IQueryHandler<BehaviorQueryMessage, int>
{
    public Task<int> Handle(BehaviorQueryMessage query, CancellationToken cancellationToken)
    {
        CortexConfiguration.Counter++;
        return Task.FromResult(query.Value + 1);
    }
}

public sealed class ThrowingQueryHandler : IQueryHandler<ThrowingQueryMessage, int>
{
    public Task<int> Handle(ThrowingQueryMessage query, CancellationToken cancellationToken) =>
        throw new InvalidOperationException("The handler of this scenario always fails.");
}

/// <summary>Counts its own query type on the way to the handler.</summary>
public sealed class CountingQueryBehavior<TQuery, TResult> : IQueryPipelineBehavior<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    public async Task<TResult> Handle(
        TQuery query,
        QueryHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        if (query is BehaviorQueryMessage)
        {
            CortexConfiguration.Observed++;
        }

        return await next().ConfigureAwait(false);
    }
}

/// <summary>
/// Cortex.Mediator handles handler failures through its own <see cref="IExceptionHandler{TResult}"/>
/// and the built-in <see cref="ExceptionHandlingQueryBehavior{TQuery,TResult}"/>.
/// </summary>
public sealed class RecoveringExceptionHandler : IExceptionHandler<int>, IExceptionHandler
{
    public Task<(bool, int)> HandleWithResultAsync(
        Exception exception,
        Type messageType,
        object message,
        CancellationToken cancellationToken)
    {
        CortexConfiguration.Handled++;
        return Task.FromResult((true, MessageDispatchData.RecoveredAnswer));
    }

    public Task<bool> HandleAsync(
        Exception exception,
        Type messageType,
        object message,
        CancellationToken cancellationToken) => Task.FromResult(false);
}
