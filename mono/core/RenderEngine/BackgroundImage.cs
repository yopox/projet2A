using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.core.Definitions;
using mono.RenderEngine;

namespace mono.core.RenderEngine
{
    /// <summary>
    /// Element comprenenant une image, sa position et le facteur de parallaxe
    /// </summary>
    public struct ParallaxElement
    {
        public AtlasName name;
        public Vector2 position;
        public float factor;
    }
    /// <summary>
    /// Affiche les images d'arrière plan avec la gestion de la parallaxe
    /// </summary>
    static class BackgroundImage
    {
        /// <summary>
        /// Affiche une image avec de la parallaxe
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="parallaxElement">Image et rapport de la parallaxe</param>
        /// <param name="am">AssetManager</param>
        public static void Draw(SpriteBatch spriteBatch, ParallaxElement parallaxElement, AssetManager am)
        {
            // On récupère la texture
            var texture = am.GetAtlas(parallaxElement.name).Texture;
            var w = texture.Width;

            // Modification de la position
            parallaxElement.position.X = -Camera.center.X / parallaxElement.factor % w;
            parallaxElement.position += Rendering.zoomOffset;
            var x = parallaxElement.position.X;
            var y = parallaxElement.position.Y;

            // Dessin
            spriteBatch.Draw(texture, parallaxElement.position, Color.White);
            var newPos = new Vector2(x - w, y);
            spriteBatch.Draw(texture, newPos, Color.White);

            if (x < Rendering.zoomOffset.X)
                spriteBatch.Draw(texture, parallaxElement.position + new Vector2(w, 0), Color.White);
            else
                spriteBatch.Draw(texture, parallaxElement.position - new Vector2(w, 0), Color.White);
        }
    }
}
