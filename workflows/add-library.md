# Add a library

This workflow describes how to add one library to any matrix category. It is intended for both humans and LLM agents. All paths and commands recorded in the repository must be relative to the repository root. An adapter or sample outside the repository may be inspected as input, but its absolute path must never be persisted.

## LLM execution policy

An LLM must not run benchmarks. BenchmarkDotNet runs can take a long time, consume significant machine resources, and make the workstation temporarily less responsive.

An LLM may:

- build the validation and benchmark conditional variants to catch compilation errors;
- run feature validation for the new library;
- regenerate IDE run configurations;
- inspect existing reports without modifying benchmark values.

An LLM must not execute:

- `<category-prefix>-benchmarks`;
- `<category-prefix>-update-library`, because it includes benchmarks;
- `prepare-commit`, or any other aggregate target that includes benchmarks;
- the benchmark executable directly.

After feature validation succeeds, the LLM must stop and give the user the exact repository-relative per-library update command. The user decides when to run the benchmarks.

## Inputs

- Category name or category identifier.
- Library name and stable identifier.
- Optional reference adapter, sample, or benchmark implementation.
- Official documentation or repository URL.

## Discover the category

1. Locate the build project and matrix module projects from repository metadata and project files. Do not assume a fixed directory layout.
2. Select the module whose category matches the requested category.
3. Read its embedded project metadata, annotated NuGet dependencies, benchmark attributes, scenario definitions, validation code, reports, and generated run configurations.
4. Read the category feature contract completely before implementing the library. For Dependency Injection, use [feature-contracts/dependency-injection.md](feature-contracts/dependency-injection.md).
5. Identify the category-specific command prefix and the existing per-library update target from build metadata or generated run configurations. Do not hardcode category names in shared build logic.

## Register the library

1. Choose the latest stable package version compatible with the module target framework. Put the exact literal version directly on the primary `PackageReference`.
2. Add the package references needed by supported scenarios. Avoid optional integration packages unless a supported feature requires them.
3. Add matrix metadata to the primary `PackageReference`. Keep the identifier stable and use it consistently in code, reports, metadata, and run configurations.
4. Add repository-relative presentation metadata to that primary reference:
   - display name;
   - concise English description;
   - official documentation URL, or repository URL when documentation is unavailable;
   - repository URL;
   - repository-relative logo path.
6. Prefer an official logo when its license and trademark policy permit redistribution. Otherwise create a neutral project-owned mark that remains legible on light and dark backgrounds.

## Dependency Injection example

The `src/Matrix.DependencyInjection` category demonstrates all integration points. Its Simple Injector integration is the concrete reference below. Use source-safe names for generated constants and directories; these do not have to be identical to the display name.

### Project and discovery metadata

Every category project defines its module identity and imports the shared module target:

```xml
<PropertyGroup>
    <MatrixModuleId>dependency-injection</MatrixModuleId>
    <MatrixModuleName>Dependency Injection</MatrixModuleName>
    <MatrixRunConfigurationPrefix>DI</MatrixRunConfigurationPrefix>
    <MatrixReportDirectory>DependencyInjection</MatrixReportDirectory>
    <MatrixLibraryCatalogNamespace>Matrix.DependencyInjection.Infrastructure</MatrixLibraryCatalogNamespace>
</PropertyGroup>

<Import Project="..\Matrix\Matrix.Module.targets"/>
```

The shared target embeds the project as `Matrix.Project.csproj`, validates required metadata, and generates `LibraryCatalog.g.cs`. A new category must use the same contract; an existing category already has it.

Update `src/Matrix.DependencyInjection/Matrix.DependencyInjection.csproj`:

1. Add the primary package with an exact literal version and all matrix metadata.
2. Add any integration packages as ordinary, unannotated references.
3. Do not add version properties, assembly attributes, or a manual catalog constant.

   ```xml
   <PackageReference Include="SimpleInjector" Version="5.6.0">
       <MatrixLibraryId>SimpleInjector</MatrixLibraryId>
       <MatrixLibraryName>Simple Injector</MatrixLibraryName>
       <MatrixCodeName>SimpleInjector</MatrixCodeName>
       <MatrixDescription>A fast, opinionated dependency injection library that promotes explicit configuration and maintainable application design.</MatrixDescription>
       <MatrixDocumentationUrl>https://docs.simpleinjector.org/</MatrixDocumentationUrl>
       <MatrixRepositoryUrl>https://github.com/simpleinjector/SimpleInjector</MatrixRepositoryUrl>
       <MatrixLogo>logos/simple-injector.svg</MatrixLogo>
   </PackageReference>
   ```

The project is embedded into its assembly as `Matrix.Project.csproj`. Runtime discovery reads the module properties and annotated package references directly from that resource. `MatrixCodeName` generates the compile-time catalog constant, such as `LibraryCatalog.SimpleInjector`.

The primary matrix package must not use an MSBuild property, version range, wildcard, or conditional `PackageReference`. Additional packages do not receive `MatrixLibraryId`.

### Benchmark implementation

Create a source-safe library directory such as `src/Matrix.DependencyInjection/Benchmarks/SimpleInjector/`. Add one file per supported feature, following the existing numeric names such as `01_Singleton.cs`, `02_Transient.cs`, and `09_IEnumerable.cs`.

Each file must:

