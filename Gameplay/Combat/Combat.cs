namespace BlueShadowMon
{
    public class Combat
    {
        public List<Pet> Allies { get; private set; }
        public List<Pet> Enemies { get; private set; }
        public Menu CurrentMenu { get; private set; }
        private int _currentTurn = -1;
        public Pet? ActivePet { get; private set; }
        private Menu _selectActionMenu;

        // Action : Attack Menus
        private Menu? _selectAbilityMenu;
        private Ability _selectedAbility = Data.NullAbility;
        List<Pet> targetables = new();
        private Menu? _selectTargetMenu;
        private List<Pet> _selectedTargets = new();

        // Action: Inventory
        private Scene _inventoryScene;

        public Combat(List<Pet> allies, List<Pet> enemies)
        {
            Allies = allies;
            Enemies = enemies;

            GoToNextTurn();

            _selectActionMenu = new Menu(
            new Window.ColoredString("What do you want to do?"),
            new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Attack", ConsoleColor.Red, Window.DefaultBgColor), () => { CurrentMenu = _selectAbilityMenu!; }),
                (new Window.ColoredString("Inventory", ConsoleColor.Yellow, Window.DefaultBgColor), () => Game.ToggleInventory() ),
                (new Window.ColoredString("Pets", ConsoleColor.Green, Window.DefaultBgColor), () => { }),
                (new Window.ColoredString("Run", ConsoleColor.DarkCyan, Window.DefaultBgColor), () => { Game.SwitchToMapScene(); })
            });
            CurrentMenu = _selectActionMenu;

            _inventoryScene = new MenuScene(new Menu(new Window.ColoredString("Inventory"),
                new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Back", ConsoleColor.DarkCyan, Window.DefaultBgColor), () => Game.SwitchToCombatScene())
            }));
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

            // Check for dead pets
            for (int i = Allies.Count - 1; i >= 0; i--)
            {
                if (!Allies[i].IsAlive)
                {
                    Window.Message("Your " + Allies[i].Name + " died!");
                    Allies.RemoveAt(i);
                }
            }
            for (int i = Enemies.Count - 1; i >= 0; i--)
            {
                if (!Enemies[i].IsAlive)
                {
                    Window.Message("The " + Enemies[i].Name + " died!");
                    Enemies.RemoveAt(i);
                }
            }

            // Check if the battle is over
            bool isPlayerWin = Enemies.All(pet => pet[PetStat.Health] <= 0);
            if (Allies.All(pet => pet[PetStat.Health] <= 0) || isPlayerWin)
            {
                // Get the experience to give to the player
                int xp = 0;
                foreach (Pet enemy in Enemies)
                {
                    xp += enemy.Level;
                }
                if (!isPlayerWin)
                    xp /= 10;

                // Give the experience to the player's team
                xp /= Allies.Count;
                foreach (Pet ally in Allies)
                {
                    ally.GainXp(xp);
                }

                // Pop up a message
                if (isPlayerWin)
                    Window.Message("You won the battle!");
                else
                    Window.Message("You lost the battle!");

                // Return to the map scene
                Game.SwitchToMapScene();
                return;
            }

            // Get the next active pet
            _currentTurn++;
            if (_currentTurn < Allies.Count)
            {
                ActivePet = Allies[_currentTurn];
            }
            else if (_currentTurn < Allies.Count + Enemies.Count)
            {
                ActivePet = Enemies[_currentTurn - Allies.Count];
            }
            else
            {
                _currentTurn = 0;
                ActivePet = Allies[0];
            }

            // If it's an ally, show the menu to select an ability
            if (Allies.Contains(ActivePet))
            {
                _selectAbilityMenu?.SelectItem(0);
                UpdateSelectAbilityMenu();
            }
            // Else it's an enemy, use the AI to make their move
            else
            {
                AI();
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
            foreach (Ability ability in ActivePet!.Abilities)
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
                targetables.Add(ActivePet!);
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
            _selectTargetMenu.SelectItem(0);
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
            else if (_selectedTargets.Count == 0 || _selectedAbility.CanTarget(EffectTarget.Multiple))
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
            int maxTarget = 1;
            if (_selectedAbility.CanTarget(EffectTarget.Multiple))
                maxTarget = targetables.Count;
            if (_selectedTargets.Count >= maxTarget)
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
        /// 
        /// </summary>
        private void ConfirmTarget()
        {
            _selectedAbility.UseOn(_selectedTargets.ToArray(), ActivePet!);
            GoToNextTurn();
            CurrentMenu = _selectActionMenu;
        }
        private struct AiHelper
        {
            public AiHelper(Pet[] team)
            {
                leastHealth = team[0];
                mostPower = team[0];
                leastPower = team[0];
                mostArmor = team[0];
                leastArmor = team[0];
                foreach (Pet p in team.Skip(1))
                {
                    if (p[PetStat.Health] < leastHealth[PetStat.Health])
                        leastHealth = p;
                    if (p[PetStat.Power] > mostPower[PetStat.Power])
                        mostPower = p;
                    if (p[PetStat.Power] < leastPower[PetStat.Power])
                        leastPower = p;
                    if (p[PetStat.Armor] > mostArmor[PetStat.Armor])
                        mostArmor = p;
                    if (p[PetStat.Armor] < leastArmor[PetStat.Armor])
                        leastArmor = p;
                }
            }

            public Pet leastHealth;
            public Pet mostPower;
            public Pet leastPower;
            public Pet mostArmor;
            public Pet leastArmor;
        }

        /// <summary>
        /// AI make their move.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void AI()
        {
            if (ActivePet == null)
                throw new Exception("Current Pet is null!");


            // First, calculate th emost effective action to do
            // (deal damage? heal allies? apply buff?)
            AiHelper allyTeam = new(Allies.ToArray());
            AiHelper enemyTeam = new(Enemies.ToArray());
            Dictionary<string, int> scores = new()
            {
                { "Attack", 0},
                { "Heal", 0},
                { "PowerBuff", 0},
                { "PowerDebuff", 0},
            };


            // Attack with Health condition

            if (enemyTeam.leastPower[PetStat.Power] <= allyTeam.leastHealth[PetStat.Health])
            {
                scores["Attack"] += 5;
            }

            if (enemyTeam.mostPower[PetStat.Power] >= allyTeam.leastHealth[PetStat.Health])
            {
                scores["Attack"] += 20;
            }

            if (enemyTeam.mostPower[PetStat.Power] <= allyTeam.leastHealth[PetStat.Health])
            {
                scores["Attack"] += 5;
            }


            // Attack with Armor Condition

            if (enemyTeam.mostPower[PetStat.Power] > allyTeam.mostArmor[PetStat.Armor])
            {
                scores["Attack"] += 10;
            }

            if (enemyTeam.mostPower[PetStat.Power] <= allyTeam.mostArmor[PetStat.Armor])
            {
                scores["Attack"] += 5;
            }

            if (enemyTeam.mostPower[PetStat.Power] <= allyTeam.leastArmor[PetStat.Armor])
            {
                scores["Attack"] += 5;
            }

            if (enemyTeam.mostPower[PetStat.Power] > allyTeam.leastArmor[PetStat.Armor])
            {
                scores["Attack"] += 20;
            }


            // Heal with Missing Health Condition

            if (enemyTeam.mostPower == enemyTeam.leastHealth && enemyTeam.mostPower.GetBonusStat(PetStat.Health, true) < -0.35f)
            {
                if (enemyTeam.mostPower == enemyTeam.leastArmor)
                {
                    scores["Heal"] += 30;
                }
            }
            if (enemyTeam.mostArmor == enemyTeam.leastHealth && enemyTeam.mostArmor.GetBonusStat(PetStat.Health, true) < -0.35f)
            {
                if (enemyTeam.mostArmor == enemyTeam.leastPower)
                {
                    scores["Heal"] += 15;
                }
            }


            // PowerDebuff with Power Condition

            if (enemyTeam.mostPower[PetStat.Power] <= allyTeam.mostPower[PetStat.Power])
            {
                scores["PowerDebuff"] += 20;
            }

            if (enemyTeam.mostPower[PetStat.Power] > allyTeam.mostPower[PetStat.Power])
            {
                scores["PowerDebuff"] += 10;
            }

            if (enemyTeam.mostPower[PetStat.Power] > allyTeam.leastPower[PetStat.Power])
            {
                scores["PowerDebuff"] += 5;
            }

            if (enemyTeam.mostPower[PetStat.Power] <= allyTeam.leastPower[PetStat.Power])
            {
                scores["PowerDebuff"] += 10;
            }


            // PowerBuff with Armor Condition

            if (enemyTeam.mostPower[PetStat.Power] > allyTeam.mostArmor[PetStat.Power])
            {
                scores["PowerBuff"] += 10;
            }

            if (enemyTeam.mostPower[PetStat.Power] <= enemyTeam.mostArmor[PetStat.Armor])
            {
                scores["PowerBuff"] += 20;
            }

            if (enemyTeam.mostPower[PetStat.Power] <= enemyTeam.leastArmor[PetStat.Armor])
            {
                scores["PowerBuff"] += 20;
            }

            if (enemyTeam.mostPower[PetStat.Power] > enemyTeam.leastArmor[PetStat.Armor])
            {
                scores["PowerBuff"] += 5;
            }

            string bestAction = scores.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            // Then, select the most effective ability to use
            Ability ability;
            switch (bestAction)
            {
                case "Attack":
                    ability = ActivePet.Abilities.Where(a => a.Type == EffectType.Damage).First();
                    break;
                case "Heal":
                    ability = ActivePet.Abilities.Where(a => a.Type == EffectType.Heal).First();
                    break;
                case "PowerBuff":
                    ability = ActivePet.Abilities.Where(a => a.Type == EffectType.Buff).First();
                    break;
                case "PowerDebuff":
                    ability = ActivePet.Abilities.Where(a => a.Type == EffectType.Debuff).First();
                    break;
                default:
                    ability = ActivePet.Abilities.First();
                    break;
            }
            
            // Finally, chose targets and use it
            Pet[] targets;
            switch (ability.Type)
            {
                case EffectType.Damage:
                    targets = Allies.ToArray();
                    break;
                case EffectType.Heal:
                    targets = Enemies.ToArray();
                    break;
                case EffectType.Buff:
                    targets = Enemies.ToArray();
                    break;
                case EffectType.Debuff:
                    targets = Allies.ToArray();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (ability.CanTarget(EffectTarget.Self))
            {
                targets = new[] { ActivePet };
            }
            else if (!ability.CanTarget(EffectTarget.Multiple))
            {
                targets = new[] { targets.First() };
            }

            ability.UseOn(targets, ActivePet);
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
