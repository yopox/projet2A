using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using mono.core;

namespace mono.PhysicsEngine
{
    static class Physics
    {
        private static List<Actor> _actors = new List<Actor>();
        private static Vector2 _gravity = Vector2.Zero;
        public static Vector2 Gravity { get => _gravity; set => _gravity = value; }

        public static void addActor(Actor actor)
        {
            _actors.Add(actor);
        }



        public static void UpdateAll(GameTime gameTime)
        {
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Actor actor in _actors)
            {
                actor.acceleration = _gravity - 15 * actor.speed + actor.forces;
                actor.speed += deltaT * actor.acceleration;
                actor.position += deltaT * actor.speed;
                actor.forces = Vector2.Zero;
            }
        }

        public static void Update(GameTime gameTime, Actor actor)
        {
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;
            actor.acceleration = _gravity - 15 * actor.speed + actor.forces;
            actor.speed += deltaT * actor.acceleration;
            actor.position += deltaT * actor.speed;
            actor.forces = Vector2.Zero;
        }
    }
}