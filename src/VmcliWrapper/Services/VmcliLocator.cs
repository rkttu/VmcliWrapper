using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace VmcliWrapper.Services;

/// <summary>
/// Locates the vmcli executable path based on configuration or default location.
/// </summary>
public sealed class VmcliLocator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliLocator"/> class.
    /// </summary>
    /// <param name="configuration">The configuration instance to retrieve settings from.</param>
    public VmcliLocator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private readonly IConfiguration _configuration;

    private readonly FileInfo _defaultVmCliPathWin32 = new FileInfo(Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
        "VMware", "VMware Workstation", "vmcli.exe"));

    /// <summary>
    /// Gets the default vmcli path for Win32 platforms.
    /// </summary>
    public FileInfo DefaultVmCliPathWin32 => _defaultVmCliPathWin32;

    /// <summary>
    /// Gets the vmcli executable path based on configuration or default location.
    /// </summary>
    /// <returns>The <see cref="FileInfo"/> representing the vmcli executable path.</returns>
    /// <exception cref="VmcliNotConfiguredException">Thrown when the vmcli path is not configured and the platform is not Win32.</exception>
    public FileInfo GetVmCliPath()
    {
        var vmcliPathSpecified = _configuration["Settings:VmcliPath"];

        if (!string.IsNullOrWhiteSpace(vmcliPathSpecified))
            return new FileInfo(vmcliPathSpecified);

        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            return _defaultVmCliPathWin32;

        throw new VmcliNotConfiguredException();
    }
}
