using Microsoft.Xna.Framework;
using System;

namespace mono.core.States
{
    static class SplashScreen
    {
        private static float time;

        public static State Update(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Console.WriteLine(time);
            if (time > 0f)
            {
                return State.Main;
            }
            return State.SplashScreen;
        }

        public static void Draw()
        {

        }
    }
}
