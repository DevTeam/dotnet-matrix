# CLI Parsing feature contract

## Scope

The category compares maintained .NET command-line parsing libraries that turn an
argument array into typed values. Only in-process parsing is measured: no console
I/O, no process launch, no interactive mode and no file access. Every
implementation receives the same argument arrays and must produce the same
observable values.

The shared command model is deliberately **one command with options and
positional arguments**. Most libraries in this category have no notion of
subcommands at all, so requiring them would compare a minority feature instead of
the parsing work every library performs. The command model is therefore:

```text
form A: <output> --input <path> [--format <name>] [--verbose] [--tag <value> ...]
form B: <input> --level <1-9>
```

Two independent flat command definitions are used, and each scenario names the
one it parses. Neither definition has subcommands or verb dispatch, and the parsed
argument array therefore contains only options and positional values: no command
word is prepended. Form A below is called the `report` form and form B the
`compact` form for identification only.

The positional value comes first in both forms. Several libraries end an option's
value only at the next token that starts with a dash, so a positional value placed
after an option is consumed as that option's value; putting the positional value
first keeps the arrays parseable by those libraries without changing what is
compared.

`report` form:

- `--input` is a required option that takes a value.
- `--format` is an optional option that takes a value.
- `--verbose` is a boolean switch.
- `--tag` accepts repeated occurrences and collects them in order.
- `<output>` is a required positional argument.

`compact` form:

- `--level` is a required integer option.
- `<input>` is a required positional argument.

The reported scenarios pass only the arguments their own command definition
declares, so a required member is never omitted by accident and an error scenario
is the only place where a required member is missing on purpose.

Argument arrays are static constants. The parser, command definition or options
type is created in setup and excluded from measurement, because a real
application builds it once at startup. The measured method performs one parse of
the fixed array and extracts the values named by its scenario.

Error scenarios measure the library's own failure reporting. A library that
returns a partially populated model instead of reporting a failure does not
support the scenario: silently accepted invalid input is a different observable
result. A library that throws and a library that returns a diagnostic collection
are both acceptable, because both report the failure; each adapter converts its
own reporting style into the shared result type.

## Scenarios

## 1. Parse Option Value

- Operation: parse `out.txt --input in.txt --format json` with the `report` form.
- Output: `ReportArguments` carrying the `--input` value `in.txt`, the `--format` value `json` and the positional `out.txt`.
- Inside invocation: option lookup, value binding, positional binding and the construction of the category's result type.
- Validation: all three values are exactly the expected ones.
- Supported: the library parses the array and exposes each value through its own typed API.
- Group: `parse`.

## 2. Parse Positional Argument

- Operation: parse `in.txt --level 5` with the `compact` form.
- Output: `CompactArguments` carrying the positional value `in.txt` and the level `5`.
- Inside invocation: positional binding, option binding, integer conversion and the construction of the result type.
- Validation: the positional value and the level are exactly the expected ones.
- Supported: the library binds a positional argument of a command that has no subcommand, without a command word in the array.
- Group: `parse`.

## 3. Parse Repeated Option

- Operation: parse `out.txt --input in.txt --format json --tag a --tag b --tag c` with the `report` form. The positional argument precedes the repeated option so that a library which lets a repeated option consume several following tokens cannot swallow it.
- Output: `TagArguments` carrying the three tag values in order.
- Inside invocation: every occurrence of the repeated option, its collection and the copy of the values into the result.
- Validation: the sequence is exactly `a`, `b`, `c`.
- Supported: the library collects repeated occurrences of one option in order. A library that collects several values only through a separator character, or that keeps only the last occurrence, has no repeated-option behaviour.
- Group: `options`.

## 4. Parse Switch

- Operation: parse the `report` form twice, once with `--verbose` and once without it.
- Output: `SwitchArguments` carrying the switch state and the positional argument of each parse, compared against the expected on and off results.
- Inside invocation: both parses with their switch binding and positional binding.
- Validation: the switch is on for the first array and off for the second, and the positional value is the same in both.
- Supported: the library reports the declared switch state of each array, so a library that always reports true does not pass.
- Group: `options`.

