using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueShadowMon
{
    internal class Gui
    {
        public Gui()
        {

        }


        public static String HealthBar(int maxHealth, int damage, int width )
        {
            String bar = "[";
            double percent = (double)damage / maxHealth;
            int actualHealth = Convert.ToInt32(percent * width);
            int missinsHealth = actualHealth - damage;
            for (int i = 0; i < maxHealth; i++)
            {
                bar += "H";
            }
            for (int i = 0; i <= damage; i++)
            {
                bar = bar.Substring(0, maxHealth - damage + 1);
            }
            for (int i = 0; i < damage; i++)
            {
                bar += "-";
            }

            bar += "]";

            return bar;
        }


    }
}
