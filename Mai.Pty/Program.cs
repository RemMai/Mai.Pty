using Mai.Pty;
using Mai.Pty.Services;

var service = new TerminalService();
service.StandardOutput += (sender, s) => { Console.Write(s); };
service.StandardError += (sender, s) => { Console.Write(s); };
await service.Start();

while (true)
{
    var input = Console.ReadKey(true);

    var bytes = ComparisonTable.GetKeyByte(input.Key);
    if (bytes != null)
    {
        await service.Write(bytes).ConfigureAwait(false);
    }
    else
    {
        await service.Write(input.KeyChar).ConfigureAwait(false);
    }
}