using System;

namespace Lirxe.Logging
{
    public class ConsoleColors
    {
        public static ConsoleColor ToConsoleColor(string c) =>
            c switch
            {
                "b"=>ConsoleColor.Black,
                "db"=>ConsoleColor.DarkBlue,
                "dg"=>ConsoleColor.DarkGreen,
                "dc"=>ConsoleColor.DarkCyan,
                "dr"=>ConsoleColor.DarkRed,
                "dm"=>ConsoleColor.DarkMagenta,
                "g"=>ConsoleColor.Gray,
                "dgr"=>ConsoleColor.DarkGray,
                "bl"=>ConsoleColor.Blue,
                "gr"=>ConsoleColor.Green,
                "c"=>ConsoleColor.Cyan,
                "r"=>ConsoleColor.Red,
                "m"=>ConsoleColor.Magenta,
                "y"=>ConsoleColor.Yellow,
                "w"=>ConsoleColor.White,
                _=>ConsoleColor.White
            };
    }
}