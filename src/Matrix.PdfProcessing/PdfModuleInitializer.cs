using System.Runtime.CompilerServices;

namespace Matrix.PdfProcessing;

/// <summary>
/// Runs the library bootstrap once when the module assembly loads, which is
/// before validation or BenchmarkDotNet touches a benchmark type. No benchmark
/// method and no constructor on the measured path performs library setup.
/// </summary>
internal static class PdfModuleInitializer
{
    [ModuleInitializer]
    internal static void Initialize() => PdfBootstrap.EnsureConfigured();
}
