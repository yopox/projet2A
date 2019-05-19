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
            AddAnimation(PlayerState.Jumping, new[] { 9 }, 10, true);
        }

        public void Idle()
        {
            Speed.X = 0;
            if (Math.Abs(Speed.X) == 0 && Math.Abs(Speed.Y) == 0)
            {

                NewState = PlayerState.Idle;
            }

        }

        public void Walk(Face face)
        {
            NewFacing = face;
            NewState = PlayerState.Walking;

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
        public new void Update(GameState gstate, GameTime gameTime)
        {
            // Update les animations et les collisions
            base.Update(gstate, gameTime);

            if (Math.Abs(Speed.Y) < 0.2 && Math.Abs(Acceleration.Y) < 0.2)
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
            if (NewState != State || NewFacing != Facing)
            {
                animations[State].Reset();
                State = NewState;
                Facing = NewFacing;
            }
        }
    }
}