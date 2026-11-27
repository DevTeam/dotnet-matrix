# PDF Processing feature contract

## Scope

The category compares .NET libraries that read and write PDF documents over the
same in-memory bytes. PdfPig, PDFsharp and iText7 do both; Docnet.Core and
PdfSharpCore read only; QuestPDF writes only. The split is the point of the
category: a PDF library that only reads cannot be compared with one that only
writes on a single operation, so every scenario names the libraries that can
express it and the others record an explicit availability declaration.

Disk and network I/O, rendering to raster images, digital signatures,
certificates, form filling, PDF/A conversion, JavaScript actions, and any
operation whose result depends on a system font, colour profile, or external
resource are outside scope. Nothing touches the file system: documents are
produced and consumed as `byte[]` in memory, because a file system benchmark
would measure the storage device rather than the library.

## Participants

| Library | Role | Package |
| --- | --- | --- |
| PdfPig | reader and writer | `PdfPig` |
| PDFsharp | reader and writer | `PDFsharp` |
| PdfSharpCore | reader | `PdfSharpCore` |
| Docnet.Core | reader | `Docnet.Core` |
| QuestPDF | writer | `QuestPDF` |
| iText7 | reader and writer | `itext7` |

## Shared fixture

Reading scenarios consume one deterministic document that is built once before
measurement and never inside it: `PdfFixture`, three A4 pages, each with a single
heading line and twenty body lines of fixed ASCII text, plus the document
information title `Matrix PDF Fixture`, author, subject and creator. It is
produced with iText7's standard-14 Helvetica writer (see the encoding note below)
and is verified to open with every reader in the category during validation.

The writer scenarios produce **their own** document from a shared page model,
`PdfPages`: a fixed description of page count and of the lines and table cells
each page carries. No library writes another library's bytes: a writer receives
the model and emits a document, and validation reads that document back. Because
the bytes differ per library, write scenarios are compared on the work each
library performs for the same model, not on byte-identical output.

## Scenarios

1. **Read Page Count** opens the fixture bytes and reports how many pages the
   document declares. Opening a document is the measured work; nothing is
   extracted from a page. Most libraries can answer from the document catalog,
   so this is the cheapest possible read.
2. **Read Metadata** opens the fixture and reads the document title and author
   from the document information dictionary. A library that needs a full parse
   to reach the information dictionary pays for it here; the scenario does not
   wrap that cost away.
3. **Extract Page Text** opens the fixture and extracts the text of page one.
   Extraction includes the content stream decoding the library performs to reach
   the glyphs, so a library with pre-parsed letters and one that runs a
   full text engine are compared on what each does for the same page.
4. **Write Text Document** produces the one-page model as a document: a page,
   a heading line, and the body lines at fixed leading. The measured work is
   everything from an empty document to the final `byte[]`, including save and
   stream finalization, because a document that exists only in memory is not a
   PDF anyone can use.
5. **Write Table Document** produces the one-page model whose content is a
   thirty-row, three-column table with a header row. This scenario exists to
   separate layout engines from text placement: a library with a table
   abstraction pays for its layout algorithm, a library that positions cells
   itself pays for its own arithmetic.
6. **Write Multi Page Document** produces the four-page model, each page carrying
   its own lines. It measures per-page overhead rather than per-document
   overhead, which is what a report generator actually costs.

## Support conditions

- **Supported** means the library performs the exact operation through an API
  intended for it, and passes every assertion below.
- **Unsupported** means the library has no API for the required behaviour. The
  reason names the missing capability, not a missing wrapper.
- **NotApplicable** is used only where an operation has no meaning for a library
  by design, and never as a substitute for Unsupported.
- A library that claims a scenario but fails its assertions reports **Failed**,
  which is a defect in the adapter.

### Reading scenarios

PdfPig, Docnet.Core and iText7 claim all three reading scenarios. PDFsharp and
PdfSharpCore claim the two document-level ones and cannot claim text extraction:
neither package exposes any text extraction API (its `XTextFormatter` only lays
text out when writing), so a wrapper would have to implement extraction itself,
which would measure the adapter instead of the library. QuestPDF claims none:
it composes documents and exposes no reader at all. A library is only allowed
to claim a scenario if the validation run proves the observable result, not
merely that a call returned.

- **Read Page Count** must report exactly three pages.
- **Read Metadata** must return the fixture title and author exactly. A library
  whose information dictionary reader returns the strings unchanged passes; a
  library that returns them with surrounding whitespace or as an object whose
  `ToString` is not the value fails, and the adapter must normalize explicitly.
- **Extract Page Text** must return text that contains the fixture heading and at
  least the first body line. The assertion is deliberately about content
  presence rather than an exact string: whitespace and line-break placement are
  library decisions, and asserting one would measure formatting policy instead
  of extraction. The extracted length must also be non-trivial, which catches a
  reader that returns an empty string successfully.

### Writing scenarios

QuestPDF, PdfPig, iText7 and PDFsharp claim all three writing scenarios. PdfPig
writes with its standard-14 builder, so it needs no font program and is the one
writer that behaves identically on a machine with no font at all; PDFsharp draws
table cells itself and therefore pays for its own arithmetic exactly as the
scenario describes. Docnet.Core and PdfSharpCore claim none: Docnet.Core
exposes no document authoring API at all, and PdfSharpCore is compared as a
reader only, because its writing API is the PDFsharp 1.x drawing code it was
forked from, so benchmarking it against PDFsharp 6 would compare a package with
its own ancestor rather than two independent implementations. Those declarations
are per scenario and state the reason.

