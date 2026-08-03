namespace Matrix.ZipArchives.Models;

public readonly record struct ArchiveDigest(int EntryCount, long Length, uint Hash);
