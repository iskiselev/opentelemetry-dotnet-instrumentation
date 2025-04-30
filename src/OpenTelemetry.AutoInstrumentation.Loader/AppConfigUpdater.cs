// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace OpenTelemetry.AutoInstrumentation.Loader;

#pragma warning disable RS0016
#pragma warning restore RS0016

/// <summary>
/// Handles update of config files for non-default AppDomain
/// </summary>
#pragma warning disable RS0016
public class AppConfigUpdater
#pragma warning restore RS0016
{
    /// <summary>
    /// Modify assembly bindings in appDomainSetup
    /// </summary>
    /// <param name="appDomainSetup">appDomainSetup to be updated</param>
#pragma warning disable RS0016
#pragma warning disable RS0037
    public static void ModifyConfig(AppDomainSetup appDomainSetup)
#pragma warning restore RS0037
#pragma warning restore RS0016
    {
        var configPath = appDomainSetup.ConfigurationFile;
        EnvironmentHelper.Logger.Information($"Try to modify ${configPath}");

        var config = XDocument.Load(configPath);

        ModifyConfig(config);

        var settings = new XmlWriterSettings { OmitXmlDeclaration = false, Encoding = Encoding.UTF8 };
        using var memoryStream = new MemoryStream();
        using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
        {
            config.WriteTo(xmlWriter);
            xmlWriter.Flush();
        }

        appDomainSetup.SetConfigurationBytes(memoryStream.ToArray());
        EnvironmentHelper.Logger.Information($"Config modified: ${config}");
    }

    /// <summary>
    /// Modify assembly bindings in appDomainSetup
    /// </summary>
    /// <param name="appDomainSetup">appDomainSetup to be updated</param>
#pragma warning disable RS0016
#pragma warning disable RS0037
    public static void ModifyConfig1(AppDomainSetup appDomainSetup)
#pragma warning restore RS0037
#pragma warning restore RS0016
    {
        var configPath = appDomainSetup.ConfigurationFile;
        try
        {
            EnvironmentHelper.Logger.Information($"Try to modify ${configPath}");
            var config = XDocument.Load(configPath);
            ModifyConfig(config);

            var settings = new XmlWriterSettings { OmitXmlDeclaration = false, Encoding = Encoding.UTF8 };
            using var memoryStream = new MemoryStream();
            using var xmlWriter = XmlWriter.Create(memoryStream, settings);
            config.WriteTo(xmlWriter);
            xmlWriter.Flush();
            appDomainSetup.SetConfigurationBytes(memoryStream.ToArray());
            EnvironmentHelper.Logger.Information($"Config modified: ${config}");
        }
        catch (Exception e)
        {
            EnvironmentHelper.Logger.Error(e, "Failed to modify app domain config ");
        }
    }

    private static void ModifyConfig(XDocument config)
    {
        var configuration = config.Element(Names.Configuration);
        if (configuration == null)
        {
            throw new Exception();
        }

        var runtime = configuration.Element(Names.Runtime);
        if (runtime == null)
        {
            runtime = new XElement(Names.Runtime);
            configuration.Add(runtime);
        }

        var assemblyBinding = runtime.Element(Names.AssemblyBinding);
        if (assemblyBinding == null)
        {
            assemblyBinding = new XElement(Names.AssemblyBinding);
            runtime.Add(assemblyBinding);
        }

        foreach (var assemblyInfo in AssemblyCatalog.GetAssemblies())
        {
            var existingRedirects = assemblyBinding
                .Descendants(Names.AssemblyIdentity)
                .Where(identity =>
                    string.Compare(identity.Attribute(Names.Name)?.Value, assemblyInfo.FullName.Name, StringComparison.InvariantCultureIgnoreCase) == 0
                                   && string.Compare(identity.Attribute(Names.PublicKeyToken)?.Value, assemblyInfo.Token, StringComparison.InvariantCultureIgnoreCase) == 0)
                .Select(identity => identity.Parent)
                .Elements(Names.BindingRedirect).ToList();
            if (existingRedirects.Count > 1)
            {
                EnvironmentHelper.Logger.Warning($"Multiple redirections for {assemblyInfo.FullName.Name} found. Skipping it.");
                continue;
            }

            if (existingRedirects.Count == 1)
            {
                var versionString = existingRedirects.First().Attribute(Names.NewVersion)?.Value ?? string.Empty;
                Version existingNewVersion;
                try
                {
                    existingNewVersion = new Version(versionString);
                }
                catch (Exception ex)
                {
                    EnvironmentHelper.Logger.Error(ex, $"Version {versionString} not parsed for {assemblyInfo.FullName.Name}. Treat it as 0.0.");
                    existingNewVersion = new Version(0, 0);
                }

                if (existingNewVersion >= assemblyInfo.Version)
                {
                    if (existingRedirects.First().Attribute(Names.OldVersion) is { } oldVersion)
                    {
                        oldVersion.Value = $"0.0.0.0-{existingNewVersion}";
                    }

                    EnvironmentHelper.Logger.Information($"Use existing version. Override version range for {assemblyInfo.FullName.Name}");
                    continue;
                }
                else
                {
                    existingRedirects[0].Parent?.Remove();
                }
            }

            var dependentAssembly = new XElement(
                Names.DependentAssembly,
                new XElement(
                    Names.AssemblyIdentity,
                    new XAttribute(Names.Name, assemblyInfo.FullName.Name),
                    new XAttribute(Names.PublicKeyToken, assemblyInfo.Token),
                    new XAttribute(Names.Culture, Names.NeutralCulture)),
                new XElement(
                    Names.BindingRedirect,
                    new XAttribute(Names.OldVersion, $"0.0.0.0-{assemblyInfo.Version}"),
                    new XAttribute(Names.NewVersion, assemblyInfo.Version)),
                new XElement(
                    Names.CodeBase,
                    new XAttribute(Names.Version, assemblyInfo.Version),
                    new XAttribute(Names.Href, new Uri(assemblyInfo.Path).AbsoluteUri)));
            assemblyBinding.Add(dependentAssembly);
        }
    }

    private static class Names
    {
        public const string NeutralCulture = "neutral";
        public static readonly XName Configuration = XName.Get("configuration");
        public static readonly XName Runtime = XName.Get("runtime");
        public static readonly XName AssemblyBinding = XName.Get("assemblyBinding", AsmNs);
        public static readonly XName AssemblyIdentity = XName.Get("assemblyIdentity", AsmNs);
        public static readonly XName BindingRedirect = XName.Get("bindingRedirect", AsmNs);
        public static readonly XName DependentAssembly = XName.Get("dependentAssembly", AsmNs);
        public static readonly XName CodeBase = XName.Get("codeBase", AsmNs);
        public static readonly XName Name = XName.Get("name");
        public static readonly XName PublicKeyToken = XName.Get("publicKeyToken");
        public static readonly XName NewVersion = XName.Get("newVersion");
        public static readonly XName OldVersion = XName.Get("oldVersion");
        public static readonly XName Culture = XName.Get("culture");
        public static readonly XName Version = XName.Get("version");
        public static readonly XName Href = XName.Get("href");
        private const string AsmNs = "urn:schemas-microsoft-com:asm.v1";
    }
}
#endif
