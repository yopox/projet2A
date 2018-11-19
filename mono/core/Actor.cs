using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mono.core

{
    public enum Facing
    {
        Left,
        Right
    }

    public enum State
    {
        Idle,
        Jumping,
        Falling,
        Walking
    }

    /// <summary>
    /// Classe parent de tous les acteurs
    /// </summary>
    public class Actor
    {
        public Atlas atlas;//Spritesheet de l'acteur
        public State state { get; set; } = State.Idle;
        internal Dictionary<State, Animation> Animations { get => _animations; set => _animations = value; }

        public Facing facing = Facing.Right;//Direction à laquelle l'acteur fait face
        public Vector2 position;
        public Vector2 speed = new Vector2(0, 0);
        public Vector2 acceleration = new Vector2(0, 0);
        public Vector2 forces = new Vector2(0, 0);

        private Dictionary<State, Animation> _animations = new Dictionary<State, Animation>();


        public Actor(Atlas atlas, Vector2 position)
        {
            this.atlas = atlas;
            this.position = position;
        }


        /// <summary>
        /// Update la frame de l'animation
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="frameTime">durée d'une frame</param>
        public void Update(GameTime gameTime, float frameTime = 0.1f)
        {
            Animations[state].UpdateFrame(gameTime, frameTime);
        }

        /// <summary>
        /// Rajoute une animation au player
        /// </summary>
        /// <param name="state">Etat de l'animation</param>
        /// <param name="frames">Nombre de frames de l'animation</param>
        /// <param name="isLooping">condition de répétition de l'animation</param>
        public void AddAnimation(State state, int[] frames, bool isLooping)
        {
            // TODO: Durée d'une frame de l'animation
            Animations.Add(state, new Animation(state, atlas, frames, isLooping));
        }

        /// <summary>
        /// Dessine un actor
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="camera"></param>
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            var displayPos = Util.center + (position - camera.center);
            Animations[state].Draw(spriteBatch, displayPos, facing);
        }

    }
}