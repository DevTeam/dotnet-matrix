# Validation feature support contract

A library is `Supported` only when its direct validation call produces the
validity, error count, and property paths required below. Returning the expected
boolean is insufficient when validation omits errors, adds errors, reports a
different path, or performs work outside the stated boundary.

- `Supported`: the implementation exists and passes every condition below.
- `Unsupported`: the library has no intended mechanism with the required
  semantics.
- `NotApplicable`: the operation has no meaningful equivalent for the library
  model. Configuration difficulty is not a reason to use this status.
- `Failed`: support is claimed, but build, validation, or execution fails.

## Common rules

- Inputs are category-owned mutable classes. DataAnnotations attributes and
  `IValidatableObject` are allowed on common inputs so the framework baseline
  and MiniValidation can use their intended APIs. FluentValidation must define
  equivalent rules without consulting those attributes.
- Error paths use dotted member access and zero-based collection indices, for
  example `Address.PostalCode` and `Items[1].Quantity`.
- Error message text is not compared. Validity, the complete multiset of paths,
  and the number of failures are compared.
- Validator instances and attribute metadata caches are prepared before the
  measured operation unless the feature is `Prepare Validator`.
- Each benchmark method calls the compared library directly and returns a
  strongly typed result. A matrix-owned universal validator interface does not
  qualify.
- The validation and benchmark runners invoke the same method and input.
- Features 1 through 9 are drawn in the synchronous chart groups and Prepare
  Validator has its own. A chart group decides where a scenario appears, never
  whether it counts: every scenario of the category enters the rating.

## 1. Valid Object

Validate one `BasicInput` with non-empty name, a valid email address, and an age
from 18 through 120. The result must be valid and contain no failures.

## 2. Single Failure

Validate one `BasicInput` whose name is empty while email and age are valid.
The result must be invalid and contain exactly one failure at `Name`.

## 3. Multiple Failures

Validate one `BasicInput` with an empty name, malformed email address, and age
below 18. The result must be invalid and contain exactly three failures at
`Name`, `Email`, and `Age`.

## 4. Nested Object

Validate one `NestedInput` containing an `AddressInput` whose postal code is
empty. The result must contain exactly one failure at
`Address.PostalCode`. Traversal of the nested object is part of the measured
operation.

## 5. Collection

Validate a `CollectionInput` containing three items. The second item has
quantity zero and the other items are valid. The result must contain exactly
one failure at `Items[1].Quantity`. Collection traversal and indexed path
construction are part of the measured operation.

## 6. Conditional Rule

Validate a `ConditionalInput` with `IsBusiness` set to `true` and an empty tax
ID. The result must contain exactly one failure at `TaxId`. The tax ID rule must
not run when `IsBusiness` is `false`.

## 7. Custom Rule

Validate a `CustomInput` whose code is the odd integer 41. A library-owned
predicate, custom validator, `ValidationAttribute`, or intended extension point
must produce exactly one failure at `Code`. Post-processing a successful result
does not qualify.

## 8. Stop On First Failure

Validate a `BasicInput` in which name, email, and age are all invalid. Validation
must stop after the first rule in the declared order and return exactly one
failure at `Name`. Gathering all errors and discarding later failures does not
qualify. A library without a fail-fast or cascade mechanism is `Unsupported`.

## 9. Async Validation

Validate one `AsyncInput` whose user name is already present in a deterministic
in-memory lookup. The rule must use the library's intended asynchronous
validation API and produce exactly one failure at `UserName`.

The lookup yields once before returning so completion is observably
asynchronous, but external I/O and timing are absent, so the measurement is
mostly task-scheduling and async-pipeline overhead rather than the rule itself.
Libraries without asynchronous rules are `Unsupported`.

Not rated: with this few rated entrants, the reference is a library's own
result, not a result earned against a competitor, so the full 200 points would
not reflect a win. The current entrant count is not repeated here — it is
computed from the report and shown live in the feature matrix and
`README.md`. See workflows/rating.md, "No per-scenario exclusion by threshold
or editorial judgment". It is still benchmarked and validated, and its own
chart still shows the result.

## 10. Prepare Validator

Create the complete validator or rule graph used by Valid Object, Single
Failure, and Multiple Failures without validating an input.

Fluent or runtime-configured libraries construct a fresh validator on every
invocation. Attribute-driven libraries with no explicit preparation API report
zero time and allocation: their internal metadata lookup and lazy-cache access
remain part of ordinary validation because the libraries expose no separate
preparation operation.
