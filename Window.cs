using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace BlueShadowMon
{
    /// <summary>
    /// Console Manager.
    /// </summary>
    [SupportedOSPlatform("windows")]
    internal static class Window
    {
        public static ConsoleColor DefaultFgColor { get; set; } = ConsoleColor.White;
        public static ConsoleColor DefaultBgColor { get; set; } = ConsoleColor.Black;
        public static int MiddleX { get { return Console.WindowWidth / 2; }}
        public static int MiddleY { get { return Console.WindowHeight / 2; }}
        public static List<ConsoleKeyInfo> Inputs { get; private set; } = new List<ConsoleKeyInfo>();

        public struct ColoredChar
        {
            public ColoredChar(char c, ConsoleColor fg, ConsoleColor bg)
            {
                Char = c;
                ForegroundColor = fg;
                BackgroundColor = bg;
            }

            public ColoredChar(char c)
            {
                Char = c;
                ForegroundColor = DefaultFgColor;
                BackgroundColor = DefaultBgColor;
            }

            public char Char;
            public ConsoleColor ForegroundColor;
            public ConsoleColor BackgroundColor;
        }

        public struct ColoredString
        {
            public ColoredString(string s, ConsoleColor fg, ConsoleColor bg)
            {
                String = s;
                ForegroundColor = fg;
                BackgroundColor = bg;
            }

            public ColoredString(string s)
            {
                String = s;
                ForegroundColor = DefaultFgColor;
                BackgroundColor = DefaultBgColor;
            }

            public string String;
            public ConsoleColor ForegroundColor;
            public ConsoleColor BackgroundColor;
        }

        private static ColoredChar _charAntiGarbageCollector = new ColoredChar();
        private static ColoredString _stringAntiGarbageCollector = new ColoredString();

        /// <summary>
        /// Disable resizing, minimize and maximize buttons.
        /// Change console properties.
        /// </summary>
        public static void Setup()
        {
            // Disable resizing and minimize/maximize buttons
            IntPtr window = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(window, false);
            int MF_BYCOMMAND = 0x00000000;
            int SC_MINIMIZE = 0xF020;
            int SC_MAXIMIZE = 0xF030;
            int SC_SIZE = 0xF000;
            DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);

            // Set console properties
            Console.Title = Game.GameTitle;
            Console.TreatControlCAsInput = true;
            Console.CursorVisible = false;

            // Set console size to 75% of the largest possible size
            ChangeSize(0.75);

            // Set console colors
            Console.ForegroundColor = DefaultFgColor;
            Console.BackgroundColor = DefaultBgColor;
        }

        /// <summary>
        /// Change the size of the console.
        /// </summary>
        /// <param name="percent">Percentage of the largest possible size</param>
        public static void ChangeSize(double percent)
        {
            int width = (int)(Console.LargestWindowWidth * percent);
            int height = (int)(Console.LargestWindowHeight * percent);
            ChangeSize(width, height);
        }


        /// <summary>
        /// Change the size of the console.
        /// </summary>
        /// <param name="width">Number of columns</param>
        /// <param name="height">Number of lines</param>
        public static void ChangeSize(int width, int height)
        {
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
        }

        /// <summary>
        /// Write a colored string in the console.
        /// </summary>
        /// <param name="str"></param>
        public static void Write(ColoredString str)
        {
            Console.ForegroundColor = str.ForegroundColor;
            Console.BackgroundColor = str.BackgroundColor;
            Console.Write(str.String);
        }

        /// <summary>
        /// Write a colored string in the console at the specified position.
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="x">Number of column from the left</param>
        /// <param name="y">Number of lines from the top</param>
        /// <param name="centered">Writes the text centered around the position</param>
        public static void Write(ColoredString str, int x, int y, bool centered = false)
        {
            if (centered)
            {
                x = x - (str.String.Length / 2);
                y = y - (str.String.Split(Environment.NewLine).Length / 2);
            }
            Console.SetCursorPosition(x, y);
            Write(str);
        }

        /// <summary>
        /// Write a string in the console at the specified position.
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="x">Number of column from the left</param>
        /// <param name="y">Number of lines from the top</param>
        /// <param name="centered">Writes the text centered around the position</param>
        public static void Write(string str, int x, int y, bool centered = false)
        {
            if (centered)
            {
                x = x - (str.Length / 2);
                y = y - (str.Split(Environment.NewLine).Length / 2);
            }
            Console.SetCursorPosition(x, y);
            _stringAntiGarbageCollector.String = str;
            Write(_stringAntiGarbageCollector);
        }

        /// <summary>
        /// Write a string in the console at the specified position.
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="x">Number of column from the left</param>
        /// <param name="y">Number of lines from the top</param>
        /// <param name="fgcolor">Foreground color</param>
        /// <param name="bgcolor">Background color</param>
        /// <param name="centered">Writes the text centered around the position</param>
        public static void Write(string str, int x, int y, ConsoleColor fgcolor, ConsoleColor bgcolor, bool centered = false)
        {
            if (centered)
            {
                x = x - (str.Length / 2);
                y = y - (str.Split(Environment.NewLine).Length / 2);
            }
            Console.SetCursorPosition(x, y);
            _stringAntiGarbageCollector.String = str;
            _stringAntiGarbageCollector.ForegroundColor = fgcolor;
            _stringAntiGarbageCollector.BackgroundColor = bgcolor;
            Write(_stringAntiGarbageCollector);
        }

        /// <summary>
        /// Write a colored character in the console.
        /// </summary>
        /// <param name="c"></param>
        public static void Write(ColoredChar c)
        {
            Console.ForegroundColor = c.ForegroundColor;
            Console.BackgroundColor = c.BackgroundColor;
            Console.Write(c.Char);
        }
        
        /// <summary>
        /// Write a colored character in the console at the specified position.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Write(ColoredChar c, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Write(c);
        }

        /// <summary>
        /// Write a character in the console at the specified position.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Write(char c, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            _charAntiGarbageCollector.Char = c;
            Write(_charAntiGarbageCollector);
        }

        /// <summary>
        /// Write a character in the console at the specified position.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fgcolor"></param>
        /// <param name="bgcolor"></param>
        public static void Write(char c, int x, int y, ConsoleColor fgcolor, ConsoleColor bgcolor)
        {
            Console.SetCursorPosition(x, y);
            _charAntiGarbageCollector.Char = c;
            _charAntiGarbageCollector.ForegroundColor = fgcolor;
            _charAntiGarbageCollector.BackgroundColor = bgcolor;
            Write(_charAntiGarbageCollector);
        }

        /// <summary>
        /// Erase a line of text.
        /// </summary>
        /// <param name="y">The line</param>
        public static void EraseLine(int y)
        {
            EraseLine(y, DefaultBgColor);
        }

        /// <summary>
        /// Erase a line of text.
        /// </summary>
        /// <param name="y">The line</param>
        /// <param name="bcolor">Background color</param>
        public static void EraseLine(int y, ConsoleColor bcolor)
        {
            Console.BackgroundColor = bcolor;
            
            Console.SetCursorPosition(0, y);
            Console.Write(new string(' ', Console.WindowWidth));
        }

        /// <summary>
        /// Catch all inputs waiting in the buffer.
        /// </summary>
        /// <returns>List of inputs caught</returns>
        public static void CatchInputs()
        {
            Inputs.Clear();
            while (Console.KeyAvailable)
            {
                Inputs.Add(Console.ReadKey(true));
            }
        }

        // Imports
        
        [DllImport("user32.dll")]
        private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
    }
}
