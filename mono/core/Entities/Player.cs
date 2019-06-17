using Microsoft.Xna.Framework;
using mono.core.Definitions;
using System;


namespace mono.core
{

    public class Player : Actor
    {
        public bool CanJump = true;
        public PlayerState NewState;
        public Face NewFacing = Face.Right;

        public Player(Vector2 size) : base(AtlasName.Player, Vector2.Zero, size)
        {
            Facing = Face.Right;
            AddAnimation(PlayerState.Idle, new[] { 0, 1, 2, 3, 4 }, 12, true);
            AddAnimation(PlayerState.Walking, new[] { 5, 6, 7, 8, 9, 10, 11 }, 8, true);
            AddAnimation(PlayerState.Jumping, new[] { 12, 13, 14, 15 }, 2, false);
            AddAnimation(PlayerState.Landing, new[] { 16, 17, 18 }, 3, false);

        }

        public void Idle()
        {
            Speed.X = 0;

            if (animations[State].canInterrupt() && Math.Abs(Speed.X) == 0 && Math.Abs(Speed.Y) == 0 && CanJump)
            {
                NewState = PlayerState.Idle;
                if (State == PlayerState.Jumping)
                    NewState = PlayerState.Landing;
            }
        }

        public void Move(Face face)
        {
            if (CanJump)
                Walk(face);
            else
                Jumping(face);
        }

        public void Walk(Face face)
        {
            NewFacing = face;

            if (animations[State].canInterrupt())
                NewState = PlayerState.Walking;
            if (State == PlayerState.Jumping)
                NewState = PlayerState.Landing;

            if (face == Face.Left)
            {
                Speed.X = -1.3f;
            }
            else
            {
                Speed.X = 1.3f;
            }
        }

        public void Jumping(Face face)
        {
            NewFacing = face;
            if (face == Face.Left)
            {
                Speed.X = -1.3f;
            }
            else
            {
                Speed.X = 1.3f;
            }

        }

        public void Jump()
        {
            Forces.Y = -15000;
            Speed.Y -= 0.8f;
            NewState = PlayerState.Jumping;
        }

        /// <summary>
        /// Update par rapport aux entrées claviers
        /// </summary>
        /// <param name="gstate">Etat du jeu</param>
        /// <param name="gameTime"></param>
        public void Update(GameState gstate, GameTime gameTime, bool block = false)
        {
            // Update les animations et les collisions
            base.Update(gstate, gameTime);

            if (Math.Abs(Speed.Y) < 0.1 && Math.Abs(Acceleration.Y) < 0.1)
            {
                CanJump = true;
            }
            else
            {
                CanJump = false;
            }

            if (!block)
            {
                // Update le joueur en fonction des touches appuyées
                PlayerControl.ReadController(this, gstate);
                PlayerControl.ReadKeypad(this, gstate);
            }

            Facing = NewFacing;

            // Reset de l'ancienne animation si on change d'état ou de direction
            if (NewState != State)
            {
                animations[State].Reset();
                State = NewState;
            }
        }
    }
}