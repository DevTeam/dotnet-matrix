namespace Matrix.MessageDispatch.Models;

/// <summary>
/// One dispatch of a request whose handler completes asynchronously.
/// </summary>
public sealed record AsyncResult(int Answer, int Invocations)
{
    public static AsyncResult Expected { get; } = new(MessageDispatchData.Answer, 1);
}
