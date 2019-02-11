using System;
using Microsoft.Xna.Framework;

namespace mono.core.PhysicsEngine
{

    public enum PolygonType
    {
        Rectangle,
        TriangleL,
        TriangleR,
        None
    }

    public class Polygon
    {
        public static void test()
        {
            Rect r1 = new Rect(1, 0, 32, 32);
            Tri t1 = new Tri(new Vector2(64, 32), 32, 32, PolygonType.TriangleR);
            r1.CollidesWithTriangle(t1);
        }
        public PolygonType type = PolygonType.None;

        public bool CollidesWith(Polygon p)
        {
            switch (p.type)
            {
                case PolygonType.Rectangle:
                    return CollidesWithRectangle((Rect)p);
                case PolygonType.TriangleL:
                case PolygonType.TriangleR:
                    return CollidesWithTriangle((Tri)p);
                default:
                    return false;
            }
        }

        public virtual bool CollidesWithRectangle(Rect r)
        {
            return false;
        }

        public virtual bool CollidesWithTriangle(Tri t)
        {
            return false;
        }

        public static Polygon FromTile(int id, int x, int y)
        {
            if (id == 33 || id == 14)
            {
                return new Tri(new Vector2(x + Util.tileSize, y + Util.tileSize), Util.tileSize, Util.tileSize, PolygonType.TriangleR);
            }
            if (id == 36 || id == 15)
            {
                return new Tri(new Vector2(x, y + Util.tileSize), Util.tileSize, Util.tileSize, PolygonType.TriangleL);
            }
            if (id != 0)
            {
                return new Rect(x, y, Util.tileSize, Util.tileSize);
            }
            return new Polygon();
        }
    }

    /// <summary>
    /// Origine des rectangles en haut à droite.
    /// </summary>
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
            return X + Width > r.X && X < r.X + r.Width && Y < r.Y + r.Height && Y + Height > r.Y;
        }

        public override bool CollidesWithTriangle(Tri t)
        {
            switch (t.type)
            {
                case PolygonType.TriangleL:
                    if (X + Width <= t.A.X || X >= t.A.X + t.Width || Y + Height <= t.A.Y - t.Height || Y >= t.A.Y)
                        return false;
                    var w0 = t.A.X + t.Width - X;
                    var sH0 = t.Height * w0 / t.Width;
                    return Y + Height > t.A.Y - sH0;
                default:
                    if (X + Width <= t.A.X - t.Width || X >= t.A.X || Y + Height <= t.A.Y - t.Height || Y >= t.A.Y)
                        return false;
                    var w1 = X + Width - t.A.X + t.Width;
                    var sH1 = t.Height * w1 / t.Width;
                    return Y + Height > t.A.Y - sH1;
            }
        }

        public override string ToString()
        {
            return "[Rect] X: " + X + " ; Y: " + Y + " ; W: " + Width + " ; H: " + Height;
        }

    }

    /// <summary>
    /// Origine des triangles : angle droit.
    /// La hauteur et la largeur sont positives.
    /// Les coordonnées du point le plus haut seront (A.X, A.Y - H).
    /// </summary>
    public class Tri : Polygon
    {
        public readonly Vector2 A;
        public readonly int Width;
        public readonly int Height;
        public readonly Vector2 Center;

        public Tri(Vector2 a, int w, int h, PolygonType type)
        {
            this.type = type;
            A = a;
            Width = w;
            Height = h;
        }

        public override bool CollidesWithRectangle(Rect r)
        {
            return r.CollidesWithTriangle(this);
        }

        public override bool CollidesWithTriangle(Tri t)
        {
            return false;
        }

        public override string ToString()
        {
            return "[Tri] A.X: " + A.X + " ; A.Y: " + A.Y + " ; W: " + Width + " ; H: " + Height;
        }

    }
}
