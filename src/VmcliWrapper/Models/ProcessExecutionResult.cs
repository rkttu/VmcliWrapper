namespace VmcliWrapper.Models;

internal sealed record class ProcessExecutionResult(string Output, string Error, int ExitCode);
