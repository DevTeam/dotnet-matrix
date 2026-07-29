using System.Reflection;
using System.Runtime.Loader;
using Matrix;

namespace Build.Targets;

internal sealed class MatrixAssemblyLoadContext(string assemblyPath)
    : AssemblyLoadContext(isCollectible: true)
{
    private readonly AssemblyDependencyResolver _resolver = new(assemblyPath);

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        if (assemblyName.Name == typeof(MatrixModule).Assembly.GetName().Name)
        {
            return typeof(MatrixModule).Assembly;
        }

        var path = _resolver.ResolveAssemblyToPath(assemblyName);
        return path is null ? null : LoadFromAssemblyPath(path);
    }
}
