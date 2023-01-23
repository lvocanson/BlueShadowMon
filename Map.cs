using System.Runtime.Versioning;

namespace BlueShadowMon
{
    internal class Map
    {
        // Colors
        private const ConsoleColor GroundColor = ConsoleColor.DarkGray;
        private const ConsoleColor WallColor = ConsoleColor.DarkRed;
        private const ConsoleColor WaterColor = ConsoleColor.DarkBlue;
        private const ConsoleColor TallGrassColor = ConsoleColor.DarkGreen;

        // Map characters
        private const char GroundChar = ' ';
        private const char WallChar = '#';
        private const char WaterChar = 'o';
        private const char TallGrassOnGroundChar = '*';

        /// <summary>
        /// Translates a map character into a tuple with a new char and colors.
        /// </summary>
        /// <param name="c"></param>
        /// <returns>Tuple<char c, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor></returns>
        public static Tuple<char, ConsoleColor, ConsoleColor> Translate(char c)
        {
            switch (c)
            {
                case GroundChar:
                    return new Tuple<char, ConsoleColor, ConsoleColor>(GroundChar, GroundColor, GroundColor);
                case WallChar:
                    return new Tuple<char, ConsoleColor, ConsoleColor>(WallChar, WallColor, WallColor);
                case WaterChar:
                    return new Tuple<char, ConsoleColor, ConsoleColor>(WaterChar, WaterColor, WaterColor);
                case TallGrassOnGroundChar:
                    return new Tuple<char, ConsoleColor, ConsoleColor>(TallGrassOnGroundChar, TallGrassColor, GroundColor);
                default:
                    return new Tuple<char, ConsoleColor, ConsoleColor>(c, ConsoleColor.White, ConsoleColor.Black);
            }
        }

        public Map(string path)
        {
            MapString = File.ReadAllLines(path);
        }
        private string[] MapString { get; set; }
        public int Width { get { return MapString[0].Length; } }
        public int Height { get { return MapString.Length; } }

        /// <summary>
        /// Draws the map on the console, centered around the player.
        /// </summary>
        /// <param name="x">X position of the player</param>
        /// <param name="y">Y position of the player</param>
        [SupportedOSPlatform("windows")]
        public void DrawMap(int x, int y)
        {
            int mapX, mapY;
            for (int j = 0; j < Console.WindowHeight; j++)
            {
                mapY = j - y;
                if (mapY < 0 || Height <= mapY) // If the map is out of bounds
                {
                    ConsoleManager.EraseLine(j, WallColor);
                    continue;
                }
                for (int i = 0; i < Console.WindowWidth; i++)
                {
                    mapX = i - x - ConsoleManager.MiddleX;
                    if (mapX < 0 || Width <= mapX) // Out of bounds
                    {
                        ConsoleManager.WriteText(" ", i, j, WallColor, WallColor);
                        continue;
                    }

                    // Write the transleted char
                    var t = Translate(MapString[mapY][mapX]);
                    ConsoleManager.WriteText(t.Item1.ToString(), i, j, t.Item2, t.Item3);
                }
            }
        }
    }
}
