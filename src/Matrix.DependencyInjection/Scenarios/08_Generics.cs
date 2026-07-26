using System.Composition;
using Export = System.Composition.ExportAttribute;
// ReSharper disable UnusedTypeParameter

namespace Matrix.DependencyInjection.Scenarios;

public interface IGenericService<T>;

[Export(typeof(IGenericService<>))]
public sealed class GenericService<T> : IGenericService<T>;

[Export]
[method: ImportingConstructor]
public sealed class GenericRoot<T>(IGenericService<T> service)
{
    public IGenericService<T> Service { get; } = service;
}
