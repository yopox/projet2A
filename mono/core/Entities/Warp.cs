using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mono.core
{
    public class Warp : MapObject
    {

        public readonly string type;

        public Warp(int id, Vector2 position, string type) : base(id, position)
        {
            this.type = type;
        }

        public override void Draw(SpriteBatch spriteBatch, Atlas atlas, Camera camera)
        {

        }
    }
}
