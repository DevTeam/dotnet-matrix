# ZIP Archives feature contract

## Scope

The category compares maintained .NET ZIP implementations over deterministic
in-memory streams. Disk I/O, network I/O, TAR, GZip, RAR, and 7-Zip are outside
scope. Every read implementation consumes the same canonical archive; a library
never receives an archive produced by itself. Corpus generation, canonical ZIP
creation, password setup, and expected digest calculation happen before
measurement.

All successful operations must preserve entry names, uncompressed lengths, and
content bytes. Validation uses entry counts, lengths, and deterministic FNV-1a
digests. Compressed writers receive the same corpus and nominal Deflate levels:
level 1 for fast compression and level 6 for optimal compression.

## Scenarios

1. **List Entries** opens a 1,000-entry archive and enumerates every entry name
   and uncompressed size. Opening, central-directory parsing, enumeration, and
   aggregation are measured.
2. **Find Entry By Name** opens the same archive and locates its final file by
   exact ordinal name. Opening and lookup are measured.
3. **Read Stored Entry** opens and reads one stored 4 MiB binary entry to a
   reusable digest sink. Opening, lookup, copying, and CRC verification are
   measured.
4. **Decompress Entry** opens and inflates one 4 MiB deflated text entry to the
   same sink. Opening, lookup, decompression, and copying are measured.
5. **Extract Many Small Entries** opens the canonical 1,000-entry archive and
   reads every file in archive order without retaining their contents.
6. **Create Stored Archive** creates a new in-memory ZIP containing the fixed
   mixed corpus as stored entries. Writer setup, headers, CRC work, copying,
   central-directory finalization, and result materialization are measured.
7. **Create Deflate Fast Archive** creates the mixed corpus with Deflate level
   1. `System.IO.Compression` uses `CompressionLevel.Fastest`, its corresponding
   public ZIP setting.
8. **Create Deflate Optimal Archive** creates the mixed corpus with Deflate
   level 6. `System.IO.Compression` uses `CompressionLevel.Optimal`, its
   corresponding public ZIP setting.
9. **Create Many Small Entries** creates 1,000 stored entries in a new in-memory
   ZIP with the canonical names and contents.
10. **Append Entry** opens a canonical mixed archive for update and appends one
   stored entry. Creating the writable in-memory destination, any archive
   rewrite required by the library, append work, and finalization are measured.
11. **Sequential Non-Seekable Read** consumes all entries through the library's
   intended forward-only API from a stream whose `CanSeek` is false. Copying the
   source bytes and emulating seekability is not support.
12. **Read Zip64 Archive** opens and enumerates a canonical archive containing
    65,536 empty entries, proving Zip64 entry-count handling.
13. **Read AES Encrypted Entry** opens and decrypts the canonical AES-256 entry
    with the fixed password. Traditional ZipCrypto is not equivalent. A library
    without WinZip AES support is `Unsupported`.

## Common validation and availability

- Read results must match the expected count, total uncompressed length, and
  digest. Lookup must return the exact requested entry.
- Created and updated archives are reopened with an independent implementation
  and checked against the complete expected manifest.
- Unicode names and nested paths are included in the mixed corpus.
- `Supported` means the library performs the exact operation through an
  intended ZIP API and passes every assertion.
- `Unsupported` means the required ZIP capability is absent.
- `NotApplicable` is not expected for the initial scenarios.
- Every scenario participates in the category rating. Unsupported scenarios
  receive no points under the shared rating rule.

## Deferred scenarios

Filesystem extraction (including Zip Slip policy) and asynchronous I/O are
deferred because they need controlled external I/O rather than an in-memory
substitute. Compressed archive size is intentionally not part of the initial
rating.
Compressed writing needs a first-class output-size metric; disk and async tests
need controlled external I/O rather than an in-memory substitute.
