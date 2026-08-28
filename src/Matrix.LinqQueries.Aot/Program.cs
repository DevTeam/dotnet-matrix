using Matrix.LinqQueries.Aot;

// The probe reports on stdout and through its exit code, so the build target can attribute a
// failure without parsing prose: 0 is Supported, 1 is a behavioural failure, 2 is a thrown
// exception. A publish that never produces a binary is handled by the target, not here.
try
{
    var mode = AotProbeHost.IsAotCompiled ? "AOT" : "JIT";
    Console.WriteLine($"probe: {AotProbe.Library}");
    Console.WriteLine($"mode: {mode}");

    var delivered = AotProbe.Run();
    if (delivered == AotProbe.ExpectedEvents)
    {
        Console.WriteLine($"PASS delivered {delivered}");
        return 0;
    }

    Console.WriteLine($"FAIL delivered {delivered}, expected {AotProbe.ExpectedEvents}");
    return 1;
}
catch (Exception e)
{
    Console.WriteLine($"FAIL {e.GetType().Name}: {e.Message}");
    return 2;
}
