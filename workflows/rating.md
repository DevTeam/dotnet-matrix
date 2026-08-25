# Library rating

This document owns the rule that turns benchmark results into the library rating
shown in the Web application and in `README.md`. It is the source of truth for
the rule itself, for the reasons behind it, and for the wording used to explain
it to readers. `MatrixRatings` implements it; nothing else may define a
competing rule.

The rating answers one question: **across everything this category measures, how
close does a library come to the best result available?**

## The rule

> In every scenario the fastest library scores 100 points and the one allocating
> least scores 100 more. Four times slower is half the points, and a scenario the
> library does not support is worth nothing. A library's rating is the sum.

Formally, for library `L`, scenario `s` and metric `m` in {execution time,
allocated bytes}:

```
points(L, s, m) = 0                                        if L reported no value of m for s
points(L, s, m) = 100 * sqrt(ratio(L, s, m))               otherwise

ratio(L, s, m) = (best(s, m) + step(m)) / (value(L, s, m) + step(m))
best(s, m)     = the smallest value of m among the rated libraries that completed s
step(m)        = 1 nanosecond for execution time, 24 bytes for allocated memory
rating(L)      = sum of points(L, s, m) over every scenario and both metrics
```

The maximum is `200 x scenario count`, so a library that is both fastest and
leanest everywhere scores exactly the maximum and the reader always knows the
scale. Time and memory carry the same weight; the two halves are shown
separately so it is visible which one a library earned its total from.

### Why the step

A source generator can complete a preparation scenario in a time that rounds to
zero nanoseconds, and an allocation-free scenario measures zero bytes for many
libraries at once. Both `best` and `value` are then zero and the ratio is
undefined. Adding the same step to both sides states that results are compared at
the smallest difference that means anything: two libraries that both measure zero
are equally best and both score 100, while two microseconds against a
zero-nanosecond best scores 0.05 rather than an error.

The step is the resolution of the metric, and the two metrics do not have the
same one:

- **Time — one nanosecond.** That is where the measurement itself stops; a
  smaller difference is not something the benchmark can see.
- **Memory — twenty-four bytes.** Allocation is counted exactly, so a byte is
  never a rounding artefact, but it is also not the smallest step a library can
  take: the smallest object the 64-bit runtime can allocate is 24 bytes (object
  header, method table pointer, one field). Allocating nothing therefore beats
  allocating something by one object, not by an unbounded factor.

The memory step matters more than it looks. With a one-byte step, a scenario
where the leader allocates zero scored everyone else at almost nothing however
little they allocated — in `Logging / Exception`, NLog's 120 bytes against
Microsoft.Extensions.Logging's 0 earned 9.1 points out of 100, barely more than a
library allocating a megabyte. At 24 bytes the same cell earns 40.8 and the
ordering below the leader stays readable.

Both steps are units of measurement: they say what counts as the same result, and
neither is fitted to produce a particular ranking.

### Why the square root

The exponent is the exchange rate between winning once and being decent often,
and there is no neutral value for it. A rating sums one score per scenario per
metric, so the shape of the curve decides how many honest mid-table results one
outright win is worth.

At an exponent of 1 — a plain ratio, which reads like the natural choice — the
scale spends nine tenths of its range on the first two-fold of the field. Losing
by a factor of two costs 50 points; losing by a factor of ten costs 90; and every
result past five-fold collapses into the same sliver above zero. One scenario won
outright was therefore worth about thirty scenarios completed thirty times
behind. In `DependencyInjection` that produced a result nobody could defend:

- **MvvmCross** enters 6 scenarios of 15. Resolving a singleton allocates
  nothing, exactly like the leaders, so that one cell scored the full 100 — 59% of
  its entire category total of 169. Everything else it did scored between 1 and 25.
- **Autofac** enters 13 scenarios of 15 and is 54x behind at the median, which is
  where MvvmCross sits too (52x). Its best cell of twenty-six was worth 10.2.
  Total: 81.

Same typical performance, twice the breadth, half the score. At an exponent of
one half the same perfect cell is worth about five mid-table cells rather than
thirty, Autofac finishes ahead of MvvmCross, and the bottom of the table gets
enough resolution to be read at all.

The exponent also repairs the coverage inversions listed further down without a
separate term for coverage: once each attempted scenario is worth meaningful
points instead of two, breadth pays for itself. Lamar (12 scenarios) moves above
Singularity (9), Microsoft.Extensions.DependencyInjection (9) above Faster.Ioc
(8), DataAnnotations (9) above MiniValidation (8). Nothing was added to the rule
to make that happen.

