# Message dispatch feature contract

## Scope

The category compares in-process message dispatch libraries: a message object is
handed to a dispatcher that resolves the handler for it, runs it and returns its
result. No transport, broker, queue, scheduler, remote call or persistence is
involved, and every implementation runs in the calling process and thread.

A library enters the category only when it dispatches messages between types of
the same application. Frameworks whose primary abstraction is a message endpoint
over a transport (Rebus, Paramore.Brighter, MassTransit) are a different level of
abstraction and are out of scope even where they can dispatch locally.

The shared model is one request type with one handler, one request type with a
handler that returns a value, and one notification type with three handlers. The
message classes, handlers and dispatcher are library specific, because the
interfaces and registration calls differ; only the observable results are
compared. Handlers increment a static counter so that a dispatch which resolves a
handler but never runs it is visible.

The dispatcher, container or generated registry is created in setup and excluded
from measurement, exactly as an application does it once at startup. The measured
method dispatches one fixed message instance and returns the category result.

## Scenarios

## 1. Send Command

- Operation: dispatch one command message whose handler has no return value.
- Output: `CommandResult` carrying the number of handler invocations observed.
- Inside invocation: resolving the handler for the message type, and running it.
- Validation: the count is exactly one.
- Supported: the library dispatches a request that has no response and runs its single handler.
- Group: `dispatch`.

## 2. Send Query

- Operation: dispatch one request whose handler returns an integer answer.
- Output: `QueryResult` carrying the answer and the handler invocation count.
- Inside invocation: resolving the handler, running it and returning its value to the caller.
- Validation: the answer is the value the handler computes from the request and the count is one.
- Supported: the library returns a handler's result to the caller through a strongly typed API.
- Group: `dispatch`.

## 3. Publish Notification

- Operation: publish one notification to three handlers registered for the same notification type.
- Output: `NotificationResult` carrying the invocation count.
- Inside invocation: resolving every handler for the notification and running all of them.
- Validation: the count is exactly three, so a dispatcher that runs only the first or the last handler does not pass.
- Supported: the library fans one message out to several handlers of the same type.
- Group: `notification`.

## 4. Pipeline Behavior

- Operation: dispatch one request through one registered pipeline behavior that observes the message before the handler and passes the result through.
- Output: `BehaviorResult` carrying the answer and whether the behavior observed the message.
- Inside invocation: building the behavior and handler chain and running it.
- Validation: the answer is unchanged and the behavior observed the message exactly once.
- Supported: the library lets application code wrap request handling in its own behavior type.
- Group: `pipeline`.

## 5. Exception Behavior

- Operation: dispatch one request whose handler throws, with the library's exception handling or wrapping extension point registered.
- Output: `ExceptionResult` carrying the recovered answer and whether the failure was handled.
- Inside invocation: running the handler, catching its exception in the extension point and producing a result.
- Validation: the dispatch returned normally with the recovered answer and the failure is reported as handled.
- Supported: the library offers a documented, per-request way to handle a handler exception and continue.
- Group: `pipeline`.

## 6. Async Handler

- Operation: dispatch one request whose handler completes asynchronously.
- Output: `AsyncResult` carrying the answer and the invocation count.
- Inside invocation: awaiting the handler's task and completing the dispatch.
- Validation: the answer is the expected one and the count is one.
- Supported: the library awaits an asynchronous handler and returns its result.
- Group: `dispatch`.

## 7. Source Generated Dispatch

- Operation: dispatch one request through a registry that a compile-time source generator produced for this application.
- Output: `GeneratedResult` carrying the answer and the invocation count.
- Inside invocation: the generated dispatcher's own call path.
- Validation: the answer is the expected one and the count is one.
- Supported: the library generates its dispatch code at compile time instead of resolving handlers with runtime reflection.
- Group: `generated`.

## 8. Register

- Operation: create the library's container or dispatcher, register the handlers of this category and resolve the dispatcher.
- Output: `RegistrationResult` carrying whether the dispatcher was created and answers one dispatch.
- Inside invocation: building the container, registering handlers and resolving the dispatcher.
- Validation: the dispatcher is created and one dispatch through it returns the expected answer.
- Supported: the library provides a registration entry point that wires its own handlers without manual per-handler code.
- Group: `setup`.

## Setup and measurement

- Message instances, the dispatcher, the container and any generated registry are
  created before the measured method runs. Registration is measured by scenario 8
  only, which is why it is a scenario of its own.
- The measured method dispatches the fixed message once and returns the category's
  strongly typed result for its scenario.
- Validation assertions run inside the measured method only in Validation mode.
- Dispatch never crosses a process, a thread pool boundary other than the
  handler's own asynchronous continuation, a file or a network.

## Support conditions

- `Supported` means the library dispatches the exact message through its own typed
  API, produces the observable result the scenario names and passes every
  assertion.
- `Unsupported` means the library has no API that produces the required observable
  result, for example no notification fan-out, no application-defined behavior, or
  no exception handling extension point. The reason names what is missing.
- `NotApplicable` is used only when the scenario has no meaningful equivalent for a
  library.

## Deferred scenarios

- Transport, broker and remote dispatch, outbox and inbox patterns, durable
  messaging, scheduling and retries.
- Streaming responses, cancellation propagation, scoped handler lifetimes and
  handler lifetime configuration.
- Handler discovery rules, open generic handlers and conditional handler
  selection.
