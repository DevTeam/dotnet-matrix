# Category roadmap

This document records the current state of dotnet-matrix categories and the
candidates for expanding it. It captures reasoning and initial scope, but it is
not a feature contract for any category. The behavior of an implemented
category is owned by `workflows/feature-contracts/<module-id>.md`.

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

## Implemented categories

Every implemented category has complete feature validation and benchmark
reports committed under `reports/<ReportDirectory>`.

Chart groups organise the overview charts. They no longer decide the rating,
which is scored across every scenario; see [rating.md](rating.md).

| Category | Module ID | Project | Libraries | Features | Chart groups |
| --- | --- | --- | ---: | ---: | --- |
| Dependency Injection | `dependency-injection` | `src/Matrix.DependencyInjection` | 23 | 15 | Basic, Advanced, Prepare |
| Object Mapping | `object-mapping` | `src/Matrix.ObjectMapping` | 4 | 10 | Basic, Advanced, Prepare |
| Validation | `validation` | `src/Matrix.Validation` | 3 | 10 | Basic, Object Graph, Rules, Prepare |
| JSON Serialization | `json-serialization` | `src/Matrix.JsonSerialization` | 3 | 14 | Basic, Nested, Collections, Advanced, Stream, Prepare |
| CSV Processing | `csv-processing` | `src/Matrix.CsvProcessing` | 3 | 10 | Read, Correctness, Throughput, Write |
| LINQ Queries | `linq-queries` | `src/Matrix.LinqQueries` | 6 | 18 | Core, Sources, Partitioning, Sequences, Advanced, Allocation-Free |
| Logging | `logging` | `src/Matrix.Logging` | 5 | 8 | Core, Structured, Prepare |
| ZIP Archives | `zip-archives` | `src/Matrix.ZipArchives` | 3 | 13 | Metadata, Read, Write, Advanced |

The planned first expansion sequence — Object Mapping, Validation, JSON
Serialization, CSV Processing, Logging — is complete. The category-neutral
execution framework in `src/Matrix`, the package-less baseline model, and
per-library partial report updates were delivered by that sequence and are now
documented in [add-category.md](add-category.md). Do not plan them again as
category work.

Adding a library to an implemented category is the cheapest contribution and
follows [add-library.md](add-library.md), not this document.

### Open items in implemented categories

- **Dependency Injection**: `Hand-coded` participates as a package-less
  baseline and is excluded from the rating through `MatrixRating`.
- **Object Mapping**: `Queryable Projection` was deliberately not implemented.
  A fair scenario requires a common LINQ provider and would otherwise measure
  more than the mapper. If it is implemented it enters the rating like every
  other scenario, so it has to be comparable before it is added.
- **Validation**: `Validot` was a candidate and remains unimplemented.
- **JSON Serialization**: `SpanJson` remains unimplemented. Binary formats stay
  out of this category by design. `BenchmarkResult.PayloadSizeBytes` is
  recorded per result in `reports/<ReportDirectory>/benchmarks.json`, but it
  does not participate in charts or ratings yet.
- **CSV Processing**: the in-memory source of `Async Read` does not model
  external I/O scheduling, so the scenario measures the cost of the
  asynchronous path itself. It is recorded as a caveat on the scenario; both it
  and `Custom Conversion` count in the rating like every other scenario.
- **Scenarios drawn in no overview group.** `DependencyInjection /
  Enumerable`, `JsonSerialization / Source Generation Round Trip` and
  `Validation / Async Validation` are absent from their `charts.json`, so they
  are rated and charted individually but appear in no group standing. This was
  how "feature-only" was actually implemented, and it does not do what that
  word promised: the rating covers every scenario either way. Decide per
  scenario whether it belongs in an existing group or whether the category
  needs another one. `generate-metadata` reports them on every run.
- **LINQ Queries**: among rated libraries, span-source support is limited to
  ZLinq and Hyperlinq, and the struct-predicate scenario is supported only by
  StructLinq. `GroupJoin`,
  `ToDictionary`, set-combination operators, `Chunk`, `Average`, and async
  variants are deliberately excluded. Hyperlinq 3.0.0-beta9 is a prerelease
  whose package omits its `Microsoft.Bcl.AsyncInterfaces` dependency;
  StructLinq 0.28.2 was last released in 2022.
- **Logging**: every scenario delivers to an equivalent in-memory sink, as
  required by its feature contract. Console, file, database, and network sinks
  remain out of scope because their I/O dominates library overhead.
- **ZIP Archives**: compressed writing compares the same deterministic corpus
  at nominal Deflate levels 1 and 6. `System.IO.Compression` exposes these as
  `Fastest` and `Optimal` rather than numeric ZIP levels. Filesystem extraction
  and asynchronous I/O remain out of scope.

## Next categories

| Priority | Category | Initial assessment |
| ---: | --- | --- |
| 1 | Binary Serialization | Useful, but payload formats and schema models differ |
| 2 | Mediator / Message Dispatch | Good local hot path, but library scope varies |
| 3 | CLI Parsing | Reproducible and infrastructure-free |
| 4 | Template Engines | Clear separation between compilation and rendering |
| 5 | Caching | Start in-memory; distributed scenarios need infrastructure |

### Binary Serialization

Candidate libraries include MessagePack for C#, MemoryPack, and protobuf-net.
Potential features include simple and nested objects, collections, schema
evolution, polymorphism, streaming, and preparation.

The report model already records `PayloadSizeBytes` per benchmark result, and
JSON Serialization populates it. What is still missing is chart and rating
participation for that metric; decide how payload size takes part in the
comparison before implementing this category. Feature groups may need to
distinguish self-describing, contract-based, and source-generated formats.
Round-trip equivalence does not by itself make different wire formats directly
interchangeable.

Note that `src/Matrix.DependencyInjection` already pins a `MessagePack`
version to resolve an advisory in a transitive dependency of
`Microsoft.VisualStudio.Composition`. That pin is not a compared library and
must not be confused with a Binary Serialization participant.

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

Decide how payload size participates in charts and ratings, then implement
Binary Serialization following [add-category.md](add-category.md). Adding
libraries to the thinnest implemented categories — CSV Processing, JSON
Serialization, and Validation, with three participants each — is a smaller
independent improvement that can happen in parallel.
