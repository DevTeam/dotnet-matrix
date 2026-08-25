# CSV Processing feature contract

## Scope

The category compares maintained .NET CSV libraries using canonical UTF-8
fixtures. Every operation creates the library reader or writer inside the
benchmark invocation. Fixture loading, deterministic large-corpus generation,
and immutable input model creation happen before measurement.

All inputs use comma as the separator, UTF-8 without a byte-order mark,
invariant-culture numbers, and LF line endings. Readers must accept the LF
fixtures exactly as stored. Writers must emit the exact canonical LF payload.
Each supported operation must use the compared library's intended CSV API;
splitting lines or fields in matrix-owned code does not count as support.

The small corpus has a four-column header (`Id`, `Name`, `Amount`, `Active`) and
three data records. The large corpus has the same schema and 10,000
deterministically generated records. Setup and validation are excluded from
measurement.

## 1. Read Simple Rows

- Operation: parse the small corpus and materialize three raw rows.
- Output: a `RawCsvRow[3]` containing all four fields as strings.
- Inside invocation: reader construction, parsing, string extraction, row
  object construction, and array materialization.
- Validation: exact row count and exact field text, including decimal scale and
  boolean casing from the fixture.
- Supported: the library parses the fixture through its CSV reader and returns
  the exact rows.
- Group: `read`.

## 2. Read Typed Records

- Operation: parse and materialize the small corpus as `CsvRecord[3]`.
- Output fields: `int Id`, `string Name`, `decimal Amount`, and `bool Active`.
- Inside invocation: reader construction, parsing, primitive conversion,
  record construction, and array materialization.
- Validation: every typed property of every record.
- Supported: typed field access or the library's record binding produces the
  exact records using invariant culture.
- Group: `read`.

## 3. Read Large Dataset

- Operation: parse and materialize all 10,000 large-corpus rows as
  `CsvRecord[]`.
- Inside invocation: reader construction, parsing, primitive conversion,
  record construction, list growth, and final array materialization.
- Validation: length, first and last record, ID sum, amount sum, and active
  count.
- Supported: the complete dataset is materialized with exact values.
- Group: `throughput`.

## 4. Quoted Fields

- Operation: parse two rows whose text fields contain doubled quote escapes.
- Output: `TextCsvRow[2]` with quotes unescaped exactly once.
- Inside invocation: reader construction, quote handling, string extraction,
  row construction, and materialization.
- Validation: exact IDs and text (`Ada "Countess"` and `"quoted"`).
- Supported: the library's quote/unescape mode returns the exact logical text.
- Group: `correctness`.

## 5. Escaped Delimiters

- Operation: parse two rows containing a comma and an LF newline inside quoted
  fields.
- Output: `TextCsvRow[2]`.
- Inside invocation: reader construction, delimiter/newline handling, string
  extraction, row construction, and materialization.
- Validation: exact IDs and text (`Ada, Lovelace` and `line one\nline two`).
- Supported: neither the embedded separator nor embedded newline splits a
  logical field or record.
- Group: `correctness`.

## 6. Header Mapping

- Operation: parse the three typed records from a fixture whose columns are
  reordered as `Active,Amount,Name,Id`.
- Inside invocation: reader construction, header lookup or binding, primitive
  conversion, record construction, and materialization.
- Validation: the same typed values as the canonical small corpus.
- Supported: values are selected by header name, not by the original ordinal.
- Group: `correctness`.

## 7. Custom Conversion

- Operation: parse two `Code` fields into the matrix-owned `ProductCode` value
  type, whose wire format is `sku-NNNN`.
- Inside invocation: reader construction, CSV parsing, converter registration
  when required by the library, custom conversion, and array materialization.
- Validation: exact numeric values 42 and 73.
- Supported: the library exposes an intended custom or generic conversion path
  that creates `ProductCode` values. Calling matrix parsing after retrieving a
  plain string is not sufficient.
- Unsupported: the CSV package has no conversion extension point capable of
  producing the custom type.
- Group: `correctness`.

## 8. Streaming Read

- Operation: consume the 10,000-row corpus once without retaining records and
  calculate `CsvAggregate`.
- Output: count, ID sum, amount sum, and active count.
- Inside invocation: reader construction, forward-only parsing, typed field
  access, and aggregation.
- Validation: the exact deterministic aggregate.
- Supported: the library exposes row-by-row forward-only consumption and the
  implementation does not materialize the dataset.
- Group: `throughput`.

## 9. Write Rows

- Operation: write three immutable `CsvWriteRecord` values with `Id` and `Name`
  columns to a new in-memory text destination.
- Output: exactly `Id,Name\n1,Ada\n2,Grace\n3,Linus\n`.
- Inside invocation: writer and destination construction, header writing, row
  writing, flushing/disposal, and extraction of the resulting string.
- Validation: ordinal equality with the canonical payload, including header,
  column order, LF newline, and final newline.
- Supported: the library's CSV writer emits the exact payload.
- Group: `write`.

## 10. Async Read

- Operation: asynchronously consume and aggregate the 10,000-row corpus using
  the library's asynchronous reader API.
- Output: the same `CsvAggregate` as Streaming Read.
- Inside invocation: reader construction, asynchronous row reads, typed field
  access, and aggregation.
- Validation: the exact deterministic aggregate.
- Supported: the library exposes and the scenario invokes a genuine async read
  API; wrapping a synchronous parser in `Task.Run` is not support.
- Caveat: the in-memory source does not model external I/O scheduling, so the
  measurement is the cost of the asynchronous path itself, not of real I/O.
- Group: `throughput`.

## Availability meanings

- `Supported`: the implementation passes all assertions above.
- `Unsupported`: the library lacks the required semantic or extension point.
- `NotApplicable`: reserved for a feature that has no meaningful equivalent;
  no initial feature is expected to use it.
- `Failed`: an implementation claims support but validation fails.

