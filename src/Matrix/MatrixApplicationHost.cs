using System.Reflection;

namespace Matrix;

public static class MatrixApplicationHost
{
    public static int Run(string[] args, Assembly moduleAssembly, MatrixRunMode mode) =>
        new MatrixComposition(args, moduleAssembly, mode).Root.Run();
}
