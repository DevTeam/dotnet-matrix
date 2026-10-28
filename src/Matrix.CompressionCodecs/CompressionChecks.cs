namespace Matrix.CompressionCodecs;

/// <summary>
/// Semantic checks for every published result. They run only in the validation
/// variant, so the benchmark assembly carries no assertion machinery on its hot
/// path. See <c>workflows/feature-contracts/compression-codecs.md</c>.
/// </summary>
internal static class CompressionChecks
{
    [Conditional("MATRIX_VALIDATION")]
    public static void Compressed(string library, ReadOnlySpan<byte> compressed, int sourceLength) =>
        MatrixValidation.Require(
            library,
            compressed.Length > 0 && compressed.Length <= CompressionData.MaximumCompressedLength(sourceLength),
            $"Compressed length {compressed.Length} is outside 1..{CompressionData.MaximumCompressedLength(sourceLength)} for a {sourceLength}-byte source.");

    [Conditional("MATRIX_VALIDATION")]
    public static void CompressibleIsSmaller(string library, ReadOnlySpan<byte> compressed) =>
        MatrixValidation.Require(
            library,
            compressed.Length < CompressionData.CompressibleLength,
            $"A repetitive {CompressionData.CompressibleLength}-byte corpus compressed to {compressed.Length} bytes, which is not compression.");

    [Conditional("MATRIX_VALIDATION")]
    public static void Decompressed(string library, ReadOnlySpan<byte> actual, ReadOnlySpan<byte> expected) =>
        MatrixValidation.Require(
            library,
            actual.SequenceEqual(expected),
            $"Round trip differs: expected {expected.Length} bytes, found {actual.Length}.");

    [Conditional("MATRIX_VALIDATION")]
    public static void SmallerPayload(string library, ReadOnlySpan<byte> actual, ReadOnlySpan<byte> larger) =>
        MatrixValidation.Require(
            library,
            actual.Length < larger.Length,
            $"Compressed {actual.Length} bytes, which is not smaller than {larger.Length} measured for the same corpus.");
}
