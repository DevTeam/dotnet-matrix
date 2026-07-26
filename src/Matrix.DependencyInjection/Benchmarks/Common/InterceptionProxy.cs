using System.Linq.Expressions;
using Castle.DynamicProxy;
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Matrix.DependencyInjection.Benchmarks;

internal static class InterceptionProxy
{
    private static readonly IInterceptor[] Interceptors = [new PassThroughInterceptor()];
    private static readonly DefaultProxyBuilder ProxyBuilder = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Create<T>(T target) => ProxyFactory<T>.Factory(target);

    // Containers that only accept a concrete decorator type need the generated proxy type.
    public static Type TypeOf<T>() => ProxyFactory<T>.ProxyType;

    private sealed class PassThroughInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation) => invocation.Proceed();
    }

    private static class ProxyFactory<T>
    {
        public static readonly Type ProxyType = ProxyBuilder.CreateInterfaceProxyTypeWithTargetInterface(
            typeof(T),
            Type.EmptyTypes,
            ProxyGenerationOptions.Default);

        public static readonly Func<T, T> Factory = CreateFactory();

        private static Func<T, T> CreateFactory()
        {
            var proxyType = ProxyType;
            var constructor = proxyType.GetConstructors().Single(i => i.GetParameters().Length == 2);
            var value = Expression.Parameter(typeof(T));
            var interceptors = Expression.Constant(Interceptors);
            var proxy = Expression.New(constructor, interceptors, value);
            return Expression.Lambda<Func<T, T>>(proxy, value).Compile();
        }
    }
}
