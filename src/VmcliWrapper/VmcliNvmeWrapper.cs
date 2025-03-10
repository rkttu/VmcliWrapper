using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VmcliWrapper.Services;

namespace VmcliWrapper;

/// <summary>
/// Wrapper for the Vmcli Nvme command module.
/// </summary>
public class VmcliNvmeWrapper : VmcliWrapperBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliNvmeWrapper"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="locator">The Vmcli locator for finding the Vmcli executable path.</param>
    public VmcliNvmeWrapper(
        ILogger<VmcliNvmeWrapper> logger,
        VmcliLocator locator)
        : base(logger, locator) { }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public override string ModuleName => "Nvme";

    /// <summary>
    /// Queries the NVMe information for the specified VMX file.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the NVMe information as a JSON document.</returns>
    public async Task<JsonDocument> QueryAsync(
        string targetVmxFilePath, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "query", "--format", "json" },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
        return JsonDocument.Parse(outputLog);
    }

    /// <summary>
    /// Purges the specified NVMe device from the VMX file.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to purge.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task PurgeAsync(
        string targetVmxFilePath, string deviceLabel, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "Purge", deviceLabel },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the presence state of the specified NVMe device in the VMX file.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the presence state for.</param>
    /// <param name="enabled">The presence state to set (true or false).</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetPresentAsync(
        string targetVmxFilePath, string deviceLabel, string enabled, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetPresent", deviceLabel, enabled },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the type of the specified NVMe device in the VMX file.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the type for.</param>
    /// <param name="hbaType">The HBA type to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetTypeAsync(
        string targetVmxFilePath, string deviceLabel, string hbaType, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetType", deviceLabel, hbaType },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the bus type of the specified NVMe device in the VMX file.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the bus type for.</param>
    /// <param name="busType">The bus type to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetBusTypeAsync(
        string targetVmxFilePath, string deviceLabel, string busType, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetBusType", deviceLabel, busType },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the PCI slot number of the specified NVMe device in the VMX file.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the PCI slot number for.</param>
    /// <param name="pciSlotNumber">The PCI slot number to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetPciSlotNumberAsync(
        string targetVmxFilePath, string deviceLabel, string pciSlotNumber, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetPciSlotNumber", deviceLabel, pciSlotNumber },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the maximum number of devices for the specified NVMe device in the VMX file.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the maximum number of devices for.</param>
    /// <param name="maxDevices">The maximum number of devices to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetMaxDevicesAsync(
        string targetVmxFilePath, string deviceLabel, string maxDevices, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetMaxDevice", deviceLabel, maxDevices },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Checks if a child device is present for the specified NVMe device in the VMX file.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to check for a child device.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the presence information as a JSON document.</returns>
    public async Task<JsonDocument> IsChildPresentAsync(
        string targetVmxFilePath, string deviceLabel, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "IsChildPresent", deviceLabel, "--format", "json" },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
        return JsonDocument.Parse(outputLog);
    }

    /// <summary>
    /// Finds the first free slot for the specified NVMe device in the VMX file.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to find the first free slot for.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first free slot information as a JSON document.</returns>
    public async Task<JsonDocument> FindFirstFreeAsync(
        string targetVmxFilePath, string deviceLabel, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "FindFirstFree", deviceLabel, "--format", "json" },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
        return JsonDocument.Parse(outputLog);
    }
}
