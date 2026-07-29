# Add a matrix category

This workflow describes how to add a new category of .NET libraries, such as
object mapping, serialization, validation, or logging. It is intended to be
self-contained so that a human or an LLM can implement a category in a separate
chat without relying on undocumented project history.

All paths and commands stored in the repository must be relative to the
repository root. Examples use `Matrix.ObjectMapping` only as a placeholder.
Choose names that describe the actual category.

## Starting a separate chat

Use this handoff prompt and replace the placeholders:

```text
Add a new <Category Name> category to dotnet-matrix.

Read workflows/add-category.md completely and follow it as the source of truth.
Also read the current shared Matrix and build implementation named by that
workflow before making changes.

Inputs:
- module id: <module-id>
- project: src/Matrix.<Category>
- report directory: <ReportDirectory>
- run configuration prefix: <Prefix>
- initial libraries or reference implementations: <list or paths>
- intended features: <list>

First inspect the repository and propose a concrete implementation plan. Keep
the implementation category-neutral outside src/Matrix.<Category>. Do not run
benchmarks. Build both conditional variants and run validation, then give me
the exact benchmark command to run myself.
```

If feature semantics or initial libraries are not yet known, the new chat must
first design the English feature contract with the user. It must not invent
performance scenarios silently.

## LLM execution policy

An LLM must not run benchmarks. BenchmarkDotNet runs can take a long time,
consume significant resources, and make the workstation temporarily less
responsive.

An LLM may:

- inspect reference projects and external adapters;
- create the category project, scenarios, validation, metadata, and build
  integration;
- build both conditional variants;
- run feature validation;
- generate metadata and IDE run configurations;
- build the WebAssembly project directly without running benchmarks;
- run the production `build-web` target only when the new category already has
  the reports required by its chart configuration.

An LLM must not execute:

- `<module-id>-benchmarks`;
- `<module-id>-update-library`, because it includes benchmarks;
- `prepare-commit`, `ci-reports`, or another aggregate target that includes
  benchmarks;
- the benchmark executable directly.

After validation succeeds, the LLM must stop and give the user the exact
repository-relative command that runs the category benchmarks.

## Architecture

The repository has three layers:

1. `src/Matrix` contains category-neutral contracts, report models, module
   metadata discovery, filtering, rating, chart, and Web catalog models.
2. Each `src/Matrix.<Category>` executable owns its feature contracts,
   scenarios, adapters, validation rules, and benchmark hot paths.
3. `build` discovers category assemblies and creates category-specific targets,
   reports, run configurations, charts, README sections, and the production Web
   catalog.

A category is discovered from the projects in `dotnet-matrix.slnx`. The build
loads projects named `Matrix.*`, builds their validation variant, and calls
`MatrixMetadata.TryRead` on the resulting assembly. A valid module is therefore
registered through project and assembly metadata; it must not be hardcoded in
the build composition.

The normal data flow is:

```text
Matrix.<Category>.csproj
  -> embedded as Matrix.Project.csproj
  -> MatrixMetadata reads module and library metadata
  -> build discovers the module
  -> validation and benchmark reports
  -> shared PNG, README, and Web renderers
```

### Current source-of-truth map

Read these files before changing shared behavior:

| Concern | Source |
| --- | --- |
| Module project contract and generated library catalog | `src/Matrix/Matrix.Module.targets` |
| Embedded project metadata parsing | `src/Matrix/MatrixMetadata.cs` |
| Library filtering | `src/Matrix/MatrixLibraryCatalog.cs` |
| Feature declaration | `src/Matrix/MatrixFeature.cs` |
| Report schema and storage | `src/Matrix/MatrixReports.cs`, `src/Matrix/MatrixReportStore.cs` |
| Environment identity | `src/Matrix/BenchmarkEnvironment.cs` |
| Charts, metrics, overviews, and ratings | `src/Matrix/MatrixCharts.cs`, `src/Matrix/MatrixMetrics.cs`, `src/Matrix/MatrixOverview.cs`, `src/Matrix/MatrixRating.cs` |
| Command names | `src/Matrix/MatrixRunner.cs` |
| Build discovery | `build/Targets/MatrixModuleDiscovery.cs` |
| Validation and benchmark process launch | `build/Targets/MatrixTarget.cs` |
| Per-library update | `build/Targets/LibraryTarget.cs` |
| Metadata and PNG generation | `build/Targets/MetadataTarget.cs`, `build/Targets/ReportChartsTarget.cs` |
| README generation | `build/Targets/ReadmeTarget.cs` |
| Rider configurations | `build/Targets/RunConfigurationsTarget.cs` |
| Full artifact preparation | `build/Targets/PrepareCommitTarget.cs` |
| CI report staging | `build/Targets/CiReportsTarget.cs` |
| Production Web catalog and publish | `build/Targets/WebTarget.cs` |
| Build command registration | `build/BuildApplication.cs` |
| Local Web catalog | `src/Matrix.Web/wwwroot/data/catalog.json` |
| GitHub report and Pages workflows | `.github/workflows/reports.yml`, `.github/workflows/pages.yml` |

Prefer extending these category-neutral points over adding checks for a
specific module name. Keep feature semantics and third-party APIs inside the
category project.

## Required inputs

Decide these values before writing code:

| Item | Example | Rules |
| --- | --- | --- |
| Project | `Matrix.ObjectMapping` | Must start with `Matrix.` |
| Module ID | `object-mapping` | Stable, URL-safe, unique |
| Display name | `Object Mapping` | User-facing category name |
| Run configuration prefix | `Mapping` | Short and unique |
| Report directory | `ObjectMapping` | Unique under `reports` and `metadata` |
| Catalog namespace | `Matrix.ObjectMapping.Infrastructure` | Valid C# namespace |
| Feature IDs | `Map`, `Project`, ... | Stable and unique inside the category |
| Library IDs | `Mapper.One`, ... | Stable and unique inside the category |

Changing IDs later breaks report merging, URLs, run configurations, and
historical comparisons. Treat IDs as persistent data keys, not presentation
text.

## 1. Define the feature contract first

Create:

```text
workflows/feature-contracts/<module-id>.md
```

Write it in English. For every feature, document:

- the exact operation being measured;
- the shape and size of the input and output;
- object graph or data model requirements;
- setup that is excluded from measurement;
- work that must happen inside the benchmark invocation;
- observable assertions used by validation;
- the exact condition for `Supported`;
- legitimate reasons for `Unsupported` or `NotApplicable`;
- whether the feature participates in a rating group;
- any required laziness, materialization, caching, allocation, or lifetime
  behavior.

The implementation, validation, feature description, and contract must agree.
Do not infer support merely because an API call completes. Validate the
behavior promised by the contract.

Use these result meanings consistently:

- `Supported`: the implementation passes all semantic checks.
- `Unsupported`: the library cannot provide the required behavior.
- `NotApplicable`: the feature has no meaningful equivalent for this library.
- `Failed`: the adapter claims or attempts support but validation fails.

An unsupported feature must be declared explicitly with a reason. Missing
benchmark code and missing unavailability metadata is a failure, not an
implicit `Unsupported`.

## 2. Create the category project

Create this initial structure:

```text
src/Matrix.<Category>/
  Benchmarks/
    Common/
    <LibraryOne>/
  Infrastructure/
  Model/
  Scenarios/
  Validation/
  Composition.cs
  GlobalUsings.cs
  Program.cs
  Matrix.<Category>.csproj

metadata/<ReportDirectory>/
  logos/
  charts.json

reports/<ReportDirectory>/
```

Only add files that the category needs. Follow the closest existing category
for shared conventions, but keep category semantics local.

Add the project to the `/src/` folder in `dotnet-matrix.slnx`:

```xml
<Project Path="src/Matrix.ObjectMapping/Matrix.ObjectMapping.csproj" />
```