What it costs: `half as good is half the points` was a stronger sentence than
`four times slower is half the points`. Both are one line and both can be checked
against the published reports with a calculator; the first was simply describing
a scale that did not work below the top of the field.

### Why both metrics count the same

The project measures execution time and allocated memory and presents them side
by side everywhere else. Weighting one above the other would be a judgement the
data cannot support, and would need a constant. Counting them equally needs
none, and the split is published, so a reader who cares only about allocation can
read the Memory column on its own.

### Properties this buys

- **A score depends only on the library's own result and on the best result.**
  It cannot change because a competitor was added or withdrawn, except through
  the reference.
- **Distance is preserved.** Being second by 5% and second by 9500% are
  different numbers, not the same medal.
- **Every scenario counts once.** Groups of different sizes no longer weigh
  differently, because groups no longer enter the rating at all.
- **Breadth is paid for.** An unsupported scenario costs its full 200 points, so
  a narrow library cannot outrank a complete one by winning where it happens to
  compete.
- **It is actionable.** An author can compute the number from the published
  reports, see which scenario costs the most, and know that making a result four
  times better doubles the points for it.

### No per-scenario exclusion by threshold or editorial judgment

The rating is computed over every scenario of the category by default, and a
scenario is not removed from it because it turned out to be niche, because it
is inconvenient for one library, or because a reader thinks few enough
applications need it. `MatrixScores` still has no concept of "this scenario
doesn't count" built into the rule; where a scenario is genuinely excluded (see
below), that is a named, per-scenario fact declared once on the scenario, not a
computed property of who happens to be entered.

The `Group:` field of a feature contract names the chart group of the scenario
and nothing else. It was called `Rating:` until the medal rule was replaced,
and two contracts kept a value of `feature-only` from that era, which read as
though a scenario could opt out. Nothing implemented it.

Two mechanical shortcuts were considered and rejected, and stay rejected:

- **A threshold on the number of implementations** — "a scenario counts only if
  at least N libraries support it". It fails the author test the same way the
  medal threshold did: there is no answer to "why three and not two". It is also
  the wrong instrument, and not just marginally: in `Logging`, `Buffered
  Logging` is supported by five libraries of six, so a threshold of three
  excludes nothing there, and a threshold high enough to exclude it leaves two
  scenarios of nine. In `DependencyInjection`, a global threshold catches
  `Enumerable`, `PerResolve` and `Child Container` together — and DryIoc, the
  category's number two, supports all three. A threshold applied uniformly
  removes exactly the breadth that separates a complete container from a
  minimal one, which is the opposite of what the rating is for.
- **A per-scenario `Rated` flag driven by an editorial criterion** — for
  instance "the scenario is rated if every user of the category pays for it".
  Applied honestly to `Logging` it also unrates `Structured Properties`, `Scope
  Or Context`, `Template Rendering` and `Formatted Output`, because plenty of
  applications never use any of them. It is a judgement in the shape of a rule,
  and it fails the same author test as the threshold: nothing distinguishes why
  scope is out and structured logging is in.
- Both mechanisms share the same deeper fault: recomputed automatically, either
  one lets a scenario cross the boundary because a library was added or
  withdrawn, changing every other library's score by up to 200 points,
  retroactively. That breaks the property under "Properties this buys" above —
  a score cannot change because a competitor was added or withdrawn.

### The `Rated` flag that does exist

A `[MatrixFeature]` can carry `rated: false`, mirroring `rated: true` on a
library. It is a named, individually justified, manually recorded exception —
never computed from the current field, never re-evaluated automatically, and
never a blanket rule applied to a whole category. Two distinct, narrow
situations earn it, and nothing else does:

- **The scenario has so few rated entrants that a library's own result becomes
  the reference.** `best(s, m)` is the smallest value among rated libraries
  that completed the scenario; with one entrant, that library's own number is
  the smallest, so `ratio = 1` and it scores the full 200 points regardless of
  whether the number is actually good. This is not a coverage problem —
  §"Scenarios with one entrant" below still exists and is still fixed by
  disclosure where two or more libraries genuinely compete. It is a
  mathematical degeneracy specific to exactly one entrant, checkable by
  counting, not by opinion: `JsonSerialization / Source Generation Round Trip`
  (System.Text.Json alone) and `Validation / Async Validation`
  (FluentValidation alone) are the two cases in the current reports.
