using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using VmcliWrapper.Services;

namespace VmcliWrapper;

/// <summary>
/// Wrapper for the Vmcli VMTemplate command module.
/// </summary>
public class VmcliVmTemplateWrapper : VmcliWrapperBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliVmTemplateWrapper"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="locator">The Vmcli locator for finding the Vmcli executable path.</param>
    public VmcliVmTemplateWrapper(
        ILogger<VmcliVmTemplateWrapper> logger,
        VmcliLocator locator)
        : base(logger, locator) { }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public override string ModuleName => "VMTemplate";

    /// <summary>
    /// Deploys a VM template asynchronously.
    /// </summary>
    /// <param name="vmtxFilePath">The file path of the VM template to deploy.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeployAsync(
        string vmtxFilePath, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { ModuleName, "Deploy", "--path", vmtxFilePath },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }
}
