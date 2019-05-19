using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mono.core;
using mono.RenderEngine;

namespace mono.PhysicsEngine
{
    static class Physics
    {
        public static List<Actor> Actors { get; } = new List<Actor>();
        public static Vector2 Gravity { get; set; } = Vector2.Zero;

        public static void addActor(Actor actor)
        {
            Actors.Add(actor);
        }


        /// <summary>
        /// Calcule acceleration, vitesse et position de tous les acteurs référencés
        /// </summary>
        /// <param name="gameTime">Contient les données sur la gestion du temps</param>
        public static void UpdateAll(GameTime gameTime)
        {
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Actor actor in Actors)
            {

                actor.Acceleration = Gravity + (actor.Forces - 40 * actor.Speed) / Util.Weight;
                actor.Speed += deltaT * actor.Acceleration;
                actor.Position += deltaT * actor.Speed * Util.BaseUnit;
                actor.Center += deltaT * actor.Speed * Util.BaseUnit;
                Util.ToIntVector2(ref actor.Position);

                actor.Forces = Vector2.Zero;
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

            actor.Acceleration = Gravity + (actor.Forces - 40 * actor.Speed) / Util.Weight;
            actor.Speed += deltaT * actor.Acceleration;
            actor.Position += deltaT * actor.Speed;
            Util.ToIntVector2(ref actor.Position);

            actor.Forces = Vector2.Zero;
        }
    }
}