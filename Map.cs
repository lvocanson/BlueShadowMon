using System.Runtime.Versioning;

namespace BlueShadowMon
{
    [SupportedOSPlatform("windows")]

    internal class Map
    {
        private struct MapChar
        {
            public char Char;
            public ConsoleColor ForegroundColor;
            public ConsoleColor BackgroundColor;
        }

        private struct MapString
        {
            public string String;
            public ConsoleColor ForegroundColor;
            public ConsoleColor BackgroundColor;
        }

        private static MapChar _cGround { get; } = new MapChar { Char = ' ', ForegroundColor = ConsoleColor.DarkGray, BackgroundColor = ConsoleColor.DarkGray };
        private static MapChar _cWall { get; } = new MapChar { Char = ' ', ForegroundColor = ConsoleColor.DarkRed, BackgroundColor = ConsoleColor.DarkRed };
        private static MapChar _cWater { get; } = new MapChar { Char = '~', ForegroundColor = ConsoleColor.DarkBlue, BackgroundColor = ConsoleColor.Blue };
        private static MapChar _cGrassOnGround { get; } = new MapChar { Char = '*', ForegroundColor = ConsoleColor.Green, BackgroundColor = ConsoleColor.DarkGray };
        private static MapChar _cUnknown { get; } = new MapChar { Char = '?', ForegroundColor = ConsoleColor.White, BackgroundColor = ConsoleColor.Black };

        private MapChar _cPlayer
        {
            get { return new MapChar { Char = PlayerChar, ForegroundColor = PlayerColor, BackgroundColor = Parse(_map[_playerPos.y, _playerPos.x]).BackgroundColor }; }
        }


        private char[,] _map { get; set; }
        private (int x, int y) _playerPos;

        public static char PlayerChar { get; set; } = '@';
        public static ConsoleColor PlayerColor { get; set; } = ConsoleColor.White;
        public int Width { get { return _map.GetLength(1); } }
        public int Height { get { return _map.GetLength(0); } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="Exception"></exception>
        public Map(string path, (int x, int y) playerPos)
        {
            // Load file
            string[] lines = File.ReadAllLines(path);

            int width = lines[0].Length;
            _map = new char[lines.Length, width];

            for (int y = 0; y < lines.Length; y++)
            {
                // Check if all lines are the same width
                if (lines[y].Length != width) throw new Exception("Loaded map is not rectangular!");

                // Create the map
                for (int x = 0; x < width; x++)
                {
                    _map[y, x] = lines[y][x];
                }
            }


            if (playerPos.x < 0 || Width <= playerPos.x || playerPos.y < 0 || Height <= playerPos.y)
                throw new Exception("Player position is not valid!");
            _playerPos = playerPos;
        }

        /// <summary>
        /// Get the MapChar corresponding to the char.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static MapChar Parse(char c)
        {
            switch (c)
            {
                case ' ':
                    return _cGround;
                case '#':
                    return _cWall;
                case 'o':
                    return _cWater;
                case '*':
                    return _cGrassOnGround;
                default:
                    return _cUnknown;
            }
        }

        private static bool IsCharWalkable(char c)
        {
            switch (c)
            {
                case ' ': // Ground
                case '*': // Grass on ground
                    return true;
                case '#': // Wall
                case 'o': // Water
                default:  // Unknown
                    return false;
            }
        }

        /// <summary>
        /// Handle a key press. 
        /// </summary>
        /// <param name="key">The key pressed</param>
        public void KeyPressed(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    TryToMoveBy(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    TryToMoveBy(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    TryToMoveBy(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    TryToMoveBy(1, 0);
                    break;
                case ConsoleKey.Escape:
                    Game.CurrState = Game.State.MainMenu;
                    break;
            }
        }

        private void TryToMoveBy(int x, int y)
        {
            int newX = _playerPos.x + x;
            int newY = _playerPos.y + y;
            if (newX < 0 || Width <= newX || newY < 0 || Height <= newY)
                return; // Can't move out of bounds
            if (IsCharWalkable(_map[newY, newX])) // Can't move on a non-walkable char
                _playerPos = (newX, newY);
        }

        /// <summary>
        /// Draws the map on the console around the player coordinates.
        /// The player is centered on the console.
        /// </summary>
        public void DrawMap()
        {
            (int left, int top, int right, int down) mapView = (
                _playerPos.x - ConsoleManager.MiddleX,
                _playerPos.y - ConsoleManager.MiddleY,
                _playerPos.x + (Console.WindowWidth - ConsoleManager.MiddleX),
                _playerPos.y + (Console.WindowHeight - ConsoleManager.MiddleY)
            );

            MapChar parsed;
            if (mapView.top < 0 || mapView.left < 0)
                parsed = _cWall;
            else
                parsed = Parse(_map[mapView.top, mapView.left]);

            List<MapString> toDraw = new List<MapString>();
            int count = 0;

            for (int y = mapView.top; y < mapView.down; y++)
            {
                if (y < 0 || Height <= y) // Out of bounds
                {
                    if (!parsed.Equals(_cWall)) // Last char was not a wall
                    {
                        toDraw.Add(new MapString { String = new string(parsed.Char, count), ForegroundColor = parsed.ForegroundColor, BackgroundColor = parsed.BackgroundColor });
                        parsed = _cWall;
                        count = 0;
                    }
                    count += Console.WindowWidth;
                    continue;
                }

                for (int x = mapView.left; x < mapView.right; x++)
                {
                    if (x == _playerPos.x && y == _playerPos.y)
                    {
                        toDraw.Add(new MapString { String = new string(parsed.Char, count), ForegroundColor = parsed.ForegroundColor, BackgroundColor = parsed.BackgroundColor });
                        parsed = _cPlayer;
                        count = 1;
                        continue;
                    }

                    // If we are out of bounds and we are building a wall string,
                    // or if we are in bounds and we are building the same string
                    if (((x < 0 || Width <= x) && parsed.Equals(_cWall)) || (0 <= x && x < Width && parsed.Equals(Parse(_map[y, x]))))
                    {
                        count++;
                        continue;
                    }

                    // If parsed need to change, we also need to build a new string
                    toDraw.Add(new MapString { String = new string(parsed.Char, count), ForegroundColor = parsed.ForegroundColor, BackgroundColor = parsed.BackgroundColor });
                    if (x < 0 || Width <= x)
                        parsed = _cWall;
                    else
                        parsed = Parse(_map[y, x]);
                    count = 1;
                }
            }

            // Build the last string
            toDraw.Add(new MapString { String = new string(parsed.Char, count), ForegroundColor = parsed.ForegroundColor, BackgroundColor = parsed.BackgroundColor });

            // Draw all srings built
            Console.SetCursorPosition(0, 0);
            foreach (MapString ms in toDraw)
                ConsoleManager.WriteText(ms.String, ms.ForegroundColor, ms.BackgroundColor);
        }
    }
}
