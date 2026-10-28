# Template Engines feature contract

## Scope

The category compares .NET libraries that turn a template and a model into
text. Two kinds are compared, and the difference is stated here rather than
split into separate chart groups:

- Interpreting engines parse a template once and walk the parsed tree on every
  render. Handlebars.Net, Scriban, Fluid, DotLiquid, Stubble and Cottle belong
  here.
- Compiling engines translate the template into C# and compile it with Roslyn.
  RazorLight, RazorEngineCore and Razor.Templating.Core belong here.

A compiling engine is naturally orders of magnitude more expensive in Parse and
usually cheaper in Render, and both numbers are honest. Keeping the two apart in
the chart groups would hide that trade-off, so the groups are formed by
operation and the nature of the work is described here.

The greeting text is the same for every engine: Hello, World!. Model shapes
differ, because a Razor engine resolves C# member names while a mustache engine
reads a dictionary key; that difference is preparation, not work under test.

## Scenarios

### Parse

- Group: `parse`.

Turns template source into the engine reusable template and returns it. No
render happens inside the measured delegate, so the result measures preparation
only. Validation requires a non-null template.

The overhead of a Roslyn compile belongs to this scenario by design and is not
spread over Render.

Stubble is Unsupported: it parses into an internal token stream behind its
cached parser and exposes no public pre-parsed template. SmartFormat is
NotApplicable: it composes a string from a format string and keeps no parsed
document. RazorLight is Unsupported: it compiles and caches templates by name
and cannot turn template text into a reusable template without rendering.
Razor.Templating.Core is Unsupported: it renders a view by name through the
ASP.NET Core view engine and has no template string API at all.
### Render

- Group: `render`.

Renders a template prepared in setup with a model and returns the text.
Validation requires Hello, World! exactly.

The template, the model and the engine are built in setup for every library, so
no parsing or compilation is measured here. Razor.Templating.Core is Unsupported
for the same reason as in Parse: there is no template string to prepare.

### Escape

- Group: `render`.

Renders a value that contains markup and returns the text. Validation requires
the HTML-escaped form of that value, which can only be produced by escaping.

Handlebars.Net, Stubble and RazorLight escape by default in the plain
interpolation form and are measured here. Scriban, Fluid, DotLiquid, SmartFormat,
Cottle and RazorEngineCore are NotApplicable: they write the value verbatim and
escaping would need an explicit filter or encoder, which is a different scenario.
Razor.Templating.Core is Unsupported.

## Availability rules

- Supported means the library performs the exact operation through its own API
  and passes every assertion.
- Unsupported means the library cannot provide the required behavior at all.
- NotApplicable means the operation has no meaningful equivalent for that kind
  of engine. The reason is recorded per library and feature.
- Failed means a benchmark method exists but validation does not pass.

## Preparation and measurement

Template source, models, engine instances and every parsed or compiled template
are created in setup, outside the measured delegate. Only Render and Escape run
per iteration, which is why they can be compared across interpreting and
compiling engines. Validation helpers are compiled only under MATRIX_VALIDATION.

## Excluded

Layout and partial resolution, file system loaders, view discovery, model
type conversion beyond a single string property, and asynchronous rendering are
out of scope: each needs a host, a file system or a framework pipeline that the
matrix cannot reproduce identically for every engine.

## Rating ceiling

Three rated scenarios put the category maximum at 200 x 3 = 600 points.
