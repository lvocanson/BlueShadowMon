using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueShadowMon
{
    public class Map
    {
        private List<List<Tile>> TileMap { get; set; }
        public Tile this[int x, int y]
        {
            get { return TileMap[x][y]; }
            set { TileMap[x][y] = value; }
        }
        public int Width { get; set; }
        public int Height { get; set; }
        public int CharacterWidth { get; set; }
        public int CharacterHeight { get; set; }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            CharacterWidth = width * Tile.Width;
            CharacterHeight = height * Tile.Height;

            // Base skin
            List<string> skin = new List<string>();
            for (int i = 0; i < Tile.Height; i++)
            {
                skin.Add(new string('#', Tile.Width));
            }

            // Create a new map
            TileMap = new List<List<Tile>>();
            for (int i = 0; i < width; i++)
            {
                TileMap.Add(new List<Tile>());
                for (int j = 0; j < height; j++)
                {
                    // Create a new tile and add it to the map
                    Tile t = new Tile(skin, ConsoleColor.White, ConsoleColor.Black, true);
                    TileMap[i].Add(t);
                }
            }
        }
    }
}