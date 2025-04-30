// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Reflection;

namespace OpenTelemetry.AutoInstrumentation.Loader;

internal class AssemblyCatalog
{
    private static readonly Dictionary<string, AssemblyInfo> Assemblies = new(StringComparer.OrdinalIgnoreCase);

    static AssemblyCatalog()
    {
        /*
        static void BuildFromFolder()
        {
            var files = Directory.GetFiles(EnvironmentHelper.ManagedProfilerDirectory, "*.dll");

            foreach (var file in files)
            {
                if (Path.GetFileName(file).ToLowerInvariant() is "netstandard.dll" or "grpc_csharp_ext.x64.dll"
                    or "grpc_csharp_ext.x86.dll")
                {
                    continue;
                }

                try
                {
                    var assemblyName = AssemblyName.GetAssemblyName(file);
                    var keyToken = assemblyName.GetPublicKeyToken();
                    if (assemblyName.Name == null || keyToken == null || keyToken.Length == 0 ||
                        assemblyName.Version == null)
                    {
                        EnvironmentHelper.Logger.Warning($"No strong name for {file} ({assemblyName}), skipping it");
                        continue;
                    }

                    var token = BitConverter.ToString(keyToken).ToLowerInvariant().Replace("-", string.Empty);

                    if (Assemblies.TryGetValue(assemblyName.Name, out var info))
                    {
                        if (!string.Equals(info.Token, token, StringComparison.OrdinalIgnoreCase))
                        {
                            EnvironmentHelper.Logger.Error(
                                $"Multiple files for {assemblyName.Name} with different tokens. Using {file}");
                            continue;
                        }

                        if (info.Version < assemblyName.Version)
                        {
                            EnvironmentHelper.Logger.Warning(
                                $"Multiple files for {assemblyName.Name}, using ${assemblyName.Version} from {file}");
                        }
                        else
                        {
                            EnvironmentHelper.Logger.Warning(
                                $"Multiple files  for {assemblyName.Name}, using ${info.Version} from {info.Path}");
                            continue;
                        }
                    }

                    Assemblies[assemblyName.Name] = new AssemblyInfo(token, assemblyName.Version, assemblyName, file);
                }
                catch (Exception ex)
                {
                    EnvironmentHelper.Logger.Error(ex, $"Failed to resolve assembly name for {file}, skipping it");
                }
            }
        }
        */

        static void BuildFromList()
        {
            var list = new List<Tuple<string, string>>()
            {
                new("Grpc.Core.Api, Version=2.0.0.0, Culture=neutral, PublicKeyToken=d754f35622e28bad", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Grpc.Core.Api.dll"),
                new("Grpc.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=d754f35622e28bad", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Grpc.Core.dll"),
                new("Microsoft.Bcl.AsyncInterfaces, Version=9.0.0.2, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Microsoft.Bcl.AsyncInterfaces.dll"),
                new("Microsoft.Extensions.Configuration.Abstractions, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Microsoft.Extensions.Configuration.Abstractions.dll"),
                new("Microsoft.Extensions.Configuration.Binder, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Microsoft.Extensions.Configuration.Binder.dll"),
                new("Microsoft.Extensions.Configuration, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Microsoft.Extensions.Configuration.dll"),
                new("Microsoft.Extensions.DependencyInjection.Abstractions, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Microsoft.Extensions.DependencyInjection.Abstractions.dll"),
                new("Microsoft.Extensions.DependencyInjection, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Microsoft.Extensions.DependencyInjection.dll"),
                new("Microsoft.Extensions.Diagnostics.Abstractions, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Microsoft.Extensions.Diagnostics.Abstractions.dll"),
                new("Microsoft.Extensions.Logging.Abstractions, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Microsoft.Extensions.Logging.Abstractions.dll"),
                new("Microsoft.Extensions.Logging.Configuration, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Microsoft.Extensions.Logging.Configuration.dll"),
                new("Microsoft.Extensions.Logging, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Microsoft.Extensions.Logging.dll"),
                new("Microsoft.Extensions.Options.ConfigurationExtensions, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Microsoft.Extensions.Options.ConfigurationExtensions.dll"),
                new("Microsoft.Extensions.Options, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Microsoft.Extensions.Options.dll"),
                new("Microsoft.Extensions.Primitives, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Microsoft.Extensions.Primitives.dll"),
                new("Microsoft.Win32.Primitives, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\Microsoft.Win32.Primitives.dll"),
                new("OpenTelemetry.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Api.dll"),
                new("OpenTelemetry.Api.ProviderBuilderExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Api.ProviderBuilderExtensions.dll"),
                new("OpenTelemetry.AutoInstrumentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c0db600a13f60b51", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.AutoInstrumentation.dll"),
                new("OpenTelemetry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.dll"),
                new("OpenTelemetry.Exporter.Console, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Exporter.Console.dll"),
                new("OpenTelemetry.Exporter.OpenTelemetryProtocol, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Exporter.OpenTelemetryProtocol.dll"),
                new("OpenTelemetry.Exporter.Prometheus.HttpListener, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Exporter.Prometheus.HttpListener.dll"),
                new("OpenTelemetry.Exporter.Zipkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Exporter.Zipkin.dll"),
                new("OpenTelemetry.Extensions.Propagators, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Extensions.Propagators.dll"),
                new("OpenTelemetry.Instrumentation.AspNet, Version=1.11.0.400, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Instrumentation.AspNet.dll"),
                new("OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, Version=1.11.0.400, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule.dll"),
                new("OpenTelemetry.Instrumentation.GrpcNetClient, Version=1.11.0.402, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Instrumentation.GrpcNetClient.dll"),
                new("OpenTelemetry.Instrumentation.Http, Version=1.11.1.407, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Instrumentation.Http.dll"),
                new("OpenTelemetry.Instrumentation.Process, Version=1.11.0.408, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Instrumentation.Process.dll"),
                new("OpenTelemetry.Instrumentation.Quartz, Version=1.11.0.409, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Instrumentation.Quartz.dll"),
                new("OpenTelemetry.Instrumentation.Runtime, Version=1.11.1.410, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Instrumentation.Runtime.dll"),
                new("OpenTelemetry.Instrumentation.SqlClient, Version=1.11.0.411, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Instrumentation.SqlClient.dll"),
                new("OpenTelemetry.Instrumentation.Wcf, Version=1.11.0.413, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Instrumentation.Wcf.dll"),
                new("OpenTelemetry.Resources.Azure, Version=1.11.0.414, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Resources.Azure.dll"),
                new("OpenTelemetry.Resources.Host, Version=1.11.0.416, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Resources.Host.dll"),
                new("OpenTelemetry.Resources.OperatingSystem, Version=1.11.0.417, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Resources.OperatingSystem.dll"),
                new("OpenTelemetry.Resources.Process, Version=1.11.0.418, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Resources.Process.dll"),
                new("OpenTelemetry.Resources.ProcessRuntime, Version=1.11.0.419, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Resources.ProcessRuntime.dll"),
                new("OpenTelemetry.Shims.OpenTracing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTelemetry.Shims.OpenTracing.dll"),
                new("OpenTracing, Version=0.12.1.0, Culture=neutral, PublicKeyToken=61503406977abdaf", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\OpenTracing.dll"),
                new("System.AppContext, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.AppContext.dll"),
                new("System.Buffers, Version=4.0.4.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Buffers.dll"),
                new("System.Collections.Concurrent, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Collections.Concurrent.dll"),
                new("System.Collections, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Collections.dll"),
                new("System.Collections.NonGeneric, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Collections.NonGeneric.dll"),
                new("System.Collections.Specialized, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Collections.Specialized.dll"),
                new("System.ComponentModel, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.ComponentModel.dll"),
                new("System.ComponentModel.EventBasedAsync, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.ComponentModel.EventBasedAsync.dll"),
                new("System.ComponentModel.Primitives, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.ComponentModel.Primitives.dll"),
                new("System.ComponentModel.TypeConverter, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.ComponentModel.TypeConverter.dll"),
                new("System.Console, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Console.dll"),
                new("System.Data.Common, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Data.Common.dll"),
                new("System.Diagnostics.Contracts, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Diagnostics.Contracts.dll"),
                new("System.Diagnostics.Debug, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Diagnostics.Debug.dll"),
                new("System.Diagnostics.DiagnosticSource, Version=9.0.0.2, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Diagnostics.DiagnosticSource.dll"),
                new("System.Diagnostics.FileVersionInfo, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Diagnostics.FileVersionInfo.dll"),
                new("System.Diagnostics.Process, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Diagnostics.Process.dll"),
                new("System.Diagnostics.StackTrace, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Diagnostics.StackTrace.dll"),
                new("System.Diagnostics.TextWriterTraceListener, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Diagnostics.TextWriterTraceListener.dll"),
                new("System.Diagnostics.Tools, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Diagnostics.Tools.dll"),
                new("System.Diagnostics.TraceSource, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Diagnostics.TraceSource.dll"),
                new("System.Diagnostics.Tracing, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Diagnostics.Tracing.dll"),
                new("System.Drawing.Primitives, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Drawing.Primitives.dll"),
                new("System.Dynamic.Runtime, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Dynamic.Runtime.dll"),
                new("System.Globalization.Calendars, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Globalization.Calendars.dll"),
                new("System.Globalization, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Globalization.dll"),
                new("System.Globalization.Extensions, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Globalization.Extensions.dll"),
                new("System.IO.Compression, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.IO.Compression.dll"),
                new("System.IO.Compression.ZipFile, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.IO.Compression.ZipFile.dll"),
                new("System.IO, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.IO.dll"),
                new("System.IO.FileSystem, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.IO.FileSystem.dll"),
                new("System.IO.FileSystem.DriveInfo, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.IO.FileSystem.DriveInfo.dll"),
                new("System.IO.FileSystem.Primitives, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.IO.FileSystem.Primitives.dll"),
                new("System.IO.FileSystem.Watcher, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.IO.FileSystem.Watcher.dll"),
                new("System.IO.IsolatedStorage, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.IO.IsolatedStorage.dll"),
                new("System.IO.MemoryMappedFiles, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.IO.MemoryMappedFiles.dll"),
                new("System.IO.Pipelines, Version=9.0.0.2, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.IO.Pipelines.dll"),
                new("System.IO.Pipes, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.IO.Pipes.dll"),
                new("System.IO.UnmanagedMemoryStream, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.IO.UnmanagedMemoryStream.dll"),
                new("System.Linq, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Linq.dll"),
                new("System.Linq.Expressions, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Linq.Expressions.dll"),
                new("System.Linq.Parallel, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Linq.Parallel.dll"),
                new("System.Linq.Queryable, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Linq.Queryable.dll"),
                new("System.Memory, Version=4.0.2.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Memory.dll"),
                new("System.Net.Http, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Net.Http.dll"),
                new("System.Net.NameResolution, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Net.NameResolution.dll"),
                new("System.Net.NetworkInformation, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Net.NetworkInformation.dll"),
                new("System.Net.Ping, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Net.Ping.dll"),
                new("System.Net.Primitives, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Net.Primitives.dll"),
                new("System.Net.Requests, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Net.Requests.dll"),
                new("System.Net.Security, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Net.Security.dll"),
                new("System.Net.Sockets, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Net.Sockets.dll"),
                new("System.Net.WebHeaderCollection, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Net.WebHeaderCollection.dll"),
                new("System.Net.WebSockets.Client, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Net.WebSockets.Client.dll"),
                new("System.Net.WebSockets, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Net.WebSockets.dll"),
                new("System.Numerics.Vectors, Version=4.1.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Numerics.Vectors.dll"),
                new("System.ObjectModel, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.ObjectModel.dll"),
                new("System.Reflection, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Reflection.dll"),
                new("System.Reflection.Extensions, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Reflection.Extensions.dll"),
                new("System.Reflection.Primitives, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Reflection.Primitives.dll"),
                new("System.Resources.Reader, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Resources.Reader.dll"),
                new("System.Resources.ResourceManager, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Resources.ResourceManager.dll"),
                new("System.Resources.Writer, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Resources.Writer.dll"),
                new("System.Runtime.CompilerServices.Unsafe, Version=6.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Runtime.CompilerServices.Unsafe.dll"),
                new("System.Runtime.CompilerServices.VisualC, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Runtime.CompilerServices.VisualC.dll"),
                new("System.Runtime, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Runtime.dll"),
                new("System.Runtime.Extensions, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Runtime.Extensions.dll"),
                new("System.Runtime.Handles, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Runtime.Handles.dll"),
                new("System.Runtime.InteropServices, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Runtime.InteropServices.dll"),
                new("System.Runtime.InteropServices.RuntimeInformation, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Runtime.InteropServices.RuntimeInformation.dll"),
                new("System.Runtime.Numerics, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Runtime.Numerics.dll"),
                new("System.Runtime.Serialization.Formatters, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Runtime.Serialization.Formatters.dll"),
                new("System.Runtime.Serialization.Json, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Runtime.Serialization.Json.dll"),
                new("System.Runtime.Serialization.Primitives, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Runtime.Serialization.Primitives.dll"),
                new("System.Runtime.Serialization.Xml, Version=4.1.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Runtime.Serialization.Xml.dll"),
                new("System.Security.Claims, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Security.Claims.dll"),
                new("System.Security.Cryptography.Algorithms, Version=4.3.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Security.Cryptography.Algorithms.dll"),
                new("System.Security.Cryptography.Csp, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Security.Cryptography.Csp.dll"),
                new("System.Security.Cryptography.Encoding, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Security.Cryptography.Encoding.dll"),
                new("System.Security.Cryptography.Primitives, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Security.Cryptography.Primitives.dll"),
                new("System.Security.Cryptography.X509Certificates, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Security.Cryptography.X509Certificates.dll"),
                new("System.Security.Principal, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Security.Principal.dll"),
                new("System.Security.SecureString, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Security.SecureString.dll"),
                new("System.Text.Encoding, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Text.Encoding.dll"),
                new("System.Text.Encoding.Extensions, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Text.Encoding.Extensions.dll"),
                new("System.Text.Encodings.Web, Version=9.0.0.2, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Text.Encodings.Web.dll"),
                new("System.Text.Json, Version=9.0.0.2, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Text.Json.dll"),
                new("System.Text.RegularExpressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Text.RegularExpressions.dll"),
                new("System.Threading, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Threading.dll"),
                new("System.Threading.Overlapped, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Threading.Overlapped.dll"),
                new("System.Threading.Tasks, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Threading.Tasks.dll"),
                new("System.Threading.Tasks.Extensions, Version=4.2.1.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Threading.Tasks.Extensions.dll"),
                new("System.Threading.Tasks.Parallel, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Threading.Tasks.Parallel.dll"),
                new("System.Threading.Thread, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Threading.Thread.dll"),
                new("System.Threading.ThreadPool, Version=4.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Threading.ThreadPool.dll"),
                new("System.Threading.Timer, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Threading.Timer.dll"),
                new("System.ValueTuple, Version=4.0.3.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.ValueTuple.dll"),
                new("System.Xml.ReaderWriter, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Xml.ReaderWriter.dll"),
                new("System.Xml.XDocument, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Xml.XDocument.dll"),
                new("System.Xml.XmlDocument, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Xml.XmlDocument.dll"),
                new("System.Xml.XmlSerializer, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Xml.XmlSerializer.dll"),
                new("System.Xml.XPath, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Xml.XPath.dll"),
                new("System.Xml.XPath.XDocument, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", @"C:\VSExclude\opentelemetry-dotnet-instrumentation\bin\tracer-home\netfx\System.Xml.XPath.XDocument.dll"),
            };

            foreach (var tuple in list)
            {
                var assemblyName = new AssemblyName(tuple.Item1);
                var keyToken = assemblyName.GetPublicKeyToken();
                var token = BitConverter.ToString(keyToken).ToLowerInvariant().Replace("-", string.Empty);

                Assemblies[assemblyName.Name] = new AssemblyInfo(token, assemblyName.Version, assemblyName, tuple.Item2);
            }
        }

        BuildFromList();
    }

    internal static AssemblyInfo? GetAssemblyInfo(string shortName)
    {
        if (Assemblies.TryGetValue(shortName, out var info))
        {
            return info;
        }

        return null;
    }

    internal static IEnumerable<AssemblyInfo> GetAssemblies()
        => Assemblies.Values;

    internal sealed class AssemblyInfo(string token, Version version, AssemblyName fullName, string path)
    {
        public string Token { get; } = token;

        public Version Version { get; } = version;

        public AssemblyName FullName { get; } = fullName;

        public string Path { get; } = path;
    }
}
#endif
