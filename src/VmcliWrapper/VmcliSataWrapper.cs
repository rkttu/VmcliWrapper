using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VmcliWrapper.Services;

namespace VmcliWrapper;

/// <summary>
/// Wrapper for the Vmcli Sata command module.
/// </summary>
public class VmcliSataWrapper : VmcliWrapperBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliSataWrapper"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="locator">The Vmcli locator for finding the Vmcli executable path.</param>
    public VmcliSataWrapper(
        ILogger<VmcliSataWrapper> logger,
        VmcliLocator locator)
        : base(logger, locator) { }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public override string ModuleName => "Sata";

    /// <summary>
    /// Queries the Vmcli Sata module for information.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the query result as a <see cref="JsonDocument"/>.</returns>
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
    /// Purges the specified device from the Vmcli Sata module.
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
    /// Moves a device from one label to another within the Vmcli Sata module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="fromDeviceLabel">The label of the device to move from.</param>
    /// <param name="toDeviceLabel">The label of the device to move to.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task MoveAsync(
        string targetVmxFilePath, string fromDeviceLabel, string toDeviceLabel, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "Move", fromDeviceLabel, toDeviceLabel },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Finds the first free device in the Vmcli Sata module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to find.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the query result as a <see cref="JsonDocument"/>.</returns>
    public async Task<JsonDocument> FindFirstFreeAsync(
        string targetVmxFilePath, string deviceLabel, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "FindFirstFree", deviceLabel, "--format", "json" },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
        return JsonDocument.Parse(outputLog);
    }

    /// <summary>
    /// Sets the presence of a device in the Vmcli Sata module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set.</param>
    /// <param name="enabled">A value indicating whether the device is enabled.</param>
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
    /// Sets the type of a device in the Vmcli Sata module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set.</param>
    /// <param name="hbaType">The HBA type to set for the device.</param>
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
    /// Sets the PCI slot number of a device in the Vmcli Sata module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set.</param>
    /// <param name="pciSlotNumber">The PCI slot number to set for the device.</param>
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
    /// Sets the maximum number of devices in the Vmcli Sata module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set.</param>
    /// <param name="maxDevices">The maximum number of devices to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetMaxDevicesAsync(
        string targetVmxFilePath, string deviceLabel, string maxDevices, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetMaxDevices", deviceLabel, maxDevices },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Checks if a device is present in the Vmcli Sata module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to check.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the query result as a <see cref="JsonDocument"/>.</returns>
    public async Task<JsonDocument> IsPresentAsync(
        string targetVmxFilePath, string deviceLabel, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "IsPresent", deviceLabel, "--format", "json" },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
        return JsonDocument.Parse(outputLog);
    }

    /// <summary>
    /// Checks if a child device is present in the Vmcli Sata module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the child device to check.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the query result as a <see cref="JsonDocument"/>.</returns>
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
    /// Checks if there are no devices in the Vmcli Sata module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to check.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the command output string.</returns>
    public async Task<string> HasNoDeviceAsync(
        string targetVmxFilePath, string deviceLabel, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "HasNoDevice", deviceLabel },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
        return outputLog;
    }

    /// <summary>
    /// Sets the NUMA node of a device in the Vmcli Sata module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set.</param>
    /// <param name="numaNode">The NUMA node to set for the device.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetNumaNodeAsync(
        string targetVmxFilePath, string deviceLabel, string numaNode, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetNumaNode", deviceLabel, numaNode },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }
}
