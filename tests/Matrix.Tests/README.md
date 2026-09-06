# Architecture regression checks

Run the checks from the repository root:

```shell
dotnet run --project tests/Matrix.Tests/Matrix.Tests.csproj --configuration Release
```

This executable needs no test framework packages and returns a nonzero exit
code if any check fails. It checks partial report merging, baseline inclusion,
failure handling, artifact cleanup, JSON reader compatibility, and presentation
targets reading reports from an injected in-memory store. It also checks that
the report-model assembly does not reference the benchmark infrastructure.

The executor is replaced with a stub; these checks do not run measured library
code or update repository reports. To exercise the real adapter and composition:

```shell
dotnet run --project src/Matrix.JsonSerialization/Matrix.JsonSerialization.csproj --configuration Release -p:MatrixMode=Benchmark -- --libraries System.Text.Json --smoke --output artifacts/di-review/benchmarks.json --evidence artifacts/di-review/evidence
```

The smoke results verify execution only; they are not suitable for rankings.
