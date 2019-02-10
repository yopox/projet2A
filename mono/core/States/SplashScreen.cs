using Microsoft.Xna.Framework;
using System;

namespace mono.core.States
{
    static class SplashScreen
    {
        private static float _time;

        public static State Update(GameTime gameTime)
        {
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Console.WriteLine(_time);
            if (_time > 0f)
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
