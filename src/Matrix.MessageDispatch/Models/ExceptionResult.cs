namespace Matrix.MessageDispatch.Models;

/// <summary>
/// One request whose handler throws, after the library exception extension point ran.
/// </summary>
public sealed record ExceptionResult(int Answer, bool Handled)
{
    public static ExceptionResult Expected { get; } = new(MessageDispatchData.RecoveredAnswer, true);
}
