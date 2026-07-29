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

Start with Object Mapping. Before writing its module, create
`workflows/feature-contracts/object-mapping.md` and agree on:

- exact source and destination models;
- feature names and order;
- null and collection semantics;
- setup versus measured work;
- initial library list;
- baseline behavior;
- rating groups;
- whether query projection is feature-only or deferred.

After the contract is approved, implement the category by following
[add-category.md](add-category.md). Do not copy Dependency Injection feature
semantics into the new module.
