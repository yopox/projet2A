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
            switch (polygon.type)
            {
                case PolygonType.Rectangle:
                    Rect rectangle = (Rect)polygon;
                    if (actor.position.Y > rectangle.Y && actor.position.Y < rectangle.Y + rectangle.Height)
                    {
                        if (actor.position.X < rectangle.X)
                        {
                            actor.position.X = rectangle.X - 1 - actor.size.X;
                        }
                        else
                        {
                            actor.position.X = rectangle.X + rectangle.Width + 1;
                        }
                        actor.forces.X = -actor.acceleration.X;
                    }
                    else
                    {
                        actor.forces.Y = -actor.acceleration.Y;
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
