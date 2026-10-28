namespace Matrix.MessageDispatch.Models;

/// <summary>
/// Building the container, registering the handlers and resolving the dispatcher.
/// </summary>
public sealed record RegistrationResult(bool Created, int Answer)
{
    public static RegistrationResult Expected { get; } = new(true, MessageDispatchData.Answer);
}
