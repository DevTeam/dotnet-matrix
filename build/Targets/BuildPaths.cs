namespace Build.Targets;

internal interface IBuildPaths
{
    string SolutionDirectory { get; }
}

internal sealed class BuildPaths : IBuildPaths
{
    public BuildPaths()
    {
        foreach (var start in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        {
            var directory = new DirectoryInfo(start);
            while (directory is not null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "dotnet-matrix.slnx")))
                {
                    SolutionDirectory = directory.FullName;
                    return;
                }

                directory = directory.Parent;
            }
        }

        throw new DirectoryNotFoundException("Could not find dotnet-matrix.slnx.");
    }

    public string SolutionDirectory { get; }
}
