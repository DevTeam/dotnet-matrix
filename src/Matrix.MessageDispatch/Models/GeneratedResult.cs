namespace Matrix.MessageDispatch.Models;

/// <summary>
/// One dispatch through a dispatcher a compile-time source generator produced.
/// </summary>
public sealed record GeneratedResult(int Answer, int Invocations)
{
    public static GeneratedResult Expected { get; } = new(MessageDispatchData.Answer, 1);
}
