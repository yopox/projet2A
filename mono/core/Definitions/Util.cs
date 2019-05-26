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
        StartJumping,
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
        Cutscene,
        Textbox
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
        public CutsceneActionType Type;
        public dynamic Content;

        public CutsceneAction(CutsceneActionType type, dynamic content) : this()
        {
            Type = type;
            Content = content;
        }
    }

    public static class Util
    {
        static public Dictionary<string, Color> ColorStringDictionary = new Dictionary<string, Color>()
        {
            {"w", Color.White},
            {"g", Color.LightGreen}
        };

        // Screen
        static public int Width = 1280;
        static public int Height = 720;
        static public int VirtualWidth = 1280;
        static public int VirtualHeight = 720;
        static public Vector2 Center = new Vector2(Width / 2, Height / 2);
        static public Color BackgroundColor = Color.LightBlue;
        static public Color ScreenBorderColor = Color.Black;

        // Tileset
        static public int TileSize = 32;
        static public string SolidLayerName = "terrain";

        // Player
        static public int PlayerHeight = 128;
        static public int PlayerWidth = 64;
        static public int Weight = 80;

        // Unité du monde
        static public int BaseUnit = 200;
        static public Vector2 Gravity = new Vector2(0, 11);

        // Font
        static public int FontSize = 8;
        static public int ButtonHeight = 64;
        static public SpriteFont Font;
        static public float scale = 4f;

        // Fading et changement d'état
        static public readonly int FadingSpeed = 4;
        static private int fadingOpacity = 0;
        static private int maxFadingOpacity = 255;
        static public bool FadingOut = false;
        static public bool FadingIn = false;
        static public bool NewState = false;

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
        public static void DrawTextRectangle(GraphicsDevice GraphicsDevice, SpriteBatch spritebatch, string stringToDraw, Rectangle boundaries, Color color)
        {
            Vector2 size = Font.MeasureString(stringToDraw) * scale;

            Vector2 positionRect = new Vector2(boundaries.X, boundaries.Y);
            Vector2 positionStr = new Vector2(boundaries.X + boundaries.Width / 2 - size.X / 2,
                boundaries.Y + boundaries.Height / 2 - size.Y / 2);

            spritebatch.Draw(GetRectangleTexture(GraphicsDevice, color, boundaries.Width, boundaries.Height),
                positionRect,
                Color.White);

            spritebatch.DrawString(Font,
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
                if (script[pos].Contains("<text>"))
                {
                    string text = "";
                    pos++;
                    while (!script[pos].Contains("</text>"))
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

                else if (script[pos].Contains("<newpage>"))
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
                    string regex = "<sfx ([a-zA-Z/0-9]*)>";
                    var matches = Regex.Split(script[pos], regex);
                    queue.Enqueue(new CutsceneAction(CutsceneActionType.Sfx, matches[1]));
                    pos++;
                }


                else if (script[pos].Contains("bgm"))
                {
                    string regex = "<bgm ([a-zA-Z_0-9]*)>";
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
                Console.WriteLine("Type : " + elem.Type.ToString());
                Console.WriteLine("Content : " + elem.Content);
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
                if (i % 3 == 0 && results.Length > i + 2)
                {
                    liste.Add(new Tuple<string, string>(results[i + 1], results[i + 2]));
                }
            }
            return liste;
        }

        /// <summary>
        /// Crée une texture si elle n'existe pas, et la renvoie telle quelle si elle existe
        /// </summary>
        /// <param name="GraphicsDevice"></param>
        /// <param name="texture">Texture</param>
        /// <param name="color">Couleur de la texture</param>
        /// <returns></returns>
        public static Texture2D GetTexture(GraphicsDevice GraphicsDevice, Texture2D texture, Color color)
        {
            // On vérifie si la texture existe déjà
            if (texture == null)
            {
                texture = GetRectangleTexture(GraphicsDevice,
                    color,
                    (int)(Rendering.VirtualWidth / Rendering.ZoomFactor),
                    (int)(Rendering.VirtualHeight / Rendering.ZoomFactor));
            }
            return texture;
        }

        /// <summary>
        /// Fondu au blanc général sur toute l'image affichée
        /// </summary>
        /// <returns></returns>
        public static bool FadeOut()
        {
            if (!FadingOut)
            {
                FadingOut = true;
                fadingOpacity = 0;
            }

            if (fadingOpacity >= maxFadingOpacity)
            {
                FadingOut = false;
            }
            return !FadingOut;
        }

        /// <summary>
        /// Fondu au noir général sur toute l'image affichée
        /// </summary>
        /// <returns></returns>
        public static bool FadeIn()
        {
            if (!FadingIn)
            {
                FadingIn = true;
                fadingOpacity = maxFadingOpacity;
            }

            if (fadingOpacity <= 0)
            {
                FadingIn = false;
            }
            return !FadingIn;
        }
        
        /// <summary>
        /// Fondu au noir sans passer par la variable globale fadingIn
        /// </summary>
        /// <param name="localFading">booléen de l'état du fade in local</param>
        public static void FadeIn(ref bool localFading)
        {
            if (!localFading)
            {
                localFading = true;
                fadingOpacity = maxFadingOpacity;
            }

            if (fadingOpacity <= 0)
            {
                localFading = false;
            }
        }

        /// <summary>
        /// Affichage du fondu au noir
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="GraphicsDevice"></param>
        public static void DrawFading(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            if (FadingIn)
            {
                DrawFading(spriteBatch, GraphicsDevice, -1 * FadingSpeed);
            }
            else
            {
                DrawFading(spriteBatch, GraphicsDevice, FadingSpeed);
            }
        }

        public static void DrawFading(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, int fadingSpeed)
        {
            spriteBatch.Draw(Util.GetRectangleTexture(GraphicsDevice, new Color(0, 0, 0, fadingOpacity), Rendering.VirtualWidth, Rendering.VirtualHeight),
                Vector2.Zero,
                Color.Black);

            fadingOpacity += fadingSpeed;
        }
    }
}
