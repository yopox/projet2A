using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mono.core
{
    class Button
    {

        private readonly string _name;
        private Vector2 _size;
        private ActionButton _action;

        public delegate State ActionButton();

        public Button(string name, Vector2 size)
        {
            _name = name;
            _size = size;
        }

        public void Draw(GraphicsDevice GraphicsDevice, SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            Util.DrawTextRectangle(GraphicsDevice, 
                spriteBatch,
                _name, 
                new Rectangle((int)position.X, (int)position.Y, (int)_size.X, (int)_size.Y), color);
        }

        /// <summary>
        /// Définie l'action du bouton
        /// </summary>
        /// <param name="ab"></param>
        public void SetAction(ActionButton ab)
        {
            _action = ab;
        }

        /// <summary>
        /// Exécute l'action associée au bouton
        /// </summary>
        /// <returns></returns>
        public State Action()
        {
            return _action();
        }
    }
}
