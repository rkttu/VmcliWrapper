using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VmcliWrapper.Models;
using VmcliWrapper.Services;

namespace VmcliWrapper;

/// <summary>
/// Wrapper for the Vmcli Power command module.
/// </summary>
public class VmcliPowerWrapper : VmcliWrapperBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliPowerWrapper"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="locator">The Vmcli locator for finding the Vmcli executable path.</param>
    public VmcliPowerWrapper(
        ILogger<VmcliPowerWrapper> logger,
        VmcliLocator locator)
        : base(logger, locator) { }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public override string ModuleName => "Power";

    /// <summary>
    /// Queries the power state of the virtual machine.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the power state as a <see cref="JsonDocument"/>.</returns>
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
    /// Starts the virtual machine.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="paused">Indicates whether to start the VM in a paused state.</param>
    /// <param name="soft">Indicates whether to perform a soft start.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task StartAsync(
        string targetVmxFilePath, bool paused = false, bool soft = false, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "Start" });
        if (paused)
            argList.Add("-p");
        if (soft)
            argList.Add("-s");
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Stops the virtual machine.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="opType">The type of stop operation to perform.</param>
    /// <param name="forRevert">Indicates whether the stop is for a revert operation.</param>
    /// <param name="snapshotId">The snapshot ID to revert to, if applicable.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task StopAsync(
        string targetVmxFilePath, VmResetOpType opType = default, bool forRevert = false, int? snapshotId = default, CancellationToken cancellationToken = default)
    {
        var opTypeArg = opType switch
        {
            VmResetOpType.RequireSoft => "requireSoft",
            VmResetOpType.TrySoft => "trySoft",
            VmResetOpType.Hard => "hard",
            _ => throw new ArgumentException($"Unknown optype: {opType}", nameof(opType)),
        };
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "Stop", "-o", opTypeArg });
        if (forRevert)
            argList.Add("-r");
        if (snapshotId.HasValue)
            argList.AddRange(new string[] { "-si", snapshotId.Value.ToString() });
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Pauses the virtual machine.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task PauseAsync(
        string targetVmxFilePath, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "Pause" },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Resets the virtual machine.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="opType">The type of reset operation to perform.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ResetAsync(
        string targetVmxFilePath, VmResetOpType opType = default, CancellationToken cancellationToken = default)
    {
        var opTypeArg = opType switch
        {
            VmResetOpType.RequireSoft => "requireSoft",
            VmResetOpType.TrySoft => "trySoft",
            VmResetOpType.Hard => "hard",
            _ => throw new ArgumentException($"Unknown optype: {opType}", nameof(opType)),
        };
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "Reset", "-o", opTypeArg },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Suspends the virtual machine.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="opType">The type of suspend operation to perform.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SuspendAsync(
        string targetVmxFilePath, VmResetOpType opType = default, CancellationToken cancellationToken = default)
    {
        var opTypeArg = opType switch
        {
            VmResetOpType.RequireSoft => "requireSoft",
            VmResetOpType.TrySoft => "trySoft",
            VmResetOpType.Hard => "hard",
            _ => throw new ArgumentException($"Unknown optype: {opType}", nameof(opType)),
        };
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "Suspend", "-o", opTypeArg },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Unpauses the virtual machine.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UnpauseAsync(
        string targetVmxFilePath, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "Unpause" },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }
}
