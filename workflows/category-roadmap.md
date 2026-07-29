# Category roadmap

This document records candidate categories for expanding dotnet-matrix beyond
Dependency Injection. It captures the reasoning and initial scope, but it is
not a feature contract for any category.

Before implementing a category:

1. Re-evaluate its libraries and current package versions.
2. Design and approve the English feature contract.
3. Follow [add-category.md](add-category.md).
4. Keep category-specific semantics inside its `Matrix.<Category>` project.

## Context

dotnet-matrix compares both feature support and runtime behavior. A suitable
category therefore needs:

- several independently useful .NET libraries;
- common operations with equivalent observable semantics;
- deterministic validation of every claimed feature;
- direct, strongly typed benchmark hot paths;
- meaningful time and allocation measurements;
- little or no external infrastructure;
- an explicit separation between preparation and measured work;
- clear rules for unsupported and non-applicable features.

Popularity alone is not sufficient. Categories in which libraries operate at
different abstraction levels, use incompatible output formats, or depend on
external services require additional care and should be implemented later.

## Current state

- Dependency Injection is implemented in `src/Matrix.DependencyInjection`.
- Object Mapping is implemented in `src/Matrix.ObjectMapping`.
- Validation is implemented in `src/Matrix.Validation`; its feature validation
  passes, and benchmark reports still need to be produced.

## Recommended order

| Priority | Category | Initial assessment |
| ---: | --- | --- |
| 1 | Object Mapping | Best next category; deterministic and inexpensive to implement |
| 2 | Validation | Strong common feature set and straightforward semantic checks |
| 3 | JSON Serialization | Popular and measurable, provided it remains separate from binary formats |
| 4 | CSV Processing | Deterministic streaming and materialization workloads |
| 5 | Logging | Valuable feature matrix, but sink cost must be isolated |
| 6 | Binary Serialization | Useful, but payload formats and schema models differ |
| 7 | Mediator / Message Dispatch | Good local hot path, but library scope varies |
| 8 | CLI Parsing | Reproducible and infrastructure-free |
| 9 | Template Engines | Clear separation between compilation and rendering |
| 10 | Caching | Start in-memory; distributed scenarios need infrastructure |

The recommended first expansion sequence is:

1. Object Mapping
2. Validation
3. JSON Serialization
4. CSV Processing
5. Logging

This sequence should exercise the shared category architecture with
progressively more difficult contracts before introducing external services or
incompatible data formats.

## 1. Object Mapping

### Proposed identity

| Item | Value |
| --- | --- |
| Project | `Matrix.ObjectMapping` |
| Module ID | `object-mapping` |
| Display name | `Object Mapping` |
| Run configuration prefix | `Mapping` |
| Report directory | `ObjectMapping` |

### Implementation sequence

#### 1. Remove category infrastructure duplication

Before defining or implementing Object Mapping, extract the category-neutral
execution framework from `src/Matrix.DependencyInjection` into `src/Matrix`.
Object Mapping must consume the extracted implementation rather than copy the
DI runners and attributes.

Move or generalize:

- `Application`;
- `FeatureValidationRunner`;
- `BenchmarkRun`;
- `LibraryFilter`;
- `FeatureStatus`;
- `FeatureUnavailableAttribute`;
- `LibraryBenchmarkAttribute`;
- `ReportedBenchmarkAttribute`;
- the category-neutral `Require`, `Same`, and `Different` validation helpers.

During the extraction:

- put every named class, interface, record, enum, delegate, and attribute in a
  separate file named after that type;
- access application and infrastructure services through interfaces and
  constructor injection;
- bind implementations in Pure.DI instead of constructing them inside other
  services;
- do not introduce service interfaces for data records, attributes, pure
  helpers, or scenario models;
- keep direct library calls in benchmark methods, without a universal mapping
  interface or DI resolution in the measured hot path.

The shared runners must receive the module assembly explicitly. They currently
discover features through `typeof(FeatureValidationRunner).Assembly` and
`typeof(BenchmarkRun).Assembly`; after moving to `Matrix`, those expressions
would inspect the shared assembly instead of the category assembly.

Remove these DI-specific assumptions from the shared benchmark runner:

- `typeof(Singleton).Assembly`;
- `LibraryCatalog.HandCoded`;
- construction of a hard-coded `Hand-coded` `BenchmarkLibrary`;
- namespace references to `Matrix.DependencyInjection`.

Keep in `Matrix.DependencyInjection`:

- the DI `FeatureId` enum;
- DI scenario models and roots;
- DI-specific validation such as lifetime, disposal, property injection, and
  lazy collection checks;
- Pure.DI benchmark compositions and their shared `DefaultComposition`;
- every direct library benchmark method;
- category-specific unavailable reasons and policies.

