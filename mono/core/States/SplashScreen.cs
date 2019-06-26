using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.core.Definitions;
using mono.core.RenderEngine;
using System;

namespace mono.core.States
{
    static class SplashScreen
    {
        private static float time;
        private static Animation animation;
        private static AtlasName terrain;
        private static int duration = 3;
        private static bool leaving = false;

        public static void Initialize()
        {
            terrain = Util.ParseEnum<AtlasName>("SplashScreen");
            animation = new Animation(new[] { 0, 1, 2}, 5, true);
        }

        public static State Update(Player player, GameTime gameTime, GameState gameState)
        {
            animation.UpdateFrame();
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (time > duration)
            {
                if (!leaving)
                {
                    leaving = true;
                    FadeObjectInOut.StartFadeOut();
                }

                if (FadeObjectInOut.IsFadingOver())
                {
                    leaving = false;
                    FadeObjectInOut.StartFadeIn();
                    return Cutscene.Update(gameState, gameTime);
                }
            }

            return State.SplashScreen;
        }

        public static void Draw(SpriteBatch spritebatch, AssetManager am)
        {
            animation.Draw(spritebatch, am.GetAtlas(terrain));
        }
    }
}
