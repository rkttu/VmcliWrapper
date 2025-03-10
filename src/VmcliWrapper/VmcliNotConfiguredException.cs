using System;
using System.IO;

namespace VmcliWrapper;

/// <summary>
/// Exception that is thrown when the Vmcli path is not specified.
/// </summary>
public sealed class VmcliNotConfiguredException : FileNotFoundException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliNotConfiguredException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public VmcliNotConfiguredException(Exception? innerException = default)
        : base("Vmcli path was not specified.", innerException)
    {
    }
}
