namespace BlueShadowMon
{
    public class MapScene : Scene
    {
        public Map Map { get; private set; }
        private static Window.ColoredChar _cGround { get; } = new Window.ColoredChar(' ', ConsoleColor.DarkGray, ConsoleColor.DarkGray);
        private static Window.ColoredChar _cWall { get; } = new Window.ColoredChar(' ', ConsoleColor.DarkRed, ConsoleColor.DarkRed);
        private static Window.ColoredChar _cSand { get; } = new Window.ColoredChar(' ', ConsoleColor.Yellow, ConsoleColor.Yellow);
        private static Window.ColoredChar _cWater { get; } = new Window.ColoredChar('~', ConsoleColor.DarkBlue, ConsoleColor.Blue);
        private static Window.ColoredChar _cGrassOnGround { get; } = new Window.ColoredChar('*', ConsoleColor.Green, ConsoleColor.DarkGray);
        private static Window.ColoredChar _cGrassOnSand { get; } = new Window.ColoredChar('*', ConsoleColor.DarkYellow, ConsoleColor.Yellow);
        private static Window.ColoredChar _cBridge { get; } = new Window.ColoredChar(' ', ConsoleColor.DarkYellow, ConsoleColor.DarkYellow);
        private static Window.ColoredChar _cUnknown { get; } = new Window.ColoredChar('?', ConsoleColor.White, ConsoleColor.Black);

        private Window.ColoredChar _cPlayer
        {
            get { return new Window.ColoredChar(PlayerChar, PlayerColor, Parse(Map[Map.Player.y, Map.Player.x]).BackgroundColor); }
        }

        private Window.ColoredChar _cPnj
        {
            get { return new Window.ColoredChar(PnjChar, PnjColor, Parse(Map[Map.Pnj.y, Map.Pnj.x]).BackgroundColor); }
        }

        public static char PlayerChar { get; set; } = '@';
        public static ConsoleColor PlayerColor { get; set; } = ConsoleColor.White;
        public static char PnjChar { get; set; } = 'P';
        public static ConsoleColor PnjColor { get; set; } = ConsoleColor.Black;

        public MapScene(Map map)
        {
            Map = map;
        }

        public void Init(Map map)
        {
            Map = map;
        }

        /// <summary>
        /// Get the MapChar corresponding to the char.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static Window.ColoredChar Parse(char c)
        {
            switch (c)
            {
                case ' ':
                    return _cGround;
                case ':':
                    return _cSand;
                case '#':
                    return _cWall;
                case 'o':
                    return _cWater;
                case '*':
                    return _cGrassOnGround;
                case '&':
                    return _cGrassOnSand;
                case 'p':
                    return _cBridge;
                default:
                    return _cUnknown;
            }
        }

        /// <summary>
        /// Draws the map on the console around the player coordinates.
        /// The player is centered on the console.
        /// </summary>
        public override void Draw()
        {
            (int left, int top, int right, int down) mapView = (
                Map.Player.x - Window.MiddleX,
                Map.Player.y - Window.MiddleY,
                Map.Player.x + (Console.WindowWidth - Window.MiddleX),
                Map.Player.y + (Console.WindowHeight - Window.MiddleY)
            );

            Window.ColoredChar parsed;
            if (mapView.top < 0 || mapView.left < 0)
                parsed = _cWall;
            else
                parsed = Parse(Map[mapView.top, mapView.left]);

            List<Window.ColoredString> toDraw = new List<Window.ColoredString>();
            int count = 0;

            for (int y = mapView.top; y < mapView.down; y++)
            {
                if (y < 0 || Map.Height <= y) // Out of bounds
                {
                    if (!parsed.Equals(_cWall)) // Last char was not a wall
                    {
                        toDraw.Add(new Window.ColoredString(new string(parsed.Char, count), parsed.ForegroundColor, parsed.BackgroundColor));
                        parsed = _cWall;
                        count = 0;
                    }
                    count += Console.WindowWidth;
                    continue;
                }

                for (int x = mapView.left; x < mapView.right; x++)
                {
                    if (x == Map.Player.x && y == Map.Player.y)
                    {
                        toDraw.Add(new Window.ColoredString(new string(parsed.Char, count), parsed.ForegroundColor, parsed.BackgroundColor));
                        parsed = _cPlayer;
                        if (Map[y, x] == ':' || Map[y, x] == '&')
                            parsed.ForegroundColor = ConsoleColor.Black;
                        count = 1;
                        continue;
                    }

                    if (x == Map.Pnj.x && y == Map.Pnj.y)
                    {
                        toDraw.Add(new Window.ColoredString(new string(parsed.Char, count), parsed.ForegroundColor, parsed.BackgroundColor));
                        parsed = _cPnj;
                        count = 1;
                        continue;
                    }

                    // If we are out of bounds and we are building a wall string,
                    // or if we are in bounds and we are building the same string
                    if (((x < 0 || Map.Width <= x) && parsed.Equals(_cWall)) || (0 <= x && x < Map.Width && parsed.Equals(Parse(Map[y, x]))))
                    {
                        count++;
                        continue;
                    }

                    // If parsed need to change, we also need to build a new string
                    toDraw.Add(new Window.ColoredString(new string(parsed.Char, count), parsed.ForegroundColor, parsed.BackgroundColor));
                    if (x < 0 || Map.Width <= x)
                        parsed = _cWall;
                    else
                        parsed = Parse(Map[y, x]);
                    count = 1;
                }
            }

            // Build the last string
            toDraw.Add(new Window.ColoredString(new string(parsed.Char, count), parsed.ForegroundColor, parsed.BackgroundColor));

            // Draw all srings built
            Console.SetCursorPosition(0, 0);
            toDraw.ForEach(s => Window.Write(s));
        }

        /// <summary>
        /// Handle a key press. 
        /// </summary>
        /// <param name="key">The key pressed</param>
        public override void KeyPressed(ConsoleKey key)
        {
            Map.KeyPressed(key);
        }
    }
}
