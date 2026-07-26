namespace Matrix.DependencyInjection.Scenarios;

public interface IScopedDependency;

public sealed class ScopedDependency : IScopedDependency, IDisposable
{
    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
#if MATRIX_VALIDATION
        IsDisposed = true;
#endif
    }
}

public sealed class ScopedRoot(IScopedDependency dependency)
{
    public IScopedDependency Dependency { get; } = dependency;
}
