using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mono.core.PhysicsEngine;
using mono.PhysicsEngine;
using System;


namespace mono.core
{


    public class Player : Actor
    {
        public bool CanJump = true;

        public Player(Atlas atlas, Vector2 size) : base(atlas, Vector2.Zero, size)
        {
            facing = Face.Right;
        }

        private State _newState;
        private Face _newFacing;

        /// <summary>
        /// Update par rapport aux entrées claviers
        /// </summary>
        /// <param name="kbState">Etat du clavier</param>
        public new void Update(GameState gstate, GameTime gameTime)
        {
            // Update les animations et les collisions
            base.Update(gstate, gameTime);

            if (Math.Abs(speed.Y) < 1 && Math.Abs(acceleration.Y) < 1)
            {
                CanJump = true;
            }

            if (gstate.ksn.IsKeyDown(Keys.D))
            {
                _newFacing = Face.Right;
                _newState = State.Walking;

                speed.X = 1.5f;
            }
            else if (gstate.ksn.IsKeyDown(Keys.Q))
            {
                _newFacing = Face.Left;
                _newState = State.Walking;

                speed.X = -1.5f;
            }
            else if (Math.Abs(speed.X) < 50)
            {
                speed.X = 0;
                _newState = State.Idle;
            }

            if (gstate.ksn.IsKeyDown(Keys.Z) && gstate.kso.IsKeyUp(Keys.Z) && CanJump)
            {
                forces.Y = -15000;
                speed.Y -= 0.8f;
                CanJump = false;
            }

            if (gstate.ksn.IsKeyDown(Keys.S))
            {
                forces.Y = 10;
            }

            // Activation mode Debug
            if (gstate.ksn.IsKeyDown(Keys.M) && gstate.kso.IsKeyUp(Keys.M))
            {
                var hitbox = GetHitboxes()[0];

                Console.WriteLine("");
                Console.WriteLine("Number of coll : " + CollisionTester.CollidesWithTerrain(hitbox, gstate.map).Count);

                var tiles = gstate.map.GetTerrain(GetHitboxes()[0].Center, 4);
                for (int i = 0; i < tiles.Length; i++)
                {
                    Console.WriteLine(String.Join(" ", tiles[i]));
                }

            }

            // Reset de l'ancienne animation si on change d'état ou de direction
            if (_newState != state || _newFacing != facing)
            {
                Animations[state].Reset();
                state = _newState;
                facing = _newFacing;
            }


        }
    }
}