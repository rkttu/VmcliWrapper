namespace VmcliWrapper.Models;

/// <summary>
/// Defines the types of reset operations that can be performed on a virtual machine.
/// </summary>
public enum VmResetOpType
{
    /// <summary>
    /// Attempt a soft reset, but do not require it.
    /// </summary>
    TrySoft,

    /// <summary>
    /// Require a soft reset.
    /// </summary>
    RequireSoft,

    /// <summary>
    /// Perform a hard reset.
    /// </summary>
    Hard,
}
