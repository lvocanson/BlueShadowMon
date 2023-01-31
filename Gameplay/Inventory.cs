

namespace BlueShadowMon
{
    class Inventory
    {
        
        private Menu _inventoryMenu;
        public Menu CurrentMenu { get; private set; };
        
        private List<Consumable> _consumables = new();
        
        //Action Consumable menu 
        private List<Pet> targetables = new();
        private List<Pet> _selectedTargets = new();
        private Pet _currentPet;
        private Consumable _selectedConsumable;
        private Menu _selectTargetMenu;
        private Menu _selectConsumableMenu;

        public Inventory()
        {

            _inventoryMenu = new Menu(
            new Window.ColoredString("What do you want to use?"),
            new (Window.ColoredString, Action)[] {
                (new Window.ColoredString("Use", ConsoleColor.Red, Window.DefaultBgColor), () => {  }),
                (new Window.ColoredString("Remove", ConsoleColor.Green, Window.DefaultBgColor), () => { }),
                (new Window.ColoredString("Back", ConsoleColor.DarkCyan, Window.DefaultBgColor), () => { Game.SwitchToCombatScene(); })
            });
            /*CurrentMenu = _inventoryMenu;*/
        }
        
        private void UpdateSelectConsumableMenu()
        {
            
        }

        private void SelectTarget(Pet target)
        {
            if (_selectedTargets.Contains(target))
                _selectedTargets.Remove(target);
            else
                _selectedTargets.Add(target);

            // Update the menu
            UpdateSelectTargetMenu();
        }

        private void ConfirmeConsumable(Consumable consumable)
        {
            _selectedConsumable = consumable;
            
            
        }

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

        private void ConfirmTarget()
        {
            _selectedConsumable.UseOn(_selectedTargets.ToArray());
            CurrentMenu = _selectConsumableMenu;
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
                    CurrentMenu = _selectConsumableMenu;
                    break;
                default:
                    break;
            }
        }

    }
}   
