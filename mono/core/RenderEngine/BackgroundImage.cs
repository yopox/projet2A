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
        public AtlasName Name;
        public Vector2 Position;
        public float Factor;
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
            var texture = am.GetAtlas(parallaxElement.Name).Texture;
            var offsetX = AssetInfo.infos[parallaxElement.Name].OffsetX;
            var offsetY = AssetInfo.infos[parallaxElement.Name].OffsetY;
            var w = texture.Width;
            var h = texture.Height;

            // Modification de la position
            parallaxElement.Position.X = -Camera.Center.X / parallaxElement.Factor % w + offsetX;
            parallaxElement.Position += Rendering.ZoomOffset;
            parallaxElement.Position.Y = offsetY - (Camera.Center.Y - 416);

            var x = parallaxElement.Position.X;
            var y = parallaxElement.Position.Y;

            // Dessin
            spriteBatch.Draw(texture, parallaxElement.Position, Color.White);
            var newPos = new Vector2(x - w, y);
            spriteBatch.Draw(texture, newPos, Color.White);

            if (x < Rendering.ZoomOffset.X)
                spriteBatch.Draw(texture, parallaxElement.Position + new Vector2(w, 0), Color.White);
            else
                spriteBatch.Draw(texture, parallaxElement.Position - new Vector2(w, 0), Color.White);
        }
    }
}
