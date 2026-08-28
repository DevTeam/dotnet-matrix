# LINQ Queries feature contract

## Scope

The category compares LINQ-to-Objects and LINQ-shaped value-enumerable
implementations over deterministic in-memory fixtures.

**Every scenario ends in a terminal operation and returns a concrete value.** A
lazy query or library-specific enumerable is not a valid result because that
would measure construction rather than execution. **Scenarios 1-17 use ordinary
non-capturing static lambdas for every library.** Struct predicates are used only
by scenario 18 and only through a documented non-delegate function API.

Expected values are computed by hand-written loops, never by `System.Linq`.
Fixtures are created once by `QueryData` outside measured invocations and are
held by benchmark instances without copying. The hand-coded baseline performs
the same predicates, projections, traversal, and materialization as the query
libraries; it may pre-size output only when the count is genuinely known.

`Numbers` and `NumberList` contain 10,000 values defined by `(i * 37) % 1000`.
`ScanNumbers` contains 10,000 values defined by `(i * 37) % 500`, except that
index 9,500 is the only value at least 700. `Batches` contains 500 arrays of 20
values. `Orders` contains 5,000 deterministic orders, with the order at index
4,500 assigned the unique amount 10,000. `Customers` contains 500 matching
customers. `Opaque()` yields the number fixture through a C# iterator and
exposes no count, indexer, or collection interface. Its one iterator allocation
per enumeration is a shared source cost, not query-library overhead.

Setup, fixture construction, expected-result construction, and validation are
excluded from measurement. Validation is conditional and absent from benchmark
builds.

## 1. Filter and Count

- Operation: filter `Numbers` by `n % 3 == 0` and count survivors.
- Input/output: `int[10_000]` to `int`.
- Inside invocation: query construction, full enumeration, predicate calls, and
  terminal count.
- Validation: exact equality with the loop-computed count.
- Supported: the library executes `Where` and terminal `Count` with a static
  lambda and returns the exact count.
- Group: `core`.

## 2. Project To Array

- Operation: project every `Numbers` value with `n * 2` and materialize it.
- Input/output: `int[10_000]` to `int[10_000]`.
- Inside invocation: selector calls, allocation, and array materialization.
- Validation: length, first and last value, and checksum.
- Supported: the library executes `Select` and `ToArray` and returns the exact
  projection.
- Group: `core`.

## 3. Filter, Project, Materialize

- Operation: keep orders whose amount is greater than 2,500, project `Id`, and
  materialize a list.
- Input/output: `Order[5_000]` to `List<int>`.
- Inside invocation: filtering, projection, list allocation and growth.
- Validation: exact count, first and last ID, and checksum.
- Supported: the library performs `Where`, `Select`, and `ToList` and returns the
  exact ordered list.
- Group: `core`.

## 4. Chained Pipeline

- Operation: filter `Numbers` by divisibility by three, multiply by two, filter
  the projected values by divisibility by four, take 1,000, and materialize.
- Input/output: `int[10_000]` to `int[]`.
- Inside invocation: all four query stages and terminal materialization.
- Validation: length, first and last value, and checksum.
- Supported: all stages remain one library query and produce the exact array.
- Group: `core`.

## 5. List Source

- Operation: run the scenario-1 filter followed by the scenario-2 projection
  over `NumberList`, then materialize.
- Input/output: `List<int>` with 10,000 values to `int[]`.
- Inside invocation: list-source enumeration, filter, projection, and `ToArray`.
- Validation: length, first and last value, and checksum.
- Supported: the library has an intended list entry point and returns the exact
  materialized result.
- Group: `sources`.

## 6. Opaque Source

- Operation: run the same filter and projection over a fresh `Opaque()`
  `IEnumerable<int>` and materialize.
- Input/output: opaque 10,000-element `IEnumerable<int>` to `int[]`.
- Inside invocation: iterator creation, enumeration, query stages, and `ToArray`.
- Validation: the same result as List Source.
- Supported: the library accepts a general enumerable without converting or
  copying it during setup and returns the exact result.
