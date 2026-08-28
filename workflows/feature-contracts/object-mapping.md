# Object Mapping feature support contract

A library is `Supported` only when its direct benchmark method passes
validation through the library's public or intended mapping API. Producing an
object of the expected type is insufficient when member values, reference
identity, collection order, null behavior, or runtime derived types differ
from this contract.

- `Supported`: the implementation exists and passes every condition below.
- `Unsupported`: the library has no intended mechanism with the required
  semantics.
- `NotApplicable`: the scenario has no meaningful equivalent for the library
  model. Poor performance or configuration difficulty is not a reason to use
  this status.
- `Failed`: an implementation is claimed, but build, validation, or runtime
  execution fails.

## Common rules

- Source and destination models are category-owned mutable classes with public
  properties and nullable reference type annotations.
- Mapping attributes must not be added to common source or destination models.
- Every runtime feature uses a preconfigured mapper. Configuration,
  registration, expression compilation, source generation, and other
  preparation are outside the measured method unless the feature name begins
  with `Prepare`.
- A benchmark method calls the compared library directly and returns a strongly
  typed result. A matrix-owned universal mapper interface does not qualify.
- A library may use its intended fluent configuration, attributes on its own
  mapper declaration, generated methods, converters, or extension points.
- A matrix-owned method that manually assigns destination members on behalf of
  a library does not qualify. The Hand-coded baseline is the only participant
  whose contract is direct member assignment.
- Every new-instance mapping must create a destination object independent from
  the source object. Mutable nested destination objects and collection elements
  must not alias their source counterparts.
- Validation and benchmarking invoke the same scenario method and inputs.

## 1. Simple Object

Map one `SimpleSource` to a new `SimpleDestination`. Copy the prescribed
integer, string, decimal, timestamp, and boolean values exactly. Every
invocation must return a new destination instance.

## 2. Nested Object

Map one `OrderSource` containing a `CustomerSource` and `AddressSource` to the
corresponding destination graph. Copy every scalar value. The order, customer,
and address destination objects must all be newly created and must not
reference their source counterparts.

## 3. Collection

Map an array of 100 distinct `SimpleSource` objects to a new
`SimpleDestination[]`. Preserve count, order, and every member value. The
destination array and every destination element must be new objects.

Materializing the result is part of the measured operation. Returning a lazy
enumerable does not satisfy the contract.

## 4. Flattening

Map an `OrderSource` to `OrderSummaryDestination`. Map the order ID and total
directly, `Customer.Name` to `CustomerName`, and
`Customer.Address.City` to `CustomerCity`.

The mapping must be configured through the library's intended flattening or
member-path mechanism. A matrix-owned wrapper that assigns the flattened
members after the library returns does not qualify.

## 5. Map To Existing

Map a `SimpleSource` into a pre-created `SimpleDestination`. Return the exact
destination instance passed to the mapping operation and overwrite all
prescribed destination members with source values.

Creating a replacement destination or mapping to a new object and then copying
it into the supplied destination does not satisfy the contract.

## 6. Null Handling

Map a `NullableSource` whose nullable text, nested address, and item collection
are all `null` to a new `NullableDestination`. All three destination members
must be `null`.

Library defaults may be changed through intended configuration. Converting a
null collection to an empty collection does not satisfy this contract.

## 7. Custom Conversion

Map `ConversionSource` to `ConversionDestination`. Convert the source string
code to `MappingCode` and the invariant decimal text to `decimal`.

The conversion must be registered or discovered through the library's intended
converter or user-mapping extension point. Post-processing the destination in
the benchmark method does not qualify.

## 8. Polymorphic Mapping

Map an `AnimalSource[]` containing `CatSource` and `DogSource` values to
`AnimalDestination[]`. Preserve order and common names. Produce
`CatDestination` with the expected `Lives` value for every cat and
`DogDestination` with the expected `GoodBoy` value for every dog.

The library must dispatch according to runtime source type through its intended
derived-type mapping mechanism. A matrix-owned type switch does not qualify,
except for the Hand-coded baseline.

## 9. Prepare Configuration

Prepare all mappings required by features 1 through 8 without performing a
mapping. For runtime-configured libraries this includes registration and eager
compilation of mapping plans when the library exposes it.

A compile-time mapper and the Hand-coded baseline have no runtime preparation.
They report zero time and allocation through `ReportedBenchmark`; zero is the
real required runtime work, not a placeholder for missing implementation.

## 10. Prepare And Simple Map

Perform the complete preparation from feature 9 and then execute one Simple
Object mapping. Runtime libraries must create a fresh configuration or mapper
inside every invocation. Compile-time and Hand-coded participants perform their
single mapping directly because they require no runtime preparation.

The returned `SimpleDestination` must satisfy the Simple Object contract.

## Deferred features

These features are intentionally outside the first category version:

- queryable projection, because a fair performance contract requires a common
  LINQ provider;
- reverse mapping, because generating two independent mappings is not
  equivalent to a native reversible configuration;
- immutable and constructor mapping;
- reference preservation and cyclic graphs;
- asynchronous mapping;
- private-member mapping.

Add a deferred feature only after defining an equivalent observable contract,
setup boundary, and rating group for all intended participants.

## Native AOT probe

`src/Matrix.ObjectMapping.Aot/Probes/<ProbeName>.cs`: map one small source
object to one destination object through the library's own configured
mapper, mirroring `SimpleObject`, and check the mapped fields. `Mapperly`'s
`[Mapper]` partial is declared right in its probe file, since its source
generator only sees the one file that compiles for that probe; `AutoMapper`'s
probe needs its own `NullLoggerFactory`, tagged as an `MatrixAotCompanion`
package on the module csproj (see `add-library.md` §3). `HandCoded` has no
probe: it exercises no library. This is a deployment capability
(`FeatureReportEntry.IsDeployment`), not a scenario: it carries no timing and
never enters the rating.
