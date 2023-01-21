using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueShadowMon
{
    public class Tile
    {
        // Empty tile
        private List<string> Skin { get; set; }
        public string this[int line]
        {
            get { return Skin[line]; }
        }
        public ConsoleColor FColor { get; set; }
        public ConsoleColor BColor { get; set; }
        public bool IsWalkable { get; set; }
        public const int Width = 3;
        public const int Height = 2;

        public Tile(List<string> skin, ConsoleColor tileColor, ConsoleColor tileBackgroundColor, bool isWalkable)
        {
            this.Skin = skin;
            FColor = tileColor;
            BColor = tileBackgroundColor;
            IsWalkable = isWalkable;
        }
        
    }
}