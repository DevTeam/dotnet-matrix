using System.Text.Json;

namespace Matrix;

public interface IMatrixReportReader
{
    bool Exists(string fileName);

    T? Read<T>(string fileName, JsonSerializerOptions? options = null);
}
