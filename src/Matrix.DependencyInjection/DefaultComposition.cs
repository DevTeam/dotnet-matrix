// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedType.Global
// ReSharper disable ArrangeTypeMemberModifiers
namespace Matrix.DependencyInjection;

internal class DefaultComposition
{
    static void Setup() =>
        DI.Setup("Default", CompositionKind.Global)
            .Hint(Hint.ThreadSafe, "Off")
            .Hint(Hint.Resolve, "Off")
            .Hint(Hint.ToString, "Off");
}