- **The scenario was never actually part of any published standing.**
  `DependencyInjection / Enumerable` was measured and rated in the underlying
  data but drawn in no chart group — the "feature-only" bug this document
  already describes, just the version where the bug left a scenario counted
  instead of excluded. It briefly gained a chart group by a documentation fix
  that mistook "undrawn" for an oversight to correct rather than a decision to
  make explicit. `rated: false` here restores what every published chart and
  standing already showed, rather than changing anything for the first time. It
  is the weaker of the two justifications — 3 of 22 rated libraries support it,
  which is thin but not degenerate — and it is recorded as such in
  `dependency-injection.md` rather than folded into the mathematical case above.

Neither situation generalizes into "exclude scenarios with low coverage": a low
percentage is necessary to notice a candidate, never sufficient to flag one. A
scenario reaches `rated: false` only when someone writes the specific reason in
its own contract, the same way `Caveat:` works for a scenario whose measurement
needs a qualification rather than an exclusion. Whether a library supports an
unrated capability is still published: `FeatureStatus` is reported per library
independently of the benchmarks, the scenario keeps its own chart, and neither
depends on the flag.

### Scope

- Only libraries with `rated: true` in `metadata/<Category>/libraries.json`
  take part. A library outside the rating **does not define `best`** either: it is
  drawn on the charts as a reference, with a dash instead of a score, but the
  competitors are measured against the best of the competitors. Otherwise a
  hand-written baseline that does no work at all would set the reference and
  annihilate everyone's score — in `DependencyInjection / Prepare` it did exactly
  that, because `PrepareAndRegister` costs a hand-written wiring 0 ns and 0 bytes.
- Scores below ten points are shown with their fraction. A library three orders of
  magnitude behind earns 0.004, and rounding that to zero would make the whole
  bottom of a table look identical. Formatting is invariant, so a chart does not
  depend on the locale of the machine that rendered it.
- The rating is per category. There is no cross-category rating.
- Every overview group carries its own standing, computed by the same
  `MatrixScores` call over the scenarios of that group. A group maximum is
  therefore `200 x group scenario count`. The two standings differ only in what
  they cover.
- A metric nobody reported for a scenario is skipped rather than scored as zero:
  it is not a competition. A scenario counts as covered when the library
  reported either metric.
- The library filter in the Web application applies: the rating describes the
  libraries currently being compared.

## What it replaces, and why

The previous rule was an Olympic medal table over the chart groups: the first
three libraries of every group in `charts.json` received gold, silver and
bronze, and the leaderboard sorted lexicographically by gold, then silver, then
bronze.

It produced results that could not be defended.

**A medal said nothing about distance.** In Dependency Injection, MvvmCross took
silver in the two-scenario `Prepare` group while being **95.5x** slower than the
group leader, and DryIoc took bronze at **147.8x**. Across all six categories,
8 of the 44 silver and bronze medals were awarded at more than 10x behind the
leader:

| Category | Group | Medal | Library | Behind leader |
| --- | --- | --- | --- | ---: |
| Object Mapping | Prepare | silver | AutoMapper | 2 750 106x |
| Object Mapping | Prepare | bronze | Mapster | 5 153 151x |
| CSV Processing | Write | bronze | CsvHelper | 1 382x |
| JSON Serialization | Prepare | silver | System.Text.Json | leader measured 0 ns |
| JSON Serialization | Prepare | bronze | Newtonsoft.Json | leader measured 0 ns |
| Dependency Injection | Prepare | bronze | DryIoc | 147.8x |
| Dependency Injection | Prepare | silver | MvvmCross | 95.5x |
| Logging | Prepare | bronze | log4net | 12.9x |

Seven of the eight are preparation groups, where the winner is a source
generator doing almost no work, so every place below first is noise.

**Group size was an invisible weight.** `Prepare` has two scenarios and
`Advanced` has five, yet a medal in each counted the same.

**Coverage was free.** MvvmCross supports 6 of the 15 Dependency Injection
scenarios. Failing to enter the `Advanced` group cost it nothing; it simply did
not appear there.

**One silver outranked any number of bronzes**, so a single medal at 95x behind
placed MvvmCross third overall, ahead of libraries that competed everywhere.

### Why not a threshold

