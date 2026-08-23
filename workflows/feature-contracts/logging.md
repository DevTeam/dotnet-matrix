# Logging feature contract

## Scope

The category compares local logging APIs without console, file, database, or
network I/O. Synchronous rated features send events to a library-specific
in-memory sink. In benchmark builds the sink performs only the minimum event
acceptance work; validation-only capture records semantic data without adding
normalization allocations to benchmark measurements.

Every steady-state logger, sink, template delegate, and configuration is
prepared before measurement. The benchmark invocation contains the logging API
call and any scope or context lifetime explicitly required by the feature.
Validation checks the native event received by the sink, normalized after the
measured call.

The category uses Information for normal events and Error for exceptions.
Disabled loggers have a Warning minimum level. All cultures are invariant.
Application category is `Matrix.Logging`.

## 1. Disabled Log

- Operation: submit the Information message `Suppressed message` to a logger
  whose minimum enabled level is Warning.
- Inside invocation: the direct logging API call, including its level check.
- Validation: the in-memory sink receives no event.
- Supported: Information is disabled and the call performs no sink delivery.
- Rating: `core`.

## 2. Simple Message

- Operation: submit the literal Information message `Order accepted`.
- Inside invocation: the direct logging call and synchronous sink delivery.
- Validation: exactly one latest event has Information level, the exact
  rendered message, and no exception.
- Supported: the sink observes the exact event.
- Rating: `core`.

## 3. Structured Properties

- Operation: submit `Order {OrderId} completed in {ElapsedMs} ms` with integer
  values 42 and 17 through the library's intended structured-event API.
- Inside invocation: template/property binding, event creation, and
  synchronous sink delivery.
- Validation: rendered message `Order 42 completed in 17 ms` and native event
  properties `OrderId=42` and `ElapsedMs=17`. The original named template is
  also checked when the library retains it natively.
- Supported: both named values remain independently queryable in the received
  event; pre-rendering a plain string is not support.
- Rating: `structured`.

## 4. Exception

- Operation: submit one pre-created `InvalidOperationException` at Error level
  with message `Order failed`.
- Inside invocation: the direct exception logging call and synchronous sink
  delivery.
- Validation: Error level, exact rendered message, and reference identity of
  the exception.
- Supported: the exception is retained as exception metadata rather than
  concatenated into the message.
- Rating: `structured`.

## 5. Scope Or Context

- Operation: create a temporary scope/context property
  `RequestId=req-42`, submit `Inside request`, and close the scope.
- Inside invocation: scope/context creation, logging, and scope/context
  disposal.
- Validation: the received event exposes `RequestId` with the exact value and
  the rendered message is exact.
- Supported: context is captured on the event and does not remain active after
  the invocation.
- Rating: `structured`.

## 6. Template Rendering

- Operation: render one parameterized Information event for amount 12.5 and
  customer `Ada` using the library's deferred template/interpolation API.
- Expected output: `Total 12.50 for Ada`.
- Inside invocation: parameter binding, invariant formatting, event creation,
  rendering, and synchronous sink delivery. Template parsing may be prepared
  only when the library exposes an explicit reusable-template API.
- Validation: exact rendered text. When the native event retains a template,
  it must match the library-appropriate parameterized input.
- Supported: the logging API receives the unrendered values and produces the
  exact invariant output; matrix-owned pre-rendering is not support.
- Rating: `structured`.

## 7. Buffered Logging

- Operation: enqueue the Information message `Buffered event` to a
  library-provided asynchronous or buffering wrapper around an in-memory sink.
- Inside invocation: only event creation and successful enqueue/buffer
  acceptance.
- Outside invocation: setup creates the wrapper; cleanup flushes or disposes
  it, waits for completion, and validates all three validation invocations.
- Validation: after flush, the sink has received three exact Information
  events with no loss or duplication.
- Supported: the library or its official companion package provides a bounded
  asynchronous queue or explicit event buffer. `Task.Run` around synchronous
  logging is not support.
- Unsupported: no async/buffering facility is provided.
- Rating: feature-only because enqueue completion is not equivalent to
  synchronous sink delivery.

## 8. Prepare Logger

- Operation: construct a logger configuration with one in-memory sink and
  Information minimum level, obtain the named logger, verify it is enabled,
  and release all configuration resources.
- Inside invocation: configuration objects, sink/provider registration,
  logger creation, enabled-level check, and deterministic disposal where the
  API supports it.
- Validation: the method reports that Information is enabled.
- Supported: a complete usable logger is created and released inside one
  invocation.
- Rating: `prepare`.

## 9. Formatted Output

- Operation: submit the Information message `Formatted event` through the
  library's own output formatter into a bounded in-memory sink.
- Inside invocation: event creation, layout/template formatting, and
  synchronous delivery of the formatted text or UTF-8 bytes to the sink.
- Outside invocation: setup builds the formatter with a library-appropriate
  timestamp/level/logger/message layout; cleanup validates all three
  validation invocations.
- Validation: the sink has received three formatted records and the last one
  ends with `Formatted event`.
- Supported: the library defines a text output formatter that the matrix can
  drive synchronously; matrix-owned formatting is not support.
- Unsupported: the library defines no comparable output formatter. This is
  the case for Microsoft.Extensions.Logging, whose core package ships none,
  and for OpenTelemetry, which exports log records instead.
- Rating: `structured`.

## Availability meanings

- `Supported`: every observable assertion above passes.
- `Unsupported`: the library lacks the required semantic or buffering
  extension point.
- `NotApplicable`: reserved for features with no meaningful equivalent; no
  initial feature is expected to use it.
- `Failed`: an implementation claims support but semantic validation fails.
