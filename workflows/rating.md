# Library rating

This document owns the rule that turns benchmark results into the library rating
shown in the Web application and in `README.md`. It is the source of truth for
the rule itself, for the reasons behind it, and for the wording used to explain
it to readers. `MatrixRatings` implements it; nothing else may define a
competing rule.

The rating answers one question: **across everything this category measures, how
close does a library come to the best result available?**

## The rule

> In every scenario the fastest library scores 100 points. A library twice as
> slow scores 50, one a hundred times slower scores 1, and a library that does
> not support the scenario scores 0. A library's rating is the sum of its
> points.

Formally, for library `L` and scenario `s`:

```
points(L, s) = 0                                    if L has no successful result for s
points(L, s) = 100 * (best(s) + 1) / (mean(L, s) + 1)   otherwise

best(s) = the smallest mean of the rated libraries that completed s
rating(L) = sum of points(L, s) over every scenario of the category
```

Means are in nanoseconds. The maximum is `100 x scenario count`, so a library
that wins every scenario scores the maximum and the reader always knows the
scale.

### Why the plus one

A source generator can complete a preparation scenario in a time that rounds to
zero nanoseconds. Both `best(s)` and `mean(L, s)` are then zero and the ratio is
undefined. Adding one nanosecond to both sides states that results are compared
at nanosecond resolution: two libraries that both measure zero score the same
100 points, and a library that takes two microseconds against a zero-nanosecond
best scores 0.05 rather than an error.

This is a unit, not a tuning knob. There is no threshold, weight, or cut-off
anywhere in the rule.

### Properties this buys

- **A score depends only on the library's own result and on the best result.**
  It cannot change because a competitor was added or withdrawn, except through
  the reference.
- **Distance is preserved.** Being second by 5% and second by 9500% are
  different numbers, not the same medal.
- **Every scenario counts once.** Groups of different sizes no longer weigh
  differently, because groups no longer enter the rating at all.
- **Breadth is paid for.** An unsupported scenario costs its full 100 points, so
  a narrow library cannot outrank a complete one by winning where it happens to
  compete.
- **It is actionable.** An author can compute the number from the published
  reports, see which scenario costs the most, and know that halving a time
  doubles the points for it.

### Scope

- Only libraries with `rated: true` in `metadata/<Category>/libraries.json`
  take part. A package-less baseline such as `Hand-coded` is excluded, as
  before, and does not define `best(s)`.
- The rating is per category. There is no cross-category rating.
- The rating uses execution time only. Allocation is an open question, see
  below.
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

## Effect on the current reports

Computed from the reports committed at the time of writing.

| Category | Maximum | Current leaderboard | Rating |
| --- | ---: | --- | --- |
| CSV Processing | 1000 | Sep, Sylvan.Data.Csv, TinyCsvParser, CsvHelper | Sep 999, Sylvan.Data.Csv 453, TinyCsvParser 351, CsvHelper 94 |
| Dependency Injection | 1500 | Pure.DI, Grace, **MvvmCross**, Simple Injector | Pure.DI 1400, Grace 647, DryIoc 632, Stashbox 550 |
| JSON Serialization | 1400 | System.Text.Json, ServiceStack.Text, Newtonsoft.Json | System.Text.Json 1229, ServiceStack.Text 1042, Newtonsoft.Json 576 |
| Logging | 800 | Microsoft.Extensions.Logging, Serilog, NLog | Microsoft.Extensions.Logging 512, NLog 471, Serilog 274 |
| Object Mapping | 1000 | Mapperly, Mapster, AutoMapper | Mapperly 1000, Mapster 442, AutoMapper 240 |
| Validation | 1000 | DataAnnotations, Microsoft.Extensions.Validation, MiniValidation | Microsoft.Extensions.Validation 723, MiniValidation 684, FluentValidation 640, DataAnnotations 629 |

Notable changes:

- **MvvmCross leaves the top of Dependency Injection.** It scores far below the
  leaders because nine scenarios are worth zero to it and it is 95x behind where
  it does compete.
- **Mapperly reaches a perfect 1000** in Object Mapping by winning all ten
  scenarios, and the 2 750 106x silver disappears.
- **Validation tightens.** Microsoft.Extensions.Validation leads on 8 of 10
  scenarios covered, ahead of FluentValidation which covers all ten but is
  slower where both compete. Coverage is visible beside the score, so a reader
  can weigh that trade themselves.

## Medals after the change

