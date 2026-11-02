using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

// Use BenchmarkDotNet's discovery path: feature validation invokes Setup{method}
// by convention and cannot detect missing or incorrectly targeted GlobalSetup attributes.
var setupCount = 0;
foreach (var type in typeof(Matrix.BinarySerialization.Benchmarks.DeserializeSimpleObject).Assembly.GetTypes())
{
    if (type.GetCustomAttributes(typeof(Matrix.MatrixFeatureAttribute), false).Length == 0)
    {
        continue;
    }

    var run = BenchmarkConverter.TypeToBenchmarks(type);
    foreach (var benchmark in run.BenchmarksCases)
    {
        var descriptor = benchmark.Descriptor;
        var expectedSetup = type.GetMethod($"Setup{descriptor.WorkloadMethod.Name}");
        if (descriptor.GlobalSetupMethod != expectedSetup)
        {
            throw new InvalidOperationException($"Wrong setup for {descriptor.DisplayInfo}: expected {expectedSetup?.Name}.");
        }

        if (expectedSetup is not null)
        {
            setupCount++;
            var instance = Activator.CreateInstance(type)!;
            descriptor.GlobalSetupMethod!.Invoke(instance, null);
            descriptor.WorkloadMethod.Invoke(instance, null);
        }
    }
}

if (setupCount == 0)
{
    throw new InvalidOperationException("No binary serialization setup scenarios were discovered.");
}

Console.WriteLine($"PASS: {setupCount} binary serialization setups discovered and executed.");

var cli = BenchmarkConverter.TypeToBenchmarks(typeof(Matrix.CliParsing.Benchmarks.ParseOptionValue));
if (cli.BenchmarksCases.Length == 0 || cli.BenchmarksCases.Any(benchmark =>
        !benchmark.Config.Options.HasFlag(ConfigOptions.DisableOptimizationsValidator)))
{
    throw new InvalidOperationException("CLI benchmarks reject the published PowerArgs package.");
}

var binary = BenchmarkConverter.TypeToBenchmarks(typeof(Matrix.BinarySerialization.Benchmarks.DeserializeSimpleObject));
if (binary.BenchmarksCases.Any(benchmark =>
        benchmark.Config.Options.HasFlag(ConfigOptions.DisableOptimizationsValidator)))
{
    throw new InvalidOperationException("The CLI optimization exception leaked into another module.");
}

Console.WriteLine("PASS: CLI package optimization exception stays within its module.");

// Exercise the actual configured generator through a public benchmark scenario.
// One-shot feature validation cannot reach the sequence limit within a tick.
var batch = new Matrix.Identifiers.Benchmarks.GenerateBatch();
long previous = -1;
for (var iteration = 0; iteration < 100; iteration++)
{
    foreach (var id in batch.IdGen())
    {
        if (id <= previous)
        {
            throw new InvalidOperationException("IdGen did not preserve unique, increasing IDs under sustained load.");
        }

        previous = id;
    }
}

Console.WriteLine("PASS: IdGen generated 100 batches without sequence overflow.");
