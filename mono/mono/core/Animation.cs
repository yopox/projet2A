using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mono.Core
{
    class Animation
    {
        public Atlas atlas;
        public int[] frames;
        public bool isLooping = true;
        public State state;

        private int _currentFrame;
        private bool _isReversed = false;
        private float _time;
        public float frameTime = 0.1f;

        public Animation(State state, Atlas atlas, int[] frames, bool isLooping)
        {
            this.atlas = atlas;
            this.frames = frames;
            this.isLooping = isLooping;
            this.state = state;
            _currentFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            //On calcule la position de la prochaine frame à afficher
            int row = (int)((float)frames[_currentFrame] / (float)atlas.Columns);
            int column = frames[_currentFrame] % atlas.Columns;

            Rectangle sourceRectange = new Rectangle(atlas.Width * column, atlas.Heigth * row, atlas.Width, atlas.Heigth);
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, atlas.Width, atlas.Heigth);
            spriteBatch.Draw(atlas.Texture, destinationRectangle, sourceRectange, Color.White);

            
        }

        public void UpdateFrame(GameTime gameTime)
        {
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds; //calcul le temps depuis le dernier appel de update
            if(_time >= frameTime)
            {
                this.Next();
                _time = 0f;
            }


        }
        

        //Change l'index _currentFrame si nécessaire
        public void Next()
        {
            int lastIndex = frames.Length - 1;
            if(_currentFrame != lastIndex)
            {
                _currentFrame++;
            }
            if (isLooping && _currentFrame == frames.Length - 1)
            {
                Array.Reverse(frames);
                _isReversed = !_isReversed;
                _currentFrame = 0;
            }
            
        }


        //Reset the current frame and reverse the array frame if necessary
        public void Reset()
        {
            _currentFrame = frames[0];
            if (_isReversed)
            {
                Array.Reverse(frames);
            }
        }
    }
}
