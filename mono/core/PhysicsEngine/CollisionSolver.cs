using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
        public static void ActorTerrain(Actor actor, Polygon polygon, GameTime gameTime)
        {
            // Calcule l'ancienne position de l'acteur
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var oldPos = actor.Position;
            oldPos -= deltaT * actor.Speed * Util.BaseUnit;
            Util.ToIntVector2(ref oldPos);

            switch (polygon.type)
            {
                case PolygonType.Rectangle:
                    var rectangle = (Rect)polygon;
                    // Collision vers le bas
                    if (oldPos.Y + actor.Size.Y <= rectangle.Y)
                    {
                        actor.SetY((int)(rectangle.Y - actor.Size.Y), 0, 0);
                        actor.Acceleration.X = 0;
                    }

                    // Collision vers le haut
                    else if (oldPos.Y >= rectangle.Y + rectangle.Height)
                        actor.SetY((int)(rectangle.Y + rectangle.Height), 0, Util.Gravity.Y);

                    // Collision à droite
                    else if (actor.Position.X < rectangle.X)
                        actor.SetX((int)(rectangle.X - actor.Size.X), 0, 0);

                    // Collision à gauche du joueur
                    else
                        actor.SetX((int)(rectangle.X + rectangle.Width), 0, 0);

                    break;

                case PolygonType.TriangleL:
                    // (int)(triangleL.A.Y - (triangleL.A.X + triangleL.Width - actor.position.X) * triangleL.Height / triangleL.Width  - actor.size.Y);
                    var triangleL = (Tri)polygon;
                    //Collision pente
                    if (actor.Position.X > triangleL.A.X && actor.Position.Y + actor.Size.Y <= triangleL.A.Y + 1)
                        actor.SetY((int)(triangleL.A.Y - (triangleL.A.X + triangleL.Width - actor.Position.X) * triangleL.Height / triangleL.Width - actor.Size.Y), 0, 0);

                    //Collision bas
                    else if (actor.Position.Y + actor.Size.Y < triangleL.A.Y - triangleL.Height + 7)
                    {
                        actor.SetY((int)(triangleL.A.Y - triangleL.Height - actor.Size.Y), 0, 0);
                    }

                    //Collision droite
                    else if (actor.Position.X < triangleL.A.X && oldPos.Y < triangleL.A.Y + 1 && oldPos.Y + actor.Size.Y > triangleL.A.Y - triangleL.Height)
                        actor.SetX((int)(triangleL.A.X - actor.Size.X), 0, 0);

                    //Collision haut
                    else if (oldPos.Y > triangleL.A.Y && oldPos.X < triangleL.A.X + triangleL.Width)
                        actor.SetY((int)(triangleL.A.Y), 0, 0);

                    //Collision coin
                    else
                        actor.SetX((int)(triangleL.A.X + triangleL.Width), 0, 0);

                    break;

                case PolygonType.TriangleR:
                    var triangleR = (Tri)polygon;

                    //Collision pente
                    if (actor.Position.X + actor.Size.X < triangleR.A.X && actor.Position.Y + actor.Size.Y <= triangleR.A.Y + 1)
                        actor.SetY((int)(triangleR.A.Y - (actor.Position.X + actor.Size.X - triangleR.A.X + triangleR.Width) * triangleR.Height / triangleR.Width - actor.Size.Y), 0, 0);

                    //Collision bas
                    else if (actor.Position.Y + actor.Size.Y < triangleR.A.Y - triangleR.Height + 7)
                        actor.SetY((int)(triangleR.A.Y - triangleR.Height - actor.Size.Y), 0, 0);
                    
                    //Collision gauche
                    else if (actor.Position.X > triangleR.A.X - triangleR.Width && oldPos.Y < triangleR.A.Y + 1)
                        actor.SetX((int)(triangleR.A.X), 0, 0);
                    
                    //Collision haut
                    else if (oldPos.Y > triangleR.A.Y && oldPos.X + actor.Size.X > triangleR.A.X - triangleR.Width)
                        actor.SetY((int)(triangleR.A.Y), 0, 0);

                    //Collision coin
                    else
                        actor.SetX((int)(triangleR.A.X - triangleR.Width - actor.Size.X), 0, 0);

                    break;
            }
        }
    }
}