## 5. Bind Typed Model

- Operation: parse `in.txt --level 5` with the `compact` form into a typed model with an `int` level and a `string` input.
- Output: `CompactArguments` built from that model.
- Inside invocation: model or command instance construction, binding of both members and value conversion.
- Validation: the integer level and the string input are exactly the expected ones, so a parse that leaves the members at their defaults does not pass.
- Supported: the library binds the command line to a typed model of the command without a command word in the array.
- Group: `parse`.

## 6. Generate Help

- Operation: build the help text of the `report` command.
- Output: `GeneratedHelp` carrying the produced text.
- Inside invocation: whatever the library's public help entry point performs; no scenario renders to a device beyond the writer the library itself requires.
- Validation: the text is not blank and names at least one declared option. Exact wording, layout and the presence of the command name are not compared, because libraries differ in what their generated header contains.
- Supported: the library produces help text through its own public API.
- Group: `help`.

## 7. Error Unknown Option

- Operation: parse `out.txt --input in.txt --format json --bogus` with the `report` form.
- Output: `ParseOutcome` carrying `false` and the reported diagnostic.
- Inside invocation: the parse up to the point where the library reports the failure.
- Validation: the parse did not succeed and a non-blank diagnostic was reported.
- Supported: the library rejects an option it does not declare. Accepting it silently does not count.
- Group: `errors`.

## 8. Error Missing Required Value

- Operation: parse `out.txt --format json` with the `report` form, where the required `--input` is absent.
- Output: `ParseOutcome` carrying `false` and the reported diagnostic.
- Inside invocation: the parse up to the point where the library reports the failure.
- Validation: the parse did not succeed and a non-blank diagnostic was reported.
- Supported: the library reports the absent required option instead of binding a default value.
- Group: `errors`.

## 9. Error Invalid Type

- Operation: parse `in.txt --level many` with the `compact` form.
- Output: `ParseOutcome` carrying `false` and the reported diagnostic.
- Inside invocation: the parse up to the failed integer conversion.
- Validation: the parse did not succeed and a non-blank diagnostic was reported.
- Supported: the library reports the unparsable integer value instead of silently substituting a default.
- Group: `errors`.

## 10. Error Missing Required Argument

- Operation: parse `--level 5` with the `compact` form, without the required positional argument.
- Output: `ParseOutcome` carrying `false` and the reported diagnostic.
- Inside invocation: the parse up to the point where the library reports the failure.
- Validation: the parse did not succeed and a non-blank diagnostic was reported.
- Supported: the library reports the absent required positional argument. A command whose positional member is optional with a default value is a different observable result and does not count.
- Group: `errors`.

## Setup and measurement

- The argument arrays, the typed model and any parser or command definition are
  created before the measured method runs.
- The measured method performs one parse of the fixed array, extracts the values
  named for its scenario, and returns them as the category's strongly typed
  result.
- Validation assertions run inside the measured method only in Validation mode.
- No scenario measures help rendering to a device, only building the text.

## Support conditions

- `Supported` means the library parses the exact argument array, reports the
  values required by the scenario through its own typed API, and passes every
  assertion.
- `Unsupported` means the library has no API that produces the required
  observable result, for example no repeated-option collection, no generated help
  text, or no failure reporting for an input kind. The reason names what is
  missing.
- `NotApplicable` is used only when the scenario has no meaningful equivalent for
  a library.
- An error scenario is `Supported` when the library reports a failure for that
  input through its documented error mechanism. Throwing and returning errors
  both count. Accepting the input silently does not count.

## Deferred scenarios

- Subcommand dispatch, response files and environment-variable fallbacks.
- Shell completion, configuration file binding and help localization.
- Interactive or long-running command execution, including console output
  capture; a parse that requires executing the command is measured only when the
  library provides no parse-only API, and that restriction is stated per library
  in its implementation comment.
