# Binary Serialization feature contract

A library is `Supported` only when its own public binary API produces a payload
that its own reader reconstructs into the complete expected object graph. A
successful API call that returns bytes is not sufficient evidence on its own.

- `Supported`: the implementation exists and passes every assertion below.
- `Unsupported`: the library has no intended mechanism with the required
  semantics for this scenario.
- `NotApplicable`: the scenario has no meaningful equivalent in the library
  model. Configuration difficulty is not a reason to use this status.
- `Failed`: support is claimed, but build, validation, or execution fails.

## Comparability

1. The measured operation is the same in every row: encode this object graph
   into bytes that the same library reads back into an equivalent graph.
2. Compared quantities are the time and the allocated bytes of that operation.
   `PayloadSizeBytes` is a property of the chosen format, not a measure of
   quality; it is reported and never enters the rating.
3. These formats share no bytes on the wire, so a payload size is comparable
   only inside one format. Differences in capability are visible in the
   `Scenarios` column and in `Unsupported` rows, never as a scoring bonus.

A category of binary serializers has no canonical payload in the sense that the
JSON and CSV categories have one. Each library round-trips its own
representation, so time and allocations are comparable across the category
while wire bytes are not. This is a recorded limit of the category scope, not a
defect of the rating: comparing one operation across independent formats is
exactly what the category exists to measure.

## Common rules

- Scenario inputs are the canonical instances produced by `SerializationData`:
  `Simple()`, `Nested()`, `Collection()`, and `Enum()`. They are created once
  before measurement.
- A schema-first library compares its own generated or schema-bound model, built
  once from the same canonical values in setup. Its API surface has no
  equivalent of a plain POCO, so the plain POCO is not part of its measured
  operation.
- Contracts, schemas, generated serializers, and any explicit warm-up run are
  prepared outside the measured method.
- Deserialization reads a payload that the same library produced from that
  canonical instance. Creating the destination and parsing are measured.
- Benchmark methods call the compared library directly and return strongly
  typed results. A matrix-owned universal serializer interface does not
  qualify.
- Every returned value is validated against the expected graph through
  `SerializationChecks` with the explicit library id.

## 1. Serialize Simple Object

Serialize `SimpleModel(42, Ada, true)` through the library API in its default
configuration and return the produced payload bytes. The payload must be
non-empty. Serializer construction, schema resolution, and generated-code
warm-up happen in setup.

## 2. Deserialize Simple Object

Deserialize the payload that the same library produced from the simple object in
setup and return a new `SimpleModel`. Validate all three members. Parsing, string
materialization, and destination allocation are measured.

## 3. Serialize Nested Object

Serialize one `OrderModel` containing a customer and an address and return the
payload bytes. Every nested member must be written: a library that silently
drops a nested value cannot satisfy the validation of its own read-back and is
`Unsupported` for this scenario.

## 4. Deserialize Nested Object

Deserialize the library's own nested payload and validate the order id, the
customer name, and both address members. Every nested object must be
materialized.

## 5. Serialize Collection

Serialize `CollectionModel`, whose single `Items` member holds three ordered
`SimpleModel` values, as one payload and return the payload bytes.

The collection is wrapped in an object rather than written as a root array.
BSON forbids an array at the root of a document and the Avro reflection writer
requires a record at the root, so a root array would measure which formats
accept that shape rather than how they encode a collection. Every library
therefore encodes the same wrapper with the same three nested objects, and the
nested elements stay a real part of the measured graph.

## 6. Deserialize Collection

Deserialize the library's own `CollectionModel` payload and validate that `Items`
holds exactly three elements in the prescribed order with every member intact.

## 7. Enum Round Trip

Encode `EnumModel` with status `Ready` through the library API, read it back, and
return the reconstructed model. The enum value must survive as a value; its wire
representation (ordinal, symbol, or generated numeric constant) is the format's
own choice and is not compared across libraries.

## Common validation and availability

- Serialization produces a non-empty payload from the exact canonical input.
- Deserialization and the round trip reconstruct every member of the canonical
  instance, including nested objects and collection order.
- `Supported` means the library performs the exact operation through an intended
  binary API and passes every assertion.
- `Unsupported` means the required capability is absent from the library. The
  declared reason names the missing capability and the observation that showed it.
  `HyperSerializer` is `Unsupported` for both nested scenarios and both collection
  scenarios: its generated reader and writer cover scalar members only, so a class
  property or a collection member produces no encoded data.
- `NotApplicable` is not used in this category: every scenario has a meaningful
  binary equivalent.
- Every scenario participates in the category rating. Unsupported scenarios
  receive no points under the shared rating rule.

## Chart groups

Groups collect scenarios that answer the same question about the same operation:

- `serialize`: scenarios 1, 3, and 5.
- `deserialize`: scenarios 2, 4, and 6.
- `roundtrip`: scenario 7.

A library is ranked inside a group only when it supports every scenario of that
group, which is the shared group rule. Groups drive overview charts and medals;
the category rating still counts every rated scenario.

## Payload size

The shared `PayloadSize` attribute encodes a compile-time UTF-8 string, so it
cannot carry a size that each library produces at runtime in its own format.
`PayloadSizeBytes` therefore stays unset for this category and is not used as a
tiebreaker or a score component.

## Deferred scenarios

Stream-based round trips, schema evolution between two contract revisions,
polymorphism, and explicit serializer preparation are deferred. They need either
a second contract revision per library or an API surface that only some formats
have, and adding them would compare library features rather than the encoding
operation this category measures.