Every written document is validated by reading it back with PdfPig, an
independent reader that is itself a category participant:

- the page count must equal the model's page count;
- the extracted text must contain every line the model specifies, so a document
  that silently dropped content fails;
- the byte array must be non-empty and must start with the `%PDF-` header, which
  catches a writer that returned a stream position or an empty buffer.

## PDFsharp and its font program

PDFsharp 6.5 refuses to draw text without a font program it can resolve: with no
resolver configured, `XFont` construction throws `InvalidOperationException`, and
before that the package refuses to work at all until `PdfSharpLicense.SetAgplLicense()`
is called. The category calls `SetAgplLicense` once, because the project accepts
the AGPL for this category (the licence decision is recorded in
`workflows/category-roadmap.md`).

For the font, the category installs a resolver that reads the first available
system font from a fixed, documented list of paths covering Windows, Debian and
Ubuntu, Alpine, and macOS, and it applies a fixed candidate family rather than
depending on a name the machine happens to have. When no candidate font file
exists, the three PDFsharp writing scenarios fail validation with an explicit
`No system font program was found for PDFsharp` reason. They are not declared
`Unsupported`, because the capability is present in the library and only the
environment lacks the resource: a `FeatureUnavailable` declaration would publish
a claim about PDFsharp that is false on any machine that has a font. The
bootstrap is applied once, before any measurement, and never inside a benchmark
method.

The GitHub Actions reports runner is `ubuntu-latest`, whose base image ships the
DejaVu and Liberation fonts at the paths the resolver searches, so the resolver
is expected to find a font there. This is not asserted as a guarantee: the
validation run is the authority.

## QuestPDF licence

QuestPDF 2026 requires a licence selection before first use and throws
otherwise. The category selects `LicenseType.Community` once at bootstrap, which
is the licence QuestPDF grants to the use this project makes of it. The same
bootstrap statement records the reason.

## iText7 and the fixture's font encoding

iText7's text extractor cannot read a font resource that declares
`/StandardEncoding`: it throws `'StandardEncoding' is not a supported encoding
name`, and registering `CodePagesEncodingProvider` does not change that, because
the name is not an encoding .NET knows at all. The validation run reported exactly
this against a PdfPig-written fixture, whose standard-14 writer emits
`/StandardEncoding`. iText7's own standard-14 writer emits `/WinAnsiEncoding`
instead, and every reader in the category opens that output and extracts the text.

The fixture is therefore written with iText7 rather than PdfPig. This is a
compatibility constraint recorded on purpose: the alternative was to let one
participant fail every reading scenario for a reason unrelated to its speed, or
to declare its text extraction Unsupported when the library extracts text from
ordinary documents perfectly well.

## Rating groups

Chart groups follow the operation, and only complete, comparable sets are rated:

- **Read**: the three reading scenarios.
- **Write**: the three writing scenarios.

No group mixes a reading library with a writing one, because a library that has
no document authoring API would score zero for a reason that says nothing about
its quality as a reader. The consequence is visible and intended: a reader-only
library is rated only in **Read**, and appears in **Write** as not ranked, while
a writer-only library is the reverse. Every scenario is rated within its group,
including the ones a single library dominates, and `MatrixFeature` is not marked
`rated: false` anywhere in this category.

`Supported` and `Unsupported` remain published per library per scenario exactly
as in every other category, so what a library cannot do is visible next to what
it costs to do what it can.

## Deferred scenarios

- **Merge and split** were considered as a seventh scenario and deliberately
  deferred. The set of libraries that can merge is small, and the semantics of
  page ordering and metadata inheritance differ enough between them that a fair
  contract would need its own document describing each output, which is more
  than a single scenario can carry honestly.
- **Encrypted documents, PDF/A, form filling, image rendering, digital
  signatures, and stream-based I/O** are deferred. Each needs a prepared
  credential, a validating consumer, or an external resource, and none of them
  can be asserted inside an in-memory hot path.
- **Font subsetting and embedding** is deferred: its result depends on the font
  program, so it would measure the font file as much as the library.

## Caveats

- **Write scenarios are resource-bound as well as library-bound.** Two of the
  participants compress their output at different default levels, and the byte
  counts are not compared, so a library that compresses less thoroughly will look
  faster. The scenario description states what is measured (a document produced
  end to end), and this caveat states what that includes, rather than claiming a
  layout-only comparison the libraries cannot provide.
- **Extract Page Text is not an OCR scenario.** None of the participants performs
  character recognition, so a scanned document would yield no text for any of
  them; the fixture is a generated document with a real text layer.
- **The fixture is written by a participant.** iText7 produces the reading
  fixture, which means the reading comparison uses one of the participants'
  output as its input. This is stated plainly rather than hidden: the choice is
  forced by the font-encoding incompatibility described above, the fixture is
  verified against every other reader during validation, and a reader that
  disagrees with it fails validation instead of quietly measuring a different
  document.
