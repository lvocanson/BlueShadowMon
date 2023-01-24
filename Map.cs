using System;
using System.Runtime.Versioning;

namespace BlueShadowMon
{
    internal class Map
    {
        // Colors
        private const ConsoleColor PlayerColor = ConsoleColor.White;
        private const ConsoleColor GroundColor = ConsoleColor.DarkGray;
        private const ConsoleColor WallColor = ConsoleColor.DarkRed;
        private const ConsoleColor WaterColor = ConsoleColor.DarkBlue;
        private const ConsoleColor TallGrassColor = ConsoleColor.DarkGreen;

        // Map characters
        private const char GroundChar = ' ';
        private const char WallChar = '#';
        private const char WaterChar = 'o';
        private const char TallGrassOnGroundChar = '*';

        public struct MapChar
        {
            public char Char;
            public ConsoleColor ForegroundColor;
            public ConsoleColor BackgroundColor;
        }

        private MapChar[,] Map_ { get; set; }
        public MapChar this[int x, int y]
        {
            get { return Map_[y, x]; }
        }
        public int Width { get { return Map_.GetLength(1); } }
        public int Height { get { return Map_.GetLength(0); } }

        /// <summary>
        /// Translates a map character into a tuple with a new char and colors.
        /// </summary>
        /// <param name="c"></param>
        /// <returns>Tuple<char c, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor></returns>
        public static MapChar Parse(char c)
        {
            switch (c)
            {
                case GroundChar:
                    return new MapChar { Char = GroundChar, ForegroundColor = GroundColor, BackgroundColor = GroundColor };
                case WallChar:
                    return new MapChar { Char = WallChar, ForegroundColor = WallColor, BackgroundColor = WallColor };
                case WaterChar:
                    return new MapChar { Char = WaterChar, ForegroundColor = WaterColor, BackgroundColor = WaterColor };
                case TallGrassOnGroundChar:
                    return new MapChar { Char = TallGrassOnGroundChar, ForegroundColor = TallGrassColor, BackgroundColor = GroundColor };
                default: // Unknown
                    return new MapChar { Char = c, ForegroundColor = ConsoleColor.White, BackgroundColor = ConsoleColor.Black };
            }
        }

        public Map(string path)
        {
            // Load file
            string[] lines = File.ReadAllLines(path);

            int width = lines[0].Length;
            Map_ = new MapChar[lines.Length, width];

            for (int y = 0; y < lines.Length; y++)
            {
                // Check if all lines are the same width
                if (lines[y].Length != width) throw new Exception("Loaded map is not rectangular!");

                // Create the map
                for (int x = 0; x < width; x++)
                {
                    Map_[y, x] = Parse(lines[y][x]);
                }
            }
        }

        /// <summary>
        /// Draws the map on the console around the player coordinates.
        /// The player is centered on the console.
        /// </summary>
        /// <param name="playerX">X position of the player</param>
        /// <param name="playerY">Y position of the player</param>
        [SupportedOSPlatform("windows")]
        public void Draw(int playerX, int playerY)
        {
            int consoleX, consoleY, mapX, mapY;
            MapChar c;

            // For each line of the console
            for (consoleY = 0; consoleY < Console.WindowHeight; consoleY++)
            {
                mapY = playerY - ConsoleManager.MiddleY + consoleY;
                if (mapY < 0 || Height <= mapY) // Out of bounds
                {
                    ConsoleManager.EraseLine(consoleY, WallColor);
                    continue;
                }

                // For each char of the line
                for (consoleX = 0; consoleX < Console.WindowWidth; consoleX++)
                {
                    mapX = playerX - ConsoleManager.MiddleX + consoleX;
                    if (mapX < 0 || Width <= mapX) // Out of bounds
                    {
                        ConsoleManager.WriteChar(' ', consoleX, consoleY, WallColor, WallColor);
                        continue;
                    }

                    c = Map_[mapY, mapX];
                    if (mapX == playerX && mapY == playerY)
                        ConsoleManager.WriteChar('@', consoleX, consoleY, PlayerColor, c.BackgroundColor);
                    else
                        ConsoleManager.WriteChar(c.Char, consoleX, consoleY, c.ForegroundColor, c.BackgroundColor);
                }
            }
        }
    }
}
