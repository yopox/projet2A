using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mono.Core

{

    public enum Facing
    {
        Left,
        Right
    }


    class Actor
    {
        public Atlas atlas;
        public Facing facing;
        public Vector2 position;
        public float time;
        public float frameTime = 0.1f;

        public Actor(Atlas atlas, Vector2 position)
        {
            this.atlas = atlas;
            this.position = position;
        }

    }
}