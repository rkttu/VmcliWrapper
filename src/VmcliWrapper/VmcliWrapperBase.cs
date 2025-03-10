using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VmcliWrapper.Components;
using VmcliWrapper.Models;
using VmcliWrapper.Services;

namespace VmcliWrapper;

/// <summary>
/// Base class for VmcliWrapper providing common functionality for executing Vmcli commands.
/// </summary>
public abstract class VmcliWrapperBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliWrapperBase"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="locator">The Vmcli locator for finding the Vmcli executable path.</param>
    public VmcliWrapperBase(
        ILogger<VmcliWrapperBase> logger,
        VmcliLocator locator)
    {
        Logger = logger;
        VmcliLocator = locator;
    }

    /// <summary>
    /// The logger instance for logging.
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    /// The Vmcli locator for finding the Vmcli executable path.
    /// </summary>
    protected readonly VmcliLocator VmcliLocator;

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public abstract string ModuleName { get; }

    /// <summary>
    /// Gets the version of the Vmcli.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the version string.</returns>
    public async Task<string> GetVersionAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(
            new string[] { "--version" },
            cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the help information for the module.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the help information string.</returns>
    public async Task<string> GetModuleHelpAsync(CancellationToken cancellationToken = default)
    {
        var argList = new List<string>();
        if (!string.IsNullOrWhiteSpace(ModuleName))
            argList.Add(ModuleName);
        argList.Add("--help");
        return await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the Vmcli command with the specified arguments.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the Vmcli command.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the command output string.</returns>
    /// <exception cref="VmcliException">Thrown when the Vmcli command exits with a non-zero exit code.</exception>
    public async Task<string> ExecuteAsync(IEnumerable<string> arguments, CancellationToken cancellationToken = default)
    {
        var result = await RunVmCliAsync(arguments, cancellationToken).ConfigureAwait(false);

        if (!string.IsNullOrWhiteSpace(result.Error))
            Logger.LogWarning(result.Error);

        if (result.ExitCode != 0)
            throw new VmcliException(result.ExitCode, arguments, result.Output, result.Error);

        return result.Output;
    }

    /// <summary>
    /// Runs the Vmcli command with the specified arguments and collects the output and error streams.
    /// </summary>
    /// <param name="arguments">The arguments to pass to the Vmcli command.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the process execution result.</returns>
    internal async Task<ProcessExecutionResult> RunVmCliAsync(IEnumerable<string> arguments, CancellationToken cancellationToken = default)
    {
        using var outputCollector = new OutputCollector();
        using var errorCollector = new OutputCollector();
        using var runner = new ProcessRunner(
            VmcliLocator.GetVmCliPath().FullName, arguments,
            outputCollector.GetReceiverDelegate(cancellationToken),
            errorCollector.GetReceiverDelegate(cancellationToken));
        var exitCode = await runner.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
        return new ProcessExecutionResult(
            await outputCollector.ToStringAsync().ConfigureAwait(false),
            await errorCollector.ToStringAsync().ConfigureAwait(false),
            exitCode);
    }
}
