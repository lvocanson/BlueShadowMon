using System.Runtime.Versioning;

namespace BlueShadowMon
{
    internal class Map
    {
        enum TileColor
        {
            Ground = ConsoleColor.DarkGray,
            Wall = ConsoleColor.DarkRed,
            Water = ConsoleColor.DarkBlue,
            Grass = ConsoleColor.DarkGreen,
            Sand = ConsoleColor.Yellow,
            Vegetation = ConsoleColor.Green,
        }

        /// <summary>
        /// Translates a map character into a tuple with a new char and colors.
        /// </summary>
        /// <param name="c"></param>
        /// <returns>Tuple<char c, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor></returns>
        public static Tuple<char, ConsoleColor, ConsoleColor> Translate(char c)
        {
            switch (c)
            {
                case ' ': // Ground
                    return new Tuple<char, ConsoleColor, ConsoleColor>(' ', (ConsoleColor)TileColor.Ground, (ConsoleColor)TileColor.Ground);
                case '%': // Bush on ground
                    return new Tuple<char, ConsoleColor, ConsoleColor>('%', (ConsoleColor)TileColor.Vegetation, (ConsoleColor)TileColor.Ground);
                case '.': // Water
                    return new Tuple<char, ConsoleColor, ConsoleColor>('.', (ConsoleColor)TileColor.Water, (ConsoleColor)TileColor.Water);
                default: // wall
                    return new Tuple<char, ConsoleColor, ConsoleColor>('W', (ConsoleColor)TileColor.Wall, (ConsoleColor)TileColor.Wall);
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
                    ConsoleManager.EraseLine(j, (ConsoleColor)TileColor.Wall);
                    continue;
                }
                for (int i = 0; i < Console.WindowWidth; i++)
                {
                    mapX = i - x - ConsoleManager.MiddleX;
                    if (mapX < 0 || Width <= mapX) // Out of bounds
                    {
                        ConsoleManager.WriteText(" ", i, j, (ConsoleColor)TileColor.Wall, (ConsoleColor)TileColor.Wall);
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
