using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VmcliWrapper.Services;

namespace VmcliWrapper;

/// <summary>
/// Wrapper for the Vmcli Tools command module.
/// </summary>
public class VmcliToolsWrapper : VmcliWrapperBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliToolsWrapper"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="locator">The Vmcli locator for finding the Vmcli executable path.</param>
    public VmcliToolsWrapper(
        ILogger<VmcliToolsWrapper> logger,
        VmcliLocator locator)
        : base(logger, locator) { }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public override string ModuleName => "Tools";

    /// <summary>
    /// Installs the Vmcli tools on the specified VM.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="cmdline">The command line options for the installation.</param>
    /// <param name="backingType">The backing type for the installation.</param>
    /// <param name="backingPath">The backing path for the installation.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InstallAsync(
        string targetVmxFilePath, string? cmdline, string? backingType, string? backingPath, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "Install" });
        if (!string.IsNullOrEmpty(cmdline))
            argList.AddRange(new string[] { "-c", cmdline });
        if (!string.IsNullOrEmpty(backingType))
            argList.AddRange(new string[] { "-bt", backingType });
        if (!string.IsNullOrWhiteSpace(backingPath))
            argList.AddRange(new string[] { "-bp", backingPath });
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Upgrades the Vmcli tools on the specified VM.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="cmdline">The command line options for the upgrade.</param>
    /// <param name="backingType">The backing type for the upgrade.</param>
    /// <param name="backingPath">The backing path for the upgrade.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpgradeAsync(
        string targetVmxFilePath, string? cmdline, string? backingType, string? backingPath, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "Upgrade" });
        if (!string.IsNullOrEmpty(cmdline))
            argList.AddRange(new string[] { "-c", cmdline });
        if (!string.IsNullOrEmpty(backingType))
            argList.AddRange(new string[] { "-bt", backingType });
        if (!string.IsNullOrWhiteSpace(backingPath))
            argList.AddRange(new string[] { "-bp", backingPath });
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Queries the Vmcli tools on the specified VM and returns the result as a JSON document.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the JSON document with the query result.</returns>
    public async Task<JsonDocument> QueryAsync(
        string targetVmxFilePath, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "Query", "--format", "json" },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
        return JsonDocument.Parse(outputLog);
    }
}