- Group: `sources`.

## 7. Span Source

- Operation: run the same filter and projection over a `ReadOnlySpan<int>` and
  materialize.
- Input/output: 10,000-element `ReadOnlySpan<int>` to `int[]`.
- Inside invocation: span query stages and `ToArray`.
- Validation: the same result as List Source.
- Supported: the library exposes a direct span query surface; converting the
  span to an array or enumerable is not support.
- Unsupported: no operator entry point accepts `ReadOnlySpan<int>`.
- Group: `sources`.

## 8. Paged Slice

- Operation: skip 4,000 `Numbers`, take 1,000, and materialize.
- Input/output: `int[10_000]` to `int[1_000]`.
- Inside invocation: `Skip`, `Take`, and `ToArray`.
- Validation: exact length, first and last value, and checksum.
- Supported: the library performs the partitioning operations and returns the
  exact slice.
- Group: `partitioning`.

## 9. Any Match

- Operation: test `ScanNumbers` for `n >= 700`; the only match is at index 9,500.
- Input/output: `int[10_000]` to `bool`.
- Inside invocation: predicate evaluation and short-circuiting `Any`.
- Validation: result is `true`.
- Supported: the library's terminal `Any` returns the exact result.
- Group: `partitioning`.

## 10. First Match

- Operation: find the first order with `Amount >= 10_000`, at index 4,500.
- Input/output: `Order[5_000]` to `Order`.
- Inside invocation: predicate evaluation and short-circuiting first-element
  selection.
- Validation: `Id == 4_501` and `Amount == 10_000`.
- Supported: the library returns that exact existing order through `First` or
  an equivalent throwing first-match terminal.
- Group: `partitioning`.

## 11. Flatten Nested Sequences

- Operation: flatten all 500 `Batches`, each containing 20 integers, and
  materialize the 10,000 results.
- Input/output: `int[500][20]` to `int[10_000]`.
- Inside invocation: `SelectMany`, traversal of every inner sequence, and
  `ToArray`.
- Validation: length, first and last value, and checksum.
- Supported: the library's flattening operator produces the exact sequence;
  hand-written nested loops in a library adapter are not support.
- Group: `sequences`.

## 12. Distinct Values

- Operation: reduce `Numbers` to its 1,000 distinct values and materialize.
- Input/output: `int[10_000]` to `int[1_000]`.
- Inside invocation: distinct-set tracking and `ToArray`.
- Validation: compare an order-insensitively sorted copy with the expected set.
- Supported: the library's `Distinct` produces every unique value exactly once.
- Group: `sequences`.

## 13. Zip Pairs

- Operation: zip `Numbers` with itself, multiply each pair, and materialize.
- Input/output: two 10,000-element integer sequences to `int[10_000]`.
- Inside invocation: paired traversal, product projection, and `ToArray`.
- Validation: length, first and last product, and checksum.
- Supported: the library exposes `Zip` and produces the exact products.
- Unsupported: no zip operator is present.
- Group: `sequences`.

## 14. Aggregate

- Operation: fold all `Numbers` with seed zero and `acc + n`.
- Input/output: `int[10_000]` to `int`.
- Inside invocation: the caller-supplied accumulator and terminal fold.
- Validation: exact equality with the loop-computed sum.
- Supported: the library exposes a seeded aggregate/fold accepting the static
  accumulator.
- Unsupported: no general aggregate operator is present.
- Group: `sequences`.

## 15. Ordered Top N

- Operation: order all orders by descending `Amount`, take 20, project `Id`, and
  materialize.
- Input/output: `Order[5_000]` to `int[20]`.
- Inside invocation: key selection, ordering, partitioning, projection, and
  `ToArray`.
- Validation: length 20, first ID 4,501, and element-wise equality with the
  loop-computed deterministic order. Amounts are unique, so stability is not a
  requirement.
- Supported: the library exposes descending ordering and produces the exact top
  identifiers.
