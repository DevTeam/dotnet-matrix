using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Matrix.MessageDispatch.Benchmarks.MediatorLibrary;

/// <summary>
/// Mediator generates its dispatcher at compile time, yet its behaviours and handlers are
/// still resolved from the container. The container and the message instances are created
/// once, so each measured method performs exactly one Send or Publish through the generated
/// call path.
/// </summary>
internal static class MediatorConfiguration
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

    private static ServiceProvider BuildMediator()
    {
        var services = new ServiceCollection();
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(CountingBehavior<,>));
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(RecoveringBehavior<,>));
        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Singleton);
        return services.BuildServiceProvider();
    }

    /// <summary>Builds one container for the registration scenario.</summary>
    public static ServiceProvider BuildScope()
    {
        var services = new ServiceCollection();
        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Singleton);
        return services.BuildServiceProvider();
    }
}

public sealed record SendQueryMessage(int Value) : IRequest<int>;

public sealed record SendCommandMessage(int Value) : IRequest;

public sealed record AsyncQueryMessage(int Value) : IRequest<int>;

public sealed record PublishNotificationMessage(int Value) : INotification;

public sealed record BehaviorQueryMessage(int Value) : IRequest<int>;

public sealed record ThrowingQueryMessage(int Value) : IRequest<int>;

public sealed class SendQueryHandler : IRequestHandler<SendQueryMessage, int>
{
    public ValueTask<int> Handle(SendQueryMessage request, CancellationToken cancellationToken)
    {
        MediatorConfiguration.Counter++;
        return new(request.Value + 1);
    }
}

public sealed class SendCommandHandler : IRequestHandler<SendCommandMessage>
{
    public ValueTask<Unit> Handle(SendCommandMessage request, CancellationToken cancellationToken)
    {
        MediatorConfiguration.Counter++;
        return new(Unit.Value);
    }
}

public sealed class AsyncQueryHandler : IRequestHandler<AsyncQueryMessage, int>
{
    public async ValueTask<int> Handle(AsyncQueryMessage request, CancellationToken cancellationToken)
    {
        await Task.Yield();
        MediatorConfiguration.Counter++;
        return request.Value + 1;
    }
}

public sealed class NotificationHandlerA : INotificationHandler<PublishNotificationMessage>
{
    public ValueTask Handle(PublishNotificationMessage notification, CancellationToken cancellationToken)
    {
        MediatorConfiguration.Counter++;
        return default;
    }
}

public sealed class NotificationHandlerB : INotificationHandler<PublishNotificationMessage>
{
    public ValueTask Handle(PublishNotificationMessage notification, CancellationToken cancellationToken)
    {
        MediatorConfiguration.Counter++;
        return default;
    }
}

public sealed class NotificationHandlerC : INotificationHandler<PublishNotificationMessage>
{
    public ValueTask Handle(PublishNotificationMessage notification, CancellationToken cancellationToken)
    {
        MediatorConfiguration.Counter++;
        return default;
    }
}

public sealed class BehaviorQueryHandler : IRequestHandler<BehaviorQueryMessage, int>
{
    public ValueTask<int> Handle(BehaviorQueryMessage request, CancellationToken cancellationToken)
    {
        MediatorConfiguration.Counter++;
        return new(request.Value + 1);
    }
}

public sealed class ThrowingQueryHandler : IRequestHandler<ThrowingQueryMessage, int>
{
    public ValueTask<int> Handle(ThrowingQueryMessage request, CancellationToken cancellationToken) =>
        throw new InvalidOperationException("The handler of this scenario always fails.");
}

/// <summary>Counts its own request type on the way to the handler.</summary>
public sealed class CountingBehavior<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse>
    where TMessage : notnull, IMessage
{
    public ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        if (message is BehaviorQueryMessage)
        {
            MediatorConfiguration.Observed++;
        }

        return next(message, cancellationToken);
    }
}

/// <summary>
/// Mediator ships a request exception handler type, but the generated dispatcher resolves
/// pipeline behaviours from the container, so the exception handler is registered exactly as
/// the library documents it: as an open generic <see cref="IPipelineBehavior{TMessage,TResponse}"/>.
/// </summary>
public sealed class RecoveringBehavior<TMessage, TResponse> : MessageExceptionHandler<TMessage, TResponse>
    where TMessage : notnull, IMessage
{
    protected override ValueTask<ExceptionHandlingResult<TResponse>> Handle(
        TMessage message,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (message is not ThrowingQueryMessage)
        {
            return NotHandled;
        }

        MediatorConfiguration.Handled++;
        return new(ExceptionHandlingResult<TResponse>.Handled(
            (TResponse)(object)MessageDispatchData.RecoveredAnswer));
    }
}
