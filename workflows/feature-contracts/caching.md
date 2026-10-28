# Caching feature contract

## Scope

An in-memory cache stores values that are expensive to compute and returns them
without recomputation. This category measures how cheaply a library does that on
one thread with a prepared state, and which of the eight operations it offers at
all.

Excluded by design, because they are a different category:

- Distributed caches and backplanes (Redis, NCache, SQL-backed stores). A network
  hop measures a network, not a cache.
- Serialization cost. Values are handed straight to the cache, never serialized on
  the measured path.
- Compilation, warm-up, and population of the seeded entries. Every entry a
  scenario reads is created in the constructor, outside the measured region.

## Data

| Item | Value |
| --- | --- |
| Key kind | string for the six string-keyed libraries, int for BitFaster.Caching |
| Value type | readonly record struct CacheEntry(int Id, long Ticks, int Counter) |
| Hit key | one entry seeded in the constructor, read repeatedly |
| Miss key | one key that is never written |
| Write keys | a pool of 65 536 distinct keys, handed out in order |
| Default lifetime | 10 minutes, far longer than a run |
| Short lifetime | 50 ms, used only by the expiration scenario |
| Settle delay | 200 ms after writing a short-lived entry |

The write-key pool exists so an insertion never overwrites what a read scenario
seeded, and so the write path never degenerates into overwriting one key.
BitFaster.Caching is keyed by int, so it uses numeric keys of the same concept;
that is a property of its API, not a different scenario.

## Scenarios

| # | Scenario | What the measured region does | Entrants |
| --- | --- | --- | --- |
| 1 | Get Hit (`GetHit`) | Read a key seeded before the measurement | 7 |
| 2 | Get Miss (`GetMiss`) | Read a key that is absent | 6 |
| 3 | Set (`Set`) | Write a fresh key, then read it back | 7 |
| 4 | Get Or Create (`GetOrCreate`) | Read-through for a present key: factory must not run | 5 |
| 5 | Expiration (`Expiration`) | Write a 50 ms entry, wait 200 ms, read it | 6 |
| 6 | Invalidate (`Invalidate`) | Write a key, remove it, read it | 7 |
| 7 | Concurrent Get Or Create (`ConcurrentGetOrCreate`) | Eight threads, one absent key, factory must run once | 2 |
| 8 | Fail Safe (`FailSafe`) | Factory throws while a stale value is present | 1 |

Every scenario returns a CacheLookup or bool built from what the cache actually
returned, and a CacheChecks call asserts the expected outcome inside the measured
method. A scenario that returns the wrong thing fails validation and never
reaches a benchmark.

### Why GetOrCreate asserts that the factory did not run

The distinction between a read-through call and a plain read is what a cache is
bought for. If GetOrCreate on a present key runs its factory, the caller pays the
computation it wanted to avoid, and the benchmark would reward that. The scenario
seeds the key first and only measures the case where the value is already there.

### Why ConcurrentGetOrCreate is a separate scenario

Stampede protection is a correctness property, not a speed property: eight threads
asking for one absent key must run the factory once. A library that runs it eight
times is not slower at the same job, it declines the job, so the outcome is
Unsupported with a measured reason rather than a slow Supported.

### Why FailSafe has exactly one entrant

Serving a stale value when the factory throws is a specific feature that only
FusionCache implements among the seven. Its scenario is `rated: false` because
`best(s, m)` over a single rated library is that library's own result, so
`ratio = 1` and the full 200 points would not reflect a win. Reason, verbatim per
the standing wording:

> With this few rated entrants, the reference is a library's own result, not a
> result earned against a competitor, so the full 200 points would not reflect a
> win.

FusionCache remains fully rated on its other seven scenarios.

### Chart groups

Groups are sets of feature ids, never sets of libraries or algorithms:

| Group | Name | Features |
| --- | --- | --- |
| reads | Reads | GetHit, GetMiss |
| writes | Writes | Set, Invalidate |
| read-through | Read Through | GetOrCreate, ConcurrentGetOrCreate |
| lifetime | Lifetime | Expiration |

