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

        public static void Initialize()
        {
            terrain = Util.ParseEnum<AtlasName>("SplashScreen");
            animation = new Animation(new[] { 0, 1, 2}, 5, true);
        }

        public static State Update(GameTime gameTime)
        {
            animation.UpdateFrame();
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > 5)
            {
                return State.Main;
            }
            return State.SplashScreen;
        }

        public static void Draw(SpriteBatch spritebatch, AssetManager am)
        {
            animation.Draw(spritebatch, am.GetAtlas(terrain));
        }
    }
}
