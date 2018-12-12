using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;


namespace mono.core
{


    public class Player : Actor
    {
        public bool CanJump => state == State.Idle || state == State.Walking;

        public Player(Atlas atlas, Vector2 position, Vector2 size) : base(atlas, position, size)
        {
            facing = Face.Right;
        }

        private State _newState;
        private Face _newFacing;

        /// <summary>
        /// Mapping des touches et de leurs effets
        /// </summary>
        /// <param name="kbState">Etat du clavier</param>
        public void Update(GameState gstate, GameTime gameTime)
        {
            if (gstate.ksn.IsKeyDown(Keys.D))
            {
                _newFacing = Face.Right;
                _newState = State.Walking;

                forces.X = 1500;
            }
            else if (gstate.ksn.IsKeyDown(Keys.Q))
            {
                _newFacing = Face.Left;
                _newState = State.Walking;

                forces.X = -1500;
            }
            else if (Math.Abs(speed.X) < 50)
            {
                speed.X = 0;
                _newState = State.Idle;
            }

            if (gstate.ksn.IsKeyDown(Keys.Z))
            {
                forces.Y = -5000;
            }

            if (gstate.ksn.IsKeyDown(Keys.M) && gstate.kso.IsKeyUp(Keys.M))
            {
                var tiles = gstate.map.GetTiles(position, 1);
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

            UpdateFrame(gstate, gameTime);
        }
    }
}