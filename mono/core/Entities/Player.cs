using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using mono.core.PhysicsEngine;
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
        /// Mapping des touches et de leurs effets
        /// </summary>
        /// <param name="kbState">Etat du clavier</param>
        public new void Update(GameState gstate, GameTime gameTime)
        {
            if (gstate.ksn.IsKeyDown(Keys.D))
            {
                _newFacing = Face.Right;
                _newState = State.Walking;

                forces.X = 3000;
            }
            else if (gstate.ksn.IsKeyDown(Keys.Q))
            {
                _newFacing = Face.Left;
                _newState = State.Walking;

                forces.X = -3000;
            }
            else if (Math.Abs(speed.X) < 50)
            {
                speed.X = 0;
                _newState = State.Idle;
            }

            if (gstate.ksn.IsKeyDown(Keys.Z) && gstate.kso.IsKeyUp(Keys.Z))
            {
                forces.Y = -100000;
                CanJump = false;
            }

            if (gstate.ksn.IsKeyDown(Keys.S))
            {
                forces.Y = 1500;
            }

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

            base.Update(gstate, gameTime);
        }
    }
}