# .NET Matrix

> [**Open the interactive .NET Matrix →**](https://matrix.dev-team.org/)

Evidence-based feature and performance comparisons for .NET libraries.


## Dependency Injection

### Rating

A gold, silver and bronze star for the first three places of every benchmark overview.

| # | Library | 🥇 | 🥈 | 🥉 | Won |
|---|---|---|---|---|---|
| 1 | Pure.DI | 3 |  |  | gold in Advanced, gold in Basic, gold in Prepare |
| 2 | Grace |  | 2 |  | silver in Advanced, silver in Basic |
| 3 | MvvmCross |  | 1 |  | silver in Prepare |
| 4 | DryIoc |  |  | 1 | bronze in Prepare |
| 5 | Singularity |  |  | 1 | bronze in Basic |
| 6 | Stashbox |  |  | 1 | bronze in Advanced |

### Benchmark overview

Performance and allocated memory are shown together. Lower values are better.

![Dependency Injection Basic benchmark overview](reports/DependencyInjection/charts/overview-basic.png)

![Dependency Injection Advanced benchmark overview](reports/DependencyInjection/charts/overview-advanced.png)

![Dependency Injection Prepare benchmark overview](reports/DependencyInjection/charts/overview-prepare.png)

### Libraries

<table>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/autofac.png" width="48" height="48" alt="Autofac logo"></td>
<td><strong><a href="https://autofac.readthedocs.io/en/latest/">Autofac</a></strong> 9.3.1<br>A flexible inversion of control container for building extensible .NET applications.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/windsor.png" width="48" height="48" alt="Castle Windsor logo"></td>
<td><strong><a href="https://github.com/castleproject/Windsor/blob/master/docs/README.md">Castle Windsor</a></strong> 6.0.0<br>The Castle Project container, with bound lifestyles and Castle DynamicProxy interception.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/catel.png" width="48" height="48" alt="Catel logo"></td>
<td><strong><a href="https://www.catelproject.com/">Catel</a></strong> 6.2.0<br>An MVVM application framework whose service locator and type factory perform constructor injection.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/dry-ioc.png" width="48" height="48" alt="DryIoc logo"></td>
<td><strong><a href="https://github.com/dadhi/DryIoc/blob/master/docs/DryIoc.Docs/README.md">DryIoc</a></strong> 5.4.3<br>A fast, small and feature-rich container with expression-compiled resolution and rich reuse options.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/faster-ioc.svg" width="48" height="48" alt="Faster.Ioc logo"></td>
<td><strong><a href="https://github.com/Wsm2110/Faster.Ioc#readme">Faster.Ioc</a></strong> 5.0.0<br>A minimalistic container focused on the shortest possible resolve path.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/grace.png" width="48" height="48" alt="Grace logo"></td>
<td><strong><a href="https://github.com/ipjohnson/Grace/wiki">Grace</a></strong> 7.2.1<br>A container with a fluent registration model, per-object-graph lifestyles and decorator support.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/hand-coded.svg" width="48" height="48" alt="Hand-coded logo"></td>
<td><strong>Hand-coded</strong><br>Direct dependency injection written in C# without a container.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/lamar.png" width="48" height="48" alt="Lamar logo"></td>
<td><strong><a href="https://jasperfx.github.io/lamar/">Lamar</a></strong> 16.0.0<br>The successor to StructureMap, built around runtime code generation.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/light-inject.svg" width="48" height="48" alt="LightInject logo"></td>
<td><strong><a href="https://www.lightinject.net/">LightInject</a></strong> 7.1.0<br>A lightweight container with an ultra-small API surface and its own interception package.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/maestro.svg" width="48" height="48" alt="Maestro logo"></td>
<td><strong><a href="https://github.com/JonasSamuelsson/Maestro#readme">Maestro</a></strong> 3.6.6<br>A small container with a fluent configuration API and pluggable activation interceptors.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/mef2.svg" width="48" height="48" alt="Managed Extensibility Framework 2 logo"></td>
<td><strong><a href="https://learn.microsoft.com/dotnet/framework/mef/">Managed Extensibility Framework 2</a></strong> 10.0.10<br>The lightweight Managed Extensibility Framework composition engine shipped as System.Composition.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/microsoft-di.png" width="48" height="48" alt="Microsoft Extensions Dependency Injection logo"></td>
<td><strong><a href="https://learn.microsoft.com/dotnet/core/extensions/dependency-injection">Microsoft Extensions Dependency Injection</a></strong> 10.0.10<br>The built-in .NET dependency injection container and its service collection abstractions.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/mvvm-cross.png" width="48" height="48" alt="MvvmCross logo"></td>
<td><strong><a href="https://www.mvvmcross.com/documentation/">MvvmCross</a></strong> 10.1.2<br>A cross-platform MVVM framework with its own inversion of control provider.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/ninject.png" width="48" height="48" alt="Ninject logo"></td>
<td><strong><a href="https://github.com/ninject/Ninject/wiki">Ninject</a></strong> 3.3.6<br>A container built around fluent bindings, contextual conditions and pluggable extensions.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/pure-di.png" width="48" height="48" alt="Pure.DI logo"></td>
<td><strong><a href="https://github.com/DevTeam/Pure.DI#readme">Pure.DI</a></strong> 2.5.2<br>A compile-time dependency injection framework that generates strongly typed compositions.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/simple-injector.svg" width="48" height="48" alt="Simple Injector logo"></td>
<td><strong><a href="https://docs.simpleinjector.org/">Simple Injector</a></strong> 5.6.0<br>A fast, opinionated dependency injection library that promotes explicit configuration and maintainable application design.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/singularity.png" width="48" height="48" alt="Singularity logo"></td>
<td><strong><a href="https://github.com/Barsonax/Singularity#readme">Singularity</a></strong> 0.18.0<br>An expression-tree based container that validates the whole object graph when the container is built.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/spring.png" width="48" height="48" alt="Spring.NET logo"></td>
<td><strong><a href="https://www.springframework.net/">Spring.NET</a></strong> 3.0.3<br>The Spring.NET application framework and its XML or code configured object factory.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/stashbox.png" width="48" height="48" alt="Stashbox logo"></td>
<td><strong><a href="https://z4kn4fein.github.io/stashbox/">Stashbox</a></strong> 5.20.0<br>A fast container with per-request lifetimes, conditional registrations and child containers.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/structure-map.svg" width="48" height="48" alt="StructureMap logo"></td>
<td><strong><a href="https://structuremap.github.io/">StructureMap</a></strong> 4.7.1<br>A mature container with a fluent registry DSL, nested containers and decorators.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/unity.png" width="48" height="48" alt="Unity logo"></td>
<td><strong><a href="https://unitycontainer.org/">Unity</a></strong> 5.11.10<br>The Unity container, offering per-resolve lifetimes and hierarchical child containers.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/vs-mef.png" width="48" height="48" alt="Visual Studio MEF logo"></td>
<td><strong><a href="https://github.com/microsoft/vs-mef/blob/main/doc/index.md">Visual Studio MEF</a></strong> 17.13.41<br>The Visual Studio composition engine, a fast attribute-driven MEF implementation.</td>
</tr>
<tr>
<td width="64"><img src="metadata/DependencyInjection/logos/zen-ioc.png" width="48" height="48" alt="ZenIoc logo"></td>
<td><strong><a href="https://github.com/zenmvvm/ZenIoc#readme">ZenIoc</a></strong> 1.0.1<br>A tiny container with compiled registrations, named resolution and nested containers.</td>
</tr>
</table>

### Benchmark scenarios

<details>
<summary><strong>01 · Singleton</strong></summary>

Registers three singleton services and resolves each of them repeatedly. Every resolve of the same service must return the same instance.

![Dependency Injection Singleton benchmark](reports/DependencyInjection/charts/01-singleton.png)

</details>
<details>
<summary><strong>02 · Transient</strong></summary>

Registers three transient services and resolves each of them repeatedly. Every resolve must create a new instance, never reusing an earlier one.

![Dependency Injection Transient benchmark](reports/DependencyInjection/charts/02-transient.png)

</details>
<details>
<summary><strong>03 · PerResolve</strong></summary>

Resolves an object graph that asks for the same dependency twice. Both requests inside one resolution share an instance, while the next resolution gets a new one.

![Dependency Injection PerResolve benchmark](reports/DependencyInjection/charts/03-perresolve.png)

</details>
<details>
<summary><strong>04 · Scoped</strong></summary>

Resolves scoped services inside explicit scopes. One instance per scope, different instances across scopes, and scope-owned disposables are disposed when the scope ends.

![Dependency Injection Scoped benchmark](reports/DependencyInjection/charts/04-scoped.png)

</details>
<details>
<summary><strong>05 · Combined</strong></summary>

Resolves three roots that mix singleton and transient dependencies. The singleton is shared across every root while each transient dependency is distinct.

![Dependency Injection Combined benchmark](reports/DependencyInjection/charts/05-combined.png)

</details>
<details>
<summary><strong>06 · Complex</strong></summary>

Registers and resolves three multi-level object graphs, checking that every nested dependency has the expected implementation type and lifetime.

![Dependency Injection Complex benchmark](reports/DependencyInjection/charts/06-complex.png)

</details>
<details>
<summary><strong>07 · Property</strong></summary>

Resolves three roots that carry writable service properties. The container, or its intended property-injection extension, must assign them during activation.

![Dependency Injection Property benchmark](reports/DependencyInjection/charts/07-property.png)

</details>
<details>
<summary><strong>08 · Generics</strong></summary>

Registers one open generic service mapping and resolves roots closed over int, float and object. Registering every closed type separately does not count.

![Dependency Injection Generics benchmark](reports/DependencyInjection/charts/08-generics.png)

</details>
<details>
<summary><strong>09 · IEnumerable</strong></summary>

Injects a sequence of five plugin implementations and requires it to be genuinely lazy: nothing is created until enumeration, and every enumeration yields new transients.

![Dependency Injection IEnumerable benchmark](reports/DependencyInjection/charts/09-ienumerable.png)

</details>
<details>
<summary><strong>10 · Array</strong></summary>

Resolves three roots that materialise their injected sequence of five plugins into an array while the root is being activated.

![Dependency Injection Array benchmark](reports/DependencyInjection/charts/10-array.png)

</details>
<details>
<summary><strong>11 · Conditional</strong></summary>

Gives each of three consumers a different implementation of one contract, chosen through the metadata, key, predicate or consumer-context mechanism of the library.

![Dependency Injection Conditional benchmark](reports/DependencyInjection/charts/11-conditional.png)

</details>
<details>
<summary><strong>12 · Child Container</strong></summary>

Creates a real nested child container that inherits the registrations of its parent and can add or override them without changing the parent.

![Dependency Injection Child Container benchmark](reports/DependencyInjection/charts/12-child-container.png)

</details>
<details>
<summary><strong>13 · Interception With Proxy</strong></summary>

Resolves a service through the interception or activation extension point of the library. The result must be a proxy whose interceptor proceeds to the real target.

![Dependency Injection Interception With Proxy benchmark](reports/DependencyInjection/charts/13-interception-with-proxy.png)

</details>
<details>
<summary><strong>14 · Prepare And Register</strong></summary>

Measures creating the container and registering the whole prescribed graph, without resolving anything from it.

![Dependency Injection Prepare And Register benchmark](reports/DependencyInjection/charts/14-prepare-and-register.png)

</details>
<details>
<summary><strong>15 · Prepare And Register And Simple Resolve</strong></summary>

Measures the same setup as Prepare And Register, followed by a single resolve of one singleton root.

![Dependency Injection Prepare And Register And Simple Resolve benchmark](reports/DependencyInjection/charts/15-prepare-and-register-and-simple-resolve.png)

</details>

## Object Mapping

### Rating

A gold, silver and bronze star for the first three places of every benchmark overview.

| # | Library | 🥇 | 🥈 | 🥉 | Won |
|---|---|---|---|---|---|
| 1 | Mapperly | 3 |  |  | gold in Advanced, gold in Basic, gold in Prepare |
| 2 | Mapster |  | 2 | 1 | silver in Advanced, silver in Basic, bronze in Prepare |
| 3 | AutoMapper |  | 1 | 2 | silver in Prepare, bronze in Advanced, bronze in Basic |

### Benchmark overview

Performance and allocated memory are shown together. Lower values are better.

![Object Mapping Basic benchmark overview](reports/ObjectMapping/charts/overview-basic.png)

![Object Mapping Advanced benchmark overview](reports/ObjectMapping/charts/overview-advanced.png)

![Object Mapping Prepare benchmark overview](reports/ObjectMapping/charts/overview-prepare.png)

### Libraries

<table>
<tr>
<td width="64"><img src="metadata/ObjectMapping/logos/auto-mapper.svg" width="48" height="48" alt="AutoMapper logo"></td>
<td><strong><a href="https://docs.automapper.io/">AutoMapper</a></strong> 16.2.0<br>A convention-based object-object mapper with runtime configuration and compiled mapping plans.</td>
</tr>
<tr>
<td width="64"><img src="metadata/ObjectMapping/logos/hand-coded.svg" width="48" height="48" alt="Hand-coded logo"></td>
<td><strong>Hand-coded</strong><br>Direct object mapping written in C# without a mapping library.</td>
</tr>
<tr>
<td width="64"><img src="metadata/ObjectMapping/logos/mapperly.svg" width="48" height="48" alt="Mapperly logo"></td>
<td><strong><a href="https://mapperly.riok.app/">Mapperly</a></strong> 4.3.1<br>A source generator that creates readable object mapping code at compile time.</td>
</tr>
<tr>
<td width="64"><img src="metadata/ObjectMapping/logos/mapster.svg" width="48" height="48" alt="Mapster logo"></td>
<td><strong><a href="https://github.com/MapsterMapper/Mapster/wiki">Mapster</a></strong> 10.0.11<br>An object mapper with runtime configuration, expression compilation, and projection support.</td>
</tr>
</table>

### Benchmark scenarios

<details>
<summary><strong>01 · Simple Object</strong></summary>

Maps one object with scalar values to a newly allocated destination object.

![Object Mapping Simple Object benchmark](reports/ObjectMapping/charts/01-simple-object.png)

</details>
<details>
<summary><strong>02 · Nested Object</strong></summary>

Maps an order with nested customer and address objects to a new destination graph.

![Object Mapping Nested Object benchmark](reports/ObjectMapping/charts/02-nested-object.png)

</details>
<details>
<summary><strong>03 · Collection</strong></summary>

Maps an array of 100 objects while preserving count, order and member values.

![Object Mapping Collection benchmark](reports/ObjectMapping/charts/03-collection.png)

</details>
<details>
<summary><strong>04 · Flattening</strong></summary>

Maps nested customer values into a flat order summary through member-path configuration.

![Object Mapping Flattening benchmark](reports/ObjectMapping/charts/04-flattening.png)

</details>
<details>
<summary><strong>05 · Map To Existing</strong></summary>

Overwrites a supplied destination object and returns that same instance.

![Object Mapping Map To Existing benchmark](reports/ObjectMapping/charts/05-map-to-existing.png)

</details>
<details>
<summary><strong>06 · Null Handling</strong></summary>

Preserves null text, nested object and collection members in the destination.

![Object Mapping Null Handling benchmark](reports/ObjectMapping/charts/06-null-handling.png)

</details>
<details>
<summary><strong>07 · Custom Conversion</strong></summary>

Maps string values through registered code and invariant decimal conversions.

![Object Mapping Custom Conversion benchmark](reports/ObjectMapping/charts/07-custom-conversion.png)

</details>
<details>
<summary><strong>08 · Polymorphic Mapping</strong></summary>

Maps a base array containing cats and dogs to matching destination runtime types.

![Object Mapping Polymorphic Mapping benchmark](reports/ObjectMapping/charts/08-polymorphic-mapping.png)

</details>
<details>
<summary><strong>09 · Prepare Configuration</strong></summary>

Creates the complete mapper configuration and eagerly prepares its runtime mapping plans.

![Object Mapping Prepare Configuration benchmark](reports/ObjectMapping/charts/09-prepare-configuration.png)

</details>
<details>
<summary><strong>10 · Prepare And Simple Map</strong></summary>

Creates the complete mapper configuration and maps one simple object.

![Object Mapping Prepare And Simple Map benchmark](reports/ObjectMapping/charts/10-prepare-and-simple-map.png)

</details>
