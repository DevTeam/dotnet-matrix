namespace Matrix.MessageDispatch.Models;

/// <summary>
/// One command dispatch: its handler returns nothing, so the observable result is how
/// many times the handler ran.
/// </summary>
public sealed record CommandResult(int Invocations)
{
    public static CommandResult Expected { get; } = new(1);
}
