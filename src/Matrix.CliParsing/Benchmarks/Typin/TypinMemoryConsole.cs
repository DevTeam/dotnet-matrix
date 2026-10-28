using System.Text;
using Typin.Console;
using Typin.Console.IO;

namespace Matrix.CliParsing.Benchmarks;

/// <summary>
/// Typin requires an <see cref="IConsole"/> implementation, so a diagnostic and the help text
/// are read from in-memory streams. The wrapped streams are created once with the scenario
/// object; only the write of a diagnostic happens inside the measured call.
/// </summary>
internal sealed class TypinMemoryConsole : IConsole
{
    private readonly MemoryStream _output = new();
    private readonly MemoryStream _error = new();

    public TypinMemoryConsole()
    {
        Output = new StandardStreamWriter(_output, true, this);
        Error = new StandardStreamWriter(_error, true, this);
        Input = new StandardStreamReader(new MemoryStream(), Encoding.UTF8, true, this);
    }

    public StandardStreamWriter Output { get; }

    public StandardStreamWriter Error { get; }

    public StandardStreamReader Input { get; }

    public ConsoleColor ForegroundColor { get; set; }

    public ConsoleColor BackgroundColor { get; set; }

    public int CursorLeft { get; set; }

    public int CursorTop { get; set; }

    public int WindowWidth { get; set; } = 120;

    public int WindowHeight { get; set; } = 40;

    public int BufferWidth { get; set; } = 120;

    public int BufferHeight { get; set; } = 40;

    public string ReadOutput()
    {
        Output.Flush();
        return Encoding.UTF8.GetString(_output.ToArray());
    }

    public string ReadError()
    {
        Error.Flush();
        return Encoding.UTF8.GetString(_error.ToArray());
    }

    public void ResetBuffers()
    {
        _output.SetLength(0);
        _error.SetLength(0);
    }

    public CancellationToken GetCancellationToken() => CancellationToken.None;

    public void SetCursorPosition(int left, int top)
    {
    }

    public ConsoleKeyInfo ReadKey(bool intercept) => throw new NotSupportedException();

    public Task<ConsoleKeyInfo> ReadKeyAsync(bool intercept) => throw new NotSupportedException();

    public void Clear()
    {
    }

    public void ResetColor()
    {
    }
}
