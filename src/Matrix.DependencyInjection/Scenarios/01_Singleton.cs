using System.Composition;
using Export = System.Composition.ExportAttribute;

namespace Matrix.DependencyInjection.Scenarios;

public interface ISingleton1;
public interface ISingleton2;
public interface ISingleton3;

[Export(typeof(ISingleton1))]
[Shared]
public sealed class Singleton1 : ISingleton1;

[Export(typeof(ISingleton2))]
[Shared]
public sealed class Singleton2 : ISingleton2;

[Export(typeof(ISingleton3))]
[Shared]
public sealed class Singleton3 : ISingleton3;
