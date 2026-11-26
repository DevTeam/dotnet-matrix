namespace Matrix.TemplateEngines;

/// <summary>
/// The base type RazorEngineCore generates a template against. The generated
/// class derives from it and reads <c>Model</c> from the template body.
/// </summary>
public abstract class RazorTemplate : RazorEngineCore.RazorEngineTemplateBase<RazorModel>;
