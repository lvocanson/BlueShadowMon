using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]
    internal static class ConsoleManager
    {
        [DllImport("user32.dll")]
        private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        public static ConsoleColor DefaultFgColor { get; set; } = ConsoleColor.White;
        public static ConsoleColor DefaultBgColor { get; set; } = ConsoleColor.Black;
        public static int MiddleX { get { return Console.WindowWidth / 2; }}
        public static int MiddleY { get { return Console.WindowHeight / 2; }}
        public static List<ConsoleKeyInfo> Inputs { get; private set; } = new List<ConsoleKeyInfo>();

        /// <summary>
        /// Disable resizing, minimize and maximize buttons.
        /// Change console properties.
        /// </summary>
        public static void WindowSetup()
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

            // Set console size to 80% of the largest possible size
            int width = (int)(Console.LargestWindowWidth * 0.8);
            int height = (int)(Console.LargestWindowHeight * 0.8);
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            // Set console colors
            Console.ForegroundColor = DefaultFgColor;
            Console.BackgroundColor = DefaultBgColor;
        }

        /// <summary>
        /// Set a text in the string builder at the specified position.
        /// </summary>
        /// <param name="text">The text</param>
        /// <param name="x">Number of column from the left</param>
        /// <param name="y">Number of lines from the top</param>
        /// <param name="fcolor">Color of the text</param>
        /// <param name="bcolor">Color of the background</param>
        /// <param name="centered">Writes the text centered around the position</param>
        public static void WriteText(string text, int x, int y, ConsoleColor fcolor, ConsoleColor bcolor, bool centered = false)
        {
            Console.ForegroundColor = fcolor;
            Console.BackgroundColor = bcolor;
            if (centered)
            {
                x = x - (text.Length / 2);
                y = y - (text.Split(Environment.NewLine).Length / 2);
            }
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        /// <summary>
        /// Set a character in the string builder at the specified position.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fcolor"></param>
        /// <param name="bcolor"></param>
        public static void WriteChar(char c, int x, int y, ConsoleColor fcolor, ConsoleColor bcolor)
        {
            Console.ForegroundColor = fcolor;
            Console.BackgroundColor = bcolor;
            Console.SetCursorPosition(x, y);
            Console.Write(c);
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
    }
}
