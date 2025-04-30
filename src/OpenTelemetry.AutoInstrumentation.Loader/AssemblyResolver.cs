// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.AutoInstrumentation.Loader;

/// <summary>
/// A class that attempts to load the OpenTelemetry.AutoInstrumentation .NET assembly.
/// </summary>
internal partial class AssemblyResolver
{
    static AssemblyResolver()
    {
        try
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve_ManagedProfilerDependencies;
        }
        catch (Exception ex)
        {
            EnvironmentHelper.Logger.Error(ex, "Unable to register a callback to the CurrentDomain.AssemblyResolve event.");
        }
    }

    internal static void Init()
    {
    }
}