- extend the corresponding partial benchmark class from `Benchmarks/Common`;
- configure the container in the feature-specific setup method;
- dispose it in cleanup when required;
- use the container's direct resolve API inside the measured method;
- annotate the measured method with its catalog constant, for example `[LibraryBenchmark(LibraryCatalog.SimpleInjector)]`;
- call the shared conditional `Validate(...)` method before returning;
- include `// ReSharper disable CheckNamespace` and `// ReSharper disable InconsistentNaming` where applicable.

Do not copy scenario models into the library directory. Shared contracts and object graphs live in `src/Matrix.DependencyInjection/Scenarios`. Shared conditional checks live in `src/Matrix.DependencyInjection/Validation` and are exposed by the partial classes in `src/Matrix.DependencyInjection/Benchmarks/Common`.

If validation needs a stronger behavioral assertion, improve the shared validation so every supporting library is checked against the same contract. Validation-only counters, state, and calls must be removed from benchmark builds through conditional compilation or `[Conditional("MATRIX_VALIDATION")]`.

For each unsupported or non-applicable feature, add a `FeatureUnavailable` entry to its file under `src/Matrix.DependencyInjection/Benchmarks/Common`. Include the library catalog constant, status, and a concise English reason. Do not add an empty library-specific benchmark file.

### Presentation metadata

Presentation metadata is mandatory and belongs to the primary `PackageReference`; do not edit `metadata/DependencyInjection/libraries.json` manually. Generate it with:

```text
dotnet run --project build/build.csproj -- generate-metadata
```

Requirements:

- `MatrixLibraryId` is the stable report and filter identifier;
- `MatrixLibraryName` is the display name;
- `MatrixCodeName` must be a unique valid C# identifier and generates the `LibraryCatalog` constant;
- `MatrixDescription` must be short, factual, and written in English;
- `MatrixDocumentationUrl` should point to official documentation and may be omitted when the repository is the best documentation;
- `MatrixRepositoryUrl` must point to the canonical source repository;
- `MatrixLogo` must be a repository-relative path below `metadata/DependencyInjection`;
- add the referenced image to `metadata/DependencyInjection/logos/`;
- verify the logo on both light and dark backgrounds;
- do not add presentation metadata to generated JSON reports.

Do not edit `metadata/DependencyInjection/libraries.json`, `reports/DependencyInjection/features.json`, `reports/DependencyInjection/benchmarks.json`, PNG charts, or `README.md` manually. Build targets regenerate those files.

### Dependency Injection validation

An LLM may run only the new library's feature validation:

```text
dotnet run --project build/build.csproj -- dependency-injection-validate --libraries <LibraryId>
```

It must inspect `reports/DependencyInjection/features.json` and confirm that every feature has the expected status and that every claimed supported feature passed its executable contract.

## Implement and validate features

For each feature in the category contract:

1. Decide `Supported`, `Unsupported`, `NotApplicable`, or `Failed` from the contract, not from the presence of a similarly named API.
2. Implement a supported feature with the library's direct API in its own library-specific benchmark file.
3. Keep the measured method free from universal adapters, reflection, delegates introduced by the matrix, boxing, and validation overhead. Setup-only reflection or callbacks are acceptable when the library itself requires them.
4. Reuse the scenario models and conditional validation invoked by the benchmark method. Validation state and calls must compile out of benchmark builds.
5. Declare unsupported or non-applicable features explicitly in category availability metadata with a concise English reason. Never infer support from a missing method.
6. Do not emulate a missing native lifetime or container capability with matrix-owned factories merely to mark the feature supported.
7. Build both conditional variants:

   ```text
   dotnet build <module-project> -c Release -p:MatrixMode=Validation
   dotnet build <module-project> -c Release -p:MatrixMode=Benchmark
   ```

8. Run validation for the new library and fix every failed contract. An LLM stops after validation; a human runs benchmarks later.

## Generate results - user action

The following steps are performed by the user, not by an LLM.

1. When convenient, run the discovered per-library update target:

   ```text
   dotnet run --project <build-project> -- <category-prefix>-update-library --library <library-id>
   ```

   The target must validate the selected library, benchmark only that library, merge only its JSON entries, and regenerate all category PNG reports and the repository README.

2. Regenerate run configurations. An LLM may run this fast target before handoff if needed:

   ```text
   dotnet run --project <build-project> -- generate-run-configurations
   ```

3. Confirm that the new library has generated configurations for validation, benchmarking, and the complete per-library update workflow.

## Completion checklist

### LLM handoff

- Both module variants build without warnings.
- Every supported feature passes its executable contract.
- Every unsupported or non-applicable feature has an explicit reason.
- The benchmark hot path uses direct library APIs.
- Required library metadata and the logo are present and consistent.
- Generated validation, benchmark, and update run configurations include the new library.
- The user receives the exact per-library update command.
- No benchmark was executed by the LLM.
- `git diff` contains no absolute machine-specific paths or unrelated changes.

### User-run result generation

- Feature and benchmark JSON contain the new library without removing unrelated libraries.
- Environment mismatch warnings are reviewed when results are merged after a partial run.
- All category PNG reports and the README are regenerated.
- Metadata URLs and logo render correctly in the local application.

If any executable contract fails, report the feature as `Failed` while diagnosing it. Do not publish benchmark numbers for a feature whose validation does not pass.
