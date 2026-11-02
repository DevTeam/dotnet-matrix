# Benchmark adapter regression checks

Run from the repository root:

```shell
dotnet run --project tests/Matrix.Benchmark.Tests --configuration Release -p:MatrixMode=Benchmark
```

These checks use BenchmarkDotNet discovery to verify binary serialization setup
selection and execute each prepared adapter once. They also check the CLI-only
exception for the non-optimized PowerArgs package and sustained IdGen generation.
They do not measure performance or change repository reports. Real benchmark
execution still requires running the modules with `MatrixMode=Benchmark`.