Medals are kept, with each of the two statements they used to conflate given its
own place.

- **A star on a chart row remains a local fact**: first, second or third in that
  overview group. It is true, it is useful, and the dialog already shows how far
  behind the leader the row is. Nothing changes here.
- **Gold, silver and bronze in the category table are awarded on the rating**,
  to the first three libraries by points. A medal then means "one of the three
  best libraries in this category", which is what a reader assumes it means.

## Explaining it in the interface

A number no reader can interpret is worse than no number. Two earlier attempts
at columns called "behind" and "coverage" failed exactly this test, so the
presentation rules are part of this document.

**Always visible, no interaction required:**

- The column is called **Points**, and the section meta states the maximum:
  `points out of 1500`. Points need no glossary and the scale is stated.
- Coverage sits beside it as `12/15 scenarios`, because it explains a low score
  without changing the order.

**Behind one control:** a single `?` button opens a dialog with the full rule,
the worked example and the reason a preparation scenario cannot be won by a
margin. One mechanism, one visual weight, used in exactly three places where a
non-obvious concept is introduced:

| Placed next to | Explains |
| --- | --- |
| `Rating / Medal table` heading | how points are computed, what the maximum is, why an unsupported scenario is zero |
| Overview chart heading | that the bars are logarithmic, that the length is a total over the group, and what partial coverage means |
| Benchmarks `Scenario` heading | mean and standard error, what `x1.00` compares against, why lower is better |

Not one button per row, not a tooltip per glyph. The feature matrix keeps its
existing visible legend and gets no button.

## Open questions

- **Allocation is not in the rating.** The project measures execution time and
  allocated memory, and the rating currently uses time only, as the medals did.
  The same rule extends naturally — every scenario would contribute two scores,
  one for time and one for bytes, doubling the maximum — but that is a separate
  decision and is not implemented.
- **Environment mixing.** Results from different benchmark environments are
  already flagged in the Benchmarks view. The rating does not check this and
  will average across environments if a report contains several.

## Implementation

| Step | Where | Notes |
| --- | --- | --- |
| 1 | `src/Matrix/MatrixRating.cs` | Replace the group-and-place algorithm with the scoring formula. `MatrixMedals` gains `Points` and `Covered`; `Awards` stays for the per-group stars. |
| 2 | `src/Matrix/MatrixMedals.cs` | `Gold`, `Silver`, `Bronze` and `Total` keep counting per-group awards for the chart stars. Category medals are derived from the points order, not from these. |
| 3 | `src/Matrix.Web/Components/RatingBoard.razor` | Points and coverage columns; ordering by points. |
| 4 | `src/Matrix.Web/Components/LibraryDialog.razor` | Points in the metric strip beside the rating place. |
| 5 | `src/Matrix.Web/Shared/HelpDialog.razor` (new) | Generic explanation dialog reusing the `.modal-*` styles and the `inert` handling of the library dialog. |
| 6 | `src/Matrix.Web/Shared/HelpButton.razor` (new) | The `?` control; raises a topic to the page, which owns the dialog state, exactly as `OnOpenLibrary` works today. |
| 7 | `src/Matrix.Web/Pages/Index.razor` | Holds the open help topic; passes `OnHelp` to the three views that need it. |
| 8 | `build/Templates/Readme.cshtml` | Rating table gains the points column; the bullet `Medals reward consistency` is replaced by the rule sentence. |
| 9 | `build/Targets/ReadmeRating.cs`, `ReadmeTarget.cs` | Carry points and coverage into the template. |
| 10 | `workflows/category-roadmap.md` | The `Rating groups` column describes chart groups, which no longer drive the rating; reword. |

`RatingBadge` and `OverviewChart` are untouched: they read `Awards` and
`MatrixRatings.Places`, which keep their present meaning as per-group results.

Regenerate `README.md` with `dotnet run --project .\build -- readme` after step 9.

### Verification

The rule is checked against the committed reports rather than against a unit
test alone, because the failure mode being fixed was a plausible-looking number:

1. No category may produce a `NaN` or an infinite score. The nanosecond
   resolution exists for this; the preparation scenarios of Object Mapping and
   JSON Serialization are the cases that break a naive ratio.
2. A library that wins every scenario must score exactly the maximum. Mapperly
   in Object Mapping is the reference case at 1000 of 1000.
3. Coverage must be visible for every library that scores below the maximum,
   so that a low score is explainable without opening anything.
4. The medal table order in the application and in `README.md` must match.
