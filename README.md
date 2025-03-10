# VmcliWrapper

[![NuGet Version](https://img.shields.io/nuget/v/VmcliWrapper)](https://www.nuget.org/packages/VmcliWrapper/) ![Build Status](https://github.com/rkttu/VmcliWrapper/actions/workflows/dotnet.yml/badge.svg) [![GitHub Sponsors](https://img.shields.io/github/sponsors/rkttu)](https://github.com/sponsors/rkttu/)

A library that helps programmatically invoke and control the **vmcli** command-line utility.

## How to use

```csharp
appBuilder.Services.AddVmcliServices();

var app = appBuilder.Build();

var power = app.Services.GetRequiredService<VmcliPowerWrapper>();
var chipset = app.Services.GetRequiredService<VmcliChipsetWrapper>();
var configParams = app.Services.GetRequiredService<VmcliConfigParameterWrapper>();
var vm = app.Services.GetRequiredService<VmcliVmWrapper>();

var vmxFileInfo = await vm.CreateAsync(
	vmxPath,
	$"myVM", GuestOSTypes.Windows_Server_2025);

var vmxPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "vmware-test", "myVM.vmx");

var chipsetProps = await chipset.QueryAsync(vmxPath);

var vmProps = await configParams.QueryAsync(vmxPath);

await configParams.SetEntryAsync(vmxPath, VirtualMachineEntryNames.DisplayName, "myVM");

var diskPath = await wrapper.CreateVirtualDiskAsync(
	vmdkPath, VirtualDiskAdapterType.IDE, "2GB", VirtualDiskType.SplittedGrowable);

var vmStatProps = await power.QueryAsync(vmxPath);

await power.StartAsync(vmxPath);
vmStatProps = await power.QueryAsync(vmxPath);

await power.StopAsync(vmxPath);
vmStatProps = await power.QueryAsync(vmxPath);
```

## License

This library follows Apache-2.0 license. See [LICENSE](./LICENSE) file for more information.
