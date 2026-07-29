namespace Matrix;

public sealed class RunnerOptionsParser : IRunnerOptionsParser
{
    public RunnerOptions Parse(string[] args, string defaultOutput)
    {
        var output = defaultOutput;
        var libraries = new List<string>();
        var smoke = false;
        for (var index = 0; index < args.Length; index++)
        {
            switch (args[index])
            {
                case "--output" when index + 1 < args.Length:
                    output = args[++index];
                    break;

                case "--libraries" when index + 1 < args.Length:
                    while (index + 1 < args.Length && !args[index + 1].StartsWith("--", StringComparison.Ordinal))
                    {
                        libraries.Add(args[++index]);
                    }

                    break;

                case "--smoke":
                    smoke = true;
                    break;

                default:
                    throw new ArgumentException($"Unknown or incomplete argument '{args[index]}'.");
            }
        }

        return new RunnerOptions(output, libraries, smoke);
    }
}