The build project should not need a direct project reference or a hardcoded
category registration.

## 3. Configure module discovery

The category project must be an executable and reference the shared project:

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <TargetFramework>net10.0</TargetFramework>
        <ImplicitUsings>enable</ImplicitUsings>
        <Nullable>enable</Nullable>
        <OutputType>Exe</OutputType>
        <MatrixMode Condition="'$(MatrixMode)' == ''">Validation</MatrixMode>
        <DefineConstants Condition="'$(MatrixMode)' == 'Validation'">$(DefineConstants);MATRIX_VALIDATION</DefineConstants>
        <DefineConstants Condition="'$(MatrixMode)' == 'Benchmark'">$(DefineConstants);MATRIX_BENCHMARK</DefineConstants>
        <OutputPath>bin\$(MatrixMode)\$(Configuration)\</OutputPath>
        <IntermediateOutputPath>obj\$(MatrixMode)\$(Configuration)\</IntermediateOutputPath>

        <MatrixModuleId>object-mapping</MatrixModuleId>
        <MatrixModuleName>Object Mapping</MatrixModuleName>
        <MatrixRunConfigurationPrefix>Mapping</MatrixRunConfigurationPrefix>
        <MatrixReportDirectory>ObjectMapping</MatrixReportDirectory>
        <MatrixLibraryCatalogNamespace>Matrix.ObjectMapping.Infrastructure</MatrixLibraryCatalogNamespace>
    </PropertyGroup>

    <ItemGroup>
        <ProjectReference Include="..\Matrix\Matrix.csproj"/>
        <PackageReference Include="BenchmarkDotNet" Version="..."/>
        <PackageReference Include="Pure.DI" Version="...">
            <PrivateAssets>all</PrivateAssets>
            <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
        </PackageReference>
    </ItemGroup>

    <Import Project="..\Matrix\Matrix.Module.targets"/>

</Project>
```

Use the target framework and shared package versions currently used by the
repository. Do not copy stale versions from this document.

`Matrix.Module.targets`:

- validates the required `MatrixModule*` properties;
- embeds the project file as `Matrix.Project.csproj`;
- validates annotated library package references;
- generates `LibraryCatalog.g.cs` in
  `MatrixLibraryCatalogNamespace`.

Do not create a manual library catalog or duplicate package versions in
attributes or source code.

## 4. Register libraries in the project file

Each compared NuGet library has one primary `PackageReference` with an exact
literal version and matrix metadata:

```xml
<PackageReference Include="Mapper.One" Version="1.2.3">
    <MatrixLibraryId>Mapper.One</MatrixLibraryId>
    <MatrixLibraryName>Mapper One</MatrixLibraryName>
    <MatrixCodeName>MapperOne</MatrixCodeName>
    <MatrixDescription>A concise English description.</MatrixDescription>
    <MatrixDocumentationUrl>https://example.org/docs</MatrixDocumentationUrl>
    <MatrixRepositoryUrl>https://github.com/example/mapper-one</MatrixRepositoryUrl>
    <MatrixLogo>logos/mapper-one.svg</MatrixLogo>
