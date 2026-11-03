using SpanJson.Resolvers;

namespace Matrix.JsonSerialization;

/// <summary>
/// SpanJson resolver that keeps the default original-case, null-including behavior and adds the
/// identifier formatter. A SpanJson serializer always works through a resolver type, so this is
/// the library's own type-specific configuration hook, not a wrapper around it: the formatter is
/// resolved and cached by SpanJson itself.
/// </summary>
/// <typeparam name="TSymbol">UTF-16 (<see cref="char"/>) or UTF-8 (<see cref="byte"/>) symbol.</typeparam>
internal sealed class SpanJsonResolver<TSymbol> : ResolverBase<TSymbol, SpanJsonResolver<TSymbol>>
    where TSymbol : struct
{
    public SpanJsonResolver()
        : base(new SpanJsonOptions())
    {
        RegisterGlobalCustomFormatter<Models.Identifier, SpanJsonIdentifierFormatter>();
    }
}
