// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using Xunit;
using Xunit.Abstractions;

namespace OpenTelemetry.AutoInstrumentation.Loader.Tests;

public class FusionAssemblyCacheTests
{
    private readonly ITestOutputHelper _output;

    public FusionAssemblyCacheTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void GetAssemblyVersions_SystemDll_ReturnsVersions()
    {
        // Arrange
        string assemblyName = "System";
        string publicKeyToken = "b77a5c561934e089";

        // Act
        var versions = FusionAssemblyCache.GetAssemblyVersions(assemblyName, publicKeyToken);

        // Assert
        Assert.NotEmpty(versions);
        
        _output.WriteLine($"Found {versions.Count} versions of {assemblyName}:");
        foreach (var version in versions)
        {
            _output.WriteLine($"  {version}");
            Assert.Contains(assemblyName, version);
            Assert.Contains(publicKeyToken, version, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void GetAssemblyVersions_WithoutPublicKeyToken_ReturnsAllVersions()
    {
        // Arrange
        string assemblyName = "System";

        // Act
        var versions = FusionAssemblyCache.GetAssemblyVersions(assemblyName);

        // Assert
        Assert.NotEmpty(versions);
        
        _output.WriteLine($"Found {versions.Count} versions of {assemblyName} (all public key tokens):");
        foreach (var version in versions)
        {
            _output.WriteLine($"  {version}");
        }
    }

    [Fact]
    public void GetAssemblyVersions_SystemMemory_ReturnsVersions()
    {
        // Arrange
        string assemblyName = "System.Memory";
        string publicKeyToken = "cc7b13ffcd2ddd51";

        // Act
        var versions = FusionAssemblyCache.GetAssemblyVersions(assemblyName, publicKeyToken);

        // Assert - System.Memory may not be in GAC in all environments
        _output.WriteLine($"Found {versions.Count} versions of {assemblyName}:");
        foreach (var version in versions)
        {
            _output.WriteLine($"  {version}");
            Assert.Contains(assemblyName, version);
            Assert.Contains(publicKeyToken, version, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void GetAssemblyVersions_NonExistentAssembly_ReturnsEmptyList()
    {
        // Arrange
        string assemblyName = "NonExistent.Assembly.Name";

        // Act
        var versions = FusionAssemblyCache.GetAssemblyVersions(assemblyName);

        // Assert
        Assert.Empty(versions);
    }

    [Fact]
    public void AssemblyCatalog_GetGacAssemblyVersions_SystemDll_ReturnsVersions()
    {
        // Arrange
        string assemblyName = "System";
        string publicKeyToken = "b77a5c561934e089";

        // Act
        var versions = AssemblyCatalog.GetGacAssemblyVersions(assemblyName, publicKeyToken);

        // Assert
        Assert.NotEmpty(versions);
        
        _output.WriteLine($"Found {versions.Count} versions via AssemblyCatalog:");
        foreach (var version in versions)
        {
            _output.WriteLine($"  {version}");
        }
    }
}
#endif
