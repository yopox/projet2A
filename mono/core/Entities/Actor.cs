using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using mono.core.PhysicsEngine;
using mono.PhysicsEngine;

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
                //listPolygon = CollisionTester.CollidesWithTerrain(rectangle, gstate.map);
                
            }

            Animations[state].UpdateFrame(gameTime, gstate.frameTime);
            if (gstate.ksn.IsKeyDown(Keys.F1) && gstate.kso.IsKeyUp(Keys.F1))
            {
                DebugMode = !DebugMode;
            }
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
        public void Draw(GraphicsDevice GraphicsDevice, SpriteBatch spriteBatch, Camera camera)
        {
            var displayPos = camera.GetScreenPosition(position);
            Animations[state].Draw(spriteBatch, displayPos, facing);

            if (DebugMode)
            {
                foreach (var rect in GetHitboxes())
                {
                    Texture2D rectangleTexture = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
                    Color[] data = new Color[rect.Width * rect.Height];
                    for (int i = 0; i < data.Length; ++i) data[i] = new Color(150, 50, 50, 50);
                    rectangleTexture.SetData(data);
                    spriteBatch.Draw(rectangleTexture, camera.GetScreenPosition(new Vector2(rect.X, rect.Y)), Color.White);
                }
            }
        }

        public Rect[] GetHitboxes()
        {
            return new Rect[] { new Rect((int)position.X, (int)position.Y, (int)size.X, (int)size.Y) };
        }
    }
}