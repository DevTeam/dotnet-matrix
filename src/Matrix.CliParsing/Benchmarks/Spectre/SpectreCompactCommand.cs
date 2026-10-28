using Spectre.Console.Cli;

namespace Matrix.CliParsing.Benchmarks;

internal sealed class SpectreCompactCommand : Command<SpectreCompactSettings>
{
    public SpectreCompactSettings? Captured { get; set; }

    public override int Execute(CommandContext context, SpectreCompactSettings settings, CancellationToken cancellationToken)
    {
        Captured = settings;
        return 0;
    }
}
