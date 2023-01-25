namespace BlueShadowMon
{
    internal abstract class Scene
    {
        /// <summary>
        /// Draws the scene in the console.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Handle a key press.
        /// </summary>
        /// <param name="key">The key pressed</param>
        public abstract void KeyPressed(ConsoleKey key);
    }
}
