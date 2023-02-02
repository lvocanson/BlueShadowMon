namespace BlueShadowMon
{
    public class NPC
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public string[] dialogs { get; private set; }

        /// <summary>
        /// Create a new NPC.
        /// </summary>
        /// <param name="x">Position on the map</param>
        /// <param name="y">Position on the map</param>
        /// <param name="dialogs">List of dialogs</param>
        public NPC(int x, int y, string[] dialogs)
        {
            (this.x, this.y) = (x, y);
            this.dialogs = dialogs;
        }

        /// <summary>
        /// Run the dialogue of the NPC.
        /// </summary>
        public void RunDialogue()
        {
            foreach (string dialog in dialogs)
            {
                Window.Message(dialog);
            }
        }
    }
}
