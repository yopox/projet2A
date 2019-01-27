using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mono.core.PhysicsEngine;

namespace mono.core

{
    /// <summary>
    /// Classe parent de tous les acteurs
    /// </summary>
    public class Actor
    {
        public Atlas atlas; // Spritesheet de l'acteur
        public State state { get; set; } = State.Idle;

        internal Dictionary<State, Animation> Animations { get => _animations; set => _animations = value; }

        public Face facing = Face.Right; // Direction à laquelle l'acteur fait face
        public Vector2 size;
        public Vector2 position;
        public Vector2 center;
        public Vector2 speed = new Vector2(0, 0);
        public Vector2 acceleration = new Vector2(0, 0);
        public Vector2 forces = new Vector2(0, 0);

        private Dictionary<State, Animation> _animations = new Dictionary<State, Animation>();

        public Boolean DebugMode = false;

        public Actor(Atlas atlas, Vector2 position, Vector2 size)
        {
            this.atlas = atlas;
            this.position = position;
            this.size = size;
            center =  new Vector2(position.X + size.X / 2, position.Y + size.Y / 2);
            Util.ToIntVector2(ref center);
        }

        /// <summary>
        /// Update la frame de l'animation
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="frameTime">durée d'une frame</param>
        public void Update(GameState gstate, GameTime gameTime)
        {
            foreach (var rectangle in GetHitboxes())
            {
                var listPolygon = CollisionTester.CollidesWithTerrain(rectangle, gstate.map);
                CollisionSolver.ActorTerrain(this, listPolygon, gameTime);
                listPolygon = CollisionTester.CollidesWithTerrain(rectangle, gstate.map);
                
            }

            Animations[state].UpdateFrame(gameTime, gstate.frameTime);
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
        public void Draw(GraphicsDevice GraphicsDevice, SpriteBatch spriteBatch)
        {
            var displayPos = Camera.GetScreenPosition(position);
            Animations[state].Draw(spriteBatch, displayPos, facing);
        }

        public Rect[] GetHitboxes()
        {
            return new Rect[] { new Rect((int)position.X, (int)position.Y, (int)size.X, (int)size.Y) };
        }
    }
}