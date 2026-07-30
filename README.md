# .NET Matrix

> [**Open the interactive .NET Matrix →**](https://matrix.dev-team.org/)

Evidence-based feature and performance comparisons for .NET libraries.


## CSV Processing

### Rating

A gold, silver and bronze star for the first three places of every benchmark overview.

| # | Library | 🥇 | 🥈 | 🥉 | Won |
|---|---|---|---|---|---|
| 1 | Sep | 4 |  |  | gold in Correctness, gold in Read, gold in Throughput, gold in Write |
| 2 | Sylvan.Data.Csv |  | 4 |  | silver in Correctness, silver in Read, silver in Throughput, silver in Write |
| 3 | CsvHelper |  |  | 4 | bronze in Correctness, bronze in Read, bronze in Throughput, bronze in Write |

### Benchmark overview

Performance and allocated memory are shown together. Lower values are better.

![CSV Processing Read benchmark overview](reports/CsvProcessing/charts/overview-read.png)

![CSV Processing Correctness benchmark overview](reports/CsvProcessing/charts/overview-correctness.png)

![CSV Processing Throughput benchmark overview](reports/CsvProcessing/charts/overview-throughput.png)

![CSV Processing Write benchmark overview](reports/CsvProcessing/charts/overview-write.png)

### Libraries

<table>
<tr>
<td width="64"><img src="metadata/CsvProcessing/logos/csv-helper.svg" width="48" height="48" alt="CsvHelper logo"></td>
<td><strong><a href="https://joshclose.github.io/CsvHelper/">CsvHelper</a></strong> 33.1.0<br>A widely used CSV library with record mapping, type conversion, and synchronous and asynchronous readers and writers.</td>
</tr>
<tr>
<td width="64"><img src="metadata/CsvProcessing/logos/sep.svg" width="48" height="48" alt="Sep logo"></td>
<td><strong><a href="https://github.com/nietras/Sep">Sep</a></strong> 0.15.1<br>A modern SIMD-accelerated separated-values reader and writer with span-based conversion and async enumeration.</td>
</tr>
<tr>
<td width="64"><img src="metadata/CsvProcessing/logos/sylvan.svg" width="48" height="48" alt="Sylvan.Data.Csv logo"></td>
<td><strong><a href="https://github.com/MarkPflug/Sylvan/blob/main/docs/Csv.md">Sylvan.Data.Csv</a></strong> 1.4.4<br>A high-performance forward-only CSV data reader and writer with strongly typed accessors and asynchronous I/O.</td>
</tr>
</table>

### Benchmark scenarios

<details>
<summary><strong>01 · Read Simple Rows</strong></summary>

Parses three CSV records and materializes every field as text.

![CSV Processing Read Simple Rows benchmark](reports/CsvProcessing/charts/01-read-simple-rows.png)

</details>
<details>
<summary><strong>02 · Read Typed Records</strong></summary>

Parses three CSV records and materializes typed scalar values.

![CSV Processing Read Typed Records benchmark](reports/CsvProcessing/charts/02-read-typed-records.png)

</details>
<details>
<summary><strong>03 · Read Large Dataset</strong></summary>

Parses and materializes 10,000 typed CSV records.

![CSV Processing Read Large Dataset benchmark](reports/CsvProcessing/charts/03-read-large-dataset.png)

</details>
<details>
<summary><strong>04 · Quoted Fields</strong></summary>

Parses doubled quote escapes inside quoted CSV fields.

![CSV Processing Quoted Fields benchmark](reports/CsvProcessing/charts/04-quoted-fields.png)

</details>
<details>
<summary><strong>05 · Escaped Delimiters</strong></summary>

Parses quoted fields containing a comma or an LF newline.

![CSV Processing Escaped Delimiters benchmark](reports/CsvProcessing/charts/05-escaped-delimiters.png)

</details>
<details>
<summary><strong>06 · Header Mapping</strong></summary>

Maps a reordered CSV header to the correct typed record members.

![CSV Processing Header Mapping benchmark](reports/CsvProcessing/charts/06-header-mapping.png)

</details>
<details>
<summary><strong>07 · Custom Conversion</strong></summary>

Converts sku-NNNN fields to the matrix-owned ProductCode value type.

![CSV Processing Custom Conversion benchmark](reports/CsvProcessing/charts/07-custom-conversion.png)

</details>
<details>
<summary><strong>08 · Streaming Read</strong></summary>

Aggregates 10,000 typed rows with forward-only reading and no row materialization.

![CSV Processing Streaming Read benchmark](reports/CsvProcessing/charts/08-streaming-read.png)

</details>
<details>
<summary><strong>09 · Write Rows</strong></summary>

Writes three records with a header to an exact LF-terminated CSV string.

![CSV Processing Write Rows benchmark](reports/CsvProcessing/charts/09-write-rows.png)

</details>
<details>
<summary><strong>10 · Async Read</strong></summary>

Asynchronously aggregates 10,000 typed CSV rows through the library async API.

![CSV Processing Async Read benchmark](reports/CsvProcessing/charts/10-async-read.png)

</details>

## Dependency Injection

### Rating

A gold, silver and bronze star for the first three places of every benchmark overview.

| # | Library | 🥇 | 🥈 | 🥉 | Won |
|---|---|---|---|---|---|
| 1 | Pure.DI | 3 |  |  | gold in Advanced, gold in Basic, gold in Prepare |
| 2 | Grace |  | 1 | 1 | silver in Advanced, bronze in Basic |
| 3 | MvvmCross |  | 1 |  | silver in Prepare |
| 4 | Simple Injector |  | 1 |  | silver in Basic |
| 5 | DryIoc |  |  | 1 | bronze in Prepare |
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

## JSON Serialization

### Rating

A gold, silver and bronze star for the first three places of every benchmark overview.

| # | Library | 🥇 | 🥈 | 🥉 | Won |
|---|---|---|---|---|---|
| 1 | ServiceStack.Text | 5 |  |  | gold in Basic, gold in Collections, gold in Nested, gold in Prepare, gold in Stream |
| 2 | Newtonsoft.Json | 1 | 5 |  | gold in Advanced, silver in Basic, silver in Collections, silver in Nested, silver in Prepare, silver in Stream |

### Benchmark overview

Performance and allocated memory are shown together. Lower values are better.

![JSON Serialization Basic benchmark overview](reports/JsonSerialization/charts/overview-basic.png)

![JSON Serialization Nested benchmark overview](reports/JsonSerialization/charts/overview-nested.png)

![JSON Serialization Collections benchmark overview](reports/JsonSerialization/charts/overview-collections.png)

![JSON Serialization Advanced benchmark overview](reports/JsonSerialization/charts/overview-advanced.png)

![JSON Serialization Stream benchmark overview](reports/JsonSerialization/charts/overview-stream.png)

![JSON Serialization Prepare benchmark overview](reports/JsonSerialization/charts/overview-prepare.png)

### Libraries

<table>
<tr>
<td width="64"><img src="metadata/JsonSerialization/logos/newtonsoft-json.svg" width="48" height="48" alt="Newtonsoft.Json logo"></td>
<td><strong><a href="https://www.newtonsoft.com/json/help/html/Introduction.htm">Newtonsoft.Json</a></strong> 13.0.4<br>A mature JSON framework for .NET with configurable contracts, converters, and streaming readers and writers.</td>
</tr>
<tr>
<td width="64"><img src="metadata/JsonSerialization/logos/service-stack-text.svg" width="48" height="48" alt="ServiceStack.Text logo"></td>
<td><strong><a href="https://docs.servicestack.net/text">ServiceStack.Text</a></strong> 10.0.8<br>A high-performance text library with typed JSON, stream, collection, and type-specific serialization APIs.</td>
</tr>
<tr>
<td width="64"><img src="metadata/JsonSerialization/logos/system-text-json.svg" width="48" height="48" alt="System.Text.Json logo"></td>
<td><strong><a href="https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/overview">System.Text.Json</a></strong><br>The reflection-based and source-generated JSON serializer included with .NET.</td>
</tr>
</table>

### Benchmark scenarios

<details>
<summary><strong>01 · Serialize Simple Object</strong></summary>

Serializes one scalar object to a compact JSON string.

![JSON Serialization Serialize Simple Object benchmark](reports/JsonSerialization/charts/01-serialize-simple-object.png)

</details>
<details>
<summary><strong>02 · Deserialize Simple Object</strong></summary>

Deserializes one compact JSON object and validates every scalar member.

![JSON Serialization Deserialize Simple Object benchmark](reports/JsonSerialization/charts/02-deserialize-simple-object.png)

</details>
<details>
<summary><strong>03 · Serialize Nested Object</strong></summary>

Serializes an order, customer, and address object graph.

![JSON Serialization Serialize Nested Object benchmark](reports/JsonSerialization/charts/03-serialize-nested-object.png)

</details>
<details>
<summary><strong>04 · Deserialize Nested Object</strong></summary>

Deserializes and materializes an order, customer, and address object graph.

![JSON Serialization Deserialize Nested Object benchmark](reports/JsonSerialization/charts/04-deserialize-nested-object.png)

</details>
<details>
<summary><strong>05 · Serialize Collection</strong></summary>

Serializes three ordered objects to a compact JSON array.

![JSON Serialization Serialize Collection benchmark](reports/JsonSerialization/charts/05-serialize-collection.png)

</details>
<details>
<summary><strong>06 · Deserialize Collection</strong></summary>

Deserializes a compact JSON array to three ordered objects.

![JSON Serialization Deserialize Collection benchmark](reports/JsonSerialization/charts/06-deserialize-collection.png)

</details>
<details>
<summary><strong>07 · Serialize Dictionary</strong></summary>

Serializes three ordered string and integer entries to a JSON object.

![JSON Serialization Serialize Dictionary benchmark](reports/JsonSerialization/charts/07-serialize-dictionary.png)

</details>
<details>
<summary><strong>08 · Deserialize Dictionary</strong></summary>

Deserializes a JSON object to three ordinal string and integer entries.

![JSON Serialization Deserialize Dictionary benchmark](reports/JsonSerialization/charts/08-deserialize-dictionary.png)

</details>
<details>
<summary><strong>09 · Enum Round Trip</strong></summary>

Serializes an enum as its string name and deserializes it back.

![JSON Serialization Enum Round Trip benchmark](reports/JsonSerialization/charts/09-enum-round-trip.png)

</details>
<details>
<summary><strong>10 · Custom Converter Round Trip</strong></summary>

Serializes a strongly typed identifier as a JSON string and deserializes it back.

![JSON Serialization Custom Converter Round Trip benchmark](reports/JsonSerialization/charts/10-custom-converter-round-trip.png)

</details>
<details>
<summary><strong>11 · Polymorphic Round Trip</strong></summary>

Round-trips a base-type collection through safe cat and dog discriminators.

![JSON Serialization Polymorphic Round Trip benchmark](reports/JsonSerialization/charts/11-polymorphic-round-trip.png)

</details>
<details>
<summary><strong>12 · UTF-8 Stream Round Trip</strong></summary>

Serializes a simple object to a new UTF-8 memory stream and deserializes it back.

![JSON Serialization UTF-8 Stream Round Trip benchmark](reports/JsonSerialization/charts/12-utf-8-stream-round-trip.png)

</details>
<details>
<summary><strong>13 · Source Generation Round Trip</strong></summary>

Round-trips a simple object with compile-time generated JSON metadata.

![JSON Serialization Source Generation Round Trip benchmark](reports/JsonSerialization/charts/13-source-generation-round-trip.png)

</details>
<details>
<summary><strong>14 · Prepare Serializer</strong></summary>

Creates fresh serializer settings and explicit type metadata without serializing data.

![JSON Serialization Prepare Serializer benchmark](reports/JsonSerialization/charts/14-prepare-serializer.png)

</details>

## Logging

### Rating

A gold, silver and bronze star for the first three places of every benchmark overview.

| # | Library | 🥇 | 🥈 | 🥉 | Won |
|---|---|---|---|---|---|
| 1 | NLog | 2 | 1 |  | gold in Core, gold in Structured, silver in Prepare |
| 2 | Serilog | 1 | 2 |  | gold in Prepare, silver in Core, silver in Structured |
| 3 | log4net |  |  | 3 | bronze in Core, bronze in Prepare, bronze in Structured |

### Benchmark overview

Performance and allocated memory are shown together. Lower values are better.

![Logging Core benchmark overview](reports/Logging/charts/overview-core.png)

![Logging Structured benchmark overview](reports/Logging/charts/overview-structured.png)

![Logging Prepare benchmark overview](reports/Logging/charts/overview-prepare.png)

### Libraries

<table>
<tr>
<td width="64"><img src="metadata/Logging/logos/log4net.svg" width="48" height="48" alt="log4net logo"></td>
<td><strong><a href="https://logging.apache.org/log4net/">log4net</a></strong> 3.3.2<br>A mature Apache logging framework with hierarchical repositories, contextual properties, layouts, and appenders.</td>
</tr>
<tr>
<td width="64"><img src="metadata/Logging/logos/microsoft-extensions-logging.svg" width="48" height="48" alt="Microsoft.Extensions.Logging logo"></td>
<td><strong><a href="https://learn.microsoft.com/dotnet/core/extensions/logging">Microsoft.Extensions.Logging</a></strong> 10.0.10<br>The standard .NET logging abstraction and logger factory with providers, filtering, structured state, and scopes.</td>
</tr>
<tr>
<td width="64"><img src="metadata/Logging/logos/nlog.svg" width="48" height="48" alt="NLog logo"></td>
<td><strong><a href="https://nlog-project.org/">NLog</a></strong> 6.1.4<br>A configurable logging platform with structured events, scope context, targets, layouts, and asynchronous wrappers.</td>
</tr>
<tr>
<td width="64"><img src="metadata/Logging/logos/serilog.svg" width="48" height="48" alt="Serilog logo"></td>
<td><strong><a href="https://serilog.net/">Serilog</a></strong> 4.4.0<br>A structured event logger with message templates, contextual enrichment, and a broad sink ecosystem.</td>
</tr>
<tr>
<td width="64"><img src="metadata/Logging/logos/zlogger.svg" width="48" height="48" alt="ZLogger logo"></td>
<td><strong><a href="https://github.com/Cysharp/ZLogger">ZLogger</a></strong> 2.5.10<br>A source-generated and interpolated-string logger built on Microsoft.Extensions.Logging with UTF-8 and processor APIs.</td>
</tr>
</table>

### Benchmark scenarios

<details>
<summary><strong>01 · Disabled Log</strong></summary>

Submits an Information event to a logger whose minimum level is Warning.

![Logging Disabled Log benchmark](reports/Logging/charts/01-disabled-log.png)

</details>
<details>
<summary><strong>02 · Simple Message</strong></summary>

Delivers one literal Information message to an in-memory sink.

![Logging Simple Message benchmark](reports/Logging/charts/02-simple-message.png)

</details>
<details>
<summary><strong>03 · Structured Properties</strong></summary>

Delivers one event with independently queryable OrderId and ElapsedMs properties.

![Logging Structured Properties benchmark](reports/Logging/charts/03-structured-properties.png)

</details>
<details>
<summary><strong>04 · Exception</strong></summary>

Delivers one Error event retaining the original exception metadata.

![Logging Exception benchmark](reports/Logging/charts/04-exception.png)

</details>
<details>
<summary><strong>05 · Scope Or Context</strong></summary>

Creates a temporary RequestId context and captures it on one event.

![Logging Scope Or Context benchmark](reports/Logging/charts/05-scope-or-context.png)

</details>
<details>
<summary><strong>06 · Template Rendering</strong></summary>

Formats amount 12.5 and customer Ada through the logger template API.

![Logging Template Rendering benchmark](reports/Logging/charts/06-template-rendering.png)

</details>
<details>
<summary><strong>07 · Buffered Logging</strong></summary>

Enqueues one event to a library-provided async or buffering wrapper and validates delivery after flush.

![Logging Buffered Logging benchmark](reports/Logging/charts/07-buffered-logging.png)

</details>
<details>
<summary><strong>08 · Prepare Logger</strong></summary>

Creates, verifies, and releases one Information-enabled logger with an in-memory sink.

![Logging Prepare Logger benchmark](reports/Logging/charts/08-prepare-logger.png)

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

## Validation

### Rating

A gold, silver and bronze star for the first three places of every benchmark overview.

| # | Library | 🥇 | 🥈 | 🥉 | Won |
|---|---|---|---|---|---|
| 1 | MiniValidation | 3 |  |  | gold in Basic, gold in Object Graph, gold in Prepare |
| 2 | FluentValidation | 1 | 3 |  | gold in Rules, silver in Basic, silver in Object Graph, silver in Prepare |

### Benchmark overview

Performance and allocated memory are shown together. Lower values are better.

![Validation Basic benchmark overview](reports/Validation/charts/overview-basic.png)

![Validation Object Graph benchmark overview](reports/Validation/charts/overview-object-graph.png)

![Validation Rules benchmark overview](reports/Validation/charts/overview-rules.png)

![Validation Prepare benchmark overview](reports/Validation/charts/overview-prepare.png)

### Libraries

<table>
<tr>
<td width="64"><img src="metadata/Validation/logos/data-annotations.svg" width="48" height="48" alt="DataAnnotations logo"></td>
<td><strong><a href="https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations">DataAnnotations</a></strong><br>The validation attributes and validation APIs included with the .NET framework.</td>
</tr>
<tr>
<td width="64"><img src="metadata/Validation/logos/fluent-validation.svg" width="48" height="48" alt="FluentValidation logo"></td>
<td><strong><a href="https://docs.fluentvalidation.net/">FluentValidation</a></strong> 12.1.1<br>A strongly typed validation library with fluent rules, nested validators, cascade modes, and asynchronous validation.</td>
</tr>
<tr>
<td width="64"><img src="metadata/Validation/logos/mini-validation.svg" width="48" height="48" alt="MiniValidation logo"></td>
<td><strong><a href="https://github.com/DamianEdwards/MiniValidation">MiniValidation</a></strong> 0.10.0<br>A minimal DataAnnotations-based validator with recursive object graph traversal and cycle detection.</td>
</tr>
</table>

### Benchmark scenarios

<details>
<summary><strong>01 · Valid Object</strong></summary>

Validates one object whose scalar properties satisfy every rule.

![Validation Valid Object benchmark](reports/Validation/charts/01-valid-object.png)

</details>
<details>
<summary><strong>02 · Single Failure</strong></summary>

Validates one object and returns its single property failure.

![Validation Single Failure benchmark](reports/Validation/charts/02-single-failure.png)

</details>
<details>
<summary><strong>03 · Multiple Failures</strong></summary>

Validates one object and materializes three independent property failures.

![Validation Multiple Failures benchmark](reports/Validation/charts/03-multiple-failures.png)

</details>
<details>
<summary><strong>04 · Nested Object</strong></summary>

Traverses a nested object and reports the complete failing property path.

![Validation Nested Object benchmark](reports/Validation/charts/04-nested-object.png)

</details>
<details>
<summary><strong>05 · Collection</strong></summary>

Traverses three collection elements and reports an indexed failure path.

![Validation Collection benchmark](reports/Validation/charts/05-collection.png)

</details>
<details>
<summary><strong>06 · Conditional Rule</strong></summary>

Applies a tax ID rule only when the input represents a business.

![Validation Conditional Rule benchmark](reports/Validation/charts/06-conditional-rule.png)

</details>
<details>
<summary><strong>07 · Custom Rule</strong></summary>

Applies a custom predicate that accepts only even integer codes.

![Validation Custom Rule benchmark](reports/Validation/charts/07-custom-rule.png)

</details>
<details>
<summary><strong>08 · Stop On First Failure</strong></summary>

Stops validation after the first failing rule in the declared order.

![Validation Stop On First Failure benchmark](reports/Validation/charts/08-stop-on-first-failure.png)

</details>
<details>
<summary><strong>09 · Async Validation</strong></summary>

Runs a deterministic asynchronous availability rule through the library async API.

![Validation Async Validation benchmark](reports/Validation/charts/09-async-validation.png)

</details>
<details>
<summary><strong>10 · Prepare Validator</strong></summary>

Creates the complete scalar validator or rule graph without validating an input.

![Validation Prepare Validator benchmark](reports/Validation/charts/10-prepare-validator.png)

</details>
