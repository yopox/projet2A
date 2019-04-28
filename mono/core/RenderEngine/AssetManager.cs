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

        private Dictionary<AtlasName, Atlas> loadedAtlas = new Dictionary<AtlasName, Atlas>();
        private readonly Microsoft.Xna.Framework.Content.ContentManager content;

        public AssetManager(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            this.content = content;
        }

        /// <summary>
        /// Prepares the specified an.
        /// </summary>
        /// <param name="an">An.</param>
        public void Prepare(AtlasName an)
        {
            if (!loadedAtlas.ContainsKey(an))
            {
                loadedAtlas.Add(an, CreateAtlas(AssetInfo.infos[an]));
            }
        }

        /// <summary>
        /// Unloads the specified an.
        /// </summary>
        /// <param name="an">An.</param>
        public void Unload(AtlasName an)
        {
            loadedAtlas.Remove(an);
        }

        /// <summary>
        /// Creates the atlas.
        /// </summary>
        /// <returns>The atlas.</returns>
        /// <param name="ai">Ai.</param>
        public Atlas CreateAtlas(AtlasInfo ai)
        {
            return new Atlas(content.Load<Texture2D>(ai.Location), ai.Width, ai.Height, ai.Padding, ai.Border);
        }

        public Atlas GetAtlas(AtlasName an)
        {
            // Atlas chargé
            if (loadedAtlas.ContainsKey(an))
            {
                return loadedAtlas[an];
            }

            // Atlas non chargé
            Prepare(an);
            return loadedAtlas[an];
        }
    }
}
