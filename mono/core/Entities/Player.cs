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

        public Player(Atlas atlas, Vector2 size) : base(atlas, Vector2.Zero, size)
        {
            facing = Face.Right;
        }

        private State _newState;
        public State NewState { get => _newState; set => _newState = value; }
        private Face _newFacing;
        public Face NewFacing { get => _newFacing; set => _newFacing = value; }

        /// <summary>
        /// Update par rapport aux entrées claviers
        /// </summary>
        /// <param name="kbState">Etat du clavier</param>
        public new void Update(GameState gstate, GameTime gameTime)
        {
            // Update les animations et les collisions
            base.Update(gstate, gameTime);

            // Update le joueur en fonction des touches appuyées
            PlayerControl.ReadKeypad(this, gstate);

            //PlayerControl.ReadController(this, gstate);

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