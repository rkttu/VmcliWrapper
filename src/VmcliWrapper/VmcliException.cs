using System;
using System.Collections.Generic;
using System.Linq;

namespace VmcliWrapper;

/// <summary>
/// Represents errors that occur during vmcli execution.
/// </summary>
public sealed class VmcliException : Exception
{
    /// <summary>
    /// Composes the error message based on the exit code, arguments, and error message.
    /// </summary>
    /// <param name="exitCode">The exit code returned by vmcli.</param>
    /// <param name="arguments">The arguments passed to vmcli.</param>
    /// <param name="error">The error message returned by vmcli.</param>
    /// <returns>A composed error message string.</returns>
    private static string ComposeErrorMessage(
        int exitCode, IEnumerable<string> arguments, string? error)
    {
        var refinedErrorMessage = (error ?? string.Empty)
            .Replace("\r", string.Empty)
            .Replace("\n", string.Empty)
            .Trim().TrimEnd('.');
        var fragments = new List<string>();
        if (string.IsNullOrWhiteSpace(error))
            fragments.Add("vmcli does not return non-zero exit code.");
        else
            fragments.Add($"vmcli returns one or more error: {refinedErrorMessage}.");
        fragments.Add($"(Arguments: {string.Join(' ', arguments)}, Exit code: {exitCode})");
        return string.Join(' ', fragments);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliException"/> class with a specified error message, exit code, arguments, output, and inner exception.
    /// </summary>
    /// <param name="exitCode">The exit code returned by vmcli.</param>
    /// <param name="arguments">The arguments passed to vmcli.</param>
    /// <param name="output">The output returned by vmcli.</param>
    /// <param name="error">The error message returned by vmcli.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public VmcliException(int exitCode, IEnumerable<string> arguments, string? output, string? error, Exception? innerException = default)
        : base(ComposeErrorMessage(exitCode, arguments, error), innerException)
    {
        ExitCode = exitCode;
        Arguments = arguments.ToArray();
        Output = output;
        Error = error;
    }

    /// <summary>
    /// Gets the output returned by vmcli.
    /// </summary>
    public string? Output { get; private set; }

    /// <summary>
    /// Gets the error message returned by vmcli.
    /// </summary>
    public string? Error { get; private set; }

    /// <summary>
    /// Gets the exit code returned by vmcli.
    /// </summary>
    public int ExitCode { get; private set; }

    /// <summary>
    /// Gets the arguments passed to vmcli.
    /// </summary>
    public IEnumerable<string> Arguments { get; private set; }
}
