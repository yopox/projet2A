using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mono.core
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


        //On fournit l'état que représente l'animation, l'atlas sur lequel trouver les sprites, et l'ordre d'apparition des sprites (le même que sur l'atlas)
        public Animation(State state, Atlas atlas, int[] frames, bool isLooping)
        {
            this.atlas = atlas;
            this.frames = frames;
            this.isLooping = isLooping;
            this.state = state;
            _currentFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Facing facing)
        {
            //On calcule la position de la prochaine frame à afficher
            int row = (int)((float)frames[_currentFrame] / (float)atlas.Columns);
            int column = frames[_currentFrame] % atlas.Columns;

            Rectangle sourceRectange = new Rectangle(atlas.Width * column, atlas.Heigth * row, atlas.Width, atlas.Heigth);
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, atlas.Width, atlas.Heigth);

            //On flip les sprites suivant la direction à laquelle le joueur fait face
            if(facing == Facing.Left)
            {
            spriteBatch.Draw(atlas.Texture, destinationRectangle, sourceRectange, Color.White, 0f, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0f);
            }
            else
            {
            spriteBatch.Draw(atlas.Texture, destinationRectangle, sourceRectange, Color.White);
            }


        }

        public void UpdateFrame(GameTime gameTime, float frameTime = 0.1f)
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


        //Reset l'animation
        public void Reset()
        {
            _currentFrame = 0;
            if (_isReversed)
            {
                Array.Reverse(frames);
            }
        }
    }
}
