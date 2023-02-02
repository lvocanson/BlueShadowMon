namespace BlueShadowMon
{
    internal class CombatScene : Scene
    {
        public Combat Combat { get; private set; }

        public CombatScene(Combat combat)
        {
            Combat = combat;
        }

        public void Init(Combat combat)
        {
            Combat = combat;
        }

        public override void Draw()
        {
            // Stats
            int width = Console.WindowWidth / 3;
            int screenOffset = width / 4;
            int leftX = screenOffset + width / 2;
            int rightX = Console.WindowWidth - screenOffset - width / 2;
            Window.ColoredString cString = new("Colored String");
            Window.ColoredChar cChar = new('│', ConsoleColor.White, ConsoleColor.DarkGray);

            int y = 3;
            Window.Write("Your team", leftX, y - 2, true);
            for (int i = 0; i < Combat.Allies.Count; i++)
            {
                Pet pet = Combat.Allies[i];
                Window.Write(pet.Name, leftX, y, true);
                
                // Health bar
                y++;
                Window.Write(cChar, screenOffset, y);
                cString.String = new string(' ', (int)((pet[PetStat.Health] * (width - 2) + 1) / pet.BaseStats[PetStat.Health]));
                if (cString.String.Length <= width * 0.2)
                    cString.BackgroundColor = ConsoleColor.DarkRed;
                else if (cString.String.Length <= width * 0.5)
                    cString.BackgroundColor = ConsoleColor.DarkYellow;
                else
                    cString.BackgroundColor = ConsoleColor.Green;
                Window.Write(cString, screenOffset + 1, y);
                Window.Write(cChar, screenOffset + width - 1, y);

                // Power and armor
                y++;
                cString.BackgroundColor = ConsoleColor.Black;
                cString.String = $"Power: {Math.Round(pet[PetStat.Power], 2)}";
                Window.Write(cString, screenOffset, y);
                cString.String = $"Armor: {Math.Round(pet[PetStat.Armor], 2)}";
                Window.Write(cString, screenOffset + width - cString.String.Length, y);
                

                y += 2;
            }


            y = 3;
            Window.Write("Enemy team", rightX, y - 2, true);
            for (int i = 0; i < Combat.Enemies.Count; i++)
            {
                Pet pet = Combat.Enemies[i];
                Window.Write(pet.Name, rightX, y, true);

                // Health bar
                y++;
                Window.Write(cChar, Console.WindowWidth - screenOffset - width, y);
                cString.String = new string(' ', (int)((pet[PetStat.Health] * (width - 2) + 1) / pet.BaseStats[PetStat.Health]));
                if (cString.String.Length <= width * 0.2)
                    cString.BackgroundColor = ConsoleColor.DarkRed;
                else if (cString.String.Length <= width * 0.5)
                    cString.BackgroundColor = ConsoleColor.DarkYellow;
                else
                    cString.BackgroundColor = ConsoleColor.Green;
                Window.Write(cString, Console.WindowWidth - screenOffset - width + 1, y);
                Window.Write(cChar, Console.WindowWidth - screenOffset - 1, y);

                // Power and armor
                y++;
                cString.BackgroundColor = ConsoleColor.Black;
                cString.String = $"Power: {Math.Round(pet[PetStat.Power], 2)}";
                Window.Write(cString, Console.WindowWidth - screenOffset - width, y);
                cString.String = $"Armor: {Math.Round(pet[PetStat.Armor], 2)}";
                Window.Write(cString, Console.WindowWidth - screenOffset - cString.String.Length, y);

                y += 3;
            }


            // Menu on the bottom
            y = Console.WindowHeight - 3;
            Window.Write(Combat.ActivePet!.Name + "'s turn.", Window.MiddleX, y - 3, true);
            Window.Write(Combat.CurrentMenu.Title, Window.MiddleX, y - 2, true); 
            for (int i = 0; i < Combat.CurrentMenu.Length; i++)
            {
                Window.Write(Combat.CurrentMenu[i], (int)(Console.WindowWidth * (i + 0.5) / Combat.CurrentMenu.Length), y, true);
            }
        }

        public override void KeyPressed(ConsoleKey key)
        {
            Combat.KeyPressed(key);
        }
    }

}
