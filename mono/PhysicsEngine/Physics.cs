using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using mono.core;

namespace mono.PhysicsEngine
{
    class Physics
    {
        private List<Actor> _actors= new List<Actor>();
        public Vector2 gravity;

        public Physics(Vector2 gravity)
        {
            this.gravity = gravity;
        }

        public void addActor(Actor actor)
        {
            _actors.Add(actor);
        }

        public void Update(GameTime gameTime)
        {
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Actor actor in _actors)
            {
                actor.acceleration += gravity;
                actor.speed += deltaT * actor.acceleration;
                actor.position += deltaT * actor.speed;
            }


        }
    }
}
