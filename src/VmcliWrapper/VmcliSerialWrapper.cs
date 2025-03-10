using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VmcliWrapper.Services;

namespace VmcliWrapper;

/// <summary>
/// Wrapper for the Vmcli Serial command module.
/// </summary>
public class VmcliSerialWrapper : VmcliWrapperBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliSerialWrapper"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="locator">The Vmcli locator for finding the Vmcli executable path.</param>
    public VmcliSerialWrapper(
        ILogger<VmcliSerialWrapper> logger,
        VmcliLocator locator)
        : base(logger, locator) { }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public override string ModuleName => "Serial";

    /// <summary>
    /// Controls the connection state of a serial device.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to control.</param>
    /// <param name="opType">The operation type to perform (e.g., connect, disconnect).</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ConnectionControlAsync(
        string targetVmxFilePath, string deviceLabel, string opType, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "ConnectionControl", deviceLabel, opType },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Purges the specified serial device.
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
    /// Queries the serial device information.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the queried information as a JSON document.</returns>
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
    /// Sets the allow guest control option for the specified serial device.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to configure.</param>
    /// <param name="allowGuestControl">The value to set for allowing guest control.</param>
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
    /// Sets the backing information for the specified serial device.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to configure.</param>
    /// <param name="backingType">The type of backing to set.</param>
    /// <param name="backingPath">The path to the backing file.</param>
    /// <param name="backingPathNetProxy">The network proxy for the backing path.</param>
    /// <param name="pipeEndPoint">The pipe endpoint for the backing path.</param>
    /// <param name="netEndPoint">The network endpoint for the backing path.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetBackingInfoAsync(
        string targetVmxFilePath, string deviceLabel, string backingType, string backingPath, string backingPathNetProxy, string pipeEndPoint, string netEndPoint, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "SetBackingInfo", deviceLabel, backingType, backingPath, backingPathNetProxy, pipeEndPoint, netEndPoint },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the present state of the specified serial device.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to configure.</param>
    /// <param name="enabled">The value to set for the present state.</param>
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
    /// Sets the start connected option for the specified serial device.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to configure.</param>
    /// <param name="startConnected">The value to set for the start connected option.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task StartConnectedAsync(
        string targetVmxFilePath, string deviceLabel, string startConnected, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "StartConnected", deviceLabel, startConnected },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the try no RX loss option for the specified serial device.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to configure.</param>
    /// <param name="tryNoRxLoss">The value to set for the try no RX loss option.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task TryNoRxLossAsync(
        string targetVmxFilePath, string deviceLabel, string tryNoRxLoss, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "TryNoRxLoss", deviceLabel, tryNoRxLoss },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Sets the yield on MSR read option for the specified serial device.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deviceLabel">The label of the device to configure.</param>
    /// <param name="yieldOnMsrRead">The value to set for the yield on MSR read option.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task YieldOnMsrReadAsync(
        string targetVmxFilePath, string deviceLabel, string yieldOnMsrRead, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "YieldOnMsrRead", deviceLabel, yieldOnMsrRead },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }
}
