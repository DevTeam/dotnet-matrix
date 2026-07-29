# Add a library

This workflow describes how to add one library to an existing dotnet-matrix
category. It is intended for humans and LLM agents and is self-contained enough
to use as the source of truth in a separate chat.

To create a category, follow [add-category.md](add-category.md) first. A library
must conform to the feature contract and implementation conventions owned by
its category.

All paths and commands stored in the repository must be relative to the
repository root. An external adapter, sample, or repository may be inspected as
input, but its absolute local path must never be persisted.

## Starting a separate chat

Use this handoff prompt and replace the placeholders:

```text
Add <Library Name> to the <Category Name> category in dotnet-matrix.

Read workflows/add-library.md completely and follow it as the source of truth.
Read the category feature contract and the current module implementation before
making changes.

Inputs:
- category/module id: <module-id>
- library name: <display name>
- NuGet package: <package id>
- official documentation or repository: <URL>
- optional reference adapter or sample: <path or URL>

Implement every natively supported feature, declare every unsupported or
non-applicable feature explicitly, and keep benchmark hot paths direct and
strongly typed. Do not run benchmarks. Build both conditional variants and run
validation only for the new library. Then give me the exact per-library update
command to run myself.
```

## LLM execution policy

An LLM must not run benchmarks. BenchmarkDotNet runs can take a long time,
consume significant resources, and make the workstation temporarily less
responsive.

An LLM may:

- inspect external source code, documentation, and reference adapters;
- modify the selected category and its metadata;
- build the validation and benchmark conditional variants;
- generate metadata and Rider run configurations;
- run feature validation only for the new library;
- inspect existing reports without changing benchmark measurements;
- build the WebAssembly project directly as a compile check.

An LLM must not execute:

- `<module-id>-benchmarks`;
- `<module-id>-update-library`, because it includes benchmarks;
- `prepare-commit` or `ci-reports` unless benchmarks are explicitly skipped and
  the command is needed for this task;
- `readme`, `render-reports`, or `build-web` when doing so would require missing
  benchmark data for the new library;
- the benchmark executable directly.

After validation succeeds, the LLM must stop and give the user the exact
repository-relative per-library update command. The user decides when to run
benchmarks.

## Required inputs

Determine:

- category name or stable module ID;
- library display name;
- stable library ID;
- primary NuGet package ID and exact version;
- source-safe `MatrixCodeName`;
- official documentation URL or canonical repository URL;
- concise English description;
- redistributable logo;
- optional reference adapter, sample, or benchmark implementation.

Do not silently invent an ID, support claim, or benchmark behavior when it
would materially change historical report identity or feature semantics.

## 1. Discover the category

Before editing:

1. Locate the category through `dotnet-matrix.slnx` and its embedded project
   metadata. Do not infer it only from a directory name.
2. Read the complete category feature contract:

   ```text
   workflows/feature-contracts/<module-id>.md
   ```

3. Read:
   - the category project file;
   - `Benchmarks/Common`;
   - at least one complete current library integration;
   - scenario models and shared validation;
   - benchmark and feature availability attributes;
   - the validation and benchmark runners;
   - current metadata, feature report, and chart groups;
   - generated Rider configurations when present.
4. Read [add-category.md](add-category.md) when the category architecture or
   shared build behavior is unclear.
5. Identify the module ID, report directory, generated `LibraryCatalog`
   namespace, and exact per-library update command from metadata or build
   discovery. Do not hardcode a category name into shared build code.

For Dependency Injection, the feature contract is
[feature-contracts/dependency-injection.md](feature-contracts/dependency-injection.md).

## 2. Inspect the library before claiming support

Use official documentation and source code to map every category feature to the
library's native behavior.

For each feature decide:

- `Supported`: the native implementation satisfies the full executable
  contract;
- `Unsupported`: the library cannot provide the required behavior;
- `NotApplicable`: the feature has no meaningful equivalent for this library;
- `Failed`: the adapter attempts or claims support but validation fails.

A similarly named API is not evidence of support. Check semantics such as
laziness, materialization, caching, lifetime boundaries, ordering, generated
code, interception, disposal, or concurrency whenever the contract requires
them.