</PackageReference>
```

Requirements:

- `Version` must be an exact literal, not a property, range, or conditional
  expression.
- `MatrixLibraryId`, package ID, and `MatrixCodeName` must be unique.
- `MatrixCodeName` must be a valid generated C# identifier.
- Provide at least a documentation or repository URL.
- `MatrixLogo` is relative to `metadata/<ReportDirectory>`.
- Add secondary integration packages as ordinary, unannotated references.
- Add `<MatrixRating>false</MatrixRating>` only when the library must be visible
  but excluded from category ratings.

The generated catalog constant is then used everywhere:

```csharp
LibraryCatalog.MapperOne
```

The current metadata contract assumes that each compared library has a primary
NuGet package. If a category needs to compare something that cannot be
represented this way, extend the shared metadata contract deliberately instead
of inventing a fake package.

## 5. Use dependency injection in the category application

Internal orchestration must use Pure.DI. Keep `Program.cs` minimal:

```csharp
return new Composition(args).Root.Run();
```

In `Composition.cs`, compose at least:

- `MatrixMetadata.Read(typeof(Composition).Assembly)` as the `MatrixModule`;
- the command-line options parser;
- `MatrixLibraryCatalog`;
- the feature and benchmark report stores;
- the category validation runner for `MATRIX_VALIDATION`;
- the category benchmark runner for `MATRIX_BENCHMARK`;
- the application root.

Follow `src/Matrix.DependencyInjection/Composition.cs` for composition style,
not for category-specific registrations or policies.

## 6. Implement category infrastructure

Each category normally needs local equivalents of:

- `LibraryBenchmarkAttribute`, containing a required library ID and optional
  baseline flag;
- `FeatureUnavailableAttribute`, containing library ID, status, and reason;
- an application that parses library filters and invokes `IMatrixRunner`;
- a validation runner;
- a BenchmarkDotNet runner;
- feature report and benchmark report merge logic where shared services do not
  already provide it.

Use the shared `Matrix` models and services whenever their behavior is truly
category-neutral. Do not move scenario semantics or library API abstractions
into `src/Matrix`.

The library ID on every benchmark and unavailability declaration is mandatory.
Validation must use that ID to associate code, feature support, and report
records. Never infer the library from a namespace, type name, file name, or
folder.

## 7. Share scenario code without adding benchmark overhead

The validation and benchmark must exercise the same scenario code and models.
Use conditional compilation only where it removes validation machinery from
the benchmark assembly:

```csharp
[Conditional("MATRIX_VALIDATION")]
private void Validate(string libraryId, Result result)
{
    ScenarioValidation.Validate(libraryId, result);
}
```

Rules for benchmark methods:

- call the library API directly;
- return a strongly typed result;
- do not route the hot path through a universal adapter;
- do not return `object`;
- do not add boxing, reflection, delegates, dictionaries, or interface
  dispatch solely for matrix infrastructure;
- keep setup and cleanup outside the measured method unless the feature
  contract explicitly measures them;
- put state used only by assertions behind `MATRIX_VALIDATION`;
- keep ordinary maintainable code outside `#if`;
- use the simplest single-threaded, single-process behavior supported by the
  scenario.

For every benchmark method named `FeatureName`, the validation runner may use
the conventional `SetupFeatureName` and `CleanupFeatureName` methods. Keep
these conventions identical across libraries in the category.

The shared validator must receive the explicit library ID:

```csharp
Validate(LibraryCatalog.MapperOne, result);
```

This detects accidental cross-library validation and produces attributable
errors.

## 8. Declare features in code

Create one common partial benchmark class per feature under
`Benchmarks/Common`. Annotate it with the shared metadata attribute:

```csharp
[MatrixFeature(
    "Map",
    1,
    "Map",
    "Maps one preconfigured source object to a new destination object.")]
public partial class MapBenchmark
{
}
```

Feature IDs and order must be unique. Descriptions must be non-empty and
English. The attributes become `metadata/<ReportDirectory>/features.json`
through the shared metadata target.

Use one file per library and feature under `Benchmarks/<Library>`. Partial
classes are acceptable when they preserve a direct, typed hot path. Follow the
repository's file naming, namespace suppression, and Rider inspection
conventions.

## 9. Decide baseline behavior explicitly

A hand-coded baseline is a library-like benchmark participant, not a hidden
special case. If the category has a meaningful hand-coded implementation:

- give it a stable library ID, display name, and benchmark metadata;
- implement and validate it feature by feature;
- report unsupported features explicitly;
- show it in feature charts and overviews under the same coverage rules;
- do not assume it is always fastest or always supported.

Do not copy the Dependency Injection `Hand-coded` policy blindly. Some
categories may have no meaningful baseline. A zero time or zero allocation is
valid only when the feature contract genuinely requires no runtime work; never
use zero as a placeholder for missing code.

