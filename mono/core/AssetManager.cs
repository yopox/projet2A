using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using mono.core.Definitions;

namespace mono.core
{
    /// <summary>
    /// Asset manager.
    /// Gère la création d'Atlas (chargement d'images).
    /// </summary>
    public class AssetManager
    {

        private Dictionary<AtlasName, Atlas> LoadedAtlas = new Dictionary<AtlasName, Atlas>();
        private readonly Microsoft.Xna.Framework.Content.ContentManager Content;

        public AssetManager(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            Content = content;
        }

        /// <summary>
        /// Prepares the specified an.
        /// </summary>
        /// <param name="an">An.</param>
        public void Prepare(AtlasName an)
        {
            if (!LoadedAtlas.ContainsKey(an))
            {
                LoadedAtlas.Add(an, CreateAtlas(AssetInfo.infos[an]));
            }
        }

        /// <summary>
        /// Unloads the specified an.
        /// </summary>
        /// <param name="an">An.</param>
        public void Unload(AtlasName an)
        {
            LoadedAtlas.Remove(an);
        }

        /// <summary>
        /// Creates the atlas.
        /// </summary>
        /// <returns>The atlas.</returns>
        /// <param name="ai">Ai.</param>
        public Atlas CreateAtlas(AtlasInfo ai)
        {
            return new Atlas(Content.Load<Texture2D>(ai.location), ai.width, ai.height, ai.padding, ai.border);
        }

        public Atlas GetAtlas(AtlasName an)
        {
            // Atlas chargé
            if (LoadedAtlas.ContainsKey(an))
            {
                return LoadedAtlas[an];
            }

            // Atlas non chargé
            Prepare(an);
            return LoadedAtlas[an];
        }
    }
}
