using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueShadowMon
{
    internal static class Gui
    {

        public static String HealthBar(int maxHealth, int currentHealth, int width = 12)
        { 
            int porcent = Convert.ToInt32(currentHealth * (width - 2) / maxHealth);
            return "[" + new string('H', porcent) + new string('-', width - 2 - porcent) + "]";
        }


    }
}