If a non-package baseline needs to appear in library metadata, first verify
that the shared metadata model supports it. Extend the shared contract in one
place if necessary.

## 10. Preserve partial report updates

Both runners must support a library filter. When only one or several libraries
are selected:

- replace records only for selected library IDs;
- preserve records for all other libraries;
- preserve unrelated environments and category data;
- validate that the existing report has the same `ModuleId`;
- warn during a partial benchmark update when the current framework, operating
  system, architecture, runtime, or relevant BenchmarkDotNet environment
  differs from the environment stored for retained results.

Reports remain schema version `1` unless a deliberate shared schema migration
is required. Do not increment the schema merely because a new category,
library, feature, or chart group was added.

Generated files are:

```text
reports/<ReportDirectory>/features.json
reports/<ReportDirectory>/benchmarks.json
metadata/<ReportDirectory>/libraries.json
metadata/<ReportDirectory>/features.json
```

Do not hand-edit generated report or metadata files.

## 11. Add presentation metadata

Store redistributable logos in:

```text
metadata/<ReportDirectory>/logos/
```

Prefer SVG or transparent images that remain legible on both light and dark
backgrounds. The logo path in the project metadata is relative to the category
metadata directory.

Create `metadata/<ReportDirectory>/charts.json` manually:

```json
{
  "schemaVersion": 1,
  "groups": [
    {
      "id": "basic",
      "name": "Basic",
      "features": [
        "Map"
      ]
    }
  ]
}
```

Chart feature IDs must match `MatrixFeatureAttribute` IDs exactly. Groups drive
overview charts and medals. A library is ranked in a group only when it has
results for the complete required feature set; incomplete libraries are shown
as not ranked. Put only comparable performance features in rating groups.

Per-feature charts combine performance and allocated memory. The shared chart
renderer, README generator, and Web application consume the same reports and
chart metadata.

## 12. Integrate the WebAssembly application

Production integration is automatic after module discovery. `build-web`
generates the production category catalog and packages reports, metadata, and
logos for all discovered modules.

For local development, also add the category to:

```text
src/Matrix.Web/wwwroot/data/catalog.json
```

Keep schema version `1`:

```json
{
  "id": "object-mapping",
  "name": "Object Mapping",
  "reportDirectory": "ObjectMapping"
}
```

The Web project includes report and metadata trees through wildcards, so a new
category should not require category-specific Razor components or explicit
content entries. If it does, first determine whether the behavior belongs in a
shared category-neutral view.

Local Web runs load checked-in data from `reports` and `metadata`. Production
runs load the same paths from GitHub for the selected semantic-version tag and
commit. A production version appears only when an exact `x.y.z` tag and the
corresponding generated data are available.

## 13. Verify automatic build targets

Once the project is in `dotnet-matrix.slnx` and its metadata is valid, the build
must generate:

```text
<module-id>-validate [--libraries <filter>]
<module-id>-benchmarks [--libraries <filter>] [--smoke]
<module-id>-update-library --library <filter>
```

It must also include the category in these shared targets:

```text
generate-run-configurations
generate-metadata
render-reports
readme
build-web
prepare-commit
finalize-commit
ci-reports
```

`generate-run-configurations` creates validation and benchmark configurations
for every library and for all libraries, plus the per-library update workflow.
Do not add category-specific names to the shared target.

No GitHub Actions change should normally be necessary: report generation and
Pages deployment operate on discovered modules. The generated reports,
metadata, logos, charts, README, and Web artifacts must exist in the tagged
commit deployed by Pages.

### Current filtered-CI limitation

The aggregate `ci-reports --libraries <filter>` currently sends the same filter
to every discovered module. With several categories, a library filter that
matches one category may match no libraries in another category and cause that
module to fail.

Until shared filtering becomes category-aware, use the category-specific
validation, benchmark, or update target for filtered work. Do not advertise the
aggregate filtered target as safe across unrelated categories. A future shared
fix should either qualify filters with a module ID or skip nonmatching modules
without weakening the category-specific “no match” error.

