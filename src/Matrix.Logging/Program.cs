// ReSharper disable RedundantUsingDirective
using System.Reflection;
using Matrix;

#if MATRIX_VALIDATION
const MatrixRunMode mode = MatrixRunMode.Validation;
#elif MATRIX_BENCHMARK
const MatrixRunMode mode = MatrixRunMode.Benchmark;
#else
#error MatrixMode must be Validation or Benchmark.
#endif

return MatrixApplicationHost.Run(args, Assembly.GetExecutingAssembly(), mode);

