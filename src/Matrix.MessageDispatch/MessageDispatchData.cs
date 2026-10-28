namespace Matrix.MessageDispatch;

/// <summary>
/// The fixed message values every adapter uses. They live in one place so that every
/// library receives identical input and no adapter can drift from the contract.
/// </summary>
internal static class MessageDispatchData
{
    /// <summary>The payload an ordinary request carries.</summary>
    public const int Value = 20;

    /// <summary>The answer every request handler computes from <see cref="Value"/>.</summary>
    public const int Answer = 21;

    /// <summary>The answer an exception handler substitutes after the request handler failed.</summary>
    public const int RecoveredAnswer = 42;
}