The extraction is complete only when Dependency Injection still builds and
validates through the shared runners, and the Object Mapping project needs no
copied runner, filter, report merge, or reflection-discovery code.

#### 2. Define the feature contract

Create `workflows/feature-contracts/object-mapping.md` and approve the exact
models, inputs, outputs, support conditions, and setup boundaries before
writing library implementations.

#### 3. Add declarative baseline support

Implement the package-less baseline model described below before adding the
Object Mapping hand-coded implementation.

#### 4. Scaffold and implement the category

Follow [add-category.md](add-category.md), reuse the extracted shared execution
framework, implement each library through direct strongly typed hot paths, and
run validation without running benchmarks.

### Why it should be first

Object mapping has deterministic inputs and outputs, requires no external
infrastructure, and supports a meaningful hand-coded baseline. It also exposes
an important architectural distinction between runtime-configured and
source-generated libraries.

### Candidate libraries

- AutoMapper
- Mapster
- Mapperly
- a hand-coded implementation as an explicit baseline

Other candidates should be added only after checking maintenance status,
licensing, package identity, and support for the approved feature contract.

### Baseline representation

#### What is wrong now

The current metadata contract treats a compared library as an annotated
`PackageReference`. This works for NuGet libraries but cannot describe a
hand-coded implementation, because it has no package or package version.

Dependency Injection works around that limitation in several disconnected
places:

- `LibraryCatalog.HandCoded` is manually declared in the DI project;
- `BenchmarkRun` manually prepends a `BenchmarkLibrary` named `Hand-coded`;
- benchmark methods mark it with `Baseline = true`;
- it appears in `reports/DependencyInjection/benchmarks.json`;
- it does not appear in the module `MatrixLibrary` collection;
- it does not appear in `metadata/DependencyInjection/libraries.json`;
- `FeatureValidationRunner` iterates only module libraries, so it does not
  validate Hand-coded feature coverage;
- metadata-driven filtering, descriptions, logos, URLs, rating policy, and
  generated catalog behavior therefore do not have one source of truth.

Copying this workaround into Object Mapping would create another category-owned
special case and make the shared runners only nominally shared.

#### Proposed model

Allow a module to declare both package-backed libraries and package-less
baselines through one metadata pipeline. A baseline should have:

- stable ID;
- display name;
- generated code name;
- description;
- optional documentation or repository URL;
- logo;
- `Baseline` flag;
- `Rated` flag;
- optional package and version.

Package and version remain required for ordinary libraries and absent for a
package-less baseline. `Baseline` and `Rated` are separate concepts: baseline
controls reference presentation and benchmark behavior, while `Rated`
controls medal eligibility.

A possible project declaration is:

```xml
<ItemGroup>
  <MatrixLibrary Include="HandCoded">
    <MatrixLibraryName>Hand-coded</MatrixLibraryName>
    <MatrixCodeName>HandCoded</MatrixCodeName>
    <MatrixDescription>Direct object mapping written in C#.</MatrixDescription>
    <MatrixLogo>logos/hand-coded.svg</MatrixLogo>
    <MatrixBaseline>true</MatrixBaseline>
    <MatrixRating>false</MatrixRating>
  </MatrixLibrary>
</ItemGroup>
```

`MatrixLibrary Include="HandCoded"` is intentionally not connected to a NuGet
package. It declares a package-less participant implemented by source code in
the category project.

Package-backed libraries remain real `PackageReference` items with matrix
metadata:

```xml
<PackageReference Include="AutoMapper" Version="16.2.0">
  <MatrixLibraryId>AutoMapper</MatrixLibraryId>
  <MatrixLibraryName>AutoMapper</MatrixLibraryName>
  <MatrixCodeName>AutoMapper</MatrixCodeName>
  <MatrixDescription>A convention-based object-object mapper.</MatrixDescription>
  <MatrixDocumentationUrl>https://docs.automapper.io/</MatrixDocumentationUrl>
  <MatrixRepositoryUrl>https://github.com/LuckyPennySoftware/AutoMapper</MatrixRepositoryUrl>
  <MatrixLogo>logos/auto-mapper.png</MatrixLogo>
</PackageReference>
```

The metadata reader and catalog generator normalize annotated
`PackageReference` items and package-less `MatrixLibrary` items into the same
`MatrixLibrary` model. Keeping the package link on `PackageReference` avoids
duplicating a package ID or version in a second item and prevents restore
metadata from drifting away from matrix metadata.

The exact MSBuild item name is an implementation detail, but it must feed the
same generated catalog and metadata validation as annotated package
references. Do not invent a fake NuGet package or fake version.

#### Required shared changes

1. Make `MatrixLibrary.Package` and `MatrixLibrary.Version` nullable.
2. Add `Baseline` to the module library definition or its unified metadata.
3. Extend `Matrix.Module.targets` to generate catalog constants for both
   package-backed and package-less declarations.
