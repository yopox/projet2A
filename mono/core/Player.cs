﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace mono.core
{
    public enum State
    {
        Idle,
        Jumping,
        Falling,
        Walking
    }

    public class Player : Actor
    {
        public Facing facing { get; set; } = Facing.Right;
        public State state { get; set; } = State.Idle;
        public bool canJump => state == State.Idle || state == State.Walking;
        Dictionary<State, Animation> animations = new Dictionary<State, Animation>();

        public Player(Atlas atlas, Vector2 position) : base(atlas, position)
        {

        }

        /// <summary>
        /// Dessine un actor
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            animations[state].Draw(spriteBatch, position, facing);
        }


        /// <summary>
        /// Rajoute une animation au player
        /// </summary>
        /// <param name="state">Etat de l'animation</param>
        /// <param name="frames">Nombre de frames de l'animation</param>
        /// <param name="isLooping">condition de répétition de l'animation</param>
        public void AddAnimation(State state, int[] frames, bool isLooping)
        {
            animations.Add(state, new Animation(state, atlas, frames, isLooping));
        }

        /// <summary>
        /// Update la frame de l'animation
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            animations[state].UpdateFrame(gameTime);
        }


        /// <summary>
        /// Mapping des touches
        /// </summary>
        /// <param name="kbState">Etat du clavier</param>
        public void Move(KeyboardState kbState)
        {
            //if(kbState.GetPressedKeys().Length == 0)
            if (kbState.IsKeyDown(Keys.D))
            {
                animations[state].Reset();
                facing = Facing.Right;
                state = State.Walking;

                position.X ++;
            }
            else if(kbState.IsKeyDown(Keys.Q))
            {
                animations[state].Reset();
                facing = Facing.Left;
                state = State.Walking;
                position.X--;
            }
            else if (speed.X == 0 && speed.Y == 0)
            {
                animations[state].Reset();
                state = State.Idle;
            }


        }

        public void Renderer(GraphicsDevice graphicsDevice, RenderTarget2D renderTarget, SpriteBatch spriteBatch)
        {
            animations[state].Renderer(graphicsDevice, renderTarget, spriteBatch, position, facing);
        }
    }
}
