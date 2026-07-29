// ReSharper disable UnusedMemberInSuper.Global
namespace Matrix;

public sealed class MatrixLibraryCatalog(MatrixModule module) : IMatrixLibraryCatalog
{
    public IReadOnlyList<MatrixLibrary> All => module.Libraries;

    public IReadOnlyList<MatrixLibrary> Filter(IEnumerable<string> filters)
    {
        var patterns = filters
            .SelectMany(i => i.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (patterns.Length == 0)
        {
            return All;
        }

        var libraries = All
            .Where(library => patterns.Any(pattern =>
                Matches(library.Id, pattern)
                || Matches(library.Name, pattern)
                || Matches(library.Package, pattern)))
            .ToArray();
        if (libraries.Length == 0)
        {
            throw new ArgumentException(
                $"No libraries match '{string.Join(", ", patterns)}'. "
                + $"Available libraries: {string.Join(", ", All.Select(i => i.Id))}.");
        }

        return libraries;
    }

    private static bool Matches(string value, string pattern)
    {
        if (pattern == "*")
        {
            return true;
        }

        var parts = pattern.Split('*');
        var position = 0;
        foreach (var part in parts)
        {
            if (part.Length == 0)
            {
                continue;
            }

            var index = value.IndexOf(part, position, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                return false;
            }

            position = index + part.Length;
        }

        return (pattern.StartsWith('*') || value.StartsWith(parts[0], StringComparison.OrdinalIgnoreCase))
               && (pattern.EndsWith('*') || value.EndsWith(parts[^1], StringComparison.OrdinalIgnoreCase));
    }
}
