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

    public enum PlayerState
    {
        Idle,
        Jumping,
        Falling,
        Walking
    }

    public enum State
    {
        SplashScreen,
        Loading,
        Title,
        Main
    }

    public static class Util
    {
        // Screen
        static public int width = 1280;
        static public int height = 720;
        static public int virtualWidth = 1280;
        static public int virtualHeight = 720;
        static public Vector2 center = new Vector2(width / 2, height / 2);
        static public Color backgroundColor = Color.LightBlue;
        static public Color screenBorderColor = Color.Black;

        // Tileset
        static public int tileSize = 32;
        static public string solidLayerName = "terrain";

        // Player
        static public int playerHeight = 128;
        static public int playerWidth = 64;
        static public int weight = 80;

        // Unité du monde
        static public int baseUnit = 200;
        static public Vector2 gravity = new Vector2(0, 11);

        /// <summary>
        /// Convertit un vecteur 2 de float en vecteur 2 d'entier
        /// </summary>
        /// <param name="vect">vecteur modifié</param>
        public static void ToIntVector2(ref Vector2 vect)
        {
            vect.X = (int)Math.Round(vect.X);
            vect.Y = (int)Math.Round(vect.Y);
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
