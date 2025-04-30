// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Reflection;

namespace OpenTelemetry.AutoInstrumentation.Loader;

/// <summary>
/// A class that attempts to load the OpenTelemetry.AutoInstrumentation .NET assembly.
/// </summary>
internal class Loader
{
    /// <summary>
    /// Initializes static members of the <see cref="Loader"/> class.
    /// This method also attempts to load the OpenTelemetry.AutoInstrumentation .NET assembly.
    /// </summary>
    static Loader()
    {
        AssemblyResolver.Init();

        TryLoadManagedAssembly();
    }

    private static void TryLoadManagedAssembly()
    {
        try
        {
            var currentAssemblyName = typeof(Loader).Assembly.GetName();
            var otelAutoInstrumentationAssemblyName =
                currentAssemblyName.FullName.Replace(
                    currentAssemblyName.Name ??
                    throw new InvalidOperationException("Current assembly name not resolved"),
                    "OpenTelemetry.AutoInstrumentation");
            var assembly = Assembly.Load(otelAutoInstrumentationAssemblyName);
            if (assembly == null)
            {
                throw new FileNotFoundException("The assembly OpenTelemetry.AutoInstrumentation could not be loaded");
            }

            var type = assembly.GetType("OpenTelemetry.AutoInstrumentation.Instrumentation", throwOnError: false);
            if (type == null)
            {
                throw new TypeLoadException("The type OpenTelemetry.AutoInstrumentation.Instrumentation could not be loaded");
            }

            var method = type.GetRuntimeMethod("Initialize", Type.EmptyTypes);
            if (method == null)
            {
                throw new MissingMethodException("The method OpenTelemetry.AutoInstrumentation.Instrumentation.Initialize could not be loaded");
            }

            method.Invoke(obj: null, parameters: null);
        }
        catch (Exception ex)
        {
            EnvironmentHelper.Logger.Error(ex, "Error when loading managed assemblies. {0}", ex.Message);
            throw;
        }
    }
}
