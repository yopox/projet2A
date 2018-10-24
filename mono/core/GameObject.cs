using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Core

{
    public enum FramesIndex
    {
        RIGHT_1 = 0,
        RIGHT_2 = 1,
        BOTTOM_1 = 2,
        BOTTOM_2 = 3,
        LEFT_1 = 4,
        LEFT_2 = 5,
        TOP_1 = 6,
        TOP_2 = 7
    }

    public enum Direction
    {
        LEFT = 0,
        RIGHT = 1,
        TOP = 2,
        BOTTOM = 3
    }

    class GameObject
    {
        public Direction direction;
        public Vector2 position;
        public Texture2D texture;
        public Rectangle source;
        public float time;
        public float frameTime = 0.1f;
        public FramesIndex frameIndex;
        private int _totalFrames;
        public int totalFrames
        {
            get
            {
                return _totalFrames;
            }
        }

        private int _frameWidth;
        public int frameWidth
        {
            get
            {
                return _frameWidth;
            }
        }

        private int _frameHeigth;
        public int frameHeigth
        {
            get
            {
                return _frameHeigth;
            }
        }


        public GameObject()
        {

        }

        public GameObject(int totalAnimationFrame, int frameWidth, int frameHeigth)
        {
            _totalFrames = totalAnimationFrame;
            _frameHeigth = frameHeigth;
            _frameWidth = frameWidth;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            try
            {
                spritebatch.Draw(texture, position, Color.White);
            }
            catch (ArgumentNullException)
            {
            }
            
        }

        public void DrawAnimation(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, source, Color.White);
        }

        public void UpdateFrame(GameTime gameTime)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(time > frameTime)
            {
                switch (direction)
                {
                    case Direction.TOP:
                        if (frameIndex == FramesIndex.TOP_1)
                            frameIndex = FramesIndex.TOP_2;
                        else
                            frameIndex = FramesIndex.TOP_1;
                        break;
                    case Direction.LEFT:
                        if (frameIndex == FramesIndex.LEFT_1)
                            frameIndex = FramesIndex.LEFT_2;
                        else
                            frameIndex = FramesIndex.LEFT_1;
                        break;
                    case Direction.BOTTOM:
                        if (frameIndex == FramesIndex.BOTTOM_1)
                            frameIndex = FramesIndex.BOTTOM_2;
                        else
                            frameIndex = FramesIndex.BOTTOM_1;
                        break;
                    case Direction.RIGHT:
                        if (frameIndex == FramesIndex.RIGHT_1)
                            frameIndex = FramesIndex.RIGHT_2;
                        else
                            frameIndex = FramesIndex.RIGHT_1;
                        break;
                }
                time = 0f;
            }


            source = new Rectangle((int)frameIndex * frameWidth, 0, frameWidth, frameHeigth);
        }
    }
}
