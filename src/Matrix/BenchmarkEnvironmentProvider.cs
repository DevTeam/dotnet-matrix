using System.Diagnostics;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

namespace Matrix;

public sealed class BenchmarkEnvironmentProvider : IBenchmarkEnvironmentProvider
{
    private readonly Lazy<string> _dotNetSdkVersion = new(GetDotNetSdkVersion);
    private readonly Lazy<string> _processor = new(GetProcessor);

    public BenchmarkEnvironment Capture(
        string benchmarkTool,
        Assembly benchmarkToolAssembly,
        string job)
    {
        var values = new[]
        {
            RuntimeInformation.OSDescription,
            RuntimeInformation.OSArchitecture.ToString(),
            RuntimeInformation.ProcessArchitecture.ToString(),
            RuntimeInformation.FrameworkDescription,
            Environment.Version.ToString(),
            RuntimeInformation.RuntimeIdentifier,
            _dotNetSdkVersion.Value,
            _processor.Value,
            Environment.ProcessorCount.ToString(),
            GCSettings.IsServerGC.ToString(),
            Stopwatch.Frequency.ToString(),
            benchmarkTool,
            benchmarkToolAssembly.GetName().Version?.ToString() ?? "unknown",
            job
        };
        var id = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(string.Join('\n', values))))
            [..12]
            .ToLowerInvariant();
        return new BenchmarkEnvironment(
            id,
            values[0],
            values[1],
            values[2],
            values[3],
            values[4],
            values[5],
            values[6],
            values[7],
            Environment.ProcessorCount,
            GCSettings.IsServerGC,
            Stopwatch.Frequency,
            benchmarkTool,
            values[12],
            job);
    }

    private static string GetDotNetSdkVersion()
    {
        try
        {
            var startInfo = new ProcessStartInfo("dotnet", "--version")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            using var process = Process.Start(startInfo);
            if (process is null)
            {
                return "unknown";
            }

            var value = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();
            return process.ExitCode == 0 && value.Length > 0 ? value : "unknown";
        }
        catch
        {
            return "unknown";
        }
    }

    private static string GetProcessor()
    {
        if (OperatingSystem.IsWindows())
        {
            var value = Registry.GetValue(
                @"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0",
                "ProcessorNameString",
                null) as string;
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }
        }

        // ReSharper disable once InvertIf
        if (OperatingSystem.IsLinux() && File.Exists("/proc/cpuinfo"))
        {
            var model = File
                .ReadLines("/proc/cpuinfo")
                .Select(line => line.Split(':', 2, StringSplitOptions.TrimEntries))
                .FirstOrDefault(parts =>
                    parts.Length == 2
                    && parts[0].Equals("model name", StringComparison.OrdinalIgnoreCase));
            if (model is { Length: 2 })
            {
                return model[1];
            }
        }

        return Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")
               ?? RuntimeInformation.ProcessArchitecture.ToString();
    }
}