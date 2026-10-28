namespace Matrix.TemplateEngines;

/// <summary>
/// The model a Razor engine reads. Razor resolves C# member names, so its model
/// is a real type rather than the anonymous object the mustache and Liquid
/// engines take.
/// </summary>
public sealed class RazorModel
{
    public string Name { get; init; } = "World";
}

internal static class TemplateData
{
    public const string GreetingExpected = "Hello, World!";

    public const string RawUnsafe = "<b>A & B</b>";

    public const string EscapedUnsafe = "&lt;b&gt;A &amp; B&lt;/b&gt;";

    public static readonly RazorModel RazorGreeting = new();

    public static readonly RazorModel RazorUnsafe = new() { Name = RawUnsafe };
}