- Unsupported: no ordering operator is present.
- Group: `advanced`.

## 16. Group and Aggregate

- Operation: group orders by region and sum `Amount` inside each group.
- Input/output: `Order[5_000]` to `RegionTotal[8]`.
- Inside invocation: grouping, per-group summation, projection, and `ToArray`.
- Validation: sort a copy by region using ordinal comparison and compare all
  region totals with loop-computed expectations.
- Supported: the library exposes grouping and performs the aggregate through
  its query surface.
- Unsupported: no grouping operator is present.
- Group: `advanced`.

## 17. Join and Project

- Operation: inner-join all orders to customers by customer ID and project each
  pair to `CustomerOrder(OrderId, CustomerName)`.
- Input/output: `Order[5_000]` plus `Customer[500]` to `CustomerOrder[5_000]`.
- Inside invocation: lookup construction or equivalent join machinery, key
  matching, projection, and `ToArray`.
- Validation: sort a copy by order ID and compare length, first and last values,
  and checksum with loop-computed expectations.
- Supported: the library exposes an intended join operator and returns every
  exact pair; a matrix-owned lookup or hand-written join is not support.
- Unsupported: neither join nor group-join is present.
- Group: `advanced`.

## 18. Struct Predicate Filter

- Operation: filter `Numbers` by divisibility by three and count matches using a
  struct-typed predicate rather than a delegate.
- Input/output: `int[10_000]` to `int`.
- Inside invocation: construction of the tiny predicate value, full filtering,
  and terminal count.
- Validation: exact equality with scenario 1. No runtime allocation assertion is
  made; published memory measurements are the allocation evidence.
- Supported: the library exposes a documented non-delegate struct-function API
  and the invocation uses it. Returning struct enumerables while accepting a
  delegate predicate is insufficient.
- Unsupported: the library exposes only delegate predicate parameters.

Not rated: with this few rated entrants, the reference is a library's own
result, not a result earned against a competitor, so the full 200 points would
not reflect a win. The current entrant count is not repeated here — it is
computed from the report and shown live in the feature matrix and
`README.md`. See workflows/rating.md, "No per-scenario exclusion by threshold
or editorial judgment". It is still benchmarked and validated, and its own
chart still shows the result. It was the only scenario in the `allocation`
chart group; the group is removed rather than left empty.

## Availability meanings

- `Supported`: the implementation passes every semantic assertion above.
- `Unsupported`: the operation is meaningful but the library lacks the required
  operator, source entry point, or struct-function contract.
- `NotApplicable`: reserved for a feature with no meaningful equivalent; no
  initial cell uses it.
- `Failed`: an adapter claims support but is missing or fails validation.

## Rating fairness

All 18 scenarios participate in the rating, for a maximum of 3,600 points per
rated library. Unsupported cells score zero, so breadth is paid for and remains
visible through coverage. Ordering, grouping, and joining are included once each
because omitting them would hide important adoption constraints; duplicating
closely related operators would over-weight breadth.

The narrowest library, Hyperlinq, has a 67% coverage ceiling. The counterweight
is that allocation-free implementations can win the memory half of supported
scenarios. Two scenarios are thinly contested: only ZLinq and Hyperlinq support
the span source, and only StructLinq supports the struct-predicate scenario.
This limitation is explicit rather than hidden by removing important features.

Deliberately excluded are `GroupJoin`, `ToDictionary`, set-combination
operators, `Chunk`, `Average`, and asynchronous variants. They either duplicate
the same support boundary, primarily measure a BCL collection, or require a
separate workload model.

## Native AOT probe

`src/Matrix.LinqQueries.Aot/Probes/<ProbeName>.cs`: filter and count a small
`int[]` (`{1,2,3,4,5,6}`, divisible-by-3 predicate, expected count 2) through
the library's own operator surface, mirroring `FilterCount`. `HandCoded` has no
probe: it exercises no library. This is a deployment capability
(`FeatureReportEntry.IsDeployment`), not a scenario: it carries no timing and
never enters the rating.
