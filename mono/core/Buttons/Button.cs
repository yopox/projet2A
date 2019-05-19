using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mono.core
{
    class Button
    {

        private readonly string name;
        private Vector2 size;
        private ActionButton action;

        public delegate State ActionButton();

        public Button(string name, Vector2 size)
        {
            this.name = name;
            this.size = size;
        }

        public void Draw(GraphicsDevice GraphicsDevice, SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            Util.DrawTextRectangle(GraphicsDevice, 
                spriteBatch,
                name, 
                new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
        }

        /// <summary>
        /// Définie l'action du bouton
        /// </summary>
        /// <param name="ab"></param>
        public void SetAction(ActionButton ab)
        {
            action = ab;
        }

        /// <summary>
        /// Exécute l'action associée au bouton
        /// </summary>
        /// <returns></returns>
        public State Action()
        {
            return action();
        }
    }
}
