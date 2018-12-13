using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mono.core.PhysicsEngine
{
    static class CollisionSolver
    {
        public static void ActorTerrain(Actor actor, List<Polygon> listPolygon)
        {
            //Console.WriteLine("Position avant rectification : " + actor.position);
            foreach (var polygon in listPolygon)
            {
                switch (polygon.type)
                {
                    case PolygonType.Rectangle:
                        var rectangle = (Rect)polygon;

                        //Le joueur se situe à gauche du début du bloc
                        if (actor.position.X < rectangle.X)
                        {
                            //Console.WriteLine(rectangle.X);
                            //Console.WriteLine("acteur : " + actor.position.X + "diff = " + (actor.position.X + actor.size.X - rectangle.X));
                            //Console.WriteLine("diffx = " + (actor.position.X + actor.size.X - rectangle.X) + " et diffy = " + (actor.position.Y + actor.size.Y - rectangle.Y));

                            //Collisision vers la droite
                            if (actor.position.X + actor.size.X - rectangle.X >= actor.position.Y + actor.size.Y - rectangle.Y) 
                            {
                                actor.position.Y = rectangle.Y - actor.size.Y;
                            }
                            else if (actor.position.X + actor.size.X - rectangle.X < actor.position.Y + actor.size.Y - rectangle.Y ||listPolygon.Count == 1)
                            {
                                actor.position.X = rectangle.X - actor.size.X + 1;
                            }


                        }
                        // Le joueur est à droite du début du bloc
                        else if (actor.position.X < rectangle.X + rectangle.Width)
                        {
                            //Console.WriteLine("dans le else diffx = " + (rectangle.X - actor.position.X + actor.size.X - rectangle.Width) + " et diffy = " + (actor.position.Y + actor.size.Y - rectangle.Y));
                            
                            if (rectangle.X - actor.position.X + actor.size.X - rectangle.Width > actor.position.Y + actor.size.Y - rectangle.Y)
                            {
                                actor.position.Y = rectangle.Y - actor.size.Y;
                            }

                            else if(actor.position.Y + actor.size.Y > rectangle.Y + rectangle.Height || listPolygon.Count == 1)
                            {
                                actor.position.X = rectangle.X + rectangle.Width;
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
