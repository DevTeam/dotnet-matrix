using System.Text.Json;
using System.Text.Json.Serialization;

namespace Matrix;

public sealed class MatrixReportStore : IMatrixReportStore
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public T? Read<T>(string fileName)
    {
        var path = Path.GetFullPath(fileName);
        return File.Exists(path)
            ? JsonSerializer.Deserialize<T>(File.ReadAllText(path), Options)
            : default;
    }

    public void Write<T>(string fileName, T value)
    {
        var path = Path.GetFullPath(fileName);
        var directory = Path.GetDirectoryName(path)
                        ?? throw new InvalidOperationException($"Cannot determine directory for '{path}'.");
        Directory.CreateDirectory(directory);
        var temporaryFile = Path.Combine(directory, $".{Path.GetFileName(path)}.{Guid.NewGuid():N}.tmp");
        try
        {
            File.WriteAllText(temporaryFile, JsonSerializer.Serialize(value, Options));
            ReplaceWithRetry(temporaryFile, path);
        }
        finally
        {
            if (File.Exists(temporaryFile))
            {
                File.Delete(temporaryFile);
            }
        }
    }

    public void WarnEnvironmentMismatch(
        IReadOnlyCollection<BenchmarkEnvironment> existing,
        BenchmarkEnvironment current)
    {
        if (existing.Count == 0)
        {
            Console.Error.WriteLine(
                "WARNING: The existing benchmark report has no environment information. "
                + "Partial results may not be directly comparable.");
            return;
        }

        foreach (var environment in existing.Where(environment => environment.Id != current.Id))
        {
            Console.Error.WriteLine(
                $"WARNING: Benchmark environment '{environment.Id}' differs from "
                + $"the current environment '{current.Id}' during a partial run.");
            foreach (var difference in BenchmarkEnvironmentComparer.GetDifferences(environment, current))
            {
                Console.Error.WriteLine(
                    $"WARNING:   {difference.Name}: "
                    + $"existing='{difference.Existing}', current='{difference.Current}'");
            }
        }
    }

    private static void ReplaceWithRetry(string source, string destination)
    {
        const int attempts = 5;
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                File.Move(source, destination, true);
                return;
            }
            catch (UnauthorizedAccessException) when (attempt == attempts)
            {
                File.Copy(source, destination, true);
                return;
            }
            catch (Exception exception) when (
                attempt < attempts
                && exception is IOException or UnauthorizedAccessException)
            {
                Thread.Sleep(50 * attempt);
            }
        }
    }
}