Do not emulate a missing native feature with matrix-owned wrappers, factories,
caches, or conversion code merely to mark it supported. Normal glue required
by the public library API is acceptable when it does not change the scenario.

Create a feature mapping before implementation:

| Feature ID | Intended status | Library API | Validation evidence | Notes |
| --- | --- | --- | --- | --- |
| `<id>` | `Supported` | `<API>` | `<observable assertion>` | `<setup constraints>` |

Keep this mapping in the implementation plan or chat; do not create a
repository artifact unless it adds lasting value beyond the feature contract.

## 3. Register the primary NuGet package

Add one annotated primary `PackageReference` to the category project:

```xml
<PackageReference Include="Mapper.One" Version="1.2.3">
    <MatrixLibraryId>Mapper.One</MatrixLibraryId>
    <MatrixLibraryName>Mapper One</MatrixLibraryName>
    <MatrixCodeName>MapperOne</MatrixCodeName>
    <MatrixDescription>A concise factual English description.</MatrixDescription>
    <MatrixDocumentationUrl>https://example.org/docs</MatrixDocumentationUrl>
    <MatrixRepositoryUrl>https://github.com/example/mapper-one</MatrixRepositoryUrl>
    <MatrixLogo>logos/mapper-one.svg</MatrixLogo>
</PackageReference>
```

Requirements:

- `Version` is an exact literal on the `PackageReference`.
- Do not use an MSBuild property, wildcard, range, or condition.
- `MatrixLibraryId` is a stable report and filter key.
- `MatrixLibraryName` is the user-facing name.
- `MatrixCodeName` is a unique valid C# identifier.
- `MatrixDescription` is short, factual, and English.
- Provide official documentation, a canonical repository, or both.
- `MatrixLogo` is relative to `metadata/<ReportDirectory>`.
- Add `<MatrixRating>false</MatrixRating>` only when the library must be visible
  but excluded from ratings. Omission means it participates in ratings.

Add packages required for optional integrations as ordinary, unannotated
references. Only one primary package receives `MatrixLibraryId`.

The category project is embedded as `Matrix.Project.csproj`. Shared discovery
reads versions and metadata directly from that resource.
`Matrix.Module.targets` generates the compile-time constant:

```csharp
LibraryCatalog.MapperOne
```

Do not add a manual catalog constant, assembly metadata attribute, or duplicate
version elsewhere.

If the compared implementation has no primary NuGet package, stop and verify
that the category already has a supported baseline mechanism. Do not create a
fake package entry to bypass the shared metadata contract.

## 4. Add presentation metadata

Put the logo in:

```text
metadata/<ReportDirectory>/logos/
```

Prefer an official asset when its license and trademark rules permit
redistribution. Otherwise create a neutral repository-owned mark. Verify that
it remains recognizable on light and dark backgrounds and at the small size
used by the Web application and README.

Generate metadata from the project:

```powershell
dotnet run --project build/build.csproj -- generate-metadata
```

Inspect:

```text
metadata/<ReportDirectory>/libraries.json
```

Confirm the ID, description, URLs, logo path, rating flag, package name, and
version. Never hand-edit `libraries.json`.

## 5. Implement supported features

Follow the category's directory and naming convention. In Dependency Injection,
for example:

```text
src/Matrix.DependencyInjection/Benchmarks/<LibraryCodeName>/
  01_Singleton.cs
  02_Transient.cs
  ...
```

Add one file per supported feature. Each implementation must:

- extend the corresponding common partial benchmark class;
- use the generated library catalog constant;
- attach the category benchmark metadata with the explicit library ID;
- use feature-specific global setup and cleanup where required;
- call the third-party API directly inside the measured method;
- return a strongly typed scenario result;
- call the shared conditional validator with the same explicit library ID;
- dispose resources according to the feature contract;
- preserve the benchmark and setup naming conventions expected by validation.

Example shape:

```csharp
[Benchmark]
[LibraryBenchmark(LibraryCatalog.MapperOne)]
public Destination MapperOne()
{
    var result = _mapper.Map<Destination>(_source);
    Validate(LibraryCatalog.MapperOne, result);
    return result;
}
```

