namespace Matrix.MessageDispatch.Models;

/// <summary>
/// One query dispatch: the handler returns the answer it computed from the request.
/// </summary>
public sealed record QueryResult(int Answer, int Invocations)
{
    public static QueryResult Expected { get; } = new(MessageDispatchData.Answer, 1);
}
