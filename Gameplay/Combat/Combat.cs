namespace BlueShadowMon.Gameplay.Combat
{
    public class Combat
    {
        public Pet[] Allies { get; private set; }
        public Pet[] Enemies { get; private set; }
        public Menu CurrentMenu { get; private set; }
        private int _currentTurn;
        private Pet _currentPet;
        private bool _isPlayerTurn;

        private Menu _combatMenu => new Menu(
            new Window.ColoredString("What do you want to do?", ConsoleColor.Blue, Window.DefaultBgColor),
            new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Attack", ConsoleColor.Red, Window.DefaultBgColor), () => { CurrentMenu = _attackMenu; }),
                (new Window.ColoredString("Inventory", ConsoleColor.Yellow, Window.DefaultBgColor), () => { }),
                (new Window.ColoredString("Pets", ConsoleColor.Green, Window.DefaultBgColor), () => { }),
                (new Window.ColoredString("Run", ConsoleColor.DarkCyan, Window.DefaultBgColor), () => { Game.SwitchToMapScene(); })
            });
        private Menu _attackMenu;

        public Combat(Pet[] allies, Pet[] enemies)
        {
            CurrentMenu = _combatMenu;
            Allies = allies;
            Enemies = enemies;
            _currentTurn = -1;
            GotoNextTurn();
        }

        public void GotoNextTurn()
        {
            _currentTurn = (_currentTurn + 1) % (Allies.Length + Enemies.Length);

            if (_currentTurn < Allies.Length)
            {
                _isPlayerTurn = true;
                _currentPet = Allies[_currentTurn];

                // Update attack menu
                _attackMenu = new Menu(
                    new Window.ColoredString("Select an Ability", ConsoleColor.Red, Window.DefaultBgColor),
                    new (Window.ColoredString, Action)[] {
                        (_currentPet[0].Name, () => { _currentPet.UseAbility(0, Enemies[0]); GotoNextTurn(); }),
                    });
            }
            else
            {
                _isPlayerTurn = false;
                _currentPet = Enemies[_currentTurn - Allies.Length];

                // IA make their move
                IA();
                GotoNextTurn();
            }
        }

        private void IA()
        {
            Pet target = Allies[0];
            _currentPet.UseAbility(0, target);
        }

        public void KeyPressed(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    Console.Clear();
                    CurrentMenu.Before();
                    break;
                case ConsoleKey.RightArrow:
                    Console.Clear();
                    CurrentMenu.After();
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    CurrentMenu.Confirm();
                    break;
                case ConsoleKey.Escape:
                    CurrentMenu.SelectItem(0);
                    CurrentMenu = _combatMenu;
                    break;
                default:
                    break;
            }
        }
    }
}
