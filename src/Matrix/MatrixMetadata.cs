using System.Reflection;
using System.Xml.Linq;

namespace Matrix;

public static class MatrixMetadata
{
    private const string ProjectResourceName = "Matrix.Project.csproj";

    public static MatrixModule Read(Assembly assembly)
    {
        if (TryRead(assembly, out var module))
        {
            return module;
        }

        throw new InvalidOperationException(
            $"Assembly '{assembly.FullName}' has no embedded matrix project metadata.");
    }

    public static bool TryRead(
        Assembly assembly,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out MatrixModule? metadata)
    {
        using var stream = assembly.GetManifestResourceStream(ProjectResourceName);
        if (stream is null)
        {
            metadata = null;
            return false;
        }

        var project = XDocument.Load(stream);
        var moduleId = Value(project, "MatrixModuleId");
        if (moduleId is null)
        {
            metadata = null;
            return false;
        }

        var packageLibraries = project
            .Descendants()
            .Where(element =>
                element.Name.LocalName == "PackageReference"
                && Value(element, "MatrixLibraryId") is not null)
            .Select(ReadPackageLibrary);
        var sourceLibraries = project
            .Descendants()
            .Where(element => element.Name.LocalName == "MatrixLibrary")
            .Select(ReadSourceLibrary);
        var libraries = packageLibraries
            .Concat(sourceLibraries)
            .OrderBy(library => library.Library.Id, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        EnsureUnique(libraries, item => item.Library.Id, "library id");
        EnsureUnique(
            [.. libraries.Where(item => item.Library.Package is not null)],
            item => item.Library.Package!,
            "primary package");
        EnsureUnique(libraries, item => item.CodeName, "library code name");

        metadata = new MatrixModule(
            moduleId,
            Required(project, "MatrixModuleName"),
            Required(project, "MatrixRunConfigurationPrefix"),
            Required(project, "MatrixReportDirectory"),
            [.. libraries.Select(item => item.Library)],
            new MatrixLibraryMetadataCatalog(
                1,
                [.. libraries.Select(item => item.Metadata)]),
            new MatrixFeatureCatalog(1, ReadFeatures(assembly)));
        return true;
    }

    /// <summary>
    /// Features are declared on the classes that benchmark them, so they are read
    /// from the module assembly rather than from the embedded project file.
    /// </summary>
    private static MatrixFeatureMetadata[] ReadFeatures(Assembly assembly)
    {
        var features = assembly
            .GetTypes()
            .Select(type => type.GetCustomAttribute<MatrixFeatureAttribute>())
            .Where(feature => feature is not null)
            .Select(feature => new MatrixFeatureMetadata(
                feature!.Id,
                feature.Order,
                feature.Name,
                feature.Description.Trim(),
                feature.Rated,
                feature.Reason?.Trim()))
            .DistinctBy(feature => feature.Id, StringComparer.OrdinalIgnoreCase)
            .OrderBy(feature => feature.Order)
            .ToArray();
        foreach (var feature in features)
        {
            if (feature.Description.Length == 0)
            {
                throw new InvalidOperationException(
                    $"Matrix feature '{feature.Id}' must define a description.");
            }

            // A false Rated with no stated reason is exactly the undocumented
            // "feature-only" leftover this mechanism replaced.
            if (!feature.Rated && string.IsNullOrEmpty(feature.Reason))
            {
                throw new InvalidOperationException(
                    $"Matrix feature '{feature.Id}' is Rated: false and must give a reason.");
            }
        }

        var duplicateOrder = features
            .GroupBy(feature => feature.Order)
            .FirstOrDefault(group => group.Count() > 1);
        return duplicateOrder is null
            ? features
            : throw new InvalidOperationException(
                $"Duplicate matrix feature order '{duplicateOrder.Key}'.");
    }

    private static MatrixLibraryDefinition ReadPackageLibrary(XElement packageReference)
    {
        if (packageReference.Attribute("Condition") is not null)
        {
            throw new InvalidOperationException(
                "A matrix library PackageReference cannot be conditional.");
        }

        var package = RequiredAttribute(packageReference, "Include");
        var version = RequiredAttribute(packageReference, "Version");
        if (version.Contains("$(", StringComparison.Ordinal)
            || version.IndexOfAny(['*', '[', ']', '(', ')', ',']) >= 0)
        {
            throw new InvalidOperationException(
                $"Matrix library package '{package}' must use an exact literal version.");
        }

        var id = Required(packageReference, "MatrixLibraryId");
        return ReadLibrary(
            packageReference,
            id,
            package,
            version,
            ReadBoolean(packageReference, "MatrixBaseline", id, false));
    }

    private static MatrixLibraryDefinition ReadSourceLibrary(XElement library)
    {
        if (library.Attribute("Condition") is not null)
        {
            throw new InvalidOperationException(
                "A package-less MatrixLibrary cannot be conditional.");
        }

        var id = RequiredAttribute(library, "Include");
        var baseline = ReadBoolean(library, "MatrixBaseline", id, false);
        if (!baseline)
        {
            throw new InvalidOperationException(
                $"Package-less matrix library '{id}' must define MatrixBaseline=true.");
        }

        return ReadLibrary(library, id, null, null, baseline);
    }

    private static MatrixLibraryDefinition ReadLibrary(
        XElement element,
        string id,
        string? package,
        string? version,
        bool baseline)
    {
        var documentationUrl = Value(element, "MatrixDocumentationUrl");
        var repositoryUrl = Value(element, "MatrixRepositoryUrl");
        if (!baseline && documentationUrl is null && repositoryUrl is null)
        {
            throw new InvalidOperationException(
                $"Matrix library '{id}' must define documentation or repository URL.");
        }

        return new MatrixLibraryDefinition(
            new MatrixLibrary(
                id,
                Required(element, "MatrixLibraryName"),
                package,
                version,
                baseline),
            Required(element, "MatrixCodeName"),
            new MatrixLibraryMetadata(
                id,
                Required(element, "MatrixDescription"),
                documentationUrl,
                repositoryUrl,
                Required(element, "MatrixLogo"),
                ReadBoolean(element, "MatrixRating", id, !baseline),
                baseline));
    }

    private static bool ReadBoolean(
        XElement element,
        string name,
        string id,
        bool defaultValue)
    {
        var value = Value(element, name);
        if (value is null)
        {
            return defaultValue;
        }

        return bool.TryParse(value, out var result)
            ? result
            : throw new InvalidOperationException(
                $"Matrix library '{id}' has a non-boolean {name} value '{value}'.");
    }

    private static void EnsureUnique(
        IReadOnlyList<MatrixLibraryDefinition> libraries,
        Func<MatrixLibraryDefinition, string> selector,
        string name)
    {
        var duplicate = libraries
            .GroupBy(selector, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicate is not null)
        {
            throw new InvalidOperationException(
                $"Duplicate matrix {name} '{duplicate.Key}'.");
        }
    }

    private static string Required(XContainer container, string name) =>
        Value(container, name)
        ?? throw new InvalidOperationException(
            $"Embedded matrix project metadata has no '{name}'.");

    private static string? Value(XContainer container, string name)
    {
        var elements = container is XDocument
            ? container.Descendants()
            : container.Elements();
        var values = elements
            .Where(element => element.Name.LocalName == name)
            .Select(element => element.Value.Trim())
            .Where(value => value.Length > 0)
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        return values.Length switch
        {
            0 => null,
            1 => values[0],
            _ => throw new InvalidOperationException(
                $"Embedded matrix project metadata has multiple '{name}' values.")
        };
    }

    private static string RequiredAttribute(XElement element, string name)
    {
        var value = element.Attribute(name)?.Value.Trim();
        return string.IsNullOrWhiteSpace(value)
            ? throw new InvalidOperationException(
                $"Matrix library item has no '{name}' attribute.")
            : value;
    }
}
