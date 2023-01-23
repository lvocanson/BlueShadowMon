using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlueShadowMon
{
    class Menu
    {
        //Variables
        private ArrayList menuOptions;
        private int selectedIndex;
        private ConsoleColor f_selectedColor;
        private ConsoleColor b_selectedColor;
        private ConsoleColor df_selectedColor;
        private ConsoleColor db_selectedColor;
        public bool isSelected { get; set; } = false;

        //Constructor
        public Menu()
        {
            addOptions();



        }

        //Public functions
        public void DisplayMenu()
        {
            for (int i = 0; i < 2; i++)
            {
                ConsoleManager.WriteText(menuOptions[i].ToString(), Console.WindowWidth / 2, Console.WindowHeight / 2 + 2 * i, df_selectedColor, db_selectedColor, true);
            }
        }

        public void addOptions()
        {
            menuOptions = new ArrayList();
            menuOptions.Add("PLAY");
            menuOptions.Add("EXIT");

            f_selectedColor = ConsoleColor.White;
            b_selectedColor = ConsoleColor.Black;

            df_selectedColor = ConsoleColor.Black;
            db_selectedColor = ConsoleColor.White;

            selectedIndex = 0;
        }

        public void Selected()
        {
            if (selectedIndex == 1)
            {
                ConsoleManager.WriteText(menuOptions[selectedIndex - selectedIndex].ToString(), Console.WindowWidth / 2, Console.WindowHeight / 2 + 2 * selectedIndex, f_selectedColor, b_selectedColor, true);
                ConsoleManager.WriteText(menuOptions[selectedIndex].ToString(), Console.WindowWidth / 2, Console.WindowHeight / 2 + 2 * selectedIndex, df_selectedColor, db_selectedColor, true);
            }
            else if (selectedIndex == 2)
            {
                ConsoleManager.WriteText(menuOptions[selectedIndex - selectedIndex].ToString(), Console.WindowWidth / 2, Console.WindowHeight / 2 + 2 * selectedIndex, df_selectedColor, db_selectedColor, true);
                ConsoleManager.WriteText(menuOptions[selectedIndex -1].ToString(), Console.WindowWidth / 2, Console.WindowHeight / 2 + 2 * selectedIndex, f_selectedColor, b_selectedColor, true);
            }

        }

        public void keyActions(String input)
        {

            switch (input)
            {
                case "UpArrow":
                    selectedIndex = 1;
                    Selected();
                    break;

                case "DownArrow":
                    selectedIndex = 2;
                    Selected();
                    break;
                default:

                    break;

            }

        }



        //Private functions



    }
}
