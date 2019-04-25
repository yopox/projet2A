using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.RenderEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

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
        Pause,
        Cutscene
    }

    public enum CutsceneActionType
    {
        Background,
        Text,
        NewPage,
        Wait,
        Sfx,
        Bgm,
        State
    }

    public struct CutsceneAction
    {
        public CutsceneActionType type;
        public dynamic content;

        public CutsceneAction(CutsceneActionType type, dynamic content) : this()
        {
            this.type = type;
            this.content = content;
        }
    }

    public static class Util
    {
        static public Dictionary<string, Color> ColorStringDictionary = new Dictionary<string, Color>()
        {
            {"w", Color.White},
            {"g", Color.Green}
        };

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
        static public int buttonHeight = 64;

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
        public static void DrawTextRectangle(GraphicsDevice GraphicsDevice, SpriteBatch spritebatch, SpriteFont font, string stringToDraw, Rectangle boundaries, Color color)
        {
            float scale = 4f;
            Vector2 size = font.MeasureString(stringToDraw) * scale;

            Vector2 positionRect = new Vector2(boundaries.X, boundaries.Y);
            Vector2 positionStr = new Vector2(boundaries.X + boundaries.Width / 2 - size.X / 2,
                boundaries.Y + boundaries.Height / 2 - size.Y / 2);

            spritebatch.Draw(GetRectangleTexture(GraphicsDevice, color, boundaries.Width, boundaries.Height),
                positionRect,
                Color.White);

            spritebatch.DrawString(font,
                stringToDraw,
                positionStr,
                Color.White, 0.0f, Vector2.Zero, scale, new SpriteEffects(), 0.0f);
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

        /// <summary>
        /// Parses the script.
        /// TODO: L'écrire de manière moins scandaleuse
        /// </summary>
        /// <returns>Une queue contenant les actions du script.</returns>
        /// <param name="path">Lien du fichier script.</param>
        public static Queue<CutsceneAction> ParseScript(string path)
        {
            Queue<CutsceneAction> queue = new Queue<CutsceneAction>();

            // Lecture du fichier
            StreamReader stream = File.OpenText("Content/Scripts/" + path);
            string[] script = stream.ReadToEnd().Split('\n');
            stream.Close();

            int pos = 0;

            while (pos < script.Length)
            {
                if (script[pos] == "<text>")
                {
                    string text = "";
                    pos++;
                    while (script[pos] != "</text>")
                    {
                        if (script[pos] == "")
                        {
                            text += "\n";
                        }
                        else
                        {
                            text += script[pos] + "\n";
                        }
                        pos++;
                    }
                    pos++;
                    queue.Enqueue(new CutsceneAction(CutsceneActionType.Text, text));
                }

                else if (script[pos] == "<newpage>")
                {
                    queue.Enqueue(new CutsceneAction(CutsceneActionType.NewPage, false));
                    pos++;
                }

                else if (script[pos].Contains("wait"))
                {
                    string regex = "<wait ([0-9]*)>";
                    var matches = Regex.Split(script[pos], regex);
                    queue.Enqueue(new CutsceneAction(CutsceneActionType.Wait, matches[1]));
                    pos++;
                }

                else if (script[pos].Contains("background"))
                {
                    string regex = "<background ([a-zA-Z]*)>";
                    var matches = Regex.Split(script[pos], regex);
                    queue.Enqueue(new CutsceneAction(CutsceneActionType.Background, matches[1]));
                    pos++;
                }

                else if (script[pos].Contains("sfx"))
                {
                    string regex = "<sfx ([a-zA-Z]*)>";
                    var matches = Regex.Split(script[pos], regex);
                    queue.Enqueue(new CutsceneAction(CutsceneActionType.Sfx, matches[1]));
                    pos++;
                }


                else if (script[pos].Contains("bgm"))
                {
                    string regex = "<bgm ([a-zA-Z]*)>";
                    var matches = Regex.Split(script[pos], regex);
                    queue.Enqueue(new CutsceneAction(CutsceneActionType.Bgm, matches[1]));
                    pos++;
                }

                else if (script[pos].Contains("state"))
                {
                    string regex = "<state ([a-zA-Z]*)>";
                    var matches = Regex.Split(script[pos], regex);
                    queue.Enqueue(new CutsceneAction(CutsceneActionType.State, matches[1]));
                    pos++;
                }
            }
            return queue;
        }

        /// <summary>
        /// Utile pour débugger une queue.
        /// </summary>
        /// <param name="queue">Queue.</param>
        static public void PrintQueue(Queue<CutsceneAction> queue)
        {
            while (queue.Count > 0)
            {
                var elem = queue.Dequeue();
                Console.WriteLine("Type : " + elem.type.ToString());
                Console.WriteLine("Content : " + elem.content);
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// Splitte un dialogue selon la couleur des paragraphes.
        /// </summary>
        /// <returns>Liste de Tuple (string couleur, string paragraphe).</returns>
        /// <param name="dialog">Dialog.</param>
        static public List<Tuple<string, string>> ParseDialog(string dialog)
        {
            // Liste de Tuple (couleur, texte)
            List<Tuple<string, string>> liste = new List<Tuple<string, string>>();

            // Séparation du dialogue avec une regex
            Regex regex = new Regex(@"(?:\[([a-z])\]([^\[]*))", RegexOptions.Singleline);
            string[] results = regex.Split(dialog);

            // Construction de la liste
            for (int i = 0; i < results.Length; i++)
            {
                if (i % 3 == 0 && results.Length > i+2)
                {
                    liste.Add(new Tuple<string, string>(results[i + 1], results[i + 2]));
                }
            }
            return liste;
        }

    }
}
