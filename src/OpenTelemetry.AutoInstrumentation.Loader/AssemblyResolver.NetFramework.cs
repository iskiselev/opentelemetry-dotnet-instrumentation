// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK

using System;
using System.Reflection;

namespace OpenTelemetry.AutoInstrumentation.Loader;

/// <summary>
/// A class that attempts to load the OpenTelemetry.AutoInstrumentation .NET assembly.
/// </summary>
internal partial class AssemblyResolver
{
    private static Assembly? AssemblyResolve_ManagedProfilerDependencies(object sender, ResolveEventArgs args)
    {
        var assemblyName = new AssemblyName(args.Name);

        // On .NET Framework, having a non-US locale can cause mscorlib
        // to enter the AssemblyResolve event when searching for resources
        // in its satellite assemblies. Exit early so we don't cause
        // infinite recursion.
        if (string.Equals(assemblyName.Name, "mscorlib.resources", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(assemblyName.Name, "System.Net.Http", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        EnvironmentHelper.Logger.Debug("Requester [{0}] requested [{1}]", args.RequestingAssembly?.FullName ?? "<null>", args.Name ?? "<null>");

        if (AssemblyCatalog.GetAssemblyInfo(assemblyName.Name) is { } assemblyInfo)
        {
            if (assemblyName.Version == null)
            {
                try
                {
                    EnvironmentHelper.Logger.Debug($"Loading dependent assembly using short name. Use {assemblyInfo.FullName}");
                    // Attempt to load dependent assembly by short name
                    return Assembly.Load(assemblyInfo.FullName);
                }
                catch (Exception ex)
                {
                    EnvironmentHelper.Logger.Debug(ex, "Assembly.Load(\"{0}\") Exception: {1}", assemblyName, ex.Message);
                }
            }

            if (string.Equals(
                    assemblyName.ToString(),
                    assemblyInfo.FullName.ToString(),
                    StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var loadedAssembly = Assembly.LoadFrom(assemblyInfo.Path);
                    EnvironmentHelper.Logger.Debug<string, bool>(
                        "Assembly.LoadFrom(\"{0}\") succeeded={1}",
                        assemblyInfo.Path,
                        loadedAssembly != null);
                    return loadedAssembly;
                }
                catch (Exception ex)
                {
                    EnvironmentHelper.Logger.Debug(ex, "Assembly.LoadFrom(\"{0}\") Exception: {1}", assemblyInfo.Path, ex.Message);
                }
            }
        }

        return null;
    }

#pragma warning disable SA1201
    private static Assembly? _asm;
#pragma warning restore SA1201

#pragma warning disable SA1300
    private static System.Reflection.Assembly? __otel_assembly_resolver__(object sender, System.ResolveEventArgs args)
#pragma warning restore SA1300
    {
        if (args.Name == "OpenTelemetry.AutoInstrumentation, Version=..., Culture=neutral, PublicKeyToken=...")
        {
            if (_asm is not { } && File.Exists("<pre-calcululated-assembly-path>"))
            {
                try
                {
                    _asm = Assembly.Load(File.ReadAllBytes("<pre-calcululated-assembly-path>"));
                }
                catch (Exception)
                {
                }
            }

            return _asm;
        }

        return null;
    }
}

#endif
