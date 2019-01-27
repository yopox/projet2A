using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.RenderEngine;

namespace mono.core.RenderEngine
{
    /// <summary>
    /// Element comprenenant une image, sa position et le facteur de parallaxe
    /// </summary>
    public struct ParallaxElement
    {
        public Texture2D texture;
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
        /// <param name="camera"></param>
        /// <param name="parallaxElement">Image et rapport de la parallaxe</param>
        public static void Draw(SpriteBatch spriteBatch, ParallaxElement parallaxElement)
        {
            parallaxElement.position.X = - Camera.center.X / parallaxElement.factor % parallaxElement.texture.Width;
            parallaxElement.position += Rendering.zoomOffset;
            spriteBatch.Draw(parallaxElement.texture, parallaxElement.position, Color.White);
            var newPos = new Vector2(parallaxElement.position.X - parallaxElement.texture.Width, parallaxElement.position.Y);
            spriteBatch.Draw(parallaxElement.texture, newPos, Color.White);

            if (parallaxElement.position.X < Rendering.zoomOffset.X)
                spriteBatch.Draw(parallaxElement.texture, parallaxElement.position + new Vector2(parallaxElement.texture.Width, 0), Color.White);
            else
                spriteBatch.Draw(parallaxElement.texture, parallaxElement.position - new Vector2(parallaxElement.texture.Width, 0), Color.White);
        }
    }
}
