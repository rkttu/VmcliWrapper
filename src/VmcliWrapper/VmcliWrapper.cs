using Microsoft.Extensions.Logging;
using VmcliWrapper.Services;

namespace VmcliWrapper;

/// <summary>
/// Wrapper for the Vmcli basic command.
/// </summary>
/// <remarks>
/// This class provides a wrapper around the basic Vmcli commands, allowing for easier interaction
/// with the Vmcli executable. It inherits from <see cref="VmcliWrapperBase"/> and provides
/// implementation for the <see cref="ModuleName"/> property.
/// </remarks>
public class VmcliWrapper : VmcliWrapperBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VmcliWrapper"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for logging.</param>
    /// <param name="locator">The Vmcli locator for finding the Vmcli executable path.</param>
    public VmcliWrapper(
        ILogger<VmcliWrapper> logger,
        VmcliLocator locator)
        : base(logger, locator) { }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public override string ModuleName => string.Empty;
}
