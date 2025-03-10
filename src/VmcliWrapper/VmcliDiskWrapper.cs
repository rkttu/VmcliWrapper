using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VmcliWrapper.Models;
using VmcliWrapper.Services;

namespace VmcliWrapper;

/// <summary>
/// Wrapper for the Vmcli Disk command module.
/// </summary>
public class VmcliDiskWrapper : VmcliWrapperBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliDiskWrapper"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="locator">The Vmcli locator for finding the Vmcli executable path.</param>
    public VmcliDiskWrapper(
        ILogger<VmcliDiskWrapper> logger,
        VmcliLocator locator)
        : base(logger, locator) { }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public override string ModuleName => "Disk";

    /// <summary>
    /// Creates a new virtual disk asynchronously.
    /// </summary>
    /// <param name="filePath">The file path where the virtual disk will be created.</param>
    /// <param name="adapterType">The type of virtual disk adapter.</param>
    /// <param name="diskSizeExpression">The size expression for the virtual disk.</param>
    /// <param name="diskType">The type of virtual disk.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of <see cref="FileInfo"/> objects representing the created virtual disk files.</returns>
    /// <exception cref="ArgumentException">Thrown when the specified adapter type or disk type is not supported.</exception>
    public async Task<IReadOnlyList<FileInfo>> CreateAsync(
        string filePath, VirtualDiskAdapterType adapterType, string diskSizeExpression, VirtualDiskType diskType,
        CancellationToken cancellationToken = default)
    {
        var adapterTypeArg = adapterType switch
        {
            VirtualDiskAdapterType.IDE => "ide",
            VirtualDiskAdapterType.BusLogic => "buslogic",
            VirtualDiskAdapterType.LSILogic => "lsilogic",
            _ => throw new ArgumentException($"Not supported disk adapter type: {adapterType}"),
        };
        var diskTypeArg = diskType switch
        {
            VirtualDiskType.SingleGrowable => 0,
            VirtualDiskType.SplittedGrowable => 1,
            VirtualDiskType.SinglePreAllocated => 2,
            VirtualDiskType.SplittedPreAllocated => 3,
            _ => throw new ArgumentException($"Not supported disk type: {diskType}"),
        };
        var outputLog = await ExecuteAsync(
            new string[] { ModuleName, "create", "--filepath", filePath, "--adapter", adapterTypeArg, "--size", diskSizeExpression, "--type", diskTypeArg.ToString() },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);

        var filenameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
        var parentDirectory = Path.GetDirectoryName(filePath);

        if (string.IsNullOrWhiteSpace(parentDirectory))
            return new FileInfo[] { new FileInfo(filePath), };

        return Directory
            .GetFiles(parentDirectory, filenameWithoutExt + "*.vmdk")
            .Select(x => new FileInfo(x)).ToArray();
    }

    /// <summary>
    /// Branches the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to branch.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task BranchAsync(
        string targetVmxFilePath, string diskLabel, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "Branch", diskLabel },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Cancels the branch operation for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task BranchCancelAsync(
        string targetVmxFilePath, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "BranchCancel" },
            cancellationToken).ConfigureAwait(false);
        Logger.LogInformation(outputLog);
    }

    /// <summary>
    /// Controls the connection state of the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to control the connection state.</param>
    /// <param name="connectOp">The connection operation to perform (e.g., connect, disconnect).</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ConnectionControlAsync(
        string targetVmxFilePath, string diskLabel, string connectOp, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "ConnectionControl", diskLabel, connectOp },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Converts the allocation type of the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to convert the allocation type.</param>
    /// <param name="diskPath">The path of the disk to convert the allocation type.</param>
    /// <param name="type">The new allocation type to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ConvertAllocTypeAsync(
        string targetVmxFilePath, string diskLabel, string diskPath, string type, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "ConvertAllocType", diskLabel, diskPath, type },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Cancels the allocation type conversion operation for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ConvertAllocTypeCancelAsync(
        string targetVmxFilePath, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "ConvertAllocTypeCancel" },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Extends the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to extend.</param>
    /// <param name="newNumSectors">The new number of sectors for the virtual disk.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ExtendAsync(
        string targetVmxFilePath, string diskLabel, int newNumSectors, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "Extended", diskLabel, newNumSectors.ToString() },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Checks if the specified virtual disk is present asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to check for presence.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="JsonDocument"/> representing the presence status of the virtual disk.</returns>
    public async Task<JsonDocument> IsPresentAsync(
        string targetVmxFilePath, string diskLabel, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "IsPresent", diskLabel, "--format", "json" },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
        return JsonDocument.Parse(outputLog);
    }

    /// <summary>
    /// Moves a virtual disk from one device label to another asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="fromDeviceLabel">The label of the device to move the disk from.</param>
    /// <param name="toDeviceLabel">The label of the device to move the disk to.</param>
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
    /// Purges the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to purge.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task PurgeAsync(
        string targetVmxFilePath, string diskLabel, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "Purge", diskLabel },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Queries the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to query.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="JsonDocument"/> representing the query result.</returns>
    public async Task<JsonDocument> QueryAsync(
        string targetVmxFilePath, string diskLabel, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "query", "--format", "json" },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
        return JsonDocument.Parse(outputLog);
    }

    /// <summary>
    /// Sets the allow guest control option for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the allow guest control option.</param>
    /// <param name="allowGuestControl">The value to set for the allow guest control option.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetAllowGuestControlAsync(
        string targetVmxFilePath, string diskLabel, string allowGuestControl, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetAllowGuestControl", diskLabel, allowGuestControl },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the backing information for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the backing information.</param>
    /// <param name="backingType">The type of backing to set.</param>
    /// <param name="backingPath">The path of the backing to set.</param>
    /// <param name="clientDevice">The client device associated with the backing.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetBackingInfoAsync(
        string targetVmxFilePath, string diskLabel, string backingType, string backingPath, string clientDevice, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetBackingInfo", diskLabel, backingType, backingPath, clientDevice },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the bandwidth cap for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the bandwidth cap.</param>
    /// <param name="bandwidthCap">The bandwidth cap to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetBandwidthCapAsync(
        string targetVmxFilePath, string diskLabel, string bandwidthCap, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetBandwidthCap", diskLabel, bandwidthCap },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Enables or disables the CBRC cache for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="enable">The value to set for enabling or disabling the CBRC cache.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetCbrcCacheEnabledAsync(
        string targetVmxFilePath, string enable, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetCbrcCacheEnabled", enable },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Enables or disables the CTK for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the CTK enabled state.</param>
    /// <param name="ctkEnabled">The value to set for enabling or disabling the CTK.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetCtkEnabledAsync(
        string targetVmxFilePath, string diskLabel, string ctkEnabled, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetCtkEnabled", diskLabel, ctkEnabled },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the digest for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the digest.</param>
    /// <param name="digest">The digest value to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetDigestAsync(
        string targetVmxFilePath, string diskLabel, string digest, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetDigest", diskLabel, digest },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the UUID for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the UUID.</param>
    /// <param name="uuid">The UUID to set for the disk.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetUuidAsync(
        string targetVmxFilePath, string diskLabel, string uuid, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetDiskUUID", diskLabel, uuid },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Enables or disables the UUID for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="enable">The value to set for enabling or disabling the UUID.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetUuidEnabledAsync(
        string targetVmxFilePath, string enable, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetDiskUuidEnabled", enable },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the exclusive access for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the exclusive access.</param>
    /// <param name="exclusiveAccess">The value to set for exclusive access.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetExclusiveAccessAsync(
        string targetVmxFilePath, string diskLabel, string exclusiveAccess, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetExclusiveAccess", diskLabel, exclusiveAccess },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the global CTK disallowed state for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="disallow">The value to set for disallowing global CTK.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetGlobalCtkDisallowedAsync(
        string targetVmxFilePath, string disallow, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetGlobalCtkDisallowed", disallow },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the host buffer mode for the specified hard disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="mode">The host buffer mode to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetHardDiskHostBufferAsync(
        string targetVmxFilePath, string mode, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetHardDiskHostBuffer", mode },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the hard disk page alignment mode asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="mode">The page alignment mode to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetHardDiskPageAlignAsync(
        string targetVmxFilePath, string mode, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetHardDiskPageAlign", mode },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the hide type of read-only partition asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="hidePartition">The hide partition value to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetHideTypeOfReadOnlyPartAsync(
        string targetVmxFilePath, string hidePartition, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetHideTypeOfROnlyPart", hidePartition },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the mode of the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the mode.</param>
    /// <param name="mode">The mode to set for the disk.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetModeAsync(
        string targetVmxFilePath, string diskLabel, string mode, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetMode", diskLabel, mode },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the policy of the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the policy.</param>
    /// <param name="policy">The policy to set for the disk.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetPolicyAsync(
        string targetVmxFilePath, string diskLabel, string policy, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetPolicy", diskLabel, policy },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the presence state of the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the presence state.</param>
    /// <param name="present">The presence state to set for the disk.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetPresentAsync(
        string targetVmxFilePath, string diskLabel, string present, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetPresent", diskLabel, present },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the read-only state of the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the read-only state.</param>
    /// <param name="readOnly">The read-only state to set for the disk.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetReadOnlyAsync(
        string targetVmxFilePath, string diskLabel, string readOnly, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetReadOnly", diskLabel, readOnly },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the reservation for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the reservation.</param>
    /// <param name="reservation">The reservation value to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetReservationAsync(
        string targetVmxFilePath, string diskLabel, string reservation, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetReservation", diskLabel, reservation },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the shares for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the shares.</param>
    /// <param name="shares">The shares value to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetSharesAsync(
        string targetVmxFilePath, string diskLabel, string shares, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetShares", diskLabel, shares },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the sharing mode for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the sharing mode.</param>
    /// <param name="sharing">The sharing mode to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetSharingAsync(
        string targetVmxFilePath, string diskLabel, string sharing, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetSharing", diskLabel, sharing },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the SPIF filters for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the SPIF filters.</param>
    /// <param name="spifSpecList">The SPIF specification list to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetSpifFiltersAsync(
        string targetVmxFilePath, string diskLabel, string spifSpecList, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetSpifFilters", diskLabel, spifSpecList },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the start connected state for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the start connected state.</param>
    /// <param name="startConnected">The start connected state to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetStartConnectedAsync(
        string targetVmxFilePath, string diskLabel, string startConnected, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetStartConnected", diskLabel, startConnected },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the throughput cap for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the throughput cap.</param>
    /// <param name="throughputCap">The throughput cap value to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetThroughputCapAsync(
        string targetVmxFilePath, string diskLabel, string throughputCap, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetThroughputCap", diskLabel, throughputCap },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the write-through state for the specified virtual disk asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The file path of the target VMX file.</param>
    /// <param name="diskLabel">The label of the disk to set the write-through state.</param>
    /// <param name="writeThrough">The write-through state to set.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetWriteThroughAsync(
        string targetVmxFilePath, string diskLabel, string writeThrough, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetWriteThrough", diskLabel, writeThrough },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }
}
