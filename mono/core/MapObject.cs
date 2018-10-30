using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mono.core
{
    public class MapObject
    {

        public Vector2 position;
        public readonly int id;

        public MapObject(int id, Vector2 position)
        {
            this.position = position;
            this.id = id;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Atlas atlas, Camera camera)
        {
            spriteBatch.Draw(atlas.Texture, Util.center + position - camera.center - new Vector2(0, 16),
                                     atlas.GetSourceRectangle(id - 1),
                                     Color.White, 0f, new Vector2(0, 0), 1f,
                                     SpriteEffects.None, 0f);
        }
    }
}
