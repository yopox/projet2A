using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mono.core.PhysicsEngine
{
    static class CollisionSolver
    {
        public static void ActorTerrain(Actor actor, Polygon polygon)
        {
            //Console.WriteLine("Position avant rectification : " + actor.position);
            switch (polygon.type)
            {
                case PolygonType.Rectangle:
                    var rectangle = (Rect)polygon;
                    if(actor.position.X < rectangle.X)
                    {
                        if(actor.position.X + actor.size.X - rectangle.X < actor.position.Y + actor.size.Y - rectangle.Y)
                        {
                            actor.position.X = rectangle.X - actor.size.X + 1;
                        }
                        else
                        {
                            actor.position.Y = rectangle.Y - actor.size.Y - 1;
                        }
                    }
                    else
                    {
                        if (rectangle.X + rectangle.Width - actor.position.X < actor.position.Y + actor.size.Y - rectangle.Y)
                        {
                            actor.position.X = rectangle.X + rectangle.Width - 1;
                        }
                        else
                        {
                            actor.position.Y = rectangle.Y - actor.size.Y + 1;
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
