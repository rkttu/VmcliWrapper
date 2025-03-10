using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VmcliWrapper.Services;

namespace VmcliWrapper;

/// <summary>
/// Wrapper for the Vmcli Guest command module.
/// </summary>
public class VmcliGuestWrapper : VmcliWrapperBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliGuestWrapper"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="locator">The Vmcli locator for finding the Vmcli executable path.</param>
    public VmcliGuestWrapper(
        ILogger<VmcliGuestWrapper> logger,
        VmcliLocator locator)
        : base(logger, locator) { }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public override string ModuleName => "Guest";

    /// <summary>
    /// Queries the guest information asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the guest information as a <see cref="JsonDocument"/>.</returns>
    public async Task<JsonDocument> QueryAsync(
        string targetVmxFilePath, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "Query", "--format", "json" },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
        return JsonDocument.Parse(outputLog);
    }

    /// <summary>
    /// Copies a file from the guest to the host asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="overwrite">A value indicating whether to overwrite the destination file if it exists.</param>
    /// <param name="fromPath">The source file path in the guest.</param>
    /// <param name="toPath">The destination file path on the host.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CopyFromAsync(
        string targetVmxFilePath, string username, string password, bool overwrite, string fromPath, string toPath, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "copyFrom", "-u", username, "-p", password });
        if (overwrite)
            argList.Add("-o");
        argList.AddRange(new string[] { fromPath, toPath });
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Copies a file from the host to the guest asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="overwrite">A value indicating whether to overwrite the destination file if it exists.</param>
    /// <param name="fromPath">The source file path on the host.</param>
    /// <param name="toPath">The destination file path in the guest.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CopyToAsync(
        string targetVmxFilePath, string username, string password, bool overwrite, string fromPath, string toPath, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "copyTo", "-u", username, "-p", password });
        if (overwrite)
            argList.Add("-o");
        argList.AddRange(new string[] { fromPath, toPath });
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Creates a temporary directory in the guest asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="prefix">The prefix for the temporary directory name.</param>
    /// <param name="suffix">The suffix for the temporary directory name.</param>
    /// <param name="directory">The parent directory in which to create the temporary directory.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CreateTempDirAsync(
        string targetVmxFilePath, string username, string password, string prefix, string suffix, string directory, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "createTempDir", "-u", username, "-p", password, prefix, suffix, directory },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Creates a temporary file in the guest asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="prefix">The prefix for the temporary file name.</param>
    /// <param name="suffix">The suffix for the temporary file name.</param>
    /// <param name="directory">The parent directory in which to create the temporary file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CreateTempFileAsync(
        string targetVmxFilePath, string username, string password, string prefix, string suffix, string directory, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "createTempFile", "-u", username, "-p", password, prefix, suffix, directory },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Retrieves the environment variables from the guest asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the environment variables as a string.</returns>
    public async Task<string> EnvAsync(
        string targetVmxFilePath, string username, string password, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "env", "-u", username, "-p", password },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
        return outputLog;
    }

    /// <summary>
    /// Kills a process in the guest asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="pid">The process ID to kill.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task KillAsync(
        string targetVmxFilePath, string username, string password, int pid, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "kill", "-u", username, "-p", password, pid.ToString() },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Lists the contents of a directory in the guest asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="path">The directory path to list.</param>
    /// <param name="regexp">The regular expression to filter the results.</param>
    /// <param name="index">The index to start listing from.</param>
    /// <param name="max">The maximum number of results to return.</param>
    /// <param name="seen">The number of results already seen.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task LsAsync(
        string targetVmxFilePath, string username, string password, string path, string? regexp = default, int? index = default, int? max = default, int? seen = default, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "ls", "-u", username, "-p", password });
        if (!string.IsNullOrEmpty(regexp))
            argList.AddRange(new string[] { "-r", regexp });
        if (index.HasValue)
            argList.AddRange(new string[] { "-i", index.Value.ToString() });
        if (max.HasValue)
            argList.AddRange(new string[] { "-m", max.Value.ToString() });
        if (seen.HasValue)
            argList.AddRange(new string[] { "-s", seen.Value.ToString() });
        argList.Add(path);
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Creates a directory in the guest asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="createParentDirectory">A value indicating whether to create parent directories if they do not exist.</param>
    /// <param name="path">The directory path to create.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task MkdirAsync(
        string targetVmxFilePath, string username, string password, bool createParentDirectory, string path, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "mkdir", "-u", username, "-p", password });
        if (createParentDirectory)
            argList.Add("--parent");
        argList.Add(path);
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Moves a file in the guest asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="overwrite">A value indicating whether to overwrite the destination file if it exists.</param>
    /// <param name="fromPath">The source file path.</param>
    /// <param name="toPath">The destination file path.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task MvAsync(
        string targetVmxFilePath, string username, string password, bool overwrite, string fromPath, string toPath, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "mv", "-u", username, "-p", password });
        if (overwrite)
            argList.Add("-o");
        argList.AddRange(new string[] { fromPath, toPath });
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Moves a directory in the guest asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="overwrite">A value indicating whether to overwrite the destination directory if it exists.</param>
    /// <param name="fromPath">The source directory path.</param>
    /// <param name="toPath">The destination directory path.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task MvDirAsync(
        string targetVmxFilePath, string username, string password, bool overwrite, string fromPath, string toPath, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "mvdir", "-u", username, "-p", password });
        if (overwrite)
            argList.Add("-o");
        argList.AddRange(new string[] { fromPath, toPath });
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Lists the processes running in the guest asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="pid">The process ID to filter the results. If null, lists all processes.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task PsAsync(
        string targetVmxFilePath, string username, string password, string? pid, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "ps", "-u", username, "-p", password });
        if (!string.IsNullOrWhiteSpace(pid))
            argList.AddRange(new string[] { "--pid", pid });
        argList.AddRange(new string[] { "--format", "json" });
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Removes a file in the guest asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="path">The file path to remove.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RmAsync(
        string targetVmxFilePath, string username, string password, string path, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "rm", "-u", username, "-p", password, path },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Removes a directory in the guest asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="recursive">A value indicating whether to remove directories and their contents recursively.</param>
    /// <param name="path">The directory path to remove.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RmDirAsync(
        string targetVmxFilePath, string username, string password, bool recursive, string path, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "rmdir", "-u", username, "-p", password });
        if (recursive)
            argList.Add("-r");
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Runs a program in the guest asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="activateWindow">A value indicating whether to activate the program window.</param>
    /// <param name="noWait">A value indicating whether to not wait for the program to exit.</param>
    /// <param name="interactive">A value indicating whether to run the program interactively.</param>
    /// <param name="workingDirectory">The working directory for the program.</param>
    /// <param name="environment">The environment variables for the program.</param>
    /// <param name="programName">The name of the program to run.</param>
    /// <param name="programArgs">The arguments to pass to the program.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RunAsync(
        string targetVmxFilePath, string username, string password, bool activateWindow,
        bool noWait, bool interactive, string? workingDirectory, string? environment,
        string programName, string? programArgs, CancellationToken cancellationToken = default)
    {
        var argList = new List<string>(new string[] { targetVmxFilePath, ModuleName, "run", "-u", username, "-p", password });
        if (activateWindow)
            argList.Add("-aw");
        if (noWait)
            argList.Add("-nw");
        if (interactive)
            argList.Add("-i");
        if (!string.IsNullOrWhiteSpace(workingDirectory))
            argList.AddRange(new string[] { "-w", workingDirectory });
        if (!string.IsNullOrWhiteSpace(environment))
            argList.AddRange(new string[] { "-e", environment });
        argList.Add(programName);
        if (!string.IsNullOrWhiteSpace(programArgs))
            argList.Add(programArgs);
        var outputLog = await ExecuteAsync(
            argList.AsReadOnly(),
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
    }

    /// <summary>
    /// Retrieves the tools properties from the guest asynchronously.
    /// </summary>
    /// <param name="targetVmxFilePath">The path to the target VMX file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the tools properties as a string.</returns>
    public async Task<string> ToolsProperties(
        string targetVmxFilePath, CancellationToken cancellationToken = default)
    {
        var outputLog = await ExecuteAsync(
            new string[] { targetVmxFilePath, ModuleName, "toolsproperties" },
            cancellationToken).ConfigureAwait(false);
        Logger.LogTrace(outputLog);
        return outputLog;
    }
}
