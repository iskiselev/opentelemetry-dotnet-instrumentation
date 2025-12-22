// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if NETFRAMEWORK
using System.Runtime.InteropServices;
using System.Text;

namespace OpenTelemetry.AutoInstrumentation.Loader;

/// <summary>
/// Wrapper for Fusion GAC enumeration APIs
/// </summary>
internal static class FusionAssemblyCache
{
    [Flags]
    private enum AssemblyCacheFlags
    {
        GAC = 2,
    }

    [Flags]
    private enum CreateAssemblyNameObjectFlags
    {
        CANOF_PARSE_DISPLAY_NAME = 0x1,
    }

    [Flags]
    private enum AssemblyNameDisplayFlags : uint
    {
        VERSION = 0x01,
        CULTURE = 0x02,
        PUBLIC_KEY_TOKEN = 0x04,
        PROCESSORARCHITECTURE = 0x20,
        RETARGETABLE = 0x80,
        ALL = VERSION | CULTURE | PUBLIC_KEY_TOKEN | PROCESSORARCHITECTURE | RETARGETABLE
    }

    [ComImport]
    [Guid("CD193BC0-B4BC-11D2-9833-00C04FC31D2E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IAssemblyName
    {
        [PreserveSig]
        int SetProperty(uint propertyId, IntPtr pvProperty, uint cbProperty);

        [PreserveSig]
        int GetProperty(uint propertyId, IntPtr pvProperty, ref uint pcbProperty);

        [PreserveSig]
#pragma warning disable CS0465 // Introducing a 'Finalize' method can interfere with destructor invocation
        int Finalize();
#pragma warning restore CS0465 // Introducing a 'Finalize' method can interfere with destructor invocation

        [PreserveSig]
        int GetDisplayName(
            [Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder szDisplayName,
            ref uint pccDisplayName,
            AssemblyNameDisplayFlags dwDisplayFlags);

        [PreserveSig]
        int Reserved(
            ref Guid refIID,
            [MarshalAs(UnmanagedType.IUnknown)] object pUnkReserved1,
            [MarshalAs(UnmanagedType.IUnknown)] object pUnkReserved2,
            [MarshalAs(UnmanagedType.LPWStr)] string szReserved,
            long llReserved,
            IntPtr pvReserved,
            uint cbReserved,
            out IntPtr ppReserved);

        [PreserveSig]
        int GetName(ref uint lpcwBuffer, [Out][MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwzName);

        [PreserveSig]
        int GetVersion(out uint pdwVersionHi, out uint pdwVersionLow);

        [PreserveSig]
        int IsEqual(IAssemblyName pName, uint dwCmpFlags);

        [PreserveSig]
        int Clone(out IAssemblyName pName);
    }

    [ComImport]
    [Guid("21B8916C-F28E-11D2-A473-00C04F8EF448")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IAssemblyEnum
    {
        [PreserveSig]
        int GetNextAssembly(
            IntPtr pvReserved,
            out IAssemblyName? ppName,
            uint dwFlags);

        [PreserveSig]
        int Reset();

        [PreserveSig]
        int Clone(out IAssemblyEnum ppEnum);
    }

    /// <summary>
    /// Enumerates all versions of an assembly in the GAC matching the given name and public key token
    /// </summary>
    /// <param name="simpleName">Simple assembly name (e.g., "System.Memory")</param>
    /// <param name="publicKeyToken">Public key token as hex string (e.g., "cc7b13ffcd2ddd51")</param>
    /// <returns>List of full assembly names with versions found in GAC</returns>
    public static List<string> GetAssemblyVersions(string simpleName, string? publicKeyToken = null)
    {
        var versions = new List<string>();
        try
        {
            IAssemblyEnum? assemblyEnum = null;

            // Create assembly name with just the name part for enumeration
            int hr = NativeMethods.CreateAssemblyNameObject(
                out var assemblyName,
                simpleName,
                CreateAssemblyNameObjectFlags.CANOF_PARSE_DISPLAY_NAME,
                IntPtr.Zero);

            if (hr < 0 || assemblyName == null)
            {
                return versions;
            }

            try
            {
                // Create enumerator
                hr = NativeMethods.CreateAssemblyEnum(
                    out assemblyEnum,
                    IntPtr.Zero,
                    assemblyName,
                    AssemblyCacheFlags.GAC,
                    IntPtr.Zero);

                if (hr < 0 || assemblyEnum == null)
                {
                    return versions;
                }

                // Enumerate all matching assemblies
                while (true)
                {
                    hr = assemblyEnum.GetNextAssembly(IntPtr.Zero, out var currentAssemblyName, 0);

                    if (hr < 0 || currentAssemblyName == null)
                    {
                        break;
                    }

                    try
                    {
                        var displayName = GetAssemblyDisplayName(currentAssemblyName);
                        // Filter by public key token if specified
                        if (string.IsNullOrEmpty(publicKeyToken) || displayName.IndexOf($"PublicKeyToken={publicKeyToken}", StringComparison.OrdinalIgnoreCase) != -1)
                        {
                            versions.Add(displayName);
                        }
                    }
                    finally
                    {
                        if (Marshal.IsComObject(currentAssemblyName))
                        {
                            Marshal.ReleaseComObject(currentAssemblyName);
                        }
                    }
                }
            }
            finally
            {
                if (assemblyEnum != null && Marshal.IsComObject(assemblyEnum))
                {
                    Marshal.ReleaseComObject(assemblyEnum);
                }

                if (Marshal.IsComObject(assemblyName))
                {
                    Marshal.ReleaseComObject(assemblyName);
                }
            }
        }
        catch (Exception ex)
        {
            EnvironmentHelper.Logger.Warning($"Failed to enumerate GAC for {simpleName}: {ex.Message}");
        }

        return versions;
    }

    /// <summary>
    /// Gets the full display name from an IAssemblyName
    /// </summary>
    private static string GetAssemblyDisplayName(IAssemblyName assemblyName)
    {
        uint bufferSize = 1024;
        var buffer = new StringBuilder((int)bufferSize);

        int hr = assemblyName.GetDisplayName(buffer, ref bufferSize, AssemblyNameDisplayFlags.ALL);
        if (hr < 0)
        {
            return string.Empty;
        }

        return buffer.ToString();
    }

    private static class NativeMethods
    {
        [DllImport("fusion.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
        internal static extern int CreateAssemblyEnum(
            out IAssemblyEnum? ppEnum,
            IntPtr pUnkReserved,
            IAssemblyName? pName,
            AssemblyCacheFlags dwFlags,
            IntPtr pvReserved);

        [DllImport("fusion.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
        internal static extern int CreateAssemblyNameObject(
            out IAssemblyName? ppAssemblyNameObj,
            [MarshalAs(UnmanagedType.LPWStr)] string szAssemblyName,
            CreateAssemblyNameObjectFlags dwFlags,
            IntPtr pvReserved);
    }
}
#endif
