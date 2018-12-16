using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mono.core.RenderEngine
{
    public struct ParallaxElement
    {
        public Texture2D texture;
        public Vector2 position;
        public float factor;
    } 

    static class BackgroundImage
    {
        public static void Update(Camera camera, ParallaxElement parallaxElement)
        {
        }

        public static void Draw(SpriteBatch spriteBatch, Camera camera, ParallaxElement parallaxElement)
        {
            parallaxElement.position.X = -camera.center.X / parallaxElement.factor % parallaxElement.texture.Width ;
            spriteBatch.Draw(parallaxElement.texture, parallaxElement.position, Color.White);
            if (parallaxElement.position.X < 0)
                spriteBatch.Draw(parallaxElement.texture, parallaxElement.position + new Vector2(parallaxElement.texture.Width, 0), Color.White);
            else
                spriteBatch.Draw(parallaxElement.texture, parallaxElement.position - new Vector2(parallaxElement.texture.Width, 0), Color.White);
        }
    }
}
