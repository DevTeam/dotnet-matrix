# JSON Serialization feature support contract

A library is `Supported` only when its direct public serialization API produces
the exact canonical JSON or reconstructs the complete expected model described
below. A successful API call or a syntactically valid JSON document is not
sufficient.

- `Supported`: the implementation exists and passes every condition below.
- `Unsupported`: the library has no intended mechanism with the required
  semantics.
- `NotApplicable`: the feature has no meaningful equivalent in the library
  model. Configuration difficulty is not a reason to use this status.
- `Failed`: support is claimed, but build, validation, or execution fails.

## Common rules

- All primary scenarios use compact JSON, PascalCase property names, ordinal
  property order, included non-null values, invariant numbers, and no comments.
- Inputs contain only ASCII text, so the UTF-8 payload size equals the canonical
  JSON character count. Every payload-bearing benchmark records
  `PayloadSizeBytes` from the same canonical JSON constant used by validation.
- Exact JSON text is required for serialization scenarios. Deserialization
  validates every model value and collection order.
- Serializer settings, converters, contracts, and type metadata are prepared
  before the measured operation unless the feature is `Prepare Serializer`.
- String serialization and deserialization are separate operations. The stream
  feature is a separate round trip because unavoidable stream adapters are part
  of that API surface.
- Benchmark methods call the compared library directly and return strongly
  typed results. A matrix-owned universal serializer interface does not
  qualify.
- The same method, model, and payload are used by feature validation and
  benchmarking.
- Chart groups compare time and allocation only after validation proves an
  identical canonical payload size. Source Generation Round Trip is drawn in no
  group and has only its own chart. A chart group decides where a scenario
  appears, never whether it counts: every scenario of the category enters the
  rating.

## 1. Serialize Simple Object

Serialize `SimpleModel(42, "Ada", true)` to the exact compact JSON:

```json
{"Id":42,"Name":"Ada","Active":true}
```

String allocation and JSON construction are measured. Cached type metadata may
be prepared before measurement.

## 2. Deserialize Simple Object

Deserialize the canonical Simple Object JSON to a new `SimpleModel`. Validate
all three members. Parsing, string access, and destination allocation are
measured.

## 3. Serialize Nested Object

Serialize one `OrderModel` containing a customer and address to the exact
`NestedJson` constant. Every nested property must be present in declaration
order.

## 4. Deserialize Nested Object

Deserialize `NestedJson` and validate the order, customer, and address members.
Every nested object must be materialized.

## 5. Serialize Collection

Serialize three ordered `SimpleModel` values to `CollectionJson`. The JSON
array and element order must match exactly.

## 6. Deserialize Collection

Deserialize `CollectionJson` to a new `SimpleModel[]` with exactly three newly
materialized elements in the prescribed order.

## 7. Serialize Dictionary

Serialize a dictionary inserted in `alpha`, `beta`, `gamma` order to
`DictionaryJson`. The exact property order and integer values are required.

## 8. Deserialize Dictionary

Deserialize `DictionaryJson` to a dictionary containing exactly the three
prescribed ordinal keys and values.

## 9. Enum Round Trip

Serialize `EnumModel` with status `Ready` as the string payload
`{"Status":"Ready"}`, then deserialize it and validate the enum value.
Integer enum output does not qualify.

## 10. Custom Converter Round Trip

Serialize `IdentifierModel` with identifier `order-42` as
`{"Id":"order-42"}`, then deserialize it and validate the strongly typed
identifier. The conversion must use the library's intended converter,
type-specific configuration, or scalar value-type convention. Matrix-owned
post-processing does not qualify.

## 11. Polymorphic Round Trip

Serialize a `ZooModel` whose `AnimalModel[]` contains a cat followed by a dog.
The canonical payload uses a `$type` discriminator with values `cat` and `dog`.
Deserialize through the base-type collection and reconstruct the correct
runtime types and derived members.

A library without an intended safe discriminator or derived-type mechanism that
can reproduce this contract is `Unsupported`. Unrestricted assembly-qualified
type loading does not qualify.

## 12. UTF-8 Stream Round Trip

Create a new in-memory stream, serialize the Simple Object directly through the
library's stream or text-writer API, rewind it, and deserialize it. The stream,
required adapters, UTF-8 encoding, flush, and both serializer calls are inside
the measured operation. The written bytes must equal `SimpleJson`.

## 13. Source Generation Round Trip

Serialize and deserialize the Simple Object with metadata generated at compile
time and validate the canonical payload and model. Runtime reflection fallback
does not qualify.

This feature is `NotApplicable` for libraries without a source-generation
programming model and is excluded from ratings.

## 14. Prepare Serializer

Create fresh settings, contract metadata, converters, or another explicit
serializer preparation object for the Simple Object without serializing data.
Preparation performed lazily by a library with no explicit public preparation
operation is represented by a zero reported benchmark; its cache lookup remains
part of ordinary steady-state serialization.

