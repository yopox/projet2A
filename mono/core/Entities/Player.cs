using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mono.core.PhysicsEngine;
using mono.core.RenderEngine;
using System;


namespace mono.core
{


    public class Player : Actor
    {
        public bool CanJump = true;
        public PlayerState NewState;
        public Face NewFacing = Face.Right;

        public Player(Atlas atlas, Vector2 size) : base(atlas, Vector2.Zero, size)
        {
            facing = Face.Right;
        }

        public void Idle()
        {
            NewState = PlayerState.Idle;
            speed.X = 0;
        }

        public void Walk(Face face)
        {
            NewFacing = face;
            NewState = PlayerState.Walking;

            if (face == Face.Left)
            {
                speed.X -= 1.5f;
            }
            else
            {
                speed.X += 1.5f;
            }
        }

        public void Jump()
        {
            forces.Y = -15000;
            speed.Y -= 0.8f;
        }

        /// <summary>
        /// Update par rapport aux entrées claviers
        /// </summary>
        /// <param name="kbState">Etat du clavier</param>
        public new void Update(GameState gstate, GameTime gameTime)
        {
            // Update les animations et les collisions
            base.Update(gstate, gameTime);

            if (Math.Abs(speed.Y) < 0.2 && Math.Abs(acceleration.Y) < 0.2)
            {
                CanJump = true;
            }
            else
            {
                CanJump = false;
            }

            // Update le joueur en fonction des touches appuyées
            PlayerControl.ReadController(this, gstate);
            PlayerControl.ReadKeypad(this, gstate);


            // Reset de l'ancienne animation si on change d'état ou de direction
            if (NewState != state || NewFacing != facing)
            {
                Animations[state].Reset();
                state = NewState;
                facing = NewFacing;
            }
        }
    }
}