namespace Matrix.DependencyInjection.Infrastructure;

public enum FeatureId
{
    Singleton,
    Transient,
    PerResolve,
    Scoped,
    Combined,
    Complex,
    Property,
    Generics,
    Enumerable,
    Array,
    Conditional,
    ChildContainer,
    InterceptionWithProxy,
    PrepareAndRegister,
    PrepareAndRegisterAndSimpleResolve
}

public enum FeatureStatus
{
    Supported,
    Unsupported,
    NotApplicable,
    Failed
}