The exact attributes, setup names, and result types belong to the category.
Copy them from its common benchmark and validation runner, not from an
unrelated category.

### Benchmark hot-path rules

The measured method must not add matrix-owned:

- universal adapters;
- `object` return values;
- boxing;
- reflection;
- delegates or closures;
- dictionaries or service locators;
- interface dispatch;
- validation allocations or counters.

Setup-only reflection or callbacks are acceptable when required by the library
itself. Work explicitly included by the feature contract must remain inside the
measured method.

The validation and benchmark variants must reuse the same scenario method and
models. Remove validation-only work from benchmark builds through
`[Conditional("MATRIX_VALIDATION")]` and narrowly scoped `#if
MATRIX_VALIDATION`. Keep ordinary maintainable code outside conditional
compilation.

Always pass the explicit library ID to shared validation:

```csharp
Validate(LibraryCatalog.MapperOne, result);
```

This is part of validation correctness, not presentation metadata.

### Rider inspection conventions

Match nearby files. Add suppressions only when the inspection genuinely applies:

```csharp
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
```

For conditional instance validation methods that intentionally cannot be
static, use:

```csharp
[SuppressMessage("Performance", "CA1822:Mark members as static")]
```

Do not add broad suppressions to unrelated code.

## 6. Declare unavailable features explicitly

For every feature without a benchmark method, add the category's
`FeatureUnavailable` metadata with:

- the generated library catalog constant;
- `Unsupported` or `NotApplicable`;
- a concise factual English reason.

In Dependency Injection these declarations live on common feature classes under
`Benchmarks/Common`, for example:

```csharp
[FeatureUnavailable(
    LibraryCatalog.MapperOne,
    FeatureStatus.Unsupported,
    "Mapper One does not provide the behavior required by this contract.")]
```

Follow the selected category's actual convention. Do not create empty
library-specific files for unavailable features, and do not let a missing
method imply support status.

Validation must fail when a feature has neither a benchmark method nor explicit
unavailability metadata.

## 7. Strengthen shared validation when necessary

Supported libraries must be checked against the same behavioral contract. When
a new integration exposes a gap in validation:

1. improve the shared category validator rather than adding a weaker
   library-specific check;
2. pass the explicit library ID through every validation call;
3. make the assertion observable and deterministic;
4. keep validation-only state behind `MATRIX_VALIDATION`;
5. ensure the benchmark variant contains no assertion overhead;
6. revalidate existing libraries if the contract became stricter.

Do not change the feature contract merely to make the new library pass. If the
contract itself is wrong, treat that as a separate deliberate category change
and explain its impact on all libraries and historical comparisons.

## 8. Build both conditional variants

Run:

```powershell
dotnet build <module-project> -c Release -p:MatrixMode=Validation
dotnet build <module-project> -c Release -p:MatrixMode=Benchmark
```

The benchmark build is only a compilation check. Do not run the executable in
benchmark mode.

Resolve compiler errors and relevant Rider inspections in the new integration.
Preserve unrelated user changes in the worktree.

## 9. Validate only the new library

Run the category-specific validation target:

```powershell
dotnet run --project build/build.csproj -- <module-id>-validate --libraries <LibraryId>
```

For Dependency Injection:

```powershell
dotnet run --project build/build.csproj -- dependency-injection-validate --libraries SimpleInjector
```

Inspect:

```text
reports/<ReportDirectory>/features.json
```

Confirm:

- only the selected library's records were replaced;
- all other library records were preserved;
- every category feature has the intended status;
- every claimed supported feature passed its executable contract;
- every unavailable feature has the intended reason;
- `ModuleId` is unchanged and correct.

If validation fails, keep the result as `Failed` while diagnosing it. Do not
publish benchmark data for a feature that does not pass.

## 10. Generate Rider workflows

Run:

```powershell
dotnet run --project build/build.csproj -- generate-run-configurations
```

The generated `.run` directory must contain, under the category folder:

- validation for the new library;
- benchmark for the new library;
- per-library update for the new library;
- the existing all-libraries configurations.

