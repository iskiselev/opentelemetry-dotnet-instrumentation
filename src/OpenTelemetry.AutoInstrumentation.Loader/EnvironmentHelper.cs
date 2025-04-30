// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.AutoInstrumentation.Logging;

namespace OpenTelemetry.AutoInstrumentation.Loader;

internal static class EnvironmentHelper
{
    internal static readonly string ManagedProfilerDirectory;
    internal static readonly IOtelLogger Logger = OtelLogging.GetLogger("Loader");

    static EnvironmentHelper()
    {
        ManagedProfilerDirectory = ResolveManagedProfilerDirectory();
    }

    private static string ResolveManagedProfilerDirectory()
    {
        var tracerHomeDirectory = ReadEnvironmentVariable("OTEL_DOTNET_AUTO_HOME") ?? string.Empty;
#if NETFRAMEWORK
        var tracerFrameworkDirectory = "netfx";
#else
        var tracerFrameworkDirectory = "net";
#endif
        return Path.Combine(tracerHomeDirectory, tracerFrameworkDirectory);
    }

    private static string? ReadEnvironmentVariable(string key)
    {
        try
        {
            return Environment.GetEnvironmentVariable(key);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Error while loading environment variable {0}", key);
        }

        return null;
    }
}
