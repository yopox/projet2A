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



    //Création de la classe parente de nos entitées : ennemies et héros.
    //On doit ici fournir un atlas de sprite, ainsi qu'une position.
    public class Actor
    {
        public Atlas atlas;
        public Facing facing;
        public Vector2 position;
        public Vector2 speed = new Vector2(0,0);

        public Actor(Atlas atlas, Vector2 position)
        {
            this.atlas = atlas;
            this.position = position;
        }

    }
}