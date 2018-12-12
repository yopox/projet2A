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
        private readonly int x;
        private readonly int y;
        private readonly int width;
        private readonly int height;

        public Rect(int x, int y, int w, int h)
        {
            type = PolygonType.Rectangle;
            this.x = x;
            this.y = y;
            width = w;
            height = h;
        }

        public bool CollidesWithRectangle(Rect r)
        {
            return false;
        }
    }
}
