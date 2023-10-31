using Mai.Pty.Services;

var service = new TerminalService(cols: 80);

while (true)
{
    var input = Console.ReadKey(true);
    byte data = input.KeyChar == 0 ? (byte)input.Key : Convert.ToByte(input.KeyChar);
    await service.Send(data).ConfigureAwait(false);
}