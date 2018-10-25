using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mono.core

{
    public enum Facing
    {
        Left,
        Right
    }

    /// <summary>
    /// Classe parent de tous les acteurs
    /// </summary>
    public class Actor
    {
        public Atlas atlas;//Spritesheet de l'acteur
        public Facing facing;//Direction à laquelle l'acteur fait face
        public Vector2 position;
        public Vector2 speed = new Vector2(0,0);

        public Actor(Atlas atlas, Vector2 position)
        {
            this.atlas = atlas;
            this.position = position;
        }

    }
}