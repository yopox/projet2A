using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.core.PhysicsEngine;

namespace mono.core
{
    public class MapObject
    {

        public Vector2 position;
        public readonly int id;
        // Tiles à dessiner pour cet objet
        private List<int> tilesToDraw = new List<int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:mono.core.MapObject"/> class.
        /// TODO: Avoir un AtlasName et demander à AssetManager le chargement
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="position">Position.</param>
        public MapObject(int id, Vector2 position)
        {
            this.position = position;
            this.id = id;
            tilesToDraw.Add(id - 1);
        }

        public virtual void Draw(SpriteBatch spriteBatch, AssetManager am)
        {}

        public virtual Polygon GetHitbox()
        {
            return new Polygon();
        }
    }
}