## 14. Verification commands for an LLM

An LLM may run these commands:

```powershell
dotnet build src/Matrix.ObjectMapping/Matrix.ObjectMapping.csproj -c Release -p:MatrixMode=Validation
dotnet build src/Matrix.ObjectMapping/Matrix.ObjectMapping.csproj -c Release -p:MatrixMode=Benchmark
dotnet run --project build/build.csproj -- generate-metadata
dotnet run --project build/build.csproj -- generate-run-configurations
dotnet run --project build/build.csproj -- object-mapping-validate
dotnet build src/Matrix.Web/Matrix.Web.csproj -c Release
```

Replace the example project and module ID with the real values. Also inspect
the build help or generated run configurations and confirm that the new module
and every library were discovered.

`build-web` invokes the shared PNG renderer first. Run it only if
`reports/<ReportDirectory>/benchmarks.json` and
`metadata/<ReportDirectory>/charts.json` already exist and are consistent.
Otherwise a direct `Matrix.Web` build is the correct compile check before the
user produces benchmark data.

The validation command must prove:

- every feature has either a benchmark method or explicit unavailability
  metadata for every library;
- every claimed feature passes semantic validation;
- filters select only the intended libraries;
- generated feature and library metadata contain the category;
- no benchmark-only compilation errors remain.

Do not run a benchmark command as part of LLM verification.

## 15. Commands the user runs

After the LLM completes validation, give the user:

```powershell
dotnet run --project build/build.csproj -- object-mapping-benchmarks
```

For one library and all derived artifacts:

```powershell
dotnet run --project build/build.csproj -- object-mapping-update-library --library Mapper.One
```

For a full repository refresh before a commit:

```powershell
dotnet run --project build/build.csproj -- prepare-commit
```

`prepare-commit` runs benchmarks for discovered modules and can be expensive.
`finalize-commit` regenerates derived artifacts from existing reports without
running benchmarks. The user chooses which command is appropriate.

## Category-specific decisions that must not be copied from DI

Review every one of these explicitly:

- feature names, ordering, inputs, and support conditions;
- setup versus measured work;
- validation assertions;
- baseline existence and behavior;
- which features participate in ratings;
- overview group names and coverage requirements;
- reasons for `Unsupported` and `NotApplicable`;
- BenchmarkDotNet job settings when the workload needs different treatment;
- scenario models and data sizes;
- report directory, module ID, prefix, namespaces, and library IDs;
- package ownership and logo licensing.

The Dependency Injection category is a structural example, not the semantic
specification for other categories.

## Completion checklist

- [ ] The English feature contract exists and defines support precisely.
- [ ] The project name starts with `Matrix.` and is included in
      `dotnet-matrix.slnx`.
- [ ] All five `MatrixModule*` properties are present and unique.
- [ ] The project imports `src/Matrix/Matrix.Module.targets`.
- [ ] Validation and benchmark outputs are isolated by `MatrixMode`.
- [ ] Pure.DI composes the internal application services.
- [ ] Every compared package has exact version and complete matrix metadata.
- [ ] Library IDs are explicit on every benchmark and availability declaration.
- [ ] Validation and benchmarks reuse scenario code without adding hot-path
      abstraction, boxing, or reflection.
- [ ] Every feature is implemented or explicitly unavailable for every library.
- [ ] Partial report updates preserve unselected libraries and warn on
      environment mismatch.
- [ ] Logos and `charts.json` exist under the category metadata directory.
- [ ] The local Web catalog contains the category.
- [ ] Generated metadata contains the expected module, libraries, and features.
- [ ] Generated run configurations contain all/category/per-library actions.
- [ ] Both conditional variants build.
- [ ] Category validation passes.
- [ ] The LLM did not run benchmarks.
- [ ] The user received the exact benchmark or per-library update command.
- [ ] README, PNG reports, Web data, and Pages deployment are regenerated after
      the user runs benchmarks.
