namespace Matrix;

public interface IRunnerOptionsParser
{
    RunnerOptions Parse(string[] args, string defaultOutput);
}