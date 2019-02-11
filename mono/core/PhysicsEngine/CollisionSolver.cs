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
        public static Boolean slope = false;

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
                if (!slope)
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
                                    actor.SetY((int)(rectangle.Y - actor.size.Y), 0, 0);
                                    actor.acceleration.X = 0;
                                }
                                else if (actor.position.X + actor.size.X - rectangle.X < actor.position.Y + actor.size.Y - rectangle.Y || listPolygon.Count == 1)
                                {

                                    // Collision vers le haut
                                    if (oldPos.Y >= rectangle.Y + rectangle.Height - 1 && oldPos.X + actor.size.X > rectangle.X)
                                    {
                                        actor.SetY((int)(rectangle.Y + rectangle.Height), 0, Util.gravity.Y);
                                    }
                                    // Collision vers la droite
                                    else
                                    {
                                        actor.SetX((int)(rectangle.X - actor.size.X), 0, 0);
                                    }
                                }
                            }
                            // Collision à gauche du joueur
                            else if (actor.position.X < rectangle.X + rectangle.Width)
                            {
                                // Collision avec le bas
                                if (rectangle.X - actor.position.X + actor.size.X - rectangle.Width > actor.position.Y + actor.size.Y - rectangle.Y && oldPos.Y + actor.size.Y <= rectangle.Y + 1)
                                {
                                    actor.SetY((int)(rectangle.Y - actor.size.Y), 0, 0);
                                    actor.acceleration.X = 0;
                                }
                                else if (actor.position.Y + actor.size.Y > rectangle.Y + rectangle.Height || listPolygon.Count == 1)
                                {
                                    // Collision avec le haut
                                    if (oldPos.Y >= rectangle.Y + rectangle.Height - 1 && oldPos.X < rectangle.X + rectangle.Width)
                                    {
                                        actor.SetY((int)(rectangle.Y + rectangle.Height), 0, Util.gravity.Y);
                                    }
                                    // Collision avec la gauche
                                    else
                                    {
                                        actor.SetX((int)(rectangle.X + rectangle.Width), 0, 0);
                                    }
                                }
                            }
                            break;
                        case PolygonType.TriangleL:
                            // (int)(triangleL.A.Y - (triangleL.A.X + triangleL.Width - actor.position.X) * triangleL.Height / triangleL.Width  - actor.size.Y);
                            var triangleL = (Tri)polygon;
                            //Collision pente
                            if (actor.position.X > triangleL.A.X && actor.position.Y + actor.size.Y <= triangleL.A.Y + 1)
                            {
                                actor.SetY((int)(triangleL.A.Y - (triangleL.A.X + triangleL.Width - actor.position.X) * triangleL.Height / triangleL.Width - actor.size.Y), 0, 0);
                                slope = true;
                            }
                            //Collision bas
                            else if (actor.position.Y + actor.size.Y < triangleL.A.Y - triangleL.Height + 7 )
                            {
                                actor.SetY((int)(triangleL.A.Y - triangleL.Height - actor.size.Y), 0, 0);
                            }
                            //Collision droite
                            else if (actor.position.X < triangleL.A.X && oldPos.Y < triangleL.A.Y + 1 && oldPos.Y + actor.size.Y > triangleL.A.Y - triangleL.Height)
                            {
                                actor.SetX((int)(triangleL.A.X - actor.size.X), 0, 0);
                            }
                            //Collision haut
                            else if (oldPos.Y > triangleL.A.Y)
                            {
                                actor.SetY((int)(triangleL.A.Y), 0, 0);
                            }
                            else
                            {
                                actor.SetX((int)(triangleL.A.X + triangleL.Width), 0, 0);
                            }
                            break;
                        case PolygonType.TriangleR:
                            var triangleR = (Tri)polygon;
                            //Collision pente
                            if (actor.position.X + actor.size.X < triangleR.A.X && actor.position.Y + actor.size.Y <= triangleR.A.Y + 1)
                            {
                                actor.SetY((int)(triangleR.A.Y - (actor.position.X + actor.size.X - triangleR.A.X + triangleR.Width) * triangleR.Height / triangleR.Width - actor.size.Y), 0, 0);
                                slope = true;
                            }
                            //Collision bas
                            else if (actor.position.Y + actor.size.Y < triangleR.A.Y - triangleR.Height + 7)
                            {
                                actor.SetY((int)(triangleR.A.Y - triangleR.Height - actor.size.Y), 0, 0);
                            }
                            //Collision gauche
                            else if (actor.position.X > triangleR.A.X - triangleR.Width && oldPos.Y < triangleR.A.Y + 1)
                            {
                                actor.SetX((int)(triangleR.A.X), 0, 0);
                            }
                            //Collision haut
                            else if (oldPos.Y > triangleR.A.Y)
                            {
                                actor.SetY((int)(triangleR.A.Y), 0, 0);
                            }
                            else
                            {
                                actor.SetX((int)(triangleR.A.X - triangleR.Width - actor.size.X), 0, 0);
                            }
                            break;
                    }
                }
            }
        }
    }
}