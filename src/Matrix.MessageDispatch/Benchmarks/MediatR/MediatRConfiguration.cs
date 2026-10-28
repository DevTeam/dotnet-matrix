using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Matrix.MessageDispatch.Benchmarks.MediatRLibrary;

/// <summary>
/// MediatR resolves every handler and behavior through the container at run time. The
/// container, the registered behaviors and the message instances are created here so that
/// each measured method performs exactly one Send or Publish. The exception scenario
/// registers a behavior for its own request type only, so it cannot affect another scenario.
/// </summary>
internal static class MediatRConfiguration
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
        services.AddLogging();
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblyContaining<SendQueryHandler>());
        services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(CountingBehavior<,>));
        services.AddTransient(
            typeof(IPipelineBehavior<ThrowingQueryMessage, int>),
            typeof(RecoveringBehavior));
        return services.BuildServiceProvider();
    }

    /// <summary>Builds one container for the registration scenario.</summary>
    public static ServiceProvider BuildScope()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblyContaining<SendQueryHandler>());
        return services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = false,
            ValidateScopes = false
        });
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
    public Task<int> Handle(SendQueryMessage request, CancellationToken cancellationToken)
    {
        MediatRConfiguration.Counter++;
        return Task.FromResult(request.Value + 1);
    }
}

public sealed class SendCommandHandler : IRequestHandler<SendCommandMessage>
{
    public Task Handle(SendCommandMessage request, CancellationToken cancellationToken)
    {
        MediatRConfiguration.Counter++;
        return Task.CompletedTask;
    }
}

public sealed class AsyncQueryHandler : IRequestHandler<AsyncQueryMessage, int>
{
    public async Task<int> Handle(AsyncQueryMessage request, CancellationToken cancellationToken)
    {
        await Task.Yield();
        MediatRConfiguration.Counter++;
        return request.Value + 1;
    }
}

public sealed class NotificationHandlerA : INotificationHandler<PublishNotificationMessage>
{
    public Task Handle(PublishNotificationMessage notification, CancellationToken cancellationToken)
    {
        MediatRConfiguration.Counter++;
        return Task.CompletedTask;
    }
}

public sealed class NotificationHandlerB : INotificationHandler<PublishNotificationMessage>
{
    public Task Handle(PublishNotificationMessage notification, CancellationToken cancellationToken)
    {
        MediatRConfiguration.Counter++;
        return Task.CompletedTask;
    }
}

public sealed class NotificationHandlerC : INotificationHandler<PublishNotificationMessage>
{
    public Task Handle(PublishNotificationMessage notification, CancellationToken cancellationToken)
    {
        MediatRConfiguration.Counter++;
        return Task.CompletedTask;
    }
}

public sealed class BehaviorQueryHandler : IRequestHandler<BehaviorQueryMessage, int>
{
    public Task<int> Handle(BehaviorQueryMessage request, CancellationToken cancellationToken)
    {
        MediatRConfiguration.Counter++;
        return Task.FromResult(request.Value + 1);
    }
}

/// <summary>Counts its own request type on the way to the handler.</summary>
public sealed class CountingBehavior<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse>
    where TMessage : notnull
{
    public Task<TResponse> Handle(
        TMessage request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is BehaviorQueryMessage)
        {
            MediatRConfiguration.Observed++;
        }

        return next(cancellationToken);
    }
}

public sealed class ThrowingQueryHandler : IRequestHandler<ThrowingQueryMessage, int>
{
    public Task<int> Handle(ThrowingQueryMessage request, CancellationToken cancellationToken) =>
        throw new InvalidOperationException("The handler of this scenario always fails.");
}

/// <summary>
/// MediatR 14 has no request-level exception handler in its pipeline, so the documented
/// wrapping extension point is a behavior: this one catches the failure of its own request
/// type and returns the recovered answer.
/// </summary>
public sealed class RecoveringBehavior : IPipelineBehavior<ThrowingQueryMessage, int>
{
    public async Task<int> Handle(
        ThrowingQueryMessage request,
        RequestHandlerDelegate<int> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken).ConfigureAwait(false);
        }
        catch (InvalidOperationException)
        {
            MediatRConfiguration.Handled++;
            return MessageDispatchData.RecoveredAnswer;
        }
    }
}
