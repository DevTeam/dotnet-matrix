namespace Matrix;

public interface IMatrixLibraryCatalog
{
    IReadOnlyList<MatrixLibrary> All { get; }

    IReadOnlyList<MatrixLibrary> Filter(IEnumerable<string> filters);
}