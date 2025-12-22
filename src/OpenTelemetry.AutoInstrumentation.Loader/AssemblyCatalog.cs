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
        static void BuildFromFolder()
        {
            var sharedFrameworkPath = Path.GetDirectoryName(EnvironmentHelper.ManagedProfilerDirectory)!;

            var files =
                Directory.GetFiles(sharedFrameworkPath, "*.dll")
                    .Concat(
                        Directory.GetFiles(EnvironmentHelper.ManagedProfilerDirectory, "*.dll"))
                    .Concat(Directory.GetFiles(EnvironmentHelper.ManagedProfilerDirectory, "*.link").Select(link =>
                    {
                        var linkPath = File.ReadAllText(link).Trim();
                        return Path.Combine(sharedFrameworkPath, linkPath, Path.GetFileNameWithoutExtension(link));
                    }));

            foreach (var file in files)
            {
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

#pragma warning disable CA1308
                    var token = BitConverter.ToString(keyToken).ToLowerInvariant().Replace("-", string.Empty);
#pragma warning restore CA1308

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

                    Assemblies[assemblyName.Name] = new AssemblyInfo(token, assemblyName.Version, assemblyName, file, false);
                }
                catch (Exception ex)
                {
                    EnvironmentHelper.Logger.Error(ex, $"Failed to resolve assembly name for {file}, skipping it");
                }
            }
        }

        /*static void BuildFromList()
        {
            var list = new List<Tuple<string, bool>>()
            {
                new("Grpc.Core.Api, Version=2.0.0.0, Culture=neutral, PublicKeyToken=d754f35622e28bad", true),
                new("Grpc.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=d754f35622e28bad", true),
                new("Microsoft.Bcl.AsyncInterfaces, Version=9.0.0.2, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", true),
                new("Microsoft.Extensions.Configuration.Abstractions, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", true),
                new("Microsoft.Extensions.Configuration.Binder, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", true),
                new("Microsoft.Extensions.Configuration, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", true),
                new("Microsoft.Extensions.DependencyInjection.Abstractions, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", true),
                new("Microsoft.Extensions.DependencyInjection, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", true),
                new("Microsoft.Extensions.Diagnostics.Abstractions, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", true),
                new("Microsoft.Extensions.Logging.Abstractions, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", true),
                new("Microsoft.Extensions.Logging.Configuration, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", true),
                new("Microsoft.Extensions.Logging, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", true),
                new("Microsoft.Extensions.Options.ConfigurationExtensions, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", true),
                new("Microsoft.Extensions.Options, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", true),
                new("Microsoft.Extensions.Primitives, Version=9.0.0.2, Culture=neutral, PublicKeyToken=adb9793829ddae60", true),
                new("Microsoft.Win32.Primitives, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("OpenTelemetry.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Api.ProviderBuilderExtensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.AutoInstrumentation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c0db600a13f60b51", true),
                new("OpenTelemetry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Exporter.Console, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Exporter.OpenTelemetryProtocol, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Exporter.Prometheus.HttpListener, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Exporter.Zipkin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Extensions.Propagators, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Instrumentation.AspNet, Version=1.11.0.400, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Instrumentation.AspNet.TelemetryHttpModule, Version=1.11.0.400, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Instrumentation.GrpcNetClient, Version=1.11.0.402, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Instrumentation.Http, Version=1.11.1.407, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Instrumentation.Process, Version=1.11.0.408, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Instrumentation.Quartz, Version=1.11.0.409, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Instrumentation.Runtime, Version=1.11.1.410, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Instrumentation.SqlClient, Version=1.11.0.411, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Instrumentation.Wcf, Version=1.11.0.413, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Resources.Azure, Version=1.11.0.414, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Resources.Host, Version=1.11.0.416, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Resources.OperatingSystem, Version=1.11.0.417, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Resources.Process, Version=1.11.0.418, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Resources.ProcessRuntime, Version=1.11.0.419, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTelemetry.Shims.OpenTracing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=7bd6737fe5b67e3c", true),
                new("OpenTracing, Version=0.12.1.0, Culture=neutral, PublicKeyToken=61503406977abdaf", true),
                new("System.AppContext, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Buffers, Version=4.0.4.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", true),
                new("System.Collections.Concurrent, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Collections, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Collections.NonGeneric, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Collections.Specialized, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.ComponentModel, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.ComponentModel.EventBasedAsync, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.ComponentModel.Primitives, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.ComponentModel.TypeConverter, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Console, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Data.Common, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Diagnostics.Contracts, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Diagnostics.Debug, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Diagnostics.DiagnosticSource, Version=9.0.0.2, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", true),
                new("System.Diagnostics.FileVersionInfo, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Diagnostics.Process, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Diagnostics.StackTrace, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Diagnostics.TextWriterTraceListener, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Diagnostics.Tools, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Diagnostics.TraceSource, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Diagnostics.Tracing, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Drawing.Primitives, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Dynamic.Runtime, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Globalization.Calendars, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Globalization, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Globalization.Extensions, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.IO.Compression, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", false),
                new("System.IO.Compression.ZipFile, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", false),
                new("System.IO, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.IO.FileSystem, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.IO.FileSystem.DriveInfo, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.IO.FileSystem.Primitives, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.IO.FileSystem.Watcher, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.IO.IsolatedStorage, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.IO.MemoryMappedFiles, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.IO.Pipelines, Version=9.0.0.2, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", true),
                new("System.IO.Pipes, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.IO.UnmanagedMemoryStream, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Linq, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Linq.Expressions, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Linq.Parallel, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Linq.Queryable, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Memory, Version=4.0.2.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", true),
                new("System.Net.Http, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Net.NameResolution, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Net.NetworkInformation, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Net.Ping, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Net.Primitives, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Net.Requests, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Net.Security, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Net.Sockets, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Net.WebHeaderCollection, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Net.WebSockets.Client, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Net.WebSockets, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Numerics.Vectors, Version=4.1.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true),
                new("System.ObjectModel, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Reflection, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Reflection.Extensions, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Reflection.Primitives, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Resources.Reader, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Resources.ResourceManager, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Resources.Writer, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Runtime.CompilerServices.Unsafe, Version=6.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true),
                new("System.Runtime.CompilerServices.VisualC, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Runtime, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Runtime.Extensions, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Runtime.Handles, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Runtime.InteropServices, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Runtime.InteropServices.RuntimeInformation, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Runtime.Numerics, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Runtime.Serialization.Formatters, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Runtime.Serialization.Json, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Runtime.Serialization.Primitives, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Runtime.Serialization.Xml, Version=4.1.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Security.Claims, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Security.Cryptography.Algorithms, Version=4.3.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Security.Cryptography.Csp, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Security.Cryptography.Encoding, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Security.Cryptography.Primitives, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Security.Cryptography.X509Certificates, Version=4.1.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Security.Principal, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Security.SecureString, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Text.Encoding, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Text.Encoding.Extensions, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Text.Encodings.Web, Version=9.0.0.2, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", true),
                new("System.Text.Json, Version=9.0.0.2, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", true),
                new("System.Text.RegularExpressions, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Threading, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Threading.Overlapped, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Threading.Tasks, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Threading.Tasks.Extensions, Version=4.2.1.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", true),
                new("System.Threading.Tasks.Parallel, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Threading.Thread, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Threading.ThreadPool, Version=4.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Threading.Timer, Version=4.0.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.ValueTuple, Version=4.0.3.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51", false),
                new("System.Xml.ReaderWriter, Version=4.1.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Xml.XDocument, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Xml.XmlDocument, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Xml.XmlSerializer, Version=4.0.11.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Xml.XPath, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false),
                new("System.Xml.XPath.XDocument, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", false)
            };

            foreach (var tuple in list)
            {
                var assemblyName = new AssemblyName(tuple.Item1);
                var keyToken = assemblyName.GetPublicKeyToken();
#pragma warning disable CA1308
                var token = BitConverter.ToString(keyToken).ToLowerInvariant().Replace("-", string.Empty);
#pragma warning restore CA1308

                Assemblies[assemblyName.Name] = new AssemblyInfo(
                    token,
                    assemblyName.Version,
                    assemblyName,
                    Path.Combine(EnvironmentHelper.ManagedProfilerDirectory, $"{assemblyName.Name}.dll"),
                    !tuple.Item2);
            }
        }

        BuildFromList();*/

        BuildFromFolder();
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

    internal sealed class AssemblyInfo(string token, Version version, AssemblyName fullName, string path, bool implicitRedirect)
    {
        public string Token { get; } = token;

        public Version Version { get; } = version;

        public AssemblyName FullName { get; } = fullName;

        public string Path { get; } = path;

        public bool ImplicitRedirect { get; } = implicitRedirect;
   }
}
#endif
