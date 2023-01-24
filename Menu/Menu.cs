using System.Runtime.Versioning;

namespace BlueShadowMon
{

    [SupportedOSPlatform("windows")]
    internal static class Menu
    {
        public static ConsoleColor FSelectedColor { get; set; } = ConsoleManager.DefaultBgColor;
        public static ConsoleColor BSelectedColor { get; set; } = ConsoleManager.DefaultFgColor;
        private static int SelectedOption { get; set; } = 0;
        public struct MenuOption
        {
            public string name;
            public ConsoleColor fcolor;
            public ConsoleColor bcolor;
        }

        public struct SettingOption
        {
            public string name;
            public ConsoleColor fcolor;
            public ConsoleColor bcolor;
        };

        private static MenuOption[] MenuOptions_ = new MenuOption[]
        {
            new MenuOption { name = "PLAY", fcolor = FSelectedColor, bcolor = BSelectedColor },
            new MenuOption { name = "SETTINGS", fcolor = ConsoleManager.DefaultFgColor, bcolor = ConsoleManager.DefaultBgColor },
            new MenuOption { name = "EXIT", fcolor = ConsoleManager.DefaultFgColor, bcolor = ConsoleManager.DefaultBgColor },
        };

        private static SettingOption[] SettingOptions_ = new SettingOption[]
        {
            new SettingOption { name = "CHANGE    WINDOW   SIZE" , fcolor = FSelectedColor, bcolor = BSelectedColor},
            new SettingOption { name = "CHANGE BACKGROUND COLOR" , fcolor = FSelectedColor, bcolor = BSelectedColor},
        };


        public static void DrawMenu()
        {
            // Write the Game name
            ConsoleManager.WriteText(Game.GameTitle, ConsoleManager.MiddleX, ConsoleManager.MiddleY - MenuOptions_.Length - 3, ConsoleColor.Blue, ConsoleManager.DefaultBgColor, true);

            // Write the options
            for (int i = 0; i < MenuOptions_.Length; i++)
            {
                int y = ConsoleManager.MiddleY - MenuOptions_.Length + i * 2;
                MenuOption option = MenuOptions_[i];
                ConsoleManager.WriteText(option.name, ConsoleManager.MiddleX, y, option.fcolor, option.bcolor, true);
            }
        }

        public static void DrawSetting()
        {
            //Write the setting title
            ConsoleManager.WriteText("SETTING", ConsoleManager.MiddleX, ConsoleManager.MiddleY - SettingOptions_.Length - 3, ConsoleColor.DarkGray, ConsoleManager.DefaultBgColor, true);

            //Write the setting's option
            for (int i = 0; i < SettingOptions_.Length; i++)
            {
                int y = ConsoleManager.MiddleY - SettingOptions_.Length + i * 2;
                SettingOption option = SettingOptions_[i];
                ConsoleManager.WriteText(option.name, ConsoleManager.MiddleX, y, option.fcolor, option.bcolor, true);
            }
        }

        public static void SelectOption(int option)
        {
            for (int i = 0; i < MenuOptions_.Length; i++)
            {
                if (i == option)
                {
                    MenuOptions_[i].fcolor = FSelectedColor;
                    MenuOptions_[i].bcolor = BSelectedColor;

                }
                else
                {
                    MenuOptions_[i].fcolor = ConsoleManager.DefaultFgColor;
                    MenuOptions_[i].bcolor = ConsoleManager.DefaultBgColor;
                }
            }
        }

        public static void KeyPressed(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    SelectedOption = (SelectedOption - 1) % MenuOptions_.Length;
                    SelectOption(SelectedOption);
                    break;
                case ConsoleKey.DownArrow:
                    SelectedOption = (SelectedOption + 1) % MenuOptions_.Length;
                    SelectOption(SelectedOption);
                    break;
                case ConsoleKey.Enter:
                    switch (MenuOptions_[SelectedOption].name)
                    {
                        case "PLAY":
                            Game.CurrState = Game.State.Map;
                            break;
                        case "SETTINGS":
                            Game.CurrState = Game.State.Settings;
                            break;
                        case "EXIT":
                            Environment.Exit(0);
                            break;
                        default:
                            break;
                    }
                    break;
                case ConsoleKey.Escape:
                    // Back
                    break;
                default:
                    break;
            }
        }

        public static void ChangeWinSize()
        {

        }

        public static void ChangeColor()
        {

        }
    }
}
