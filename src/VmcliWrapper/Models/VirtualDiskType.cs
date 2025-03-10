namespace VmcliWrapper.Models;

/// <summary>
/// Specifies the type of virtual disk.
/// </summary>
public enum VirtualDiskType
{
    /// <summary>
    /// A single growable virtual disk.
    /// </summary>
    SingleGrowable,

    /// <summary>
    /// A splitted growable virtual disk.
    /// </summary>
    SplittedGrowable,

    /// <summary>
    /// A single pre-allocated virtual disk.
    /// </summary>
    SinglePreAllocated,

    /// <summary>
    /// A splitted pre-allocated virtual disk.
    /// </summary>
    SplittedPreAllocated,
}
