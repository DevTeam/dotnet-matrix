using Maestro;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

public partial class Conditional
{
    private Container _maestro = null!;

    [GlobalSetup(Target = nameof(Maestro))]
    public void SetupMaestro() =>
        _maestro = new Container(builder =>
        {
            builder.Add<IConditionalService>().Named("1").Type<ConditionalService1>().Transient();
            builder.Add<IConditionalService>().Named("2").Type<ConditionalService2>().Transient();
            builder.Add<IConditionalService>().Named("3").Type<ConditionalService3>().Transient();
            builder.Add<ConditionalRoot1>().Self()
                .CtorArg<IConditionalService>(context => context.GetService<IConditionalService>("1"))
                .Transient();
            builder.Add<ConditionalRoot2>().Self()
                .CtorArg<IConditionalService>(context => context.GetService<IConditionalService>("2"))
                .Transient();
            builder.Add<ConditionalRoot3>().Self()
                .CtorArg<IConditionalService>(context => context.GetService<IConditionalService>("3"))
                .Transient();
        });

    [GlobalCleanup(Target = nameof(Maestro))]
    public void CleanupMaestro() => _maestro.Dispose();

    [Benchmark]
    [LibraryBenchmark(LibraryCatalog.Maestro)]
    public BenchmarkRoots<ConditionalRoot1, ConditionalRoot2, ConditionalRoot3> Maestro()
    {
        var first = _maestro.GetService<ConditionalRoot1>();
        var second = _maestro.GetService<ConditionalRoot2>();
        var third = _maestro.GetService<ConditionalRoot3>();
        Validate(LibraryCatalog.Maestro, first, second, third);
        return new(first, second, third);
    }
}
