using System;
using Microsoft.Xna.Framework;

namespace mono.core.Entities
{
    public class Source
    {

        private readonly string id;
        private readonly int radius;
        private readonly int volume;
        private readonly Vector2 position;

        public Source(string id, int radius, int volume, int x, int y)
        {
            this.id = id;
            this.radius = radius;
            this.volume = volume;
            position = new Vector2(x, y);
        }

        public void Activate()
        {

        }

    }
}
