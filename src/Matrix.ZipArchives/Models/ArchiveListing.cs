namespace Matrix.ZipArchives.Models;

public readonly record struct ArchiveListing(int EntryCount, long TotalLength, int TotalNameLength);