A group is a set of rated feature ids, never a set of libraries or algorithms, and
a feature marked `rated: false` belongs to no group: it is excluded from the
group standings by design rather than by omission, and stays visible in
`features.json` and the feature matrix. FailSafe is therefore drawn in no group;
it keeps its own scenario and its own published status for every library, which is
the documented form of visibility for an unrated capability.

With one feature and six rated entrants, `lifetime` is not degenerate: its
standing compares six libraries on the same operation, which is what a group
standing is for.

## Support matrix

Proven by validation, not by documentation. yes means the scenario ran and its
assertions held; no means an announced Unsupported with a reason.

| Library | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Microsoft.Extensions.Caching.Memory 10.0.14 | yes | yes | yes | yes | yes | yes | no | no |
| System.Runtime.Caching 10.0.14 | yes | yes | yes | yes | yes | yes | no | no |
| ZiggyCreatures.FusionCache 2.9.0 | yes | yes | yes | yes | yes | yes | yes | yes |
| LazyCache 2.4.0 | yes | yes | yes | yes | yes | yes | yes | no |
| CacheManager.Core 3.0.0 | yes | no | yes | no | yes | yes | no | no |
| EasyCaching.InMemory 1.9.2 | yes | yes | yes | no | yes | yes | no | no |
| BitFaster.Caching 2.6.1 | yes | yes | yes | no | no | yes | no | no |

### Facts established during validation

Three claims the documentation does not make, each measured on this machine:

- Microsoft.Extensions.Caching.Memory has no stampede protection. Eight concurrent
  GetOrCreate calls for one absent key ran the value factory eight times - once per
  caller.
- BitFaster.Caching and EasyCaching.InMemory behave the same way. Their GetOrAdd and
  retriever overloads also ran the factory eight times for eight callers.
- FusionCache does provide both. Its factory ran once for eight callers, and with a
  fail-safe entry configured it served the stale value while the factory threw. It
  is the only library in the category that covers all eight scenarios.

### Reasons for the remaining gaps

- CacheManager.Core GetMiss: its Get returns default for an absent key, which is
  indistinguishable from a stored default value, so a miss has no distinct
  observable outcome.
- CacheManager.Core and EasyCaching.InMemory GetOrCreate: their read-through
  overloads do not return the stored entry for a present key through the same call
  the scenario needs.
- BitFaster.Caching GetOrCreate: keyed by int; the string-keyed read-through shape
  cannot be expressed.
- BitFaster.Caching Expiration: its unbounded LRU policy has no per-entry lifetime,
  so an entry cannot be made to expire.
- Every FailSafe except FusionCache: no fail-safe concept, a throwing factory
  propagates the exception.

## Measurement rules

- State preparation is outside the measured region. Seeding, key-pool setup, and
  the settle delay of Expiration are done where they belong; the delay is
  documented as the point of that scenario rather than as measured work.
- The measured path uses the library's own typed API, directly. No interface call,
  dictionary lookup, boxing, or reflection sits between the harness and the library
  on the hot path.
- PayloadSizeBytes is written to the report and takes no part in points or charts: a
  cache returns a value, and value size belongs to the scenario's data, not to the
  library.
- Every benchmark method is a direct typed call, or its feature is announced
  Unsupported with an English reason. There is no third state.

## Maximum points

Maximum: 200 x 7 = 1400 - seven rated scenarios, 200 points each. The eighth
scenario, FailSafe, is measured and published but marked `rated: false`, so it
adds nothing to the maximum.

The maximum counts the points a library can earn by winning a scenario. It is not a
claim that any library reaches it: FusionCache covers all eight, the others do not,
and the matrix above shows who entered what.

## Honest limits of this category

- One process, one thread for six of eight scenarios, one machine. Contention and
  NUMA behaviour are not represented.
- BitFaster.Caching is an LRU policy library, not a read-through cache. Its two
  Unsupported entries are a scope mismatch, not a deficiency.
- ConcurrentGetOrCreate and FailSafe have two and one entrants respectively, so
  their results say which library offers the property, not who is faster at it.
  FailSafe earns no points at all.