Refusing a medal beyond, say, 10x behind the leader would remove exactly those
eight cases and change nothing else. It was rejected because it fails the author
test: a library 11x behind gets nothing while one 9x behind gets a medal, and
there is no answer to "why ten and not eight". A rule with a tunable constant
cannot be defended and will be argued about forever.

### Precedent

No serious competition decides a champion by counting medals.

- The **IOC does not officially rank countries**; the medal table is published
  for information. The gold-first order is a convention, and the weighted
  alternatives in circulation (3-2-1, 4-2-1, 5-3-1, 5-3-2, 6-2-1) are all
  unofficial. Medal tables work in sport because qualification happens first,
  so only comparable competitors reach the final. Benchmarks have no
  qualification round.
- **Formula 1** awards 25-18-15-12-10-8-6-4-2-1 to the top ten, deliberately
  widened so the midfield scores regularly. The **Alpine Ski World Cup** pays
  100-80-60 down to a single point for thirtieth. Both reward the whole field.
- The **decathlon** solves our exact problem — combining results measured in
  different units into one number — by scoring the *result*, not the place:
  `A * (B - P)^C`, with constants calibrated so a world-class performance in any
  discipline yields about 1000 points. A failed discipline scores zero and the
  athlete still gets a total. Medals are awarded on the total, never per
  discipline.

The rule in this document is the decathlon shape with the reference taken from
the field rather than from a calibration table, which is what benchmark suites
such as SPEC do when they normalise against a reference and aggregate.

## Effect at the time of the change

A historical snapshot, computed from the reports committed when the medal rule
was replaced. It is deliberately not refreshed: it exists to show what the
change did, and recomputing it against today's reports would destroy that. The
numbers below therefore do not match `README.md` — `Logging` has since gained a
ninth scenario, so its maximum is 1800 rather than the 1600 shown here. The
current standings are in `README.md`, which is generated from the reports.

| Category | Maximum | Previous leaderboard | Rating, as `total (time + memory)` |
| --- | ---: | --- | --- |
| CSV Processing | 2000 | Sep, Sylvan.Data.Csv, TinyCsvParser, CsvHelper | Sep 1999 (999+1000), TinyCsvParser 950 (558+392), Sylvan.Data.Csv 882 (612+270), CsvHelper 443 (231+212) |
| Dependency Injection | 3000 | Pure.DI, Grace, **MvvmCross**, Simple Injector | Pure.DI 2765 (1400+1365), DryIoc 1996 (846+1150), Stashbox 1847 (750+1097), Grace 1843 (798+1045) |
| JSON Serialization | 2800 | System.Text.Json, ServiceStack.Text, Newtonsoft.Json | System.Text.Json 2513 (1264+1249), ServiceStack.Text 1999 (1111+887), Newtonsoft.Json 1225 (827+398) |
| Logging | 1600 | Microsoft.Extensions.Logging, Serilog, NLog | Microsoft.Extensions.Logging 1184 (546+638), NLog 1021 (595+426), Serilog 875 (445+430), log4net 651 (303+348) |
| Object Mapping | 2000 | Mapperly, Mapster, AutoMapper | Mapperly 2000 (1000+1000), Mapster 1375 (583+792), AutoMapper 1204 (402+802) |
| Validation | 2000 | DataAnnotations, Microsoft.Extensions.Validation, MiniValidation | Microsoft.Extensions.Validation 1545 (753+791), DataAnnotations 1477 (741+736), MiniValidation 1455 (734+721), FluentValidation 1419 (750+669) |

Notable changes:

- **MvvmCross leaves the top of Dependency Injection**, from third to nineteenth
  of twenty-two on 340 points. Nine scenarios are worth zero to it, and where it
  does compete it is 52x behind on time at the median.
- **Mapperly reaches a perfect 2000** in Object Mapping by being both fastest and
  leanest in all ten scenarios, and the 2 750 106x silver disappears.
- **Memory changes the order, which is the point of counting it.** DryIoc rises
  above Grace in Dependency Injection on the strength of 1150 memory points and
  full coverage, and AutoMapper closes most of the gap to Mapster on 802 memory
  points despite scoring 402 on time.
- **Coverage stays visible.** ZLogger is fifth in Logging on 386 points from
  three of eight scenarios; the reader sees `3/8` beside the score and can weigh
  that trade themselves.

## The overview groups

A group is scored by the same rule over its own scenarios, and its chart is
ordered by that score. Before, a group was ordered by the raw sum of execution
times, which had two faults of its own:

