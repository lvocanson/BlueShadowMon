namespace BlueShadowMon
{
    public class Combat
    {
        public Pet[] Allies { get; private set; }
        public Pet[] Enemies { get; private set; }
        public Menu CurrentMenu { get; private set; }
        private int _currentTurn = -1;
        private Pet _currentPet;
        private Menu _selectActionMenu;

        // Action : Attack Menus
        private Menu _selectAbilityMenu;
        private Ability _selectedAbility = Data.NullAbility;
        List<Pet> targetables = new();
        private Menu _selectTargetMenu;
        private List<Pet> _selectedTargets = new();

        public Combat(Pet[] allies, Pet[] enemies)
        {
            Allies = allies;
            Enemies = enemies;

            GoToNextTurn(); // Here we define _selectAbilityMenu

            _selectActionMenu = new Menu(
            new Window.ColoredString("What do you want to do?"),
            new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Attack", ConsoleColor.Red, Window.DefaultBgColor), () => { CurrentMenu = _selectAbilityMenu!; }),
                (new Window.ColoredString("Inventory", ConsoleColor.Yellow, Window.DefaultBgColor), () => { }),
                (new Window.ColoredString("Pets", ConsoleColor.Green, Window.DefaultBgColor), () => { }),
                (new Window.ColoredString("Run", ConsoleColor.DarkCyan, Window.DefaultBgColor), () => { Game.SwitchToMapScene(); })
            });
            CurrentMenu = _selectActionMenu;
        }

        /// <summary>
        /// Go to the next turn.
        /// </summary>
        public void GoToNextTurn()
        {
            // Reset the selected ability and targets from the previous turn
            _selectedAbility = Data.NullAbility;
            targetables.Clear();
            _selectedTargets.Clear();

            // Get the next active pet
            _currentTurn++;
            if (_currentTurn < Allies.Length)
            {
                _currentPet = Allies[_currentTurn];
            }
            else if (_currentTurn < Allies.Length + Enemies.Length)
            {
                _currentPet = Enemies[_currentTurn - Allies.Length];
            }
            else
            {
                _currentTurn = 0;
                _currentPet = Allies[0];
            }

            // If it's an ally, show the menu to select an ability
            if (Allies.Contains(_currentPet))
            {
                UpdateSelectAbilityMenu();
            }
            // Else it's an enemy, use the IA to make their move
            else
            {
                IA();
                GoToNextTurn();
            }
        }

        /// <summary>
        /// Update the menu to select an ability with the current pet.
        /// </summary>
        private void UpdateSelectAbilityMenu()
        {
            // Create the menu actions
            List<(Window.ColoredString, Action)> attackActions = new();
            foreach (Ability ability in _currentPet.Abilities)
            {
                void Confirm()
                {
                    ConfirmAbility(ability);
                }
                attackActions.Add((ability.Name, Confirm));
            }

            // Create the menu
            if (_selectAbilityMenu == null)
                _selectAbilityMenu = new(
                new Window.ColoredString("Select an Ability to use:", ConsoleColor.Red, Window.DefaultBgColor),
                attackActions.ToArray()
                );
            else
                _selectAbilityMenu.Init(
                new Window.ColoredString("Select an Ability to use:", ConsoleColor.Red, Window.DefaultBgColor),
                attackActions.ToArray()
            );
        }

        /// <summary>
        /// Confirm the ability to use and create the list of targetable pets.
        /// Then show the menu to select the target(s).
        /// </summary>
        /// <param name="ability">Ability selected</param>
        private void ConfirmAbility(Ability ability)
        {
            _selectedAbility = ability;
            targetables.Clear();

            // Add each Pet that can be targeted with the ability
            if (ability.CanTarget(EffectTarget.Self))
                targetables.Add(_currentPet);
            if (ability.CanTarget(EffectTarget.Ally))
            {
                foreach (Pet ally in Allies)
                {
                    targetables.Add(ally);
                }
            }
            if (ability.CanTarget(EffectTarget.Enemy))
            {
                foreach (Pet enemy in Enemies)
                {
                    targetables.Add(enemy);
                }
            }

            // Select target Menu
            UpdateSelectTargetMenu();
            CurrentMenu = _selectTargetMenu;
        }

        /// <summary>
        /// Select a target for the ability.
        /// </summary>
        /// <param name="target">New selection</param>
        private void SelectTarget(Pet target)
        {
            if (_selectedTargets.Contains(target))
                _selectedTargets.Remove(target);
            else
                _selectedTargets.Add(target);

            // Update the menu
            UpdateSelectTargetMenu();
        }

        /// <summary>
        /// Update the menu to select the target(s) for the ability.
        /// Selected targets are highlighted.
        /// The confirm button is disabled if no target is selected.
        /// </summary>
        private void UpdateSelectTargetMenu()
        {
            // Create the title
            Window.ColoredString title;
            if (_selectedTargets.Count >= targetables.Count)
                title = new("Maximum number of target reached.", ConsoleColor.Red, Window.DefaultBgColor);
            else
                title = new("Select targets:");

            // Create the confirm button
            (Window.ColoredString, Action) confirm;
            if (_selectedTargets.Count > 0)
                confirm = (new("Confirm", ConsoleColor.Yellow, Window.DefaultBgColor), ConfirmTarget);
            else
                confirm = (new("Confirm", ConsoleColor.Gray, Window.DefaultBgColor), () => { });

            // Create and add menu actions
            List<(Window.ColoredString, Action)> menuActions = new();
            foreach (Pet target in targetables)
            {
                // Name
                Window.ColoredString targetName = new(target.Name);
                if (_selectedTargets.Contains(target))
                    targetName.ForegroundColor = ConsoleColor.Yellow;
                // Action
                void Select()
                {
                    SelectTarget(target);
                }

                menuActions.Add((targetName, Select));
            }
            menuActions.Add(confirm);

            // Create and replace the old menu
            if (_selectTargetMenu == null)
                _selectTargetMenu = new(title, menuActions.ToArray());
            else
                _selectTargetMenu.Init(title, menuActions.ToArray(), _selectTargetMenu.SelectedItem);
        }

        /// <summary>
        /// Confirm the target(s) and use the ability on them.
        /// </summary>
        private void ConfirmTarget()
        {
            _selectedAbility.UseOn(_selectedTargets.ToArray(), _currentPet);
            CurrentMenu = _selectActionMenu;
        }

        /// <summary>
        /// IA make their move.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void IA()
        {
            if (_currentPet == null)
                throw new Exception("Current Pet is null!");
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
                    Console.Clear();
                    CurrentMenu = _selectActionMenu;
                    break;
                default:
                    break;
            }
        }
    }
}
