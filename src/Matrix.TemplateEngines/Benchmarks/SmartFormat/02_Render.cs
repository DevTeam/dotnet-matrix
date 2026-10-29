// ReSharper disable CheckNamespace
namespace Matrix.TemplateEngines.Benchmarks;

public partial class Render
{
    private const string SmartFormatSource = "Hello, {name}!";

    private global::SmartFormat.SmartFormatter _smartFormatter = null!;

    private object[] _smartArguments = null!;

    [GlobalSetup(Target = nameof(SmartFormat))]
    public void SetupSmartFormat()
    {
        _smartFormatter = global::SmartFormat.Smart.CreateDefaultSmartFormat(
            new global::SmartFormat.Core.Settings.SmartSettings());
        _smartArguments = [new { name = "World" }];
    }

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.SmartFormat)]
    public string SmartFormat()
    {
        var text = _smartFormatter.Format(SmartFormatSource, _smartArguments);
        TemplateChecks.Output(LibraryCatalog.SmartFormat, text, TemplateData.GreetingExpected);
        return text;
    }
}
