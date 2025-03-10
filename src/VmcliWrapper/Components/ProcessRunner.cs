using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VmcliWrapper.Components;

internal sealed class ProcessRunner : IDisposable
{
    public ProcessRunner(
        string executableFileName, IEnumerable<string> arguments,
        Func<string, CancellationToken, Task>? outputReceived = default,
        Func<string, CancellationToken, Task>? errorReceived = default,
        Encoding? inputEncoding = default,
        Encoding? outputEncoding = default,
        Encoding? errorEncoding = default)
    {
        _outputReceived = outputReceived;
        _errorReceived = errorReceived;

        _startInfo = new ProcessStartInfo
        {
            FileName = executableFileName,
            RedirectStandardInput = true,
            RedirectStandardOutput = outputReceived != null,
            RedirectStandardError = errorReceived != null,
            StandardInputEncoding = inputEncoding,
            StandardOutputEncoding = outputEncoding,
            StandardErrorEncoding = errorEncoding,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        foreach (var eachArgument in arguments)
            _startInfo.ArgumentList.Add(eachArgument);

        _process = new Process
        {
            StartInfo = _startInfo,
            EnableRaisingEvents = true,
        };

        if (!_process.Start())
            throw new Exception($"Cannot start the process '{executableFileName}'.");

        if (outputReceived != null)
            _stdoutTask = ReadStreamAsync(_process.StandardOutput, _outputReceived);

        if (errorReceived != null)
            _stderrTask = ReadStreamAsync(_process.StandardError, _errorReceived);
    }

    private readonly ProcessStartInfo _startInfo;
    private readonly Process _process;
    private readonly Task? _stdoutTask;
    private readonly Task? _stderrTask;

    public Func<string, CancellationToken, Task>? _outputReceived;
    public Func<string, CancellationToken, Task>? _errorReceived;

    public bool IsRunning => !_process.HasExited;

    private async Task ReadStreamAsync(
        StreamReader reader, Func<string, CancellationToken, Task>? callback = default,
        CancellationToken cancellationToken = default)
    {
        string? line;
        while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
        {
            if (callback != null)
                await callback.Invoke(line, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task SendInputAsync(string input, CancellationToken cancellationToken = default)
    {
        if (!_process.StandardInput.BaseStream.CanWrite)
            return;

        await _process.StandardInput.WriteLineAsync(input.ToCharArray(), cancellationToken).ConfigureAwait(false);
        await _process.StandardInput.FlushAsync().ConfigureAwait(false);
    }

    public async Task<int> WaitForExitAsync(CancellationToken cancellationToken = default)
    {
        if (_stdoutTask != null)
            await _stdoutTask.ConfigureAwait(false);

        if (_stderrTask != null)
            await _stderrTask.ConfigureAwait(false);

        await _process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);

        if (_stdoutTask != null)
            await _process.StandardOutput.BaseStream.FlushAsync(cancellationToken).ConfigureAwait(false);

        if (_stderrTask != null)
            await _process.StandardError.BaseStream.FlushAsync(cancellationToken).ConfigureAwait(false);

        return _process.ExitCode;
    }

    public void Dispose()
    {
        _process?.Dispose();
    }
}
