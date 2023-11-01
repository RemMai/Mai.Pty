using System.Text;

namespace Mai.Pty;

public static class ComparisonTable
{
    public static Dictionary<ConsoleKey, string> ConsoleKeyMap = new()
    {
        { ConsoleKey.LeftArrow, "[D" },
        { ConsoleKey.RightArrow, "[C" },
        { ConsoleKey.UpArrow, "[A" },
        { ConsoleKey.DownArrow, "[B" },

        { ConsoleKey.F1, "OP" },
        { ConsoleKey.F2, "OQ" },
        { ConsoleKey.F3, "OR" },
        { ConsoleKey.F4, "OS" },
        { ConsoleKey.F5, "[15~" },
        { ConsoleKey.F6, "[17~" },
        { ConsoleKey.F7, "[18~" },
        { ConsoleKey.F8, "[19~" },
        { ConsoleKey.F9, "[20~" },
        { ConsoleKey.F10, "[21~" },
        { ConsoleKey.F11, "[23~" },
        { ConsoleKey.F12, "[24~" },
        { ConsoleKey.Insert, "[2~" },
        { ConsoleKey.Delete, "[3~" },
        { ConsoleKey.Home, "[H" },
        { ConsoleKey.End, "[F" },
        { ConsoleKey.PageUp, "[5~" },
        { ConsoleKey.PageDown, "[6~" },
    };

    public static byte[]? GetKeyByte(ConsoleKey key)
    {
        if (ConsoleKeyMap.TryGetValue(key, out var value))
        {
            return Encoding.Default.GetBytes(value);
        }

        return null;
    }
}