using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.core.RenderEngine;
using mono.RenderEngine;

namespace mono.core
{
    class PlayerAnimation : Animation
    {
        public PlayerState State { get; private set; } // Etat que représente l'animation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="atlas">Atlas de l'animation</param>
        /// <param name="state">Etat de l'acteur</param>
        /// <param name="frames">Frames de l'animation</param>
        /// <param name="isLooping">condition de répétition de l'animation</param>
        /// <param name="duration">durée de l'affichage des animations en frame</param>
        public PlayerAnimation(PlayerState state, int[] frames, Atlas atlas, int duration, bool isLooping) : base(frames, atlas, duration, isLooping)
        {
            State = state;
        }


        /// <summary>
        /// Dessine un sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name=""
        /// <param name="position">position de l'acteur</param>
        /// <param name="facing">Direction dans laquelle regarde l'acteur</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Face facing)
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
    }
}
