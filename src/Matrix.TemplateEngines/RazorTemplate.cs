namespace Matrix.TemplateEngines;

/// <summary>
/// The base type RazorEngineCore generates a template against. The generated
/// class derives from it and reads <see cref="Model"/> from the template body.
/// </summary>
public abstract class RazorTemplate : global::RazorEngineCore.RazorEngineTemplateBase<RazorModel>
{
}
