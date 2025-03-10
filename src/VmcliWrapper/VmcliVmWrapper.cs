using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VmcliWrapper.Services;

namespace VmcliWrapper;

/// <summary>
/// Wrapper for the Vmcli VM command module.
/// </summary>
public class VmcliVmWrapper : VmcliWrapperBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliVmWrapper"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="locator">The Vmcli locator for finding the Vmcli executable path.</param>
    public VmcliVmWrapper(
        ILogger<VmcliVmWrapper> logger,
        VmcliLocator locator)
        : base(logger, locator) { }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public override string ModuleName => "VM";

    /// <summary>
    /// Creates a new virtual machine asynchronously.
    /// </summary>
    /// <param name="targetDirectory">The directory where the virtual machine will be created.</param>
    /// <param name="virtualMachineName">The name of the virtual machine to be created.</param>
    /// <param name="guestOSType">The type of the guest operating system. If null or whitespace, defaults to "Other_64".</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="FileInfo"/> object representing the created virtual machine file.</returns>
    public async Task<FileInfo> CreateAsync(
        string targetDirectory, string virtualMachineName,
        string? guestOSType = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(guestOSType))
            guestOSType = GuestOSTypes.Other_64;
        var outputLog = await ExecuteAsync(
            new string[] { ModuleName, "Create", "--name", virtualMachineName, "--dirpath", targetDirectory, "--custom-guesttype", guestOSType },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
        return new FileInfo(Path.Combine(targetDirectory, virtualMachineName + ".vmx"));
    }
}
