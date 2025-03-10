namespace VmcliWrapper.Models;

/// <summary>
/// Specifies the type of virtual disk adapter.
/// </summary>
public enum VirtualDiskAdapterType
{
    /// <summary>
    /// The adapter type is unknown.
    /// </summary>
    Unknown,
    /// <summary>
    /// Integrated Drive Electronics (IDE) adapter.
    /// </summary>
    IDE,
    /// <summary>
    /// BusLogic SCSI adapter.
    /// </summary>
    BusLogic,
    /// <summary>
    /// LSI Logic SCSI adapter.
    /// </summary>
    LSILogic,
}
