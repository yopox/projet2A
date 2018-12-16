using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mono.core.PhysicsEngine
{
    static class CollisionSolver
    {
        /// <summary>
        /// Place un acteur au bon endroit suivant les collisions
        /// </summary>
        /// <param name="actor">acteur qui va être déplacé au bont endroit</param>
        /// <param name="listPolygon">Liste des polygones qui collisionnent avec l'acteur</param>
        /// <param name="gameTime">acteur dont on va résoudre les collisions</param>
        public static void ActorTerrain(Actor actor, List<Polygon> listPolygon, GameTime gameTime)
        {
            // Calcule l'ancienne position de l'acteur
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var oldPos = actor.position;
            oldPos -= deltaT * actor.speed * Util.baseUnit;
            Util.ToIntVector2(ref oldPos);

            foreach (var polygon in listPolygon)
            {
                switch (polygon.type)
                {
                    case PolygonType.Rectangle:
                        var rectangle = (Rect)polygon;
                        // Collision à droite
                        if (actor.position.X < rectangle.X)
                        {
                            // Collision vers le bas
                            if (actor.position.X + actor.size.X - rectangle.X >= actor.position.Y + actor.size.Y - rectangle.Y && oldPos.Y + actor.size.Y <= rectangle.Y + 1) 
                            {
                                actor.position.Y = rectangle.Y - actor.size.Y;
                                actor.acceleration.Y = 0;
                                actor.speed.Y = 0;

                                actor.acceleration.X = 0;
                            }
                            else if (actor.position.X + actor.size.X - rectangle.X < actor.position.Y + actor.size.Y - rectangle.Y || listPolygon.Count == 1)
                            {

                                // Collision vers le haut
                                if (oldPos.Y >=  rectangle.Y + rectangle.Height - 1 && oldPos.X + actor.size.X > rectangle.X)
                                {
                                    actor.position.Y = rectangle.Y + rectangle.Height;
                                    actor.acceleration.Y = Util.gravity.Y;
                                    actor.speed.Y = 0;
                                }
                                // Collision vers la droite
                                else
                                {
                                    actor.position.X = rectangle.X - actor.size.X;
                                    actor.acceleration.X = 0;
                                    actor.speed.X = 0;
                                }
                            }
                        }
                        // Collision à gauche du joueur
                        else if (actor.position.X < rectangle.X + rectangle.Width)
                        {
                            // Collision avec le bas
                            if (rectangle.X - actor.position.X + actor.size.X - rectangle.Width > actor.position.Y + actor.size.Y - rectangle.Y && oldPos.Y + actor.size.Y <= rectangle.Y + 1)
                            {
                                actor.position.Y = rectangle.Y - actor.size.Y;
                                actor.acceleration.Y = 0;
                                actor.speed.Y = 0;

                                actor.acceleration.X = 0;
                            }
                            else if(actor.position.Y + actor.size.Y > rectangle.Y + rectangle.Height || listPolygon.Count == 1)
                            {
                                // Collision avec le haut
                                if (oldPos.Y >= rectangle.Y + rectangle.Height - 1 && oldPos.X < rectangle.X + rectangle.Width)
                                {
                                    actor.position.Y = rectangle.Y + rectangle.Height;
                                    actor.acceleration.Y = Util.gravity.Y;
                                    actor.speed.Y = 0;
                                }
                                // Collision avec la gauche
                                else
                                {
                                    actor.position.X = rectangle.X + rectangle.Width;
                                    actor.acceleration.X = 0;
                                    actor.speed.X = 0;
                                }
                            }
                        }
                        break;
                    case PolygonType.Triangle:
                        break;
                    default:
                        break;

                }
            }
        }
    }
}
