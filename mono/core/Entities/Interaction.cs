using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

namespace mono.core
{
    public class Interaction
    {
        static public readonly int RADIUS = 128;
        public Vector2 Position;
        public readonly int Id;
        public readonly List<Tuple<string, string>> text = new List<Tuple<string, string>>();
        // Tiles à dessiner pour cet objet
        private List<int> tilesToDraw = new List<int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:mono.core.MapObject"/> class.
        /// TODO: Avoir un AtlasName et demander à AssetManager le chargement
        /// TODO: Clarifier la relation avec Actor
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="position">Position.</param>
        public Interaction(int id, Vector2 position, string content)
        {
            Position = position;
            Id = id;
            tilesToDraw.Add(id - 1);
            ParseContent(content);
        }

        internal void ParseContent(string content)
        {
            Regex regex = new Regex(@"(\[([a-zA-Z? 0-9]*)\]([^\[]*))", RegexOptions.Singleline);
            string[] results = regex.Split(content);

            // Construction de la liste
            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine(results[i]);
                if (i % 4 == 0 && results.Length > i + 3)
                {
                    text.Add(new Tuple<string, string>(results[i + 2], results[i + 3]));
                }
            }
        }
    }
}
