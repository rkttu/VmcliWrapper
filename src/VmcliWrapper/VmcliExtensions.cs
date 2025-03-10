using Microsoft.Extensions.DependencyInjection;
using VmcliWrapper.Services;

namespace VmcliWrapper;

/// <summary>
/// Provides extension methods for adding VMCLI services to the dependency injection container.
/// </summary>
public static class VmcliExtensions
{
    /// <summary>
    /// Adds VMCLI services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddVmcliServices(
        this IServiceCollection services)
        => services
            .AddSingleton<VmcliLocator>()
            .AddSingleton<VmcliWrapper>()
            .AddSingleton<VmcliChipsetWrapper>()
            .AddSingleton<VmcliConfigParameterWrapper>()
            .AddSingleton<VmcliDiskWrapper>()
            .AddSingleton<VmcliEthernetWrapper>()
            .AddSingleton<VmcliGuestWrapper>()
            .AddSingleton<VmcliHgfsWrapper>()
            .AddSingleton<VmcliMksWrapper>()
            .AddSingleton<VmcliNvmeWrapper>()
            .AddSingleton<VmcliPowerWrapper>()
            .AddSingleton<VmcliSataWrapper>()
            .AddSingleton<VmcliSerialWrapper>()
            .AddSingleton<VmcliSnapshotWrapper>()
            .AddSingleton<VmcliToolsWrapper>()
            .AddSingleton<VmcliVmWrapper>()
            .AddSingleton<VmcliVmTemplateWrapper>()
            .AddSingleton<VmcliVProbesWrapper>()
            ;
}
