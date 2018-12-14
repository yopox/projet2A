using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.core.PhysicsEngine;
using mono.PhysicsEngine;

namespace mono.core
{
    class Debuger
    {
        public static bool debugActors = false;
        public static bool debugTiles = false;
        public static int radius = 4;

        /// <summary>
        /// Affiche la hitbox autour des acteurs
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="GraphicsDevice"></param>
        /// <param name="spriteBatch"></param>
        public static void DebugActors(Camera camera, GraphicsDevice GraphicsDevice, SpriteBatch spriteBatch)
        {
            if (debugActors)
            {
                foreach (var actor in Physics.Actors)
                {
                    foreach (var polygon in actor.GetHitboxes())
                    {
                        switch (polygon.type)
                        {
                            case PolygonType.Rectangle:
                                var rect = (Rect)polygon;
                                Texture2D rectangleTexture = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
                                Color[] data = new Color[rect.Width * rect.Height];
                                for (int i = 0; i < data.Length; ++i) data[i] = new Color(150, 50, 50, 50);
                                rectangleTexture.SetData(data);
                                spriteBatch.Draw(rectangleTexture, camera.GetScreenPosition(new Vector2(rect.X, rect.Y)), Color.White);
                                break;
                            case PolygonType.Triangle:
                                break;
                            case PolygonType.None:
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}
