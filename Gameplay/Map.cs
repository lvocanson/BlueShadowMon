namespace BlueShadowMon
{
    public class Map
    {
        private char[,] _map { get; set; } = new char[0, 0];
        public char this[int y, int x] { get { return _map[y, x]; } }

        public Player Player { get; }
        public int Width { get { return _map.GetLength(1); } }
        public int Height { get { return _map.GetLength(0); } }
        public static float ChanceTriggerCombat = 0.05F;
        
        public Map(string path, Player player)
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

            if (player.x < 0 || Width <= player.x || player.y < 0 || Height <= player.y)
                throw new Exception("Player position is not valid!");
            Player = player;
        }

            private static bool IsCharWalkable(char c)
        {
            switch (c)
            {
                case ' ': // Ground
                case ':': // Sand
                case '*': // Grass on ground
                case '&': // Grass on sand
                case 'p': // Bridge
                    return true;
                case '#': // Wall
                case 'o': // Water
                default:  // Unknown
                    return false;
            }
        }

        /// <summary>
        /// Try to move the player by the given coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void TryToMoveBy(int x, int y)
        {
            int newX = Player.x + x;
            int newY = Player.y + y;
            if (newX < 0 || Width <= newX || newY < 0 || Height <= newY)
                return; // Can't move out of bounds
            if (IsCharWalkable(_map[newY, newX])) // Can't move on a non-walkable char
            {
                Player.Move(newX,newY);
                if (_map[Player.y, Player.x] == '*' || _map[Player.y, Player.x] == '&')
                    WalkInBush();
            }
        }
        
        public void WalkInBush()
        {
            float rand = (float)new Random().NextDouble();
            if (rand <= ChanceTriggerCombat)
                Game.SwitchToCombatScene();

        }

        internal void KeyPressed(ConsoleKey key)
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
                case ConsoleKey.I:
                    Game.ToggleInventory();
                    break;
                case ConsoleKey.Escape:
                    Game.SwitchToMenuScene("Main Menu");
                    break;
            }
        }
    }
}
