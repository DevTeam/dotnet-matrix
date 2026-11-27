namespace Matrix.PdfProcessing;

/// <summary>
/// Semantic checks for every published result. They run only in the validation
/// variant, so the benchmark assembly carries no assertion machinery on its hot
/// path. See <c>workflows/feature-contracts/pdf-processing.md</c>.
/// </summary>
internal static class PdfChecks
{
    [Conditional("MATRIX_VALIDATION")]
    public static void PageCount(string library, int actual) =>
        MatrixValidation.Require(
            library,
            actual == PdfFixture.PageCount,
            $"Expected {PdfFixture.PageCount} pages, found {actual}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void Metadata(string library, string? title, string? author)
    {
        MatrixValidation.Require(
            library,
            string.Equals(title?.Trim(), PdfFixture.Title, StringComparison.Ordinal),
            $"Expected title '{PdfFixture.Title}', found '{title}'.");
        MatrixValidation.Require(
            library,
            string.Equals(author?.Trim(), PdfFixture.Author, StringComparison.Ordinal),
            $"Expected author '{PdfFixture.Author}', found '{author}'.");
    }

    /// <summary>
    /// Text presence, not an exact string: whitespace and line-break placement are
    /// library decisions, and asserting one would measure formatting policy.
    /// </summary>
    [Conditional("MATRIX_VALIDATION")]
    public static void Extracted(string library, string? text)
    {
        var value = text ?? string.Empty;
        MatrixValidation.Require(library, value.Length > 100, $"Extracted {value.Length} characters, which is too little for a full page.");
        MatrixValidation.Require(library, value.Contains(PdfFixture.Heading, StringComparison.Ordinal), $"Extracted text does not contain '{PdfFixture.Heading}'.");
        MatrixValidation.Require(library, value.Contains("Row 00", StringComparison.Ordinal), "Extracted text does not contain the first body line.");
    }

    /// <summary>
    /// Every written document is read back with PdfPig, an independent reader that
    /// is itself a participant, so a document that dropped content fails here.
    /// </summary>
    [Conditional("MATRIX_VALIDATION")]
    public static void Document(string library, byte[] document, int pages, IReadOnlyList<string> requiredText)
    {
        MatrixValidation.Require(library, document.Length > 0, "The writer produced no bytes.");
        MatrixValidation.Require(
            library,
            document.AsSpan().StartsWith("%PDF-"u8),
            "The writer produced bytes without a %PDF- header.");
        using var parsed = UglyToad.PdfPig.PdfDocument.Open(document);
        MatrixValidation.Require(
            library,
            parsed.NumberOfPages == pages,
            $"Expected {pages} pages, found {parsed.NumberOfPages}.");
        var text = string.Concat(parsed.GetPages().Select(static page => page.Text));
        foreach (var line in requiredText)
        {
            MatrixValidation.Require(
                library,
                text.Contains(line, StringComparison.Ordinal),
                $"The written document does not contain '{line}'.");
        }
    }

    [Conditional("MATRIX_VALIDATION")]
    public static void FontAvailable(string library)
    {
        var path = PdfBootstrap.FontProgramPath;
        MatrixValidation.Require(
            library,
            path is not null,
            "No system font program was found for PDFsharp. The writer needs a resolvable font file.");
    }
}
