using Microsoft.Xna.Framework;
using System;

namespace mono.core
{
    public enum Face
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum State
    {
        Idle,
        Jumping,
        Falling,
        Walking
    }

    public static class Util
    {
        // Screen
        static public int width = 640;
        static public int height = 360;
        static public Vector2 center = new Vector2(width / 2, height / 2);

        // Tileset
        static public int tileSize = 32;
        static public string solidLayerName = "terrain";

        // Player
        static public int playerHeight = 128;
        static public int playerWidth = 64;

        public static void ToIntVector2(ref Vector2 vect)
        {
            vect.X = (int)Math.Round(vect.X);
            vect.Y = (int)Math.Round(vect.Y);
        }
    }
}
