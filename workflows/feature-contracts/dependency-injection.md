# Dependency Injection feature support contract

A library is `Supported` only when the executable validation passes using its public or intended extension APIs. A similarly named API is insufficient when its observable behavior differs from this contract.

- `Supported`: the implementation exists and passes every condition below.
- `Unsupported`: the library has no intended mechanism with the required semantics.
- `NotApplicable`: the scenario has no meaningful equivalent for the library model. Difficulty or poor performance is not a reason to use this status.
- `Failed`: an implementation is claimed, but build, validation, or runtime execution fails.

## 1. Singleton

Register three singleton services and resolve each service repeatedly. Every resolve of the same service must return the same object reference. Different service contracts must resolve to their corresponding implementation types.

## 2. Transient

Register three transient services and resolve each service repeatedly. Every resolve must create a new object reference. No transient instance may be reused between top-level resolutions.

## 3. PerResolve

Resolve an object graph that requests the same dependency more than once. Both requests within one top-level resolution must receive the same object reference, while a second top-level resolution must receive a different reference. The behavior must come from the library's native or officially supported per-object-graph lifetime; a matrix-owned caching factory does not qualify.

## 4. Scoped

Create an explicit scope and resolve scoped services repeatedly. Resolves inside one scope must return the same reference. Resolves in different scopes must return different references. Scope-owned disposable instances must be disposed when the scope ends.

## 5. Combined

Resolve three roots that combine singleton and transient dependencies. The singleton dependency must be shared across roots and repeated resolutions. Every transient dependency must be different across roots and top-level resolutions.

## 6. Complex

Register and resolve three multi-level object graphs containing all prescribed services, subobjects, and nested dependencies. Every root must be constructed by the library, and every dependency must have the expected implementation type and lifetime.

## 7. Property

Resolve three roots with three writable service properties. The container or its intended property-injection extension must assign all properties during activation. Manual property assignment in the benchmark or a matrix-owned factory does not qualify.

## 8. Generics

Register one open generic service mapping and an open generic root. Resolve closed roots for `int`, `float`, and `object`. Each root must receive the correctly closed service implementation. Registering every closed type separately does not satisfy the open-generic contract.

## 9. IEnumerable

Register five distinct transient implementations of `IPlugin` and inject `IEnumerable<IPlugin>` into three roots.

The collection must be genuinely lazy:

1. Resolving all three roots must create zero plugin instances.
2. Enumerating the first root must create exactly five plugins, one of every registered implementation type.
3. Enumerating the second and third roots must independently create five new plugins each.
4. No transient plugin instance may be shared between roots.
5. Enumerating the first root a second time must create five new plugin instances; a cached or materialized collection does not qualify.

Libraries that materialize an array, list, or cached collection before consumer activation are `Unsupported` for this feature even if they can inject `IEnumerable<T>`.

Not rated: too few rated libraries implement genuine lazy enumeration for this
to be a competitive result, and the scenario was measured and drawn in no
chart group before it briefly entered one by accident. The current support
count is not repeated here — it is computed from the report and shown live in
the feature matrix and `README.md`, so this page cannot fall out of sync with
it the way `Rating: feature-only` once did. It is still benchmarked and
validated, and its own chart still shows what supports it; see
workflows/rating.md, "No per-scenario exclusion by threshold or editorial
judgment".

## 10. Array

Register five distinct transient implementations of `IPlugin` and resolve three roots that materialize their injected sequence to an array during activation. Every root must contain exactly five plugins, one of every registered implementation type, and transient instances must be created for each root. Direct `T[]` injection is not required; this scenario measures the materialized collection path observed by the consumer.

## 11. Conditional

Register three implementations of one contract and three consumer roots. Select the implementation using the library's metadata, key, predicate, or consumer-context mechanism. Each root must receive its prescribed implementation. A matrix-owned switch or manual root factory does not qualify unless keyed resolution is the library's intended conditional mechanism.

## 12. Child Container

Create a real nested child container that inherits parent registrations and can add or override a registration without changing the parent. Validate both inheritance and isolation. A lifetime scope that cannot add or override registrations is `Unsupported`.

## 13. Interception With Proxy

Resolve `ICalculator` through the library's interception or activation extension point. The returned object must be a proxy rather than the concrete calculator, the interceptor must proceed to the target, and `Add(5, 10)` must return `15`. Constructing the proxy directly inside the measured benchmark method does not qualify for a runtime container.

## 14. Prepare And Register

During the measured operation, create the container and register the prescribed singleton, transient, service, subobject, and complex-root graph without resolving it. Dispose the container when applicable. For a compile-time DI library, configuration and code generation are excluded, but construction of the generated composition is measured. A hand-coded approach with no container or composition remains represented by an explicit reported result of zero time and zero memory.

## 15. Prepare And Register And Simple Resolve

During the measured operation, perform the same setup as `Prepare And Register`, then resolve one singleton service exactly once and dispose the container when applicable. For a compile-time DI library, configuration and code generation are excluded; construct the generated composition and resolve one singleton root. A hand-coded approach measures its direct object construction without container setup.
