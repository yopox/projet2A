using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mono.core
{
    public class Warp : MapObject
    {

        public readonly string Type;

        public Warp(int id, Vector2 position, string type) : base(id, position)
        {
            Type = type;
        }

        public override void Draw(SpriteBatch spriteBatch, AssetManager am)
        {

        }
    }
}
