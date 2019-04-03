using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.RenderEngine;
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
        Main,
        Pause
    }

    public enum CutsceneActionType
    {
        Background,
        Color,
        Text,
        NewPage,
        Wait,
        Sfx,
        Gfx,
        State
    }

    public struct CutsceneAction
    {
        public CutsceneActionType type;
        public dynamic content;
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

        // Font
        static public int fontSize = 8;

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

        /// <summary>
        /// Affiche une chaine de caractère dans un rectangle donné
        /// </summary>
        /// <param name="GraphicsDevice"></param>
        /// <param name="spritebatch"></param>
        /// <param name="font"></param>
        /// <param name="stringToDraw">Dessin à afficher</param>
        /// <param name="boundaries">"boite" dans laquelle on va afficher le texte</param>
        public static void DrawString(GraphicsDevice GraphicsDevice, SpriteBatch spritebatch, SpriteFont font, string stringToDraw, Rectangle boundaries, Color color)
        {
            Vector2 size = font.MeasureString(stringToDraw);

            float xscale = boundaries.Width / size.X;
            float yscale = boundaries.Height / size.Y;

            float scale = Math.Min(xscale, yscale);

            int strWidth = (int)Math.Round(size.X * scale);
            int strHeight = (int)Math.Round(size.Y * scale);

            Vector2 positionRect = new Vector2(boundaries.X, boundaries.Y);
            Vector2 positionStr = new Vector2(boundaries.X + (boundaries.Width - strWidth) / 2,
                boundaries.Y + (boundaries.Height - strHeight) / 2);
            
            spritebatch.Draw(GetRectangleTexture(GraphicsDevice, color, boundaries.Width, boundaries.Height), 
                positionRect, 
                Color.White);

            spritebatch.DrawString(font, 
                stringToDraw, 
                positionStr, 
                Color.White, 0.0f, Vector2.Zero, 4f, new SpriteEffects(), 0.0f);
        }

        /// <summary>
        /// Renvoie une texture pour dessiner un rectangle
        /// </summary>
        /// <param name="GraphicsDevice"></param>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <param name="heigth"></param>
        /// <returns></returns>
        public static Texture2D GetRectangleTexture(GraphicsDevice GraphicsDevice, Color color, int width, int heigth)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, width, heigth);
            Color[] data = new Color[width * heigth];
            for (int i = 0; i < data.Length; ++i)
                data[i] = color;
            texture.SetData(data);

            return texture;
        }
        /// <summary>
        /// Calcule le modulo de a par b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Mod(int a, int b)
        {
            int m = a % b;
            if (m < 0)
                return b + m;
            else
                return m;
        }
    }
}
