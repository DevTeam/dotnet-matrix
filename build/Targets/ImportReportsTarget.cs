using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;

namespace Build.Targets;

/// <summary>
/// Imports the single artifact downloaded from the reports workflow. Validation
/// completes before repository files are touched, and changed files are rolled back
/// if an update fails.
/// </summary>
internal sealed class ImportReportsTarget(IBuildPaths buildPaths) : IImportReportsTarget
{
    public int Run(string archivePath)
    {
        var archive = Path.GetFullPath(archivePath, buildPaths.SolutionDirectory);
        if (!File.Exists(archive))
        {
            Console.Error.WriteLine($"Archive '{archive}' does not exist.");
            return 1;
        }

        var temporaryRoot = Path.Combine(
            Path.GetTempPath(),
            "dotnet-matrix-import",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temporaryRoot);
        try
        {
            ExtractSafely(archive, temporaryRoot);
            Validate(temporaryRoot);
            Import(temporaryRoot);
            Host.Info($"Imported reports and evidence from: {archive}");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"Cannot import '{archive}': {exception.Message}");
            return 1;
        }
        finally
        {
            if (Directory.Exists(temporaryRoot))
            {
                Directory.Delete(temporaryRoot, true);
            }
        }
    }

    private static void ExtractSafely(string archive, string destination)
    {
        var destinationPrefix = Path.GetFullPath(destination)
                                + Path.DirectorySeparatorChar;
        using var zip = ZipFile.OpenRead(archive);
        foreach (var entry in zip.Entries)
        {
            var target = Path.GetFullPath(
                Path.Combine(destination, entry.FullName.Replace('/', Path.DirectorySeparatorChar)));
            if (!target.StartsWith(destinationPrefix, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException($"Archive entry '{entry.FullName}' escapes its root.");
            }

            if (string.IsNullOrEmpty(entry.Name))
            {
                Directory.CreateDirectory(target);
                continue;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            entry.ExtractToFile(target, true);
        }
    }

    private static void Validate(string root)
    {
        var manifest = Path.Combine(root, "manifest.json");
        var checksums = Path.Combine(root, "checksums.sha256");
        var reports = Path.Combine(root, "reports");
        if (!File.Exists(manifest) || !File.Exists(checksums) || !Directory.Exists(reports))
        {
            throw new InvalidDataException(
                "The archive must contain manifest.json, checksums.sha256, and reports/.");
        }

        using (var document = JsonDocument.Parse(File.ReadAllText(manifest)))
        {
            var value = document.RootElement;
            if (!value.TryGetProperty("schemaVersion", out var schemaVersion)
                || schemaVersion.GetInt32() != 1
                || !value.TryGetProperty("archive", out var archive)
                || archive.GetString() != "matrix-reports"
                || !value.TryGetProperty("categories", out var categories)
                || categories.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidDataException("The archive manifest is invalid or unsupported.");
            }

            foreach (var category in categories.EnumerateArray())
            {
                var name = category.GetString();
                if (string.IsNullOrWhiteSpace(name)
                    || !File.Exists(Path.Combine(reports, name, "features.json")))
                {
                    throw new InvalidDataException($"Category '{name}' has no feature report.");
                }
            }
        }

        var checkedFiles = new HashSet<string>(StringComparer.Ordinal);
        foreach (var line in File.ReadLines(checksums).Where(line => !string.IsNullOrWhiteSpace(line)))
        {
            if (line.Length < 66)
            {
                throw new InvalidDataException($"Invalid checksum line: '{line}'.");
            }

            var expected = line[..64];
            var relativeName = NormalizeRelativePath(line[64..].TrimStart(' ', '*'));
            if (!checkedFiles.Add(relativeName))
            {
                throw new InvalidDataException($"Duplicate checksum for '{relativeName}'.");
            }

            var relativePath = relativeName.Replace('/', Path.DirectorySeparatorChar);
            var path = Path.GetFullPath(Path.Combine(root, relativePath));
            var rootPrefix = Path.GetFullPath(root) + Path.DirectorySeparatorChar;
            if (!path.StartsWith(rootPrefix, StringComparison.OrdinalIgnoreCase) || !File.Exists(path))
            {
                throw new InvalidDataException($"Checksummed file '{relativePath}' is missing.");
            }

            var actual = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
            if (!actual.Equals(expected, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException($"Checksum mismatch for '{relativePath}'.");
            }
        }


        var archiveFiles = Directory
            .EnumerateFiles(root, "*", SearchOption.AllDirectories)
            .Where(path => !path.Equals(checksums, StringComparison.OrdinalIgnoreCase))
            .Select(path => NormalizeRelativePath(Path.GetRelativePath(root, path)))
            .ToHashSet(StringComparer.Ordinal);
        if (!checkedFiles.SetEquals(archiveFiles))
        {
            throw new InvalidDataException("The checksum list does not cover every archive file.");
        }
    }

    private void Import(string root)
    {
        var mappings = new[]
        {
            (Source: Path.Combine(root, "reports"),
                Destination: Path.Combine(buildPaths.SolutionDirectory, "reports")),
            (Source: Path.Combine(root, "summaries"),
                Destination: Path.Combine(buildPaths.SolutionDirectory, "artifacts", "report-summaries"))
        };
        var metadataDestination = Path.Combine(
            buildPaths.SolutionDirectory,
            "artifacts",
            "imported-reports");
        var files = mappings
            .Where(mapping => Directory.Exists(mapping.Source))
            .SelectMany(mapping => Directory
                .EnumerateFiles(mapping.Source, "*", SearchOption.AllDirectories)
                .Select(source => (
                    Source: source,
                    Destination: Path.Combine(
                        mapping.Destination,
                        Path.GetRelativePath(mapping.Source, source)))))
            .Concat(
            [
                (Path.Combine(root, "manifest.json"), Path.Combine(metadataDestination, "manifest.json")),
                (Path.Combine(root, "checksums.sha256"), Path.Combine(metadataDestination, "checksums.sha256"))
            ])
            .ToArray();

        var backupRoot = Path.Combine(root, ".backup");
        var completed = new List<(string Destination, string? Backup)>();
        var removedDirectories = new List<(string Destination, string Backup)>();
        try
        {
            foreach (var obsolete in FindObsoleteEvidenceDirectories(root))
            {
                var backup = Path.Combine(backupRoot, $"directory-{removedDirectories.Count:D8}");
                CopyDirectory(obsolete, backup);
                Directory.Delete(obsolete, true);
                removedDirectories.Add((obsolete, backup));
            }

            foreach (var (source, destination) in files)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                string? backup = null;
                if (File.Exists(destination))
                {
                    backup = Path.Combine(backupRoot, completed.Count.ToString("D8"));
                    Directory.CreateDirectory(Path.GetDirectoryName(backup)!);
                    File.Copy(destination, backup);
                }

                var temporary = destination + $".import-{Guid.NewGuid():N}";
                File.Copy(source, temporary, true);
                File.Move(temporary, destination, true);
                completed.Add((destination, backup));
            }
        }
        catch
        {
            foreach (var (destination, backup) in completed.AsEnumerable().Reverse())
            {
                if (backup is null)
                {
                    File.Delete(destination);
                }
                else
                {
                    File.Copy(backup, destination, true);
                }
            }

            foreach (var (destination, backup) in removedDirectories.AsEnumerable().Reverse())
            {
                CopyDirectory(backup, destination);
            }

            throw;
        }
    }

    private IEnumerable<string> FindObsoleteEvidenceDirectories(string root)
    {
        var sourceReports = Path.Combine(root, "reports");
        foreach (var sourceEvidence in Directory.EnumerateDirectories(
                     sourceReports,
                     "evidence",
                     SearchOption.AllDirectories))
        {
            var relative = Path.GetRelativePath(sourceReports, sourceEvidence);
            var destinationEvidence = Path.Combine(
                buildPaths.SolutionDirectory,
                "reports",
                relative);
            if (!Directory.Exists(destinationEvidence))
            {
                continue;
            }

            var activeIds = Directory
                .EnumerateDirectories(sourceEvidence)
                .Select(Path.GetFileName)
                .ToHashSet(StringComparer.Ordinal);
            foreach (var destination in Directory.EnumerateDirectories(destinationEvidence))
            {
                if (!activeIds.Contains(Path.GetFileName(destination)))
                {
                    yield return destination;
                }
            }
        }
    }

    private static void CopyDirectory(string source, string destination)
    {
        foreach (var file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories))
        {
            var target = Path.Combine(destination, Path.GetRelativePath(source, file));
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            File.Copy(file, target, true);
        }
    }

    private static string NormalizeRelativePath(string path) =>
        path.Replace('\\', '/').TrimStart('.', '/');
}
