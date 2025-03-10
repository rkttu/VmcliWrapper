using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VmcliWrapper.Components;

internal sealed class OutputCollector : IAsyncDisposable, IDisposable
{
    public OutputCollector()
    {
        _buffer = new StringBuilder();
        _writer = new StringWriter(_buffer);
    }

    private readonly StringBuilder _buffer;
    private readonly StringWriter _writer;

    public Func<string, CancellationToken, Task> GetReceiverDelegate(CancellationToken cancellationToken)
    {
        return async (str, ct) =>
        {
            await _writer.WriteAsync(str.ToCharArray(), ct).ConfigureAwait(false);
            await _writer.WriteLineAsync().ConfigureAwait(false);
        };
    }

    public async Task<string> ToStringAsync()
    {
        await _writer.FlushAsync().ConfigureAwait(false);
        return _buffer.ToString();
    }

    public override string ToString()
    {
        _writer.Flush();
        return _buffer.ToString();
    }

    public ValueTask DisposeAsync()
    {
        return ((IAsyncDisposable)_writer).DisposeAsync();
    }

    public void Dispose()
    {
        ((IDisposable)_writer).Dispose();
    }
}
