using Microsoft.Xna.Framework;
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
    

    public class Player : Actor
    {
        public bool canJump => state == State.Idle || state == State.Walking;
       
        public Player(Atlas atlas, Vector2 position) : base(atlas, position)
        {
            facing = Facing.Right;
        }

        private State _newState;
        private Facing _newFacing;

        /// <summary>
        /// Mapping des touches
        /// </summary>
        /// <param name="kbState">Etat du clavier</param>
        public void Move(KeyboardState kbState)
        {
            if (kbState.IsKeyDown(Keys.D))
            {
                //Animations[state].Reset();
                _newFacing = Facing.Right;
                _newState = State.Walking;

                position.X ++;
            }
            else if(kbState.IsKeyDown(Keys.Q))
            {
                //Animations[state].Reset();
                _newFacing = Facing.Left;
                _newState = State.Walking;
                position.X--;
            }
            else if (speed.X == 0 && speed.Y == 0)
            {
                //Animations[state].Reset();
                _newState = State.Idle;
            }


            //Reset de l'ancienne animation si on change d'état ou de direction
            if(_newState != state || _newFacing != facing)
            {
                Animations[state].Reset();
                state = _newState;
                facing = _newFacing;
            }


        }

        public void Renderer(GraphicsDevice graphicsDevice, RenderTarget2D renderTarget, SpriteBatch spriteBatch)
        {
            Animations[state].Renderer(graphicsDevice, renderTarget, spriteBatch, position, facing);
        }
    }
}
