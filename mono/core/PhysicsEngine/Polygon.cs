using System;
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
    }

    public class Rect : Polygon
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        public Rect(int x, int y, int w, int h)
        {
            type = PolygonType.Rectangle;
            this.X = x;
            this.Y = y;
            Width = w;
            Height = h;
        }

        public bool CollidesWithRectangle(Rect r)
        {
            return false;
        }
    }
}
