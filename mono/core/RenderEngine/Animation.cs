using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.RenderEngine;

namespace mono.core
{
    class Animation
    {

        public int[] frames; // Frames de l'animation
        public bool isLooping = true; // Répétition de l'animation
        public PlayerState state; // Etat que représente l'animation

        private int _currentFrame;
        private bool _isReversed; // Frame inversée
        readonly int duration;
        private int _currentDuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state">Etat de l'acteur</param>
        /// <param name="frames">Frames de l'animation</param>
        /// <param name="isLooping">condition de répétition de l'animation</param>
        public Animation(PlayerState state, int[] frames, int duration, bool isLooping)
        {
            this.frames = frames;
            this.isLooping = isLooping;
            this.state = state;
            this.duration = duration;
            _currentFrame = 0;
            _currentDuration = 0;
        }


        /// <summary>
        /// Dessine un sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="position">position de l'acteur</param>
        /// <param name="facing">Direction dans laquelle regarde l'acteur</param>
        public void Draw(SpriteBatch spriteBatch, Atlas atlas, Vector2 position, Face facing)
        {
            Rectangle sourceRectangle = atlas.GetSourceRectangle(frames[_currentFrame]);

            // On flip les sprites suivant la direction à laquelle le joueur fait face
            if (facing == Face.Left)
            {
                spriteBatch.Draw(atlas.Texture, position + Rendering.zoomOffset, sourceRectangle, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 0f);
            }
            else
            {
                spriteBatch.Draw(atlas.Texture, position + Rendering.zoomOffset, sourceRectangle, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateFrame()
        {
            _currentDuration++;

            if (_currentDuration > duration)
            {
                Next();// Appel du prochain sprite à afficher
                _currentDuration = 0;
            }
        }


        /// <summary>
        /// Calcul de la prochaine frame à afficher
        /// </summary>
        public void Next()
        {
            int lastIndex = frames.Length - 1;
            if (_currentFrame != lastIndex)
            {
                _currentFrame++;
            }
            else if (isLooping && _currentFrame == frames.Length - 1)
            {
                Array.Reverse(frames);
                _isReversed = !_isReversed;
                _currentFrame = 0;
            }

        }


        /// <summary>
        /// Reset de l'animation
        /// </summary>
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
