# Identifiers feature contract

## Scope

The category compares .NET libraries that produce, convert, inspect or encode
identifiers. Four families of libraries are compared:

- **Time-ordered identities** - ULID, KSUID and UUID version 7 values whose
  ordinal order follows their creation time.
- **Snowflake identities** - 64-bit values composed from a timestamp, a
  generator identity and a sequence number.
- **Encoding and obfuscation** - reversible mappings between numbers and short
  strings.
- **Random strings** - URL-safe random text with a caller-defined length and
  alphabet.

The family is a property of a library, not of a chart group: the scenario
Generate is one measurement, and all ten libraries are compared in it. Family
differences show up where they are real - a scenario that has no meaning for a
kind of identifier is NotApplicable with a stated reason, and a difference the
scenario can express is described in the text below rather than split out.

A single category rating spans every scenario, exactly as every other category
does; a library that provides fewer operations scores fewer points.

Nothing leaves the process: no filesystem, network, clock injection outside the
scenario, or database identity column.
## Common data

- BatchSize is 1,024 identifiers.
- BaseTimestamp is 2024-01-01T00:00:00Z; every scenario draws its sample from
  one strictly increasing sequence of millisecond timestamps starting there, so
  a library that keeps a process-wide monotonic guard never sees the clock move
  backwards.
- Numbers is the fixed sequence 1, 42, 1000, 999999 encoded and decoded by the
  codec group.
- Canonical text lengths are 26 characters for ULID, 36 for a D-formatted UUID,
  27 for KSUID and 21 for the configured random string.

## Scenarios

### Generate One

- Group: `generate`.

Creates one identifier with a timestamp the scenario chooses and returns the
library own value type. Validation checks the canonical text length for that
identifier kind and requires the extracted timestamp to equal the requested one,
so a library that ignores the timestamp argument fails.

Sqids and Hashids.net are NotApplicable: they encode existing numbers and have
no generation step.

### Generate Batch

- Group: `generate`.

Fills a preallocated array of BatchSize identifiers from the same increasing
timestamp sequence and returns it. Only generation is measured; the array is
allocated outside the measured delegate. Validation requires the full count and
every identifier to be distinct.
### Format

- Group: `convert`.

Converts a pre-generated identifier into its canonical text and returns the
string. Validation requires the canonical length for that kind, so a library
that formats differently, for example with a different separator or case, fails.

Nanoid, Sqids and Hashids.net are NotApplicable: they produce text directly and
have no separate identifier value to format. UUIDNext is Unsupported: it returns
a System.Guid and exposes no formatting API, so formatting it would measure the
framework rather than the library.

### Parse

- Group: `convert`.

Reconstructs an identifier from its canonical representation and returns the
library own value type. Validation re-formats the result and requires the
original text.

Nanoid is NotApplicable: its identifiers are opaque random text with no
canonical value to reconstruct. Sqids and Hashids.net are NotApplicable: they
decode numbers, which Decode Numbers measures. UUIDNext is Unsupported: it
exposes no parsing API.

### Extract Timestamp

- Group: `time`.

Reads the creation time embedded in a pre-generated identifier and returns Unix
milliseconds. Validation requires the exact value the scenario requested.

Nanoid, Sqids and Hashids.net are NotApplicable: their values carry no time.
FlakeId is NotApplicable: Id has no public accessor for its timestamp, and its
bit layout is internal, so reading it would measure a reimplementation rather
than the library.
### Encode Numbers

- Group: `codec`.

Maps the fixed number sequence to a short string. Validation requires a
non-empty result. Every identity generator is NotApplicable: encoding arbitrary
numbers is not its operation.

### Decode Numbers

- Group: `codec`.

Maps the string produced by the same library in setup back to numbers.
Validation requires the exact fixed sequence. Encode and Decode together prove
the round trip; neither repeats the other work inside its own delegate.
Identity libraries are NotApplicable for the same reason as in Encode Numbers.

### Sortable Order

- Group: `time`.

Compares adjacent identifiers generated at strictly increasing timestamps and
returns how many adjacent pairs are ordered. Validation requires every pair to
be ordered, which proves that ordinal comparison follows generation order.

IdGen is NotApplicable: its generator takes time from a shared ITimeSource
guarded against moving backwards, so it has no factory that generates at an
independent chosen timestamp. KSUID is NotApplicable: it generates only with the
current clock. Nanoid, Sqids and Hashids.net have no time order at all.
## Availability rules

- Supported means the library performs the exact operation through its own API
  and passes every assertion.
- Unsupported means the library cannot provide the required behavior at all.
- NotApplicable means the operation has no meaningful equivalent for that
  identifier kind. The reason is recorded per library and feature.
- Failed means a benchmark method exists but validation does not pass.

## Preparation and measurement

Library construction, option building, pre-generated sample values, canonical
texts, the fixed arrays and identifier arrays passed into a measured comparison
are prepared outside the measured delegate. Hash sets and other validation
structures are created only under MATRIX_VALIDATION and never exist in a
benchmark build.

## Excluded

Distributed identity generation, database column strategies, collisions across
processes, clock recovery and monotonicity guarantees under sequence overflow
are out of scope because they need process coordination or wall-clock simulation
that the matrix cannot reproduce deterministically.
## Rating ceiling

Eight rated scenarios put the category maximum at 200 x 8 = 1600 points. The
number comes from the declared scenarios, not from the chart groups: a scenario
that belongs to no group is still rated.

## Runner constraint: strictly increasing timestamps

Every scenario draws its timestamp from one strictly increasing sequence
(`IdentifierData.NextTimestampMilliseconds()`), never from a fixed constant used
more than once. FlakeId throws `InvalidOperationException: Clock shifted
backwards; can't reliably generate ID` when it is asked to generate at a
timestamp earlier than one it has already returned in the process, so a fixed
constant reused across scenarios fails validation in a way that looks like a
library bug.

When adding a scenario, draw a new timestamp from that sequence and keep the
value it used for comparison. Do not lower it, and do not share one constant
between a generation and an assertion that expects a different value.
