using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Text;
using Pty.Net;

namespace Mai.Pty.Services;

public class TerminalService
{
    private PtyOptions _options { get; set; }
    private CancellationToken TimeoutToken { get; } = new CancellationTokenSource(300_000).Token;
    private readonly TaskCompletionSource<uint> _processExitedTcs = new();
    private UTF8Encoding _encoding = new(encoderShouldEmitUTF8Identifier: false);
    public ConcurrentBag<string> Output { get; private set; }

    public string App = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
        ? Path.Combine(Environment.SystemDirectory, "cmd.exe")
        : "sh";

    private IPtyConnection? Terminal { get; set; }

    public TerminalService(int rows = 25, int cols = 80, string? cwd = null)
    {
        if (cwd != null && !Directory.Exists(cwd))
        {
            throw new ArgumentNullException(nameof(cwd), "cwd must be a valid directory");
        }

        _options = new PtyOptions
        {
            Name = "Mai.Pty.Core",
            Cols = cols,
            Rows = rows,
            Cwd = cwd ?? Environment.CurrentDirectory,
            App = App,
        };

        Output = new ConcurrentBag<string>();
        Builder();
    }

    public async Task Builder()
    {
        if (Terminal == null)
        {
            Terminal = await PtyProvider.SpawnAsync(_options, TimeoutToken);
            Terminal.ProcessExited += (sender, e) => _processExitedTcs.TrySetResult((uint)Terminal.ExitCode);

            BuilderMessageOutContainer();
        }
    }


    private void BuilderMessageOutContainer()
    {
        Task.Run(async () =>
        {
            while (!TimeoutToken.IsCancellationRequested && !_processExitedTcs.Task.IsCompleted)
            {
                try
                {
                    var bytes = new byte[4096];
                    var count = await Terminal!.ReaderStream.ReadAsync(bytes, TimeoutToken).ConfigureAwait(false);

                    if (count == 0)
                    {
                        break;
                    }

                    var data = _encoding.GetString(bytes);
                    Console.Write(data);
                    Output.Add(data);
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine(e);
                    throw;
                }
            }
        }, TimeoutToken).ConfigureAwait(false);
    }


    public async Task<bool> Send(string data)
    {
        return await Send(_encoding.GetBytes(data)).ConfigureAwait(false);
    }


    public async Task<bool> Send(char data)
    {
        return await Send(new[] { Convert.ToByte(data) }).ConfigureAwait(false);
    }

    public async Task<bool> Send(byte data)
    {
        // return await Send(new[] { data }).ConfigureAwait(false);

        Terminal!.WriterStream.WriteByte(data);
        await Terminal.WriterStream.FlushAsync(TimeoutToken).ConfigureAwait(false);

        return true;
    }

    public async Task<bool> Send(byte[] data)
    {
        await Terminal!.WriterStream.WriteAsync(data, TimeoutToken).ConfigureAwait(false);
        await Terminal.WriterStream.FlushAsync(TimeoutToken).ConfigureAwait(false);

        return true;
    }
}