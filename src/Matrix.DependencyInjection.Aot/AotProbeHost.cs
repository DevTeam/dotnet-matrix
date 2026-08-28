using System.Reflection;

namespace Matrix.DependencyInjection.Aot;

internal static class AotProbeHost
{
    /// <summary>
    /// True when this process was published with Native AOT.
    /// </summary>
    /// <remarks>
    /// <c>RuntimeFeature.IsDynamicCodeSupported</c> cannot be used here: setting
    /// <c>PublishAot</c> turns it off for a plain <c>dotnet run</c> of the same project as well,
    /// so it reports the project's intent rather than how this process was produced.
    /// <c>Assembly.GetCallingAssembly</c> throwing <see cref="PlatformNotSupportedException"/> is
    /// what actually distinguishes the two, and is the probe log4net.Tests.Aot uses.
    /// </remarks>
    public static bool IsAotCompiled
    {
        get
        {
            try
            {
                _ = Assembly.GetCallingAssembly();
                return false;
            }
            catch (PlatformNotSupportedException)
            {
                return true;
            }
        }
    }
}
