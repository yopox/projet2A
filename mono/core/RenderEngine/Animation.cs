using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mono.core.RenderEngine
{
    class Animation
    {
        public int[] Frames; // Frames de l'animation
        public bool IsLooping = true; // Répétition de l'animation
        protected int currentFrame;
        protected readonly int FrameDuration;
        protected int currentDuration;
        public bool IsOver = false;

        public Animation(int[] frames, int duration, bool isLooping)
        {
            Frames = frames;
            IsLooping = isLooping;
            FrameDuration = duration;
            currentFrame = 0;
            currentDuration = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateFrame()
        {
            currentDuration++;

            if (currentDuration >= FrameDuration)
            {
                Next();// Appel du prochain sprite à afficher
                currentDuration = 0;
            }
        }


        /// <summary>
        /// Calcul de la prochaine frame à afficher
        /// </summary>
        public void Next()
        {
            int lastIndex = Frames.Length - 1;
            if (currentFrame != lastIndex)
                currentFrame++;
            else if (IsLooping && currentFrame == Frames.Length - 1)
                currentFrame = 0;
            else
                IsOver = true;

        }

        /// <summary>
        /// Reset de l'animation
        /// </summary>
        public void Reset()
        {
            IsOver = false;
            currentFrame = 0;
        }

        /// <summary>
        /// Vérifie si on peut interrompre une animation
        /// </summary>
        /// <returns></returns>
        public bool canInterrupt()
        {
            if (IsLooping)
                return true;
            else
                return IsOver;
        }

        public void Draw(SpriteBatch spriteBatch, Atlas atlas)
        {
            Rectangle sourceRectangle = atlas.GetSourceRectangle(Frames[currentFrame]);
            spriteBatch.Draw(atlas.Texture, Vector2.Zero, sourceRectangle, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        }
    }
}