namespace BlueShadowMon
{
    public class NPC
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public string[] dialogs { get; private set; }

        public NPC(int x, int y, string[] dialogs)
        {
            (this.x, this.y) = (x, y);
            this.dialogs = dialogs;
        }

        public void RunDialogue()
        {
            foreach (string dialog in dialogs)
            {
                Window.Message(dialog);
            }
        }
    }
}
