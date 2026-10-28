namespace Matrix.MessageDispatch.Models;

/// <summary>
/// One notification published to every handler registered for its type.
/// </summary>
public sealed record NotificationResult(int Invocations)
{
    public static NotificationResult Expected { get; } = new(3);
}
