using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wolverine;
using Wolverine.Attributes;

namespace Matrix.MessageDispatch.Benchmarks.WolverineLibrary;

/// <summary>
/// Wolverine builds its handler chains when the host starts. The host, the message bus and the
/// message instances are created once, so each measured method performs exactly one in-process
/// invocation without a transport.
/// </summary>
/// <remarks>
/// The module compiles the handlers of every library into one assembly, so conventional handler
/// discovery would also claim the handlers of other libraries. Discovery is therefore restricted
/// to the handler types of this category.
/// </remarks>
internal static class WolverineConfiguration
{
    public static readonly SendQueryMessage Query = new(MessageDispatchData.Value);

    public static readonly SendCommandMessage Command = new(MessageDispatchData.Value);

    public static readonly AsyncQueryMessage AsyncQuery = new(MessageDispatchData.Value);

    public static readonly PublishNotificationMessage Notification = new(MessageDispatchData.Value);

    public static readonly BehaviorQueryMessage BehaviorQuery = new(MessageDispatchData.Value);

    public static readonly ThrowingQueryMessage ThrowingQuery = new(MessageDispatchData.Value);

    public static readonly IHost Host = BuildHost();

    public static readonly IMessageBus Bus = StartAndResolve();

    public static int Counter;

    public static int Observed;

    public static int Handled;

    /// <summary>Builds one host for the registration scenario.</summary>
    public static IHost BuildScope()
    {
        var host = BuildHost();
        host.Start();
        return host;
    }

    private static IHost BuildHost()
    {
        var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
        builder.UseWolverine(options =>
        {
            options.Discovery.DisableConventionalDiscovery();
            options.Discovery.IncludeType<SendQueryHandler>();
            options.Discovery.IncludeType<SendCommandHandler>();
            options.Discovery.IncludeType<AsyncQueryHandler>();
            options.Discovery.IncludeType<PublishNotificationHandlerA>();
            options.Discovery.IncludeType<PublishNotificationHandlerB>();
            options.Discovery.IncludeType<PublishNotificationHandlerC>();
            options.Discovery.IncludeType<BehaviorQueryHandler>();
            options.Discovery.IncludeType<ThrowingQueryHandler>();
        });
        return builder.Build();
    }

    private static IMessageBus StartAndResolve()
    {
        Host.Start();
        return Host.Services.GetRequiredService<IMessageBus>();
    }
}

public sealed record QueryAnswer(int Value);

[WolverineMessage]
public sealed record SendQueryMessage(int Value);

[WolverineMessage]
public sealed record SendCommandMessage(int Value);

[WolverineMessage]
public sealed record AsyncQueryMessage(int Value);

[WolverineMessage]
public sealed record PublishNotificationMessage(int Value);

[WolverineMessage]
public sealed record BehaviorQueryMessage(int Value);

[WolverineMessage]
public sealed record ThrowingQueryMessage(int Value);

public sealed class SendQueryHandler
{
    public QueryAnswer Handle(SendQueryMessage message)
    {
        WolverineConfiguration.Counter++;
        return new QueryAnswer(message.Value + 1);
    }
}

public sealed class SendCommandHandler
{
    public void Handle(SendCommandMessage message) => WolverineConfiguration.Counter++;
}

public sealed class AsyncQueryHandler
{
    public async Task<QueryAnswer> HandleAsync(AsyncQueryMessage message, CancellationToken cancellationToken)
    {
        await Task.Yield();
        WolverineConfiguration.Counter++;
        return new QueryAnswer(message.Value + 1);
    }
}

public sealed class PublishNotificationHandlerA
{
    public void Handle(PublishNotificationMessage message) => WolverineConfiguration.Counter++;
}

public sealed class PublishNotificationHandlerB
{
    public void Handle(PublishNotificationMessage message) => WolverineConfiguration.Counter++;
}

public sealed class PublishNotificationHandlerC
{
    public void Handle(PublishNotificationMessage message) => WolverineConfiguration.Counter++;
}

public sealed class BehaviorQueryHandler
{
    /// <summary>Runs before the handler of its own message type only.</summary>
    [WolverineBefore]
    public static void Count(BehaviorQueryMessage message) => WolverineConfiguration.Observed++;

    public QueryAnswer Handle(BehaviorQueryMessage message)
    {
        WolverineConfiguration.Counter++;
        return new QueryAnswer(message.Value + 1);
    }
}

public sealed class ThrowingQueryHandler
{
    public QueryAnswer Handle(ThrowingQueryMessage message) =>
        throw new InvalidOperationException("The handler of this scenario always fails.");

    /// <summary>Wolverine's documented per-handler continuation for a failed message.</summary>
    [WolverineOnException]
    public QueryAnswer HandleFailure(InvalidOperationException exception, ThrowingQueryMessage message)
    {
        WolverineConfiguration.Handled++;
        return new QueryAnswer(MessageDispatchData.RecoveredAnswer);
    }
}
