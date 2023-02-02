namespace BlueShadowMon
{
    public class Map
    {
        private char[,] _map { get; set; } = new char[0, 0];
        public char this[int y, int x] { get { return _map[y, x]; } }
        
        public Player Player { get; }
        public NPC[] NPCs { get; }
        public int Width { get { return _map.GetLength(1); } }
        public int Height { get { return _map.GetLength(0); } }

        public string Name { get; private set; }

        public static float ChanceTriggerCombat = 0.05F;

        public Map(string path, Player player, NPC[] npcs)
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

            foreach (NPC p in npcs)
            {
                if (p.x < 0 || Width <= p.x || p.y < 0 || Height <= p.y)
                    throw new Exception("NPC position is not valid!");
            }
            NPCs = npcs;
        }

        /// <summary>
        /// Check if the given char is walkable.
        /// </summary>
        /// <param name="c"></param>
        /// <returns>True if the char is walkable, false otherwise</returns>
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

            foreach (NPC p in NPCs)
            {
                if (newX == p.x && newY == p.y)
                {
                    p.RunDialogue();
                    return;
                }
            }

            if (IsCharWalkable(_map[newY, newX])) // Can't move on a non-walkable char
            {
                Player.Move(newX, newY);

                if (_map[Player.y, Player.x] == '*' || _map[Player.y, Player.x] == '&')
                    WalkInBush();
            }
        }

        /// <summary>
        /// Called when the player walks in a bush.
        /// Has a chance to trigger a combat.
        /// </summary>
        public void WalkInBush()
        {
            float rand = (float)new Random().NextDouble();
            if (rand <= ChanceTriggerCombat)
            {
                List<Pet> enemies = new List<Pet>();

                int EnemyTeamCount = Player.Pets.Count(p => p != null);
                for (int i = 0; i < EnemyTeamCount; i++)
                {
                    int randEnemyType = new Random().Next(0, 3);
                    switch (randEnemyType)
                    {
                        case 0:
                            enemies.Add(new Pet(Name, PetType.Cat, Data.StarterStats, Data.StarterIncrements));
                            break;
                        case 1:
                            enemies.Add(new Pet(Name, PetType.Dog, Data.StarterStats, Data.StarterIncrements));
                            break;
                        case 2:
                            enemies.Add(new Pet(Name, PetType.Snake, Data.StarterStats, Data.StarterIncrements));
                            break;
                    }
                }

                Game.SwitchToCombatScene(enemies);
            }

        }

        /// <summary>
        /// Called when the player presses a key.
        /// </summary>
        /// <param name="key"></param>
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
