using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mono.core
{
    public class Warp
    {

        public readonly string Type;
        public readonly Vector2 Position;

        public Warp(int id, Vector2 position, string type)
        {
            Type = type;
            Position = position;
        }

    }
}