- **Memory did not count**, although the chart drew it beside the time.
- **A sum is dominated by its largest term.** In the Dependency Injection
  `Basic` group the four scenarios contribute 9%, 12%, 22% and 56% of the median
  total, so the group was in effect ordered by `Complex` alone. Points weigh
  every scenario the same by construction.

Measured against the previous order, six of the twenty-three groups change, and
two change leader: `CsvProcessing / Throughput` from Sylvan.Data.Csv to Sep, and
`JsonSerialization / Nested` from System.Text.Json to ServiceStack.Text. In
`DependencyInjection / Basic` sixteen of twenty-two rows move — seven of those
from the sum-to-average change alone, the rest from memory entering.

### What this costs

The bars still show measured totals, so **the chart is no longer sorted by its
bars**: a shorter bar can sit below a longer one. That is stated in the caption
and explained in the chart's help dialog. The alternative — drawing the score as
the bar — would make the picture consistent at the price of no longer showing
real times beside it, which is the opposite of what this project is for.

The `Not ranked · partial coverage` divider is gone. A library that misses
scenarios scores nothing for them and takes its place in the same list, with the
count of completed scenarios under its name, exactly as in the category
standings.

## Medals after the change

Medals are kept, and both now mean the same kind of thing.

- **A star on a chart row** is the first, second or third place of that group's
  standing.
- **Gold, silver and bronze in the category table** are the first three of the
  category standing.

Both are places in a standing computed by the same rule; they differ only in
scope. That is what makes the two readable together.

## Explaining it in the interface

A number no reader can interpret is worse than no number. Two earlier attempts
at columns called "behind" and "coverage" failed exactly this test, so the
presentation rules are part of this document.

**Always visible, no interaction required:**

- The standings are a table with named columns — `Scenarios`, `Time`, `Memory`,
  `Points` — and the section meta states the maximum: `points out of 3000`.
  Points need no glossary, the scale is stated, and every number is labelled.
- The table layout is fixed and every cell is always rendered, including the
  medal. A star appearing on the podium, or a coverage figure appearing on one
  row and not the next, must never move the column beside it.
- `RatingBadge` is a box of constant size for the same reason: it is empty below
  the podium rather than absent, so the controls beside it in the sidebar and on
  the cards do not shift from row to row.
- Every standings row is filled from the left to the share of the maximum it
  holds. A place number states an order; the fill states the size of the gap, so
  a reader can see at a glance that two adjacent places near the bottom of a
  table are two ways of being nowhere. It is a background gradient on the row, so
  it costs no column and survives the stacked phone layout untouched.
- **Every printed points value carries its own arithmetic as a hint**: one line
  per scenario giving the library's measurement, the best measurement, the step
  applied to both, and the points that came out. Restating the formula does not
  let anyone check a total; the two figures behind each term do. The lines come
  from `MatrixScores.Explain`, which runs the same per-cell function the rating
  sums, so a hint cannot disagree with the number it explains. Present on the
  standings `Time`, `Memory` and `Points` cells, on the chart's points cell
  scoped to that group's scenarios, and on the library card — its points tile,
  each row of the rating breakdown, and each group standing. Every one of them
  carries `cursor: help`, which is the only thing that says a hint is there.
- **A hint is a shortcut, never the only copy.** `title` does not fire on a touch
  screen, so anything that exists only in one would not exist on a phone. The
  library card therefore prints the full breakdown: the `Scenarios` table carries
  a `Points` column with the total for the scenario and the `time + memory` split
  under it, and a footer row that adds up to the rating. Same numbers, same
  source, on the page rather than behind a pointer.
- **The compared set is in the address.** `?libraries=` names what is being
  compared, or what to leave out after a leading `-`. It has to be there: points
  are measured against the best result among the compared libraries, so a link
  that dropped the selection would show a different rating under the same names.
- **Points in a hint print to one decimal** (`MatrixScores.FormatExact`), while a
  table cell keeps whole points. A breakdown exists to be added up: at whole
  points a scenario lost by half a percent shows as a perfect 100, four of them
  show as 400, and the total beside them says 399.
- **`README.md` carries the same breakdown as text**, in a collapsed
  `How the points were earned` block under each rating table: one section per
  library, one row per scenario, with the library's result, the best result and
  the points for both metrics. A generated report is where the full numbers
  belong, and a reader with no browser can still check a total. The charts get
  none of this — a PNG has no hover and the numbers would only crowd it.

