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
        private ConsoleColor defaultFColor;
        private ConsoleColor defaultBColor;
        private ConsoleColor selectedFColor;
        private ConsoleColor selectedBColor;
        public bool isSelected { get; set; } = false;

        //Constructor
        public Menu()
        {
            InitVariables();
            DisplayOption("PLAY", 1, ConsoleColor.White, ConsoleColor.Black);
            DisplayOption("EXIT", 3, ConsoleColor.White, ConsoleColor.Black);

        }

        //Public functions
        public void DisplayOption(String msg, int offset, ConsoleColor f_color, ConsoleColor b_color)
        {
            msg = String.Format("==== {0} ====", msg);
            int x, y;
            x = Console.WindowWidth / 2;
            y = Console.WindowHeight / 2 + offset;

            x = x - (msg.Length / 2);
            y = y - (msg.Split(Environment.NewLine).Length / 2);

            Console.ForegroundColor = f_color;
            Console.BackgroundColor = b_color;

            Console.SetCursorPosition(x, y);
            Console.Write(msg);
        }

        public void InitVariables()
        {
            defaultFColor = ConsoleColor.White;
            defaultBColor = ConsoleColor.Black;

            selectedFColor = ConsoleColor.Yellow;
            selectedBColor = ConsoleColor.White;
        }


        public void keyActions(String input)
        {

            switch (input)
            {
                case "UpArrow":
                   DisplayOption("PLAY", 1, selectedFColor, selectedBColor);
                    break;

                case "DownArrow":

                    break;
                default:

                    break;

            }

        }



        //Private functions



    }
}
