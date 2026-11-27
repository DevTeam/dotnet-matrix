namespace Matrix.PdfProcessing;

/// <summary>
/// Builds the shared fixture and the page model once per benchmark class, so the
/// first invocation of a measured method never pays for document generation. The
/// contract requires setup outside measurement; a lazy property alone would let
/// the first iteration absorb it.
/// </summary>
public abstract class PdfBenchmarkBase
{
    [GlobalSetup]
    public void PreparePdfInputs()
    {
        PdfBootstrap.EnsureConfigured();
        _ = PdfFixture.Bytes;
        _ = PdfModel.TextDocument;
        _ = PdfModel.TableDocument;
        _ = PdfModel.MultiPage;
    }
}
