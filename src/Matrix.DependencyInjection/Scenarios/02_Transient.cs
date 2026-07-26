// ReSharper disable RedundantUsingDirective
using System.Composition;
using Export = System.Composition.ExportAttribute;

namespace Matrix.DependencyInjection.Scenarios;

public interface ITransient1;
public interface ITransient2;
public interface ITransient3;

[Export(typeof(ITransient1))]
public sealed class Transient1 : ITransient1;

[Export(typeof(ITransient2))]
public sealed class Transient2 : ITransient2;

[Export(typeof(ITransient3))]
public sealed class Transient3 : ITransient3;
