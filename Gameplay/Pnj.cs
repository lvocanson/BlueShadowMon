namespace BlueShadowMon
{
    public class Pnj
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public Pnj((int x, int y) mapPosition)
        {
            (x, y) = mapPosition;
        }

        
}
    }
