namespace Build.Targets;

internal interface ITemplateEngine
{
    Task RenderAsync<TModel>(
        string templateName,
        TModel model,
        Stream stream,
        CancellationToken cancellationToken);
}
