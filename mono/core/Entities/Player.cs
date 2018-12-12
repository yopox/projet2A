using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.PhysicsEngine;
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
        /// Mapping des touches et de leurs effets
        /// </summary>
        /// <param name="kbState">Etat du clavier</param>
        public void Move(KeyboardState kbState)
        {
            if (kbState.IsKeyDown(Keys.D))
            {
                _newFacing = Facing.Right;
                _newState = State.Walking;

                forces.X = 1500;
            }
            else if (kbState.IsKeyDown(Keys.Q))
            {
                _newFacing = Facing.Left;
                _newState = State.Walking;

                forces.X = -1500;
            }
            else if (Math.Abs(speed.X) < 50)
            {
                speed.X = 0;
                _newState = State.Idle;
            }

            if (kbState.IsKeyDown(Keys.Z))
            {
                forces.Y = -5000;
            }

            //Reset de l'ancienne animation si on change d'état ou de direction
            if (_newState != state || _newFacing != facing)
            {
                Animations[state].Reset();
                state = _newState;
                facing = _newFacing;
            }
        }
    }
}