using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Game1.Core
{
    class Player : GameObject
    {
        public Player(int totalAnimationFrame, int frameWidth, int frameHeigth) : base(totalAnimationFrame, frameWidth, frameHeigth)
        {
            direction = Direction.RIGHT;
            frameIndex = FramesIndex.RIGHT_1;
        }

        public void Move(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Z))
            {
                direction = Direction.TOP;
                position.Y--;
            }

            if (state.IsKeyDown(Keys.Q))
            {
                direction = Direction.LEFT;
                position.X--;
            }

            if (state.IsKeyDown(Keys.S))
            {
                direction = Direction.BOTTOM;
                position.Y++;
            }

            if (state.IsKeyDown(Keys.D))
            {
                direction = Direction.RIGHT;
                position.X++;
            }
        }
    }
}
