using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mono.core;

namespace mono.PhysicsEngine
{
    static class Physics
    {
        private static List<Actor> _actors = new List<Actor>();
        public static List<Actor> Actors { get => _actors; }
        private static Vector2 _gravity = Vector2.Zero;
        public static Vector2 Gravity { get => _gravity; set => _gravity = value; }

        public static void addActor(Actor actor)
        {
            _actors.Add(actor);
        }


        /// <summary>
        /// Calcule acceleration, vitesse et position de tous les acteurs référencés
        /// </summary>
        /// <param name="gameTime">Contient les données sur la gestion du temps</param>
        public static void UpdateAll(GameTime gameTime)
        {
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Actor actor in _actors)
            {

                actor.acceleration = _gravity + (actor.forces - 40 * actor.speed) / Util.weight ;
                actor.speed += deltaT * actor.acceleration;
                actor.position += deltaT * actor.speed * Util.baseUnit;
                actor.center += deltaT * actor.speed * Util.baseUnit;
                Util.ToIntVector2(ref actor.position);

                actor.forces = Vector2.Zero;
            }
        }

        /// <summary>
        /// Calcule acceleration, vitesse et position d'un acteur
        /// </summary>
        /// <param name="gameTime">Contient les données sur la gestion du temps</param>
        /// <param name="actor">Acteur qui va être modifié</param>
        public static void Update(GameTime gameTime, Actor actor)
        {
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            actor.acceleration = _gravity - 10000000 * actor.speed + actor.forces;
            actor.speed += deltaT * actor.acceleration;
            actor.position += deltaT * actor.speed;
            Util.ToIntVector2(ref actor.position);

            actor.forces = Vector2.Zero;
        }
    }
}