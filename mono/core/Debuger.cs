using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.core.PhysicsEngine;
using mono.PhysicsEngine;

namespace mono.core
{
    class Debuger
    {
        public static bool DebugingActors;
        public static int Radius = 4;

        /// <summary>
        /// Affiche la hitbox autour des acteurs
        /// </summary>
        /// <param name="GraphicsDevice"></param>
        /// <param name="spriteBatch"></param>
        public static void DebugActors(GraphicsDevice GraphicsDevice, SpriteBatch spriteBatch)
        {
            if (DebugingActors)
            {
                foreach (var actor in Physics.Actors)
                {
                    var polygon = actor.GetHitbox();
                    switch (polygon.type)
                    {
                        case PolygonType.Rectangle:
                            var rect = polygon;
                            Texture2D rectangleTexture = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
                            Color[] data = new Color[rect.Width * rect.Height];
                            for (int i = 0; i < data.Length; ++i) data[i] = new Color(150, 50, 50, 50);
                            rectangleTexture.SetData(data);
                            spriteBatch.Draw(rectangleTexture, Camera.GetScreenPosition(new Vector2(rect.X, rect.Y)), Color.White);
                            break;
                        case PolygonType.TriangleL:
                            break;
                        case PolygonType.None:
                            break;
                    }
                }
            }
        }
    }
}
