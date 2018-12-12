using System;
using Microsoft.Xna.Framework;

namespace mono.core.PhysicsEngine
{

    public enum PolygonType
    {
        Rectangle,
        Triangle
    }

    public class Polygon
    {
        public PolygonType type;

        public bool CollidesWith(Polygon p)
        {
            switch (p.type)
            {
                case PolygonType.Rectangle:
                    return CollidesWithRectangle((Rect)p);
                default:
                    return false;
            }
        }

        public virtual bool CollidesWithRectangle(Rect r)
        {
            return false;
        }

        public static Polygon FromTile(int id, int x, int y)
        {
            if (id != 0)
            {
                return new Rect(x, y - Util.tileSize, Util.tileSize, Util.tileSize);
            }
            else
            {
                return new Polygon();
            }
        }
    }

    public class Rect : Polygon
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;
        public readonly Vector2 Center;

        public Rect(int x, int y, int w, int h)
        {
            type = PolygonType.Rectangle;
            X = x;
            Y = y;
            Width = w;
            Height = h;
            Center = new Vector2(x + w / 2, y + h / 2);
        }

        public override bool CollidesWithRectangle(Rect r)
        {
            return X + Width >= r.X && X <= r.X + r.Width && Y >= r.Y - r.Height && Y - Height <= r.Y ;
        }

        public override string ToString()
        {
            return "[Rect] X: " + X + " ; Y: " + Y + " ; W: " + Width + " ; H: " + Height;
        }

    }
}
