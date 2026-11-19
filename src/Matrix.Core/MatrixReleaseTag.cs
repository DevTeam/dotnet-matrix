using System.Text.RegularExpressions;

namespace Matrix;

/// <summary>
/// A release is named by a Git tag with no <c>v</c> prefix: <c>1.2.3</c> for a release
/// and <c>1.2.3-rc1</c> for a prerelease of it. The suffix is part of the order rather
/// than decoration: <c>1.2.3-rc1</c> sorts below <c>1.2.3</c>, so the release stays
/// newer than the prereleases that led to it although they were tagged first.
/// </summary>
/// <remarks>
/// Numbers decide first, then the suffix: a prerelease sorts below the release of the
/// same number, and an identifier with a number inside compares as that number, so
/// <c>1.2.3-rc10</c> ranks above <c>1.2.3-rc2</c>. That is the order
/// <c>git tag --sort=v:refname</c> itself gives the tags, so the list a reader sees and
/// the repository never disagree about which tag is newer. In the rest of the suffix
/// the semantic-version precedence of <see href="https://semver.org"/> applies: a
/// numeric identifier sorts below an alphanumeric one, and a shorter list of equal
/// identifiers below a longer one. A tag that does not match the shape is not a release
/// at all — <see cref="TryParse"/> rejects it, so a mistyped tag never becomes a version
/// a reader can select.
/// </remarks>
public sealed partial class MatrixReleaseTag : IComparable<MatrixReleaseTag>
{
    private MatrixReleaseTag(string name, int major, int minor, int patch, string[] prerelease)
    {
        Name = name;
        Major = major;
        Minor = minor;
        Patch = patch;
        Prerelease = prerelease;
    }

    /// <summary>The tag exactly as it is spelled in the repository.</summary>
    public string Name { get; }

    public int Major { get; }

    public int Minor { get; }

    public int Patch { get; }

    /// <summary>Identifiers of the suffix, empty for a release.</summary>
    public IReadOnlyList<string> Prerelease { get; }

    public bool IsPrerelease => Prerelease.Count > 0;

    public static bool TryParse(string? name, out MatrixReleaseTag tag)
    {
        tag = null!;
        if (name is null)
        {
            return false;
        }

        var match = TagRegex().Match(name);
        if (!match.Success)
        {
            return false;
        }

        var prerelease = match.Groups["prerelease"].Value;
        tag = new MatrixReleaseTag(
            name,
            int.Parse(match.Groups["major"].Value),
            int.Parse(match.Groups["minor"].Value),
            int.Parse(match.Groups["patch"].Value),
            prerelease.Length == 0 ? [] : prerelease.Split('.'));
        return true;
    }

    public int CompareTo(MatrixReleaseTag? other)
    {
        if (other is null)
        {
            return 1;
        }

        var result = Major.CompareTo(other.Major);
        if (result != 0)
        {
            return result;
        }

        result = Minor.CompareTo(other.Minor);
        if (result != 0)
        {
            return result;
        }

        result = Patch.CompareTo(other.Patch);
        if (result != 0)
        {
            return result;
        }

        // A release outranks every prerelease of the same number, whatever the suffix says.
        if (!IsPrerelease || !other.IsPrerelease)
        {
            return other.IsPrerelease.CompareTo(IsPrerelease);
        }

        for (var index = 0; index < Math.Min(Prerelease.Count, other.Prerelease.Count); index++)
        {
            result = CompareIdentifier(Prerelease[index], other.Prerelease[index]);
            if (result != 0)
            {
                return result;
            }
        }

        return Prerelease.Count.CompareTo(other.Prerelease.Count);
    }

    public override string ToString() => Name;

    private static int CompareIdentifier(string left, string right)
    {
        var leftIsNumber = int.TryParse(left, out var leftNumber);
        var rightIsNumber = int.TryParse(right, out var rightNumber);
        return (leftIsNumber, rightIsNumber) switch
        {
            (true, true) => leftNumber.CompareTo(rightNumber),
            // A numeric identifier always sorts below an alphanumeric one.
            (true, false) => -1,
            (false, true) => 1,
            _ => CompareNatural(left, right)
        };
    }

    /// <summary>
    /// Compares letter by letter, except that a run of digits is compared as the number
    /// it spells: reading <c>rc10</c> character by character would put it below
    /// <c>rc2</c>, which is not the order the tags were created in.
    /// </summary>
    private static int CompareNatural(string left, string right)
    {
        int leftIndex = 0, rightIndex = 0;
        while (leftIndex < left.Length && rightIndex < right.Length)
        {
            if (char.IsAsciiDigit(left[leftIndex]) && char.IsAsciiDigit(right[rightIndex]))
            {
                var leftEnd = leftIndex;
                while (leftEnd < left.Length && char.IsAsciiDigit(left[leftEnd]))
                {
                    leftEnd++;
                }

                var rightEnd = rightIndex;
                while (rightEnd < right.Length && char.IsAsciiDigit(right[rightEnd]))
                {
                    rightEnd++;
                }

                // Leading zeros are dropped so the numbers compare by value, not by width.
                var leftDigits = left[leftIndex..leftEnd].TrimStart('0');
                var rightDigits = right[rightIndex..rightEnd].TrimStart('0');
                var byLength = leftDigits.Length.CompareTo(rightDigits.Length);
                var byValue = byLength != 0
                    ? byLength
                    : string.CompareOrdinal(leftDigits, rightDigits);
                if (byValue != 0)
                {
                    return byValue;
                }

                leftIndex = leftEnd;
                rightIndex = rightEnd;
                continue;
            }

            var byCharacter = left[leftIndex].CompareTo(right[rightIndex]);
            if (byCharacter != 0)
            {
                return byCharacter;
            }

            leftIndex++;
            rightIndex++;
        }

        return (left.Length - leftIndex).CompareTo(right.Length - rightIndex);
    }

    /// <summary>
    /// Numbers carry no leading zero, so <c>01.0.0</c> is not a release: it would name
    /// the same version as <c>1.0.0</c> and make the picker show one release twice.
    /// </summary>
    [GeneratedRegex(
        @"^(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)" +
        @"(?:-(?<prerelease>[0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*))?$",
        RegexOptions.CultureInvariant)]
    private static partial Regex TagRegex();
}
