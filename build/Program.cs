using Build;

using var cancellation = new CancellationTokenSource();
ConsoleCancelEventHandler cancel = (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    // ReSharper disable once AccessToDisposedClosure
    cancellation.Cancel();
};
Console.CancelKeyPress += cancel;
try
{
    return await new Composition(args, cancellation.Token).Root.RunAsync();
}
finally
{
    Console.CancelKeyPress -= cancel;
}
