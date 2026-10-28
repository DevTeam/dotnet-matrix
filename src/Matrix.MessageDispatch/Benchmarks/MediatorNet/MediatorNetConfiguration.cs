using Mediator.Net;
using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Mediator.Net.Pipeline;

namespace Matrix.MessageDispatch.Benchmarks.MediatorNetLibrary;

/// <summary>
/// Mediator.Net reflects over the registered assemblies once when the mediator is built and
/// keeps the handler registry. The mediator, its pipes and the message instances are created
/// once, so each measured method performs exactly one Send, Request or Publish.
/// </summary>
internal static class MediatorNetConfiguration
{
    public static readonly SendQueryMessage Query = new(MessageDispatchData.Value);

    public static readonly SendCommandMessage Command = new(MessageDispatchData.Value);

    public static readonly AsyncQueryMessage AsyncQuery = new(MessageDispatchData.Value);

    public static readonly PublishNotificationMessage Notification = new(MessageDispatchData.Value);

    public static readonly BehaviorQueryMessage BehaviorQuery = new(MessageDispatchData.Value);

    public static readonly ThrowingQueryMessage ThrowingQuery = new(MessageDispatchData.Value);

    public static readonly IMediator Mediator = BuildMediator();

    public static int Counter;

    public static int Observed;

    public static int Handled;

    /// <summary>Builds one mediator for the registration scenario.</summary>
    public static IMediator BuildScope() => BuildMediator();

    private static IMediator BuildMediator() =>
        new MediatorBuilder()
            .RegisterHandlers(typeof(MediatorNetConfiguration).Assembly)
            .ConfigureGlobalReceivePipe(configurator =>
            {
                configurator.AddPipeSpecification(new CountingSpecification());
                configurator.AddPipeSpecification(new RecoveringSpecification());
            })
            .Build();
}

public sealed record QueryAnswer(int Value) : IResponse;

public sealed class SendQueryMessage : IRequest
{
    public SendQueryMessage(int value) => Value = value;

    public int Value { get; set; }
}

public sealed class SendCommandMessage : ICommand
{
    public SendCommandMessage(int value) => Value = value;

    public int Value { get; set; }
}

public sealed class AsyncQueryMessage : IRequest
{
    public AsyncQueryMessage(int value) => Value = value;

    public int Value { get; set; }
}

public sealed class PublishNotificationMessage : IEvent
{
    public PublishNotificationMessage(int value) => Value = value;

    public int Value { get; set; }
}

public sealed class BehaviorQueryMessage : IRequest
{
    public BehaviorQueryMessage(int value) => Value = value;

    public int Value { get; set; }
}

public sealed class ThrowingQueryMessage : IRequest
{
    public ThrowingQueryMessage(int value) => Value = value;

    public int Value { get; set; }
}

public sealed class SendQueryHandler : IRequestHandler<SendQueryMessage, QueryAnswer>
{
    public Task<QueryAnswer> Handle(IReceiveContext<SendQueryMessage> context, CancellationToken cancellationToken)
    {
        MediatorNetConfiguration.Counter++;
        return Task.FromResult(new QueryAnswer(context.Message.Value + 1));
    }
}

public sealed class SendCommandHandler : ICommandHandler<SendCommandMessage>
{
    public Task Handle(IReceiveContext<SendCommandMessage> context, CancellationToken cancellationToken)
    {
        MediatorNetConfiguration.Counter++;
        return Task.CompletedTask;
    }
}

public sealed class AsyncQueryHandler : IRequestHandler<AsyncQueryMessage, QueryAnswer>
{
    public async Task<QueryAnswer> Handle(IReceiveContext<AsyncQueryMessage> context, CancellationToken cancellationToken)
    {
        await Task.Yield();
        MediatorNetConfiguration.Counter++;
        return new QueryAnswer(context.Message.Value + 1);
    }
}

public sealed class PublishNotificationHandlerA : IEventHandler<PublishNotificationMessage>
{
    public Task Handle(IReceiveContext<PublishNotificationMessage> context, CancellationToken cancellationToken)
    {
        MediatorNetConfiguration.Counter++;
        return Task.CompletedTask;
    }
}

public sealed class PublishNotificationHandlerB : IEventHandler<PublishNotificationMessage>
{
    public Task Handle(IReceiveContext<PublishNotificationMessage> context, CancellationToken cancellationToken)
    {
        MediatorNetConfiguration.Counter++;
        return Task.CompletedTask;
    }
}

public sealed class PublishNotificationHandlerC : IEventHandler<PublishNotificationMessage>
{
    public Task Handle(IReceiveContext<PublishNotificationMessage> context, CancellationToken cancellationToken)
    {
        MediatorNetConfiguration.Counter++;
        return Task.CompletedTask;
    }
}

public sealed class BehaviorQueryHandler : IRequestHandler<BehaviorQueryMessage, QueryAnswer>
{
    public Task<QueryAnswer> Handle(IReceiveContext<BehaviorQueryMessage> context, CancellationToken cancellationToken)
    {
        MediatorNetConfiguration.Counter++;
        return Task.FromResult(new QueryAnswer(context.Message.Value + 1));
    }
}

public sealed class ThrowingQueryHandler : IRequestHandler<ThrowingQueryMessage, QueryAnswer>
{
    public Task<QueryAnswer> Handle(IReceiveContext<ThrowingQueryMessage> context, CancellationToken cancellationToken) =>
        throw new InvalidOperationException("The handler of this scenario always fails.");
}

/// <summary>Counts its own request type on the way to the handler.</summary>
public sealed class CountingSpecification : IPipeSpecification<IReceiveContext<IMessage>>
{
    public bool ShouldExecute(IReceiveContext<IMessage> context, CancellationToken cancellationToken) =>
        context.Message is BehaviorQueryMessage;

    public Task BeforeExecute(IReceiveContext<IMessage> context, CancellationToken cancellationToken)
    {
        MediatorNetConfiguration.Observed++;
        return Task.CompletedTask;
    }

    public Task Execute(IReceiveContext<IMessage> context, CancellationToken cancellationToken) =>
        Task.CompletedTask;

    public Task AfterExecute(IReceiveContext<IMessage> context, CancellationToken cancellationToken) =>
        Task.CompletedTask;

    public Task OnException(Exception exception, IReceiveContext<IMessage> context) => Task.CompletedTask;
}

/// <summary>
/// Mediator.Net handles a handler failure in a pipe specification's OnException, which is
/// the documented exception extension point of its receive pipeline.
/// </summary>
public sealed class RecoveringSpecification : IPipeSpecification<IReceiveContext<IMessage>>
{
    public bool ShouldExecute(IReceiveContext<IMessage> context, CancellationToken cancellationToken) =>
        context.Message is ThrowingQueryMessage;

    public Task BeforeExecute(IReceiveContext<IMessage> context, CancellationToken cancellationToken) =>
        Task.CompletedTask;

    public Task Execute(IReceiveContext<IMessage> context, CancellationToken cancellationToken) =>
        Task.CompletedTask;

    public Task AfterExecute(IReceiveContext<IMessage> context, CancellationToken cancellationToken) =>
        Task.CompletedTask;

    public Task OnException(Exception exception, IReceiveContext<IMessage> context)
    {
        MediatorNetConfiguration.Handled++;
        context.Result = new QueryAnswer(MessageDispatchData.RecoveredAnswer);
        context.ResultDataType = typeof(QueryAnswer);
        return Task.CompletedTask;
    }
}
