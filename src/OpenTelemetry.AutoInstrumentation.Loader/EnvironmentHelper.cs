// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Runtime.InteropServices;
using OpenTelemetry.AutoInstrumentation.Logging;

namespace OpenTelemetry.AutoInstrumentation.Loader;

internal static class EnvironmentHelper
{
    private const string LoaderLoggerSuffix = "Loader";

    static EnvironmentHelper()
    {
        Logger = OtelLogging.GetLogger(LoaderLoggerSuffix);
        ManagedProfilerDirectory = ResolveManagedProfilerDirectory();
    }

    internal static string ManagedProfilerDirectory { get; }

    internal static IOtelLogger Logger { get; }

    /// <summary>
    /// Return redirection table used in runtime that will match TFM folder to load assemblies.
    /// It may not be actual .NET Framework version.
    /// </summary>
    [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
    [DllImport("OpenTelemetry.AutoInstrumentation.Native.dll")]
    private static extern int GetNetFrameworkRedirectionVersion();

    private static string ResolveManagedProfilerDirectory()
    {
        var tracerHomeDirectory = ReadEnvironmentVariable("OTEL_DOTNET_AUTO_HOME") ?? string.Empty;
#if NETFRAMEWORK
        var tracerFrameworkDirectory = "netfx";
#else
        var tracerFrameworkDirectory = "net";
#endif

        var basePath = Path.Combine(tracerHomeDirectory, tracerFrameworkDirectory);
#if NETFRAMEWORK
        // fallback to net462 in case of any issues
        var frameworkFolderName = "net462";
        try
        {
            var detectedVersion = GetNetFrameworkRedirectionVersion();
            var candidateFolderName = detectedVersion % 10 != 0 ? $"net{detectedVersion}" : $"net{detectedVersion / 10}";
            if (Directory.Exists(Path.Combine(basePath, candidateFolderName)))
            {
                frameworkFolderName = candidateFolderName;
            }
            else
            {
                Logger.Warning($"Framework folder {candidateFolderName} not found. Fallback to {frameworkFolderName}.");
            }
        }
        catch (Exception ex)
        {
            Logger.Warning(ex, $"Error getting .NET Framework version from native profiler. Fallback to {frameworkFolderName}.");
        }

        return Path.Combine(basePath, frameworkFolderName);
#else
        return basePath;
#endif
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