The generator discovers the library through embedded package metadata. Do not
add its name to `build/Targets/RunConfigurationsTarget.cs`.

## 11. Generated artifacts and ownership

These files are generated and must not be edited manually:

```text
metadata/<ReportDirectory>/libraries.json
metadata/<ReportDirectory>/features.json
reports/<ReportDirectory>/features.json
reports/<ReportDirectory>/benchmarks.json
reports/<ReportDirectory>/charts/*.png
README.md
.run/*.run.xml
```

Source-controlled inputs are:

```text
<category project file>
<category benchmark and validation source>
workflows/feature-contracts/<module-id>.md
metadata/<ReportDirectory>/charts.json
metadata/<ReportDirectory>/logos/*
```

Adding a library normally does not require changes to:

- `src/Matrix`;
- shared build target registrations;
- `src/Matrix.Web/wwwroot/data/catalog.json`;
- category chart groups;
- GitHub Actions workflows.

Change those only when the library reveals a genuinely category-neutral
infrastructure gap or a deliberate category contract change.

## 12. Partial report behavior

The category runners must merge filtered results:

- validation replaces only the selected library in `features.json`;
- benchmarking replaces only the selected library in `benchmarks.json`;
- results for unselected libraries remain intact;
- the existing report must have the same module ID;
- a partial benchmark run warns when the current environment differs from the
  environment associated with retained results.

Review environment warnings before committing mixed results. Do not suppress or
normalize a real framework, runtime, operating-system, architecture, or
BenchmarkDotNet environment difference.

Adding a library does not require a schema change. Keep schema version `1`
unless a deliberate shared report migration is required.

## 13. User-run result generation

The user, not an LLM, runs:

```powershell
dotnet run --project build/build.csproj -- <module-id>-update-library --library <LibraryId>
```

For example:

```powershell
dotnet run --project build/build.csproj -- dependency-injection-update-library --library SimpleInjector
```

The target:

1. validates the selected library;
2. benchmarks only that library;
3. merges only its report records;
4. regenerates metadata and all PNG reports;
5. regenerates the repository README.

After it completes, review:

- feature statuses and reasons;
- benchmark time and allocated memory;
- the recorded environment;
- every per-feature chart;
- overview eligibility and “not ranked” explanations;
- README ratings and library entry;
- logo, links, and description in the local Web application;
- the complete diff for unrelated or deleted results.

If the library lacks one or more features required by an overview group, it
must remain visible but not ranked in that group. Do not award a favorable
ranking from an incomplete feature subset.

## Completion checklist

### LLM handoff

- [ ] The category feature contract was read completely.
- [ ] Official APIs and semantics were inspected for every feature.
- [ ] The primary package has an exact literal version.
- [ ] Package metadata, description, URLs, rating choice, and logo are complete.
- [ ] The generated `LibraryCatalog` constant is used everywhere.
- [ ] Every benchmark and availability declaration contains the explicit
      library ID.
- [ ] Every supported feature uses a direct, strongly typed hot path.
- [ ] Validation and benchmark code reuse the same scenario implementation.
- [ ] Validation-only work compiles out of benchmark builds.
- [ ] Every unsupported or non-applicable feature has an English reason.
- [ ] Both conditional variants build.
- [ ] Validation passes only for the new library.
- [ ] Partial feature report merging preserves other libraries.
- [ ] Generated metadata and Rider configurations contain the new library.
- [ ] Relevant Rider inspections are resolved or narrowly suppressed.
- [ ] No absolute machine-specific paths or unrelated changes are present.
- [ ] No benchmark was executed by the LLM.
- [ ] The user received the exact per-library update command.

### User-run result generation

- [ ] The per-library update target completes.
- [ ] Existing libraries remain present in both JSON reports.
- [ ] Environment mismatch warnings are reviewed.
- [ ] Performance and memory results are plausible and validated.
- [ ] PNG reports and README are regenerated.
- [ ] Ratings use complete feature coverage.
- [ ] Metadata URLs and logo render correctly locally and from GitHub.
- [ ] The final commit contains all intended generated artifacts.
