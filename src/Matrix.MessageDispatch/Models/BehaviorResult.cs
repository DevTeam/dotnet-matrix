namespace Matrix.MessageDispatch.Models;

/// <summary>
/// One request dispatched through an application-supplied pipeline behavior.
/// </summary>
public sealed record BehaviorResult(int Answer, int Observed)
{
    public static BehaviorResult Expected { get; } = new(MessageDispatchData.Answer, 1);
}
