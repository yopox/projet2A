using System;
using Microsoft.Xna.Framework;

namespace mono.core.Entities
{
    /// <summary>
    /// Moving box.
    /// TODO: Hitbox
    /// </summary>
    public class MovingBox : MapObject
    {
        public MovingBox(int id, Vector2 position) : base(id, position)
        {
        }
    }
}