4. Extend `MatrixMetadata` to read, validate, and deduplicate both forms.
5. Require package and version only for package-backed libraries.
6. Generate the baseline into `libraries.json` with its description, logo,
   baseline status, and `rated: false`.
7. Make filtering and validation iterate the same complete module catalog.
8. Make the shared benchmark runner derive `BenchmarkLibrary` records from
   module metadata instead of prepending `HandCoded`.
9. Preserve baselines correctly during partial report updates without
   hard-coded IDs.
10. Migrate the DI hand-coded declaration to the new model and delete its
    manual catalog constant and runner special case.

#### Expected result

AutoMapper, Mapster, Mapperly, and Hand-coded will all be first-class module
participants. The baseline will be validated, filterable, described in
metadata, shown in feature and benchmark views, and excluded from medals by
declaration rather than convention.

### Candidate features

1. Simple Object
2. Nested Object
3. Collection
4. Flattening
5. Map To Existing
6. Null Handling
7. Custom Conversion
8. Polymorphic Mapping
9. Prepare Configuration

`Queryable Projection` should initially be feature-only or placed in a
separate non-rated group. A fair performance scenario requires a common LINQ
provider and would otherwise measure more than the mapper.

### Benchmark design

- Measure configuration or generated mapper preparation separately.
- Measure steady-state mapping after all allowed preparation.
- Use identical source data and validate every destination member.
- Separate single-object and collection workloads.
- Record execution time and allocations.
- Do not hide reflection, compilation, or caching inside global setup if the
  corresponding feature contract says that work is measured.

## 2. Validation

### Proposed identity

| Item | Value |
| --- | --- |
| Project | `Matrix.Validation` |
| Module ID | `validation` |
| Display name | `Validation` |
| Run configuration prefix | `Validation` |
| Report directory | `Validation` |

### Why it is a strong early category

Validation libraries share understandable semantics, and both successful and
failed results can be checked precisely. The main contract decision is whether
the scenario requires the first failure or the complete error set.

### Candidate libraries

- FluentValidation
- Validot
- MiniValidation
- DataAnnotations as a framework baseline

The final list must be refreshed immediately before implementation.

### Candidate features

1. Valid Object
2. Single Failure
3. Multiple Failures
4. Nested Object
5. Collection
6. Conditional Rule
7. Custom Rule
8. Stop On First Failure
9. Async Validation
10. Prepare Validator

### Benchmark design

- Use separate valid, first-failure, and many-failure inputs.
- Validate the exact property paths, error count, and rule outcomes.
- Keep synchronous features in the primary rating groups.
- Treat asynchronous validation as a feature first; add it to ratings only if
  the contract avoids measuring artificial scheduling or no-op async work.
- Measure rule graph construction separately from repeated validation.

## 3. JSON Serialization

### Proposed identity

| Item | Value |
| --- | --- |
| Project | `Matrix.JsonSerialization` |
| Module ID | `json-serialization` |
| Display name | `JSON Serialization` |
| Run configuration prefix | `JSON` |
| Report directory | `JsonSerialization` |

### Scope boundary

This category must contain only JSON serializers. MessagePack, Protobuf,
MemoryPack, and other binary formats belong in a separate category. Combining
them would produce a misleading rating because wire format, schema, payload
size, and supported type semantics differ.

### Candidate libraries

- System.Text.Json as the framework baseline
- Newtonsoft.Json
- SpanJson
- ServiceStack.Text
- other maintained JSON serializers that can satisfy the same contract

### Candidate features

1. Serialize Simple Object
2. Deserialize Simple Object
3. Serialize Nested Object
4. Deserialize Nested Object
5. Collection
6. Dictionary
7. Enum
8. Polymorphism
9. Custom Converter
10. UTF-8 Stream
11. Source Generation
12. Prepare Serializer

### Benchmark design

- Use a canonical semantic model and validate round trips.
- Define naming, null, enum, number, and polymorphism policies explicitly.
- Benchmark serialization and deserialization separately.
- Keep string, byte array, stream, and `IBufferWriter<byte>` APIs in separate
  scenarios when their unavoidable costs differ.
- Add encoded payload size to the report model before using it in ratings; do
  not treat speed alone as the complete serializer result.

## 4. CSV Processing

### Proposed identity

| Item | Value |
| --- | --- |
| Project | `Matrix.CsvProcessing` |
| Module ID | `csv-processing` |
| Display name | `CSV Processing` |
| Run configuration prefix | `CSV` |
| Report directory | `CsvProcessing` |

### Why it is suitable

CSV parsing and writing are deterministic, work without external services, and
offer both streaming and materialized APIs. A fixed corpus can cover common
correctness traps as well as throughput.

### Candidate features

