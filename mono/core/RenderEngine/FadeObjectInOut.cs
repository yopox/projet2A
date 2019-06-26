using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.RenderEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mono.core.RenderEngine
{
    enum FadeState
    {
        FadingIn,
        FadingOut,
        None
    }
    static class FadeObjectInOut
    {
        static FadeState fadeState = FadeState.None;
        static public readonly int FadingSpeed = 4;
        static private int fadingOpacity = 0;
        static private int maxFadingOpacity = 255;

        public static void StartFadeIn()
        {
            if (fadeState == FadeState.None)
            {
                fadeState = FadeState.FadingIn;
                fadingOpacity = 255;
            }
        }

        public static void StartFadeOut()
        {
            if (fadeState == FadeState.None)
            {
                fadeState = FadeState.FadingOut;
                fadingOpacity = 0;
            }
        }
        public static void UpdateFadeObject()
        {
            switch (fadeState)
            {
                case FadeState.FadingIn:
                    if (fadingOpacity <= 0)
                        fadeState = FadeState.None;
                    break;
                case FadeState.FadingOut:
                    if (fadingOpacity >= maxFadingOpacity)
                        fadeState = FadeState.None;
                    break;
                case FadeState.None:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Fondu au noir sans passer par la variable globale fadingIn
        /// </summary>
        /// <param name="localFading">booléen de l'état du fade in local</param>
        public static void FadeIn(ref bool localFading)
        {
            if (!localFading)
            {
                localFading = true;
                fadingOpacity = maxFadingOpacity;
            }

            if (fadingOpacity <= 0)
            {
                localFading = false;
            }
        }

        /// <summary>
        /// Affichage du fondu au noir
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="GraphicsDevice"></param>
        public static void DrawFading(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            switch (fadeState)
            {
                case FadeState.FadingIn:
                    DrawFading(spriteBatch, GraphicsDevice, -1 * FadingSpeed);
                    break;
                case FadeState.FadingOut:
                    DrawFading(spriteBatch, GraphicsDevice, FadingSpeed);
                    break;
                case FadeState.None:
                    break;
                default:
                    break;
            }
        }

        public static void DrawFading(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, int fadingSpeed)
        {
            spriteBatch.Draw(Util.GetRectangleTexture(GraphicsDevice, new Color(0, 0, 0, fadingOpacity), Rendering.VirtualWidth, Rendering.VirtualHeight),
                Vector2.Zero,
                Color.Black);

            fadingOpacity += fadingSpeed;
        }

        public static bool IsFadingOver()
        {
            return (fadeState == FadeState.None);
        }
    }
}
