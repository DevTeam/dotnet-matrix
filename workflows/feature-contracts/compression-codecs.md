# Compression Codecs feature contract

## Scope

The category compares maintained .NET byte-range compression codecs over the
same in-memory corpora: zstd (a managed port and a native binding), LZ4, Snappy,
Brotli (a managed port and the platform stream). Disk and network I/O, archive
containers, encryption, asynchronous streaming, dictionaries, and multi-frame
concatenation are outside scope. Corpus generation, payload creation, decoder
construction and level selection happen before measurement.

All participants compress the identical byte array, so a time comparison is
also a comparison of the same input. A codec never receives a payload produced
by another codec for decompression, because no decoder reads a foreign format:
each decoder gets a payload that codec itself produced beforehand, verified by
decoding it back to the shared corpus.

## Scenarios

1. **Compress Text** compresses the 256 KiB repetitive text corpus at the
   codec's own fastest level: zstd level 1, LZ4 `L00_FAST`, Brotli quality 1
   (`.NET` `CompressionLevel.Fastest`). Snappy has one level. Codec
   construction, compression, and materialization of the compressed array are
   measured.
2. **Compress Text Max Effort** compresses the same corpus at the codec's
   maximum general-purpose level: zstd 19, LZ4 `L12_MAX`, Brotli quality 11
   (`.NET` `CompressionLevel.SmallestSize`). The comparison isolates what extra
   effort costs at equal quality settings rather than mixing codecs at
   different levels.
3. **Compress Incompressible** compresses the 256 KiB random corpus at the
   fastest level. No codec can win by finding redundancy, so this measures raw
   scanner throughput and header overhead, and it exposes codecs that expand
   input.
4. **Decompress Text** decompresses a 256 KiB text payload built before
   measurement. Only decoder construction and decoding are measured.
5. **Decompress Incompressible** decompresses a 256 KiB random-content payload
   built before measurement. A decoder cannot expand quickly by copying long
   runs here, which separates real decoding throughput from that shortcut.
6. **Round Trip Small** compresses and then decompresses a 1 KiB payload in a
   single invocation, the shape most individual messages take. Both directions
   are measured together because a small message pays fixed costs on both.

## Compression levels

Levels are part of the contract, not adapter detail. Requesting the fastest
level of one codec and the default of another would compare different amounts
of requested work and make the numbers meaningless.

| Codec | Fastest | Maximum |
| --- | --- | --- |
| ZstdSharp.Port | level 1 | level 19 |
| ZstdNet | level 1 | level 19 |
| K4os.Compression.LZ4 | `L00_FAST` | `L12_MAX` |
| BrotliSharpLib | quality 1 | quality 11 |
| System.IO.Compression | `Fastest` | `SmallestSize` |
| Snappier | its only level | not applicable |

zstd levels above 19 only enlarge the window beyond what a 256 KiB corpus can
use, so 19 is the maximum this category compares.

## Common validation and availability

- Every compressed result must be non-empty and no larger than the widest
  documented bound of the six codecs, `n + n/6 + 64`, which also sizes the
  destination buffers.
- Compression of the repetitive corpus must produce fewer bytes than the source;
  a "compressor" that returns its input has not compressed anything.
- Every decompressed result must equal the shared corpus byte for byte.
- `Supported` means the library performs the exact operation through an
  intended byte-range compression API and passes every assertion.
- `Unsupported` means the required capability is absent.
- `NotApplicable` is used only for **Compress Text Max Effort** with
  `Snappier`: Snappy defines a single compression level, so it has no
  maximum-effort mode to compare against the levels the other codecs expose.
  The other five library/scenario pairs claim support and are validated.
- Rating groups follow operations: **Compress** (all three compression
  scenarios), **Decompress** (both decompression scenarios), and **Round Trip**
  (the small round trip). Every listed library is rated, including the
  `System.IO.Compression` baseline, so the platform Brotli stream is compared
  on the same terms as the third-party codecs.

## Compressed payload size is recorded but not rated

The contract deliberately does not turn compression ratio into a rating
metric. `BenchmarkResult.PayloadSizeBytes` exists in the report model, but its
only producer, `PayloadSizeAttribute`, takes a compile-time UTF-8 string
constant and records that string's length. A compressed size is a runtime value
that differs per codec, and compressed bytes are not text, so a compression
category cannot populate that field honestly: a constant would be a number no
codec produced. `PayloadSizeBytes` therefore stays `null` here, and no
substitute value is written. Ratio is observable in the adapter's own result
(the compressed array's length, which validation bounds) and in
`workflows/category-roadmap.md`, but it takes no part in a standing. Adding it
as a first-class metric is a shared-schema change, not a category decision.

## Deferred scenarios

Streaming into and out of `Stream`, asynchronous codecs, dictionary training,
multi-threaded codecs, and per-codec configuration beyond compression level are
deferred: each needs either external I/O, a prepared dictionary corpus, or a
thread-count decision that would change what is being measured. Container
formats (ZIP, TAR, GZip framing) belong to the ZIP Archives category.
