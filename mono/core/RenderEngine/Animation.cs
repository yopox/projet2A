using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.RenderEngine;

namespace mono.core
{
    class Animation
    {

        public int[] Frames; // Frames de l'animation
        public bool IsLooping = true; // Répétition de l'animation
        public PlayerState State { get; private set; } // Etat que représente l'animation

        private int currentFrame;
        readonly int FrameDuration;
        private int currentDuration;
        public bool IsOver = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state">Etat de l'acteur</param>
        /// <param name="frames">Frames de l'animation</param>
        /// <param name="isLooping">condition de répétition de l'animation</param>
        /// <param name="duration">durée de l'affichage des animations en frame</param>
        public Animation(PlayerState state, int[] frames, int duration, bool isLooping)
        {
            Frames = frames;
            IsLooping = isLooping;
            State = state;
            FrameDuration = duration;
            currentFrame = 0;
            currentDuration = 0;
        }


        /// <summary>
        /// Dessine un sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name=""
        /// <param name="position">position de l'acteur</param>
        /// <param name="facing">Direction dans laquelle regarde l'acteur</param>
        public void Draw(SpriteBatch spriteBatch, Atlas atlas, Vector2 position, Face facing)
        {
            Rectangle sourceRectangle = atlas.GetSourceRectangle(Frames[currentFrame]);

            // On flip les sprites suivant la direction à laquelle le joueur fait face
            if (facing == Face.Left)
            {
                spriteBatch.Draw(atlas.Texture, position + Rendering.ZoomOffset, sourceRectangle, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 0f);
            }
            else
            {
                spriteBatch.Draw(atlas.Texture, position + Rendering.ZoomOffset, sourceRectangle, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            }
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
    }
}
