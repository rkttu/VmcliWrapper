using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VmcliWrapper.Services;

namespace VmcliWrapper;

/// <summary>
/// Wrapper for the Vmcli Snapshot command module.
/// </summary>
public class VmcliSnapshotWrapper : VmcliWrapperBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliSnapshotWrapper"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="locator">The Vmcli locator for finding the Vmcli executable path.</param>
    public VmcliSnapshotWrapper(
        ILogger<VmcliSnapshotWrapper> logger,
        VmcliLocator locator)
        : base(logger, locator) { }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public override string ModuleName => "Snapshot";

    /// <summary>
    /// Queries the snapshot information for the specified VMX file.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the snapshot information as a <see cref="JsonDocument"/>.</returns>
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
    /// Clones the specified VMX file snapshot.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="overwrite">A value indicating whether to overwrite the existing snapshot.</param>
    /// <param name="linked">A value indicating whether to create a linked clone.</param>
    /// <param name="uid">The unique identifier for the snapshot.</param>
    /// <param name="filePath">The file path for the cloned snapshot.</param>
    /// <param name="name">The name of the cloned snapshot.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CloneAsync(
        string targetVmxFilePath, bool overwrite, bool linked, string uid, string filePath, string name, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "Clone" });
        if (overwrite)
            argList.Add("-o");
        if (linked)
            argList.Add("-l");
        argList.AddRange(new string[] { uid, filePath, name });
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Deletes the specified VMX file snapshot.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="deleteChildren">A value indicating whether to delete child snapshots.</param>
    /// <param name="uid">The unique identifier for the snapshot.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteAsync(
        string targetVmxFilePath, bool deleteChildren, string uid, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "Delete" });
        if (deleteChildren)
            argList.Add("-d");
        argList.Add(uid);
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Takes a snapshot of the specified VMX file.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="native">A value indicating whether to take a native snapshot.</param>
    /// <param name="memory">A value indicating whether to include memory in the snapshot.</param>
    /// <param name="description">The description of the snapshot.</param>
    /// <param name="name">The name of the snapshot.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task TakeAsync(
        string targetVmxFilePath, bool native, bool memory, string? description, string name, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "Take" });
        if (native)
            argList.Add("-n");
        if (memory)
            argList.Add("-d");
        if (!string.IsNullOrEmpty(description))
            argList.AddRange(new string[] { "-d", description });
        argList.Add(name);
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Reverts the specified VMX file to a snapshot.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="native">A value indicating whether to revert to a native snapshot.</param>
    /// <param name="uid">The unique identifier for the snapshot.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RevertAsync(
        string targetVmxFilePath, bool native, string uid, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "Revert" });
        if (native)
            argList.Add("-n");
        argList.Add(uid);
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }
}
