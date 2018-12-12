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
            if (gstate.ksn.IsKeyDown(Keys.M) && gstate.kso.IsKeyUp(Keys.M))
            {
                var hitbox = getHitBoxes()[0];

                Console.WriteLine("");
                Console.WriteLine("Number of coll : " + CollisionTester.CollidesWithTerrain(hitbox, gstate.map).Count);

                var tiles = gstate.map.GetTerrain(position, 1);
                for (int i = 0; i < tiles.Length; i++)
                {
                    Console.WriteLine(String.Join(" ", tiles[i]));
                }

            }
            Animations[state].UpdateFrame(gameTime, gstate.frameTime);
            foreach (var rectangle in getHitBoxes())
            {
                var listPolygon = CollisionTester.CollidesWithTerrain(rectangle, gstate.map);
                if (listPolygon.Count != 0)
                {
                    if (gstate.ksn.IsKeyDown(Keys.M) && gstate.kso.IsKeyUp(Keys.M))
                    {
                        Console.WriteLine(((Rect)listPolygon[0]));
                    Console.WriteLine(position);

                    }
                    CollisionSolver.ActorTerrain(this, listPolygon[0]);
                }
            }

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
            var displayPos = getScreenPosition(camera);
            Animations[state].Draw(spriteBatch, displayPos, facing);

            if (DebugMode)
            {
                foreach (var rect in getHitBoxes())
                {
                    Texture2D rectangleTexture = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
                    Color[] data = new Color[rect.Width * rect.Height];
                    for (int i = 0; i < data.Length; ++i) data[i] = new Color(150, 50, 50, 50);
                    rectangleTexture.SetData(data);
                    spriteBatch.Draw(rectangleTexture, displayPos + new Vector2(rect.X, rect.Y), Color.White);
                }
            }
        }

        public Vector2 getScreenPosition(Camera camera)
        {
            return Util.center + (position - camera.center);
        }

        public Rect[] getHitBoxes()
        {
            return new Rect[] { new Rect((int)position.X, (int)(position.Y - size.Y), (int)size.X, (int)size.Y) };
        }
    }
}