1. Read Simple Rows
2. Read Typed Records
3. Read Large Dataset
4. Quoted Fields
5. Escaped Delimiters
6. Header Mapping
7. Custom Conversion
8. Streaming Read
9. Write Rows
10. Async Read

### Benchmark design

- Store canonical UTF-8 input fixtures in the category project.
- Validate field values, row counts, culture, quoting, and newline behavior.
- Separate parsing from materialization.
- Separate synchronous and asynchronous scenarios.
- Use both a small correctness-oriented corpus and a larger throughput corpus.

## 5. Logging

### Proposed identity

| Item | Value |
| --- | --- |
| Project | `Matrix.Logging` |
| Module ID | `logging` |
| Display name | `Logging` |
| Run configuration prefix | `Logging` |
| Report directory | `Logging` |

### Candidate libraries

- Microsoft.Extensions.Logging as the framework abstraction or baseline
- Serilog
- NLog
- log4net
- ZLogger

### Candidate features

1. Disabled Log
2. Simple Message
3. Structured Properties
4. Exception
5. Scope Or Context
6. Template Rendering
7. Async Or Buffered Logging
8. Prepare Logger

### Benchmark design

- Use an equivalent null or in-memory sink for primary ratings.
- Verify the emitted level, message template, properties, exception, and
  context rather than only counting calls.
- Separate disabled and enabled paths.
- Keep console, file, database, and network sinks out of the primary rating
  groups because their I/O dominates library overhead.
- Define whether flushing is inside or outside the measured operation.
- Do not compare buffered enqueue with synchronous durable write as though they
  had equivalent completion semantics.

## Second-wave categories

### Binary Serialization

Candidate libraries include MessagePack for C#, MemoryPack, and protobuf-net.
Potential features include simple and nested objects, collections, schema
evolution, polymorphism, streaming, and preparation.

Before implementation, extend reporting so payload size can participate in the
comparison. Feature groups may need to distinguish self-describing,
contract-based, and source-generated formats. Round-trip equivalence does not
by itself make different wire formats directly interchangeable.

### Mediator / Message Dispatch

Potential features:

- request with one handler;
- result-returning request;
- notification with multiple handlers;
- pipeline behavior;
- exception behavior;
- async handler;
- source-generated dispatch;
- preparation or registration.

Avoid comparing a small mediator with a full application or messaging
framework unless the contract isolates genuinely equivalent in-process work.

### CLI Parsing

Potential features:

- option;
- positional argument;
- multiple option values;
- subcommand;
- validation;
- generated help;
- binding to a typed model;
- parse failure.

This category is deterministic, but feature validation must cover error
semantics as well as successful parsing.

### Template Engines

Potential features:

- parse or compile;
- render plain values;
- property access;
- conditions;
- loops;
- nested templates;
- escaping;
- cached render.

Compilation and cached rendering must be separate rating groups.

### Caching

Begin with in-memory behavior:

- hit;
- miss;
- get or create;
- expiration;
- concurrent get or create;
- stampede protection;
- fail-safe value;
- invalidation.

Distributed cache, serialization, backplane, and Redis scenarios should be
separate because they introduce environmental noise and deployment
requirements.

## Later or higher-risk categories

### HTTP Clients

Possible libraries include declarative and fluent clients. A reproducible
benchmark requires a local loopback server, fixed responses, connection reuse,
and explicit separation of request construction from network transport.

### Data Access / ORM

This is attractive but should be delayed. EF Core, Dapper, LINQ to DB, RepoDB,
and NHibernate operate at different abstraction levels. A local SQLite database
can make execution repeatable, but the contract must decide whether it measures
SQL construction, materialization, change tracking, identity maps, or raw data
access. A single overall rating would likely be misleading.

### Resilience

Potential features include retry, timeout, circuit breaker, fallback, hedging,
rate limiting, and composed pipelines. The contract is technically strong, but
the category should proceed only if enough independent, maintained libraries
provide equivalent strategies. Otherwise it risks becoming a comparison of one
dominant library with framework wrappers around the same implementation.

## Cross-category decisions

Before starting any category, answer these questions explicitly:

1. What observable result makes each feature supported?
2. Which preparation is allowed before the measured operation?
3. Which features are comparable enough to participate in ratings?
4. Does a hand-coded or framework baseline have meaningful semantics?
5. Are libraries at the same abstraction level?
6. Does the scenario require external infrastructure?
7. Does another metric, such as payload size or output durability, matter in
   addition to time and allocations?
8. Can every feature be validated without executing benchmarks?
9. Are async and sync operations truly comparable?
10. Which unsupported cases are `Unsupported`, and which are genuinely
    `NotApplicable`?

## Next action

Produce the Validation benchmark report, then regenerate its charts, README
content, and Web data through the shared workflow. After Validation is
complete, JSON Serialization is the next recommended category.
