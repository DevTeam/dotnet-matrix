using System.Globalization;

namespace Matrix.CsvProcessing.Models;

public readonly record struct ProductCode(int Value) :
    IParsable<ProductCode>,
    ISpanParsable<ProductCode>
{
    public static ProductCode Parse(string value, IFormatProvider? provider) =>
        Parse(value.AsSpan(), provider);

    public static bool TryParse(
        string? value,
        IFormatProvider? provider,
        out ProductCode result) =>
        TryParse(value.AsSpan(), provider, out result);

    public static ProductCode Parse(
        ReadOnlySpan<char> value,
        IFormatProvider? provider)
    {
        if (TryParse(value, provider, out var result))
        {
            return result;
        }

        throw new FormatException($"'{value.ToString()}' is not a product code.");
    }

    public static bool TryParse(
        ReadOnlySpan<char> value,
        IFormatProvider? provider,
        out ProductCode result)
    {
        const string prefix = "sku-";
        if (value.StartsWith(prefix, StringComparison.Ordinal)
            && int.TryParse(
                value[prefix.Length..],
                NumberStyles.None,
                provider ?? CultureInfo.InvariantCulture,
                out var number))
        {
            result = new ProductCode(number);
            return true;
        }

        result = default;
        return false;
    }
}