**Behind one control:** a single `?` button opens a dialog with the full rule,
the worked example and the reason a preparation scenario cannot be won by a
margin. One mechanism, one visual weight, used in exactly three places where a
non-obvious concept is introduced:

| Placed next to | Explains |
| --- | --- |
| `Rating / Standings` heading | how points are computed, that time and memory count the same, why an unsupported scenario is zero |
| Overview chart heading | that rows are ordered by the group score, why a shorter bar can therefore sit below a longer one, that the bar scale is logarithmic, and what partial coverage costs |
| Benchmarks `Scenario` heading | mean and standard error, what `x1.00` compares against, why lower is better |

Not one button per row, not a tooltip per glyph. The feature matrix keeps its
existing visible legend and gets no button.

## Where the rule can still read as unfair

Audited against the committed reports. Each entry says what the data shows, so a
library author who feels wronged finds the case here instead of guessing.

### The preparation scenarios (the open one)

A source generator does its preparation at compile time, so at run time it
measures zero on both metrics — and every library that prepares at run time then
scores next to nothing for that scenario on both metrics at once. This is the
memory-step problem again, but no step fixes it: the difference is real, and it
is the whole point of a compile-time approach.

What it costs, measured as the share of the maximum a library reaches with and
without the preparation scenarios:

| Category | Prepare scenarios | Effect |
| --- | --- | --- |
| Object Mapping | 2 of 10 — 20% of the maximum | Mapster 69% → 86%, AutoMapper 60% → 75% |
| Dependency Injection | 2 of 15 — 13% | DryIoc 67% → 75%; Stashbox and Grace swap places |
| Validation | 1 of 10 — 10% | FluentValidation is 4th with it and 1st without |
| JSON Serialization | 1 of 14 — 7% | System.Text.Json 90% → 96% |

Two things are wrong here and they are not the same thing. The first is that
preparation cost is a genuine cost a user pays, so removing it would hide
something true. The second is that the same handicap is worth 20% of one category
and 7% of another, purely because of how many scenarios each category happens to
define — and that is not a property of the rule, it is a property of the scenario
set. Balancing the scenario sets is the fix; changing the rule is not.

### Scenarios with few entrants

Four scenarios were contested by fewer than three rated libraries at the time
of writing:

| Scenario | Entrants | Rated |
| --- | --- | --- |
| `JsonSerialization / Source Generation Round Trip` | 1 of 3 | No — see §"The `Rated` flag that does exist" |
| `Validation / Async Validation` | 1 of 4 | No — see §"The `Rated` flag that does exist" |
| `JsonSerialization / Polymorphic Round Trip` | 2 of 3 | Yes |
| `Validation / Stop On First Failure` | 2 of 4 | Yes |

The two 1-of-N rows are the degenerate case: with exactly one entrant, that
library's own result is the reference, so it scores the full 200 points
regardless of whether the result is good — a race with one runner is not a
race, and the flag removes it rather than mislabel it as one. This is a
narrower move than it looks: §"No per-scenario exclusion by threshold or
editorial judgment" above lists two mechanisms that were tried and rejected for
this same problem, and the difference between those and the `Rated` flag is
exactly the line drawn here — one entrant, not few entrants.

The two 2-of-N rows are a real, if small, competition, and stay rated. The fix
for them is disclosure, not exclusion: the points are stated as though a
contested race was won, and the reader is not told how few ran it. The
Scenarios column shows who did not enter; nothing shows how many did. What is
missing is a mark on the scenario saying how many rated libraries contested it,
so that a score earned against one other competitor is readable as such.

### A narrow library above a broad one (fixed by the curve)

This was the case that produced the square root. With a plain ratio, MvvmCross
finished 16th on 6 scenarios of 15 while Autofac finished 18th on 13, at the same
median distance from the leaders — because a single tied-for-best cell was worth
more than an entire library's work. See *Why the square root* above. Under the
current curve MvvmCross is 19th and Autofac 16th.

Inversions between coverage and rank still exist and most of them are correct:
Simple Injector enters 12 scenarios and finishes above Unity, which enters 13,
because it is an order of magnitude faster in every scenario both of them enter.
What no longer happens is an inversion at equal performance. The Scenarios column
remains the thing that makes such a row readable, which is why it is not optional.

### Checked and clean

