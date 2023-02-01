using System.Runtime.InteropServices;

namespace BlueShadowMon
{
    /// <summary>
    /// Console Manager.
    /// </summary>
    public static class Window
    {
        public static ConsoleColor DefaultFgColor { get; set; } = ConsoleColor.White;
        public static ConsoleColor DefaultBgColor { get; set; } = ConsoleColor.Black;
        public static int MiddleX { get { return Console.WindowWidth / 2; } }
        public static int MiddleY { get { return Console.WindowHeight / 2; } }
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

        private static ColoredChar _charAntiGarbageCollector = new ColoredChar(' ');
        private static ColoredString _stringAntiGarbageCollector = new ColoredString("");

        /// <summary>
        /// Disable resizing, minimize and maximize buttons.
        /// Change console properties.
        /// </summary>
        public static void Setup()
        {
            // Disable resizing and minimize/maximize buttons
            IntPtr window = GetConsoleWindow();

            int WS_MAXIMIZEBOX = 0x00010000;
            int WS_THICKFRAME = 0x00040000;
            int GWL_STYLE = -16;
            int style = GetWindowLongA(window, GWL_STYLE);
            SetWindowLongA(window, GWL_STYLE, style & ~(WS_MAXIMIZEBOX | WS_THICKFRAME));

            // Set console properties
            Console.Title = Game.GameTitle;
            Console.TreatControlCAsInput = true;
            Console.CursorVisible = false;

            // Set console size to 70% of the largest possible size
            Resize(0.7F);

            // Set console colors
            Console.ForegroundColor = DefaultFgColor;
            Console.BackgroundColor = DefaultBgColor;
        }

        /// <summary>
        /// Change the size of the console.
        /// </summary>
        /// <param name="percent">Percentage of the largest possible size</param>
        public static void Resize(float percent)
        {
            int width = (int)(Console.LargestWindowWidth * percent);
            int height = (int)(Console.LargestWindowHeight * percent);
            Resize(width, height);
            // Do it again to fix a console bug
            Resize(width, height);
        }


        /// <summary>
        /// Change the size of the console.
        /// </summary>
        /// <param name="width">Number of columns</param>
        /// <param name="height">Number of lines</param>
        public static void Resize(int width, int height)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Set console size
                Console.SetWindowSize(width, height);
                Console.SetBufferSize(width, height);
            }
            // TODO: Change console size on other OS
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
            _stringAntiGarbageCollector.ForegroundColor = DefaultFgColor;
            _stringAntiGarbageCollector.BackgroundColor = DefaultBgColor;
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

        private static (string topLeft, string topRight, string bottomLeft, string BottomRight) boxCorners = new("┌", "┐", "└", "┘");
        private static (char topBottom, string LeftRight) boxSides = new('─', "│");

        /// <summary>
        /// Show a string in a box, centered on the screen.
        /// </summary>
        /// <param name="input"></param>
        public static void Message(string input)
        {
            int width = (int)(Console.WindowWidth * 0.66); // Message width
            string[] words = input.Split(' ');
            List<string> lines = new List<string>();
            string line = "";
            while (words.Length > 0)
            {
                if (line.Length + words[0].Length > width)
                {
                    lines.Add(line);
                    line = "";
                }
                if (words[0].Contains(Environment.NewLine))
                {
                    string[] split = words[0].Split(Environment.NewLine);
                    if (line.Length + split[0].Length > width)
                    {
                        lines.Add(line);
                        line = "";
                    }
                    line += split[0];
                    lines.Add(line);
                    line = "";
                    words[0] = string.Join(" ", split.Skip(1));
                }
                else if (words[0].Length >= width)
                {
                    lines.Add(words[0].Substring(0, width));
                    words[0] = words[0].Substring(width);
                }
                else
                {
                    line += words[0] + " ";
                    words = words.Skip(1).ToArray();   
                }
            }
            if (line.Length > 0)
                lines.Add(line);

            int height = lines.Count + 2;
            int topY = MiddleY - (height / 2);

            // Box
            Write(boxCorners.topLeft + new string(boxSides.topBottom, width) + boxCorners.topRight, MiddleX, topY - 1, true);
            for (int i = 0; i < height; i++)
            {
                Write(boxSides.LeftRight + new string(' ', width) + boxSides.LeftRight, MiddleX, topY + i, true);
            }
            Write("Press any key to continue...", MiddleX, topY + height - 1, ConsoleColor.DarkGray, DefaultBgColor, true);
            Write(boxCorners.bottomLeft + new string(boxSides.topBottom, width) + boxCorners.BottomRight, MiddleX, topY + height, true);

            // Message
            for (int i = 0; i < lines.Count; i++)
            {
                Write(lines[i], MiddleX, topY + i, true);
            }

            // Wait for a key press
            Console.ReadKey(true);
            Console.Clear();
        }

        /// <summary>
        /// Close the console.
        /// </summary>
        public static void Quit()
        {
            Console.BackgroundColor = Window.DefaultBgColor;
            Console.Clear();
            Environment.Exit(0);
        }

        // Imports

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int GetWindowLongA(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int SetWindowLongA(IntPtr hwnd, int nIndex, int value);
    }
}
