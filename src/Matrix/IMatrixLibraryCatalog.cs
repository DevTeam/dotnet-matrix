namespace Matrix;

public interface IMatrixLibraryCatalog
{
    IReadOnlyList<MatrixLibrary> Filter(IEnumerable<string> filters);
}