- **No silent forfeit.** Every successful result reports both metrics; there is
  no case of a library earning time points and quietly scoring zero for memory
  while still counting as having covered the scenario.
- **No medal decided by noise.** In no category are the first and second placed
  libraries within two combined standard errors of each other in any scenario.
  Across all 352 adjacent pairs in all reports, 18 fall inside measurement noise;
  9 of those are inside the two Dependency Injection preparation scenarios, which
  are the noisiest measurements in the project, and none is at the top of a table.
- **No winning by doing less.** A result counts only when it is `Successful`, and
  success means the validation layer of the category confirmed the library
  actually did the work.

## Open questions

- **Environment mixing.** Results from different benchmark environments are
  already flagged in the Benchmarks view. The rating does not check this and
  will average across environments if a report contains several.
- **Preparation weighting.** Whether the preparation scenarios belong in the
  category total, and whether the scenario sets should be balanced so the same
  handicap costs the same everywhere. See above; this is a scenario-set decision,
  not a rule change.

## Implementation

| Step | Where | Notes |
| --- | --- | --- |
| 0 | `src/Matrix/MatrixScores.cs` | The rule itself, applied to whatever set of scenarios it is given. Both standings call it, so they cannot drift apart in method. |
| 1 | `src/Matrix/MatrixRating.cs` | Replace the group-and-place algorithm with a call to `MatrixScores` over every scenario. `MatrixMedals` gains `TimePoints`, `MemoryPoints` and `Covered`; `Awards` stays for the per-group stars, now taken from the group standing. |
| 1a | `src/Matrix/MatrixOverviews.cs` | One list of rows ordered by the group score; `Ranked`/`Unranked` and the divider are gone. `MatrixOverviewRow` gains `TimePoints` and `MemoryPoints`. |
| 1b | `build/Targets/ReportChartsTarget.cs` | One row loop, a `POINTS` column, coverage under every name, no divider. Rewrites all overview PNGs. |
| 2 | `src/Matrix/MatrixMedals.cs` | `Gold`, `Silver`, `Bronze` and `Total` keep counting per-group awards for the chart stars. Category medals are derived from the points order, not from these. |
| 3 | `src/Matrix.Web/Components/RatingBoard.razor` | A fixed-layout table: place, library, scenarios, time, memory, points; ordering by points. |
| 4 | `src/Matrix.Web/Components/LibraryDialog.razor` | Points in the metric strip beside the rating place, with the split in its tooltip. |
| 5 | `src/Matrix.Web/Shared/HelpDialog.razor` (new) | Generic explanation dialog reusing the `.modal-*` styles and the `inert` handling of the library dialog. |
| 6 | `src/Matrix.Web/Shared/HelpButton.razor` (new) | The `?` control; raises a topic to the page, which owns the dialog state, exactly as `OnOpenLibrary` works today. |
| 7 | `src/Matrix.Web/Pages/Index.razor` | Holds the open help topic; passes `OnHelp` to the three views that need it. |
| 8 | `build/Templates/Readme.cshtml` | Rating table gains the scenarios, time, memory and points columns; the bullet `Medals reward consistency` is replaced by the rule sentence. |
| 9 | `build/Targets/ReadmeRating.cs`, `ReadmeTarget.cs` | Carry points and coverage into the template. |
| 10 | `workflows/category-roadmap.md` | The `Rating groups` column describes chart groups, which no longer drive the rating; reword. |

`RatingBadge` and `OverviewChart` are untouched: they read `Awards` and
`MatrixRatings.Places`, which keep their present meaning as per-group results.

Regenerate `README.md` with `dotnet run --project .\build -- readme` after step 9.

### Verification

The rule is checked against the committed reports rather than against a unit
test alone, because the failure mode being fixed was a plausible-looking number:

1. No category may produce a `NaN` or an infinite score. The step exists for
   this; the preparation scenarios of Object Mapping and JSON Serialization are
   the cases that break a naive ratio.
2. A library that wins every scenario on both metrics must score exactly the
   maximum. Mapperly in Object Mapping is the reference case at 2000 of 2000.
3. Coverage must be visible for every library that scores below the maximum,
   so that a low score is explainable without opening anything.
4. The medal table order in the application and in `README.md` must match.
5. A group standing in the application and in the rendered PNG must list the same
   libraries in the same order with the same points. They share
   `MatrixOverviews`, so a difference means one of the two renderers is stale.
