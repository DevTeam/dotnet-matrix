namespace Matrix;

public sealed class MatrixApplication(
    string[] args,
    IRunnerOptionsParser optionsParser,
    IMatrixLibraryCatalog libraryCatalog,
    IMatrixRunner runner)
{
    public int Run()
    {
        try
        {
            var options = optionsParser.Parse(args, runner.DefaultOutputFile);
            var libraries = libraryCatalog.Filter(options.Libraries);
            return runner.Run(libraries, options);
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }
}
