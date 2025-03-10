using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VmcliWrapper.Services;

namespace VmcliWrapper;

/// <summary>
/// Wrapper for the Vmcli Ethernet command module.
/// </summary>
public class VmcliEthernetWrapper : VmcliWrapperBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliEthernetWrapper"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="locator">The Vmcli locator for finding the Vmcli executable path.</param>
    public VmcliEthernetWrapper(
        ILogger<VmcliEthernetWrapper> logger,
        VmcliLocator locator)
        : base(logger, locator) { }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public override string ModuleName => "Ethernet";

    /// <summary>
    /// Queries the Ethernet module for information in JSON format.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the JSON document with the query result.</returns>
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
    /// Controls the connection state of the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="connectOp">The connection operation to perform (e.g., connect, disconnect).</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ConnectionControlAsync(
        string targetVmxFilePath, string connectOp, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "ConnectionControl", connectOp },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Checks if the specified device is present in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to check for presence.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the JSON document with the query result.</returns>
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
    /// Moves a device from one label to another within the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="fromDeviceLabel">The current label of the device.</param>
    /// <param name="toDeviceLabel">The new label for the device.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task MoveAsync(
        string targetVmxFilePath, string fromDeviceLabel, string toDeviceLabel, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "MoveDevice", fromDeviceLabel, toDeviceLabel },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Purges the specified device from the Ethernet module.
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
    /// Sets the address type for the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="addressType1">The first address type to set.</param>
    /// <param name="addressType2">The second address type to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetAddressTypeAsync(
        string targetVmxFilePath, string addressType1, string addressType2, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetAddressType", addressType1, addressType2 },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets whether guest control is allowed for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set guest control for.</param>
    /// <param name="allowGuestControl">A value indicating whether guest control is allowed.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetAllowGuestControlAsync(
        string targetVmxFilePath, string deviceLabel, string allowGuestControl, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetAllowGuestControl", deviceLabel, allowGuestControl },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the connection type for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the connection type for.</param>
    /// <param name="connectionType">The connection type to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetConnectionTypeAsync(
        string targetVmxFilePath, string deviceLabel, string connectionType, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetConnectionType", deviceLabel, connectionType },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the custom type backing for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the custom type backing for.</param>
    /// <param name="vnet">The virtual network to set.</param>
    /// <param name="bsdName">The BSD name to set.</param>
    /// <param name="displayName">The display name to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetCustomTypeBackingAsync(
        string targetVmxFilePath, string deviceLabel, string vnet, string bsdName, string displayName, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetCustomTypeBacking", deviceLabel, vnet, bsdName, displayName },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the DVS type backing for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the DVS type backing for.</param>
    /// <param name="dvsSwitchId">The DVS switch ID to set.</param>
    /// <param name="dvsPortId">The DVS port ID to set.</param>
    /// <param name="dvsPortgroupId">The DVS port group ID to set.</param>
    /// <param name="dvsConnectionId">The DVS connection ID to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetDvsTypeBackingAsync(
        string targetVmxFilePath, string deviceLabel, string dvsSwitchId, string dvsPortId, string dvsPortgroupId, string dvsConnectionId, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetDvsTypeBacking", deviceLabel, dvsSwitchId, dvsPortId, dvsPortgroupId, dvsConnectionId },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the external ID for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the external ID for.</param>
    /// <param name="externalId">The external ID to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetExternalIdAsync(
        string targetVmxFilePath, string deviceLabel, string externalId, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetExternalId", deviceLabel, externalId },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the features for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the features for.</param>
    /// <param name="features">The features to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetFeaturesAsync(
        string targetVmxFilePath, string deviceLabel, string features, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetFeatures", deviceLabel, features },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the link state propagation for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the link state propagation for.</param>
    /// <param name="linkStatePropagationEnable">A value indicating whether link state propagation is enabled.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetLinkStatePropagationAsync(
        string targetVmxFilePath, string deviceLabel, string linkStatePropagationEnable, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetLinkStatePropagation", deviceLabel, linkStatePropagationEnable },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the network name for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the network name for.</param>
    /// <param name="networkName">The network name to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetNetworkNameAsync(
        string targetVmxFilePath, string deviceLabel, string networkName, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetNetworkName", deviceLabel, networkName },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the migrate control for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the migrate control for.</param>
    /// <param name="migrateControl">The migrate control to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetMigrateControlAsync(
        string targetVmxFilePath, string deviceLabel, string migrateControl, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetMigrateControl", deviceLabel, migrateControl },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the NIOC type backing for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the NIOC type backing for.</param>
    /// <param name="reservation">The reservation value to set.</param>
    /// <param name="shares">The shares value to set.</param>
    /// <param name="limit">The limit value to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetNiocTypeBackingAsync(
        string targetVmxFilePath, string deviceLabel, string reservation, string shares, string limit, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetNiocTypeBacking", deviceLabel, reservation, shares, limit },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the opaque network type backing for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the opaque network type backing for.</param>
    /// <param name="networkId">The network ID to set.</param>
    /// <param name="networkType">The network type to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetOpaqueNetworkTypeBackingAsync(
        string targetVmxFilePath, string deviceLabel, string networkId, string networkType, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetOpaqueNetworkTypeBacking", deviceLabel, networkId, networkType },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the PCI slot number for the specified device in the Ethernet module.
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
    /// Sets the presence state for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the presence state for.</param>
    /// <param name="present">A value indicating whether the device is present.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetPresentAsync(
        string targetVmxFilePath, string deviceLabel, string present, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetPresent", deviceLabel, present },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the PVN type backing for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the PVN type backing for.</param>
    /// <param name="pvnId">The PVN ID to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetPvnTypeBackingAsync(
        string targetVmxFilePath, string deviceLabel, string pvnId, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetPvnTypeBacking", deviceLabel, pvnId },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the security policy for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the security policy for.</param>
    /// <param name="noPromisc">A value indicating whether promiscuous mode is disabled.</param>
    /// <param name="downWhenAddrMismatch">A value indicating whether the device is down when address mismatch occurs.</param>
    /// <param name="noForgedSrcAddr">A value indicating whether forged source addresses are disabled.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetSecurityPolicyAsync(
        string targetVmxFilePath, string deviceLabel, string noPromisc, string downWhenAddrMismatch, string noForgedSrcAddr, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetSecurityPolicy", deviceLabel, noPromisc, downWhenAddrMismatch, noForgedSrcAddr },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets whether the specified device in the Ethernet module starts connected.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the start connected state for.</param>
    /// <param name="startConnected">A value indicating whether the device starts connected.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetStartConnectedAsync(
        string targetVmxFilePath, string deviceLabel, string startConnected, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetStartConnected", deviceLabel, startConnected },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the transfer latency for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the transfer latency for.</param>
    /// <param name="txLatency">The transmit latency to set.</param>
    /// <param name="rxLatency">The receive latency to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetTransferLatencyAsync(
        string targetVmxFilePath, string deviceLabel, string txLatency, string rxLatency, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetTransferLatency", deviceLabel, txLatency, rxLatency },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the transfer rate for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the transfer rate for.</param>
    /// <param name="txfiDroprate">The transmit frame drop rate to set.</param>
    /// <param name="rxfiDroprate">The receive frame drop rate to set.</param>
    /// <param name="txfiDropsize">The transmit frame drop size to set.</param>
    /// <param name="rxfiDropsize">The receive frame drop size to set.</param>
    /// <param name="txbwLimit">The transmit bandwidth limit to set.</param>
    /// <param name="rxbwLimit">The receive bandwidth limit to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetTransferRateAsync(
        string targetVmxFilePath, string deviceLabel, string txfiDroprate, string rxfiDroprate, string txfiDropsize, string rxfiDropsize, string txbwLimit, string rxbwLimit, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetTransferRate", deviceLabel, txfiDroprate, rxfiDroprate, txfiDropsize, rxfiDropsize, txbwLimit, rxbwLimit },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the UPT compatibility for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the UPT compatibility for.</param>
    /// <param name="uptCompatibilityEnabled">A value indicating whether UPT compatibility is enabled.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetUptCompatibilityAsync(
        string targetVmxFilePath, string deviceLabel, string uptCompatibilityEnabled, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetUptCompatibility", deviceLabel, uptCompatibilityEnabled },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the virtual device for the specified device in the Ethernet module.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the virtual device for.</param>
    /// <param name="virtualDevice">The virtual device to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetVirtualDeviceAsync(
        string targetVmxFilePath, string deviceLabel, string virtualDevice, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetVirtualDevice", deviceLabel, virtualDevice },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets whether the specified device in the Ethernet module wakes on packet receive.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to set the wake on packet receive state for.</param>
    /// <param name="wakeOnPcktRcv">A value indicating whether the device wakes on packet receive.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetWakeOnPcktRcvAsync(
        string targetVmxFilePath, string deviceLabel, string wakeOnPcktRcv, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetWakeOnPcktRcv", deviceLabel, wakeOnPcktRcv },